using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PeggyTheGameApp.src.GameObjects;
using PeggyTheGameApp.src.Global;

namespace PeggyTheGameApp.src
{
    public class GameWorld
    {
        public Peggy Peggy { get; private set; }
        private Dictionary<string, Room> _rooms;
        private string _startRoomId;

        public GameWorld()
        {
            _rooms = new Dictionary<string, Room>();
            Peggy = new Peggy();
        }

        public void LoadMapFromJson(string path)
        {
            string fullPath = Path.Combine(Environment.CurrentDirectory, path);
            if (!File.Exists(fullPath))
            {
                Console.WriteLine("Error loading world map, no file found ({0}).", path);
            }

            using (StreamReader sr = File.OpenText(fullPath))
            {
                JObject map = (JObject)JToken.ReadFrom(new JsonTextReader(sr))["map"];

                foreach (var jRoom in map)
                {
                    Room r = new Room(jRoom.Key, (JObject)jRoom.Value);
                    if (r.IsStartRoom) { _startRoomId = r.Id; }
                    _rooms.Add(r.Id, r);
                }
            }

            // Put Peggy in the start location
            Peggy.SetCurrentRoom(_rooms[_startRoomId]);
        }

        public void MovePeggyToRoom(Room newRoom)
        {
            if (!_rooms.ContainsKey(newRoom.Id))
            {
                throw new InvalidOperationException($"{newRoom.FriendlyName} does not exist in this map.");
            }
            Peggy.SetCurrentRoom(newRoom);
        }

        public void AddRoom(string roomId, Room room)
        {
            if (string.IsNullOrEmpty(roomId))
            {
                throw new ArgumentException("AddRoom() missing roomId argument.");
            }
            if (room == null)
            {
                throw new ArgumentNullException("AddRoom() missing room argument.");
            }
            _rooms.Add(roomId, room);
        }

        public string MovePeggy(string direction)
        {
            Direction d = Utils.GetDirectionFromString(direction);
            if (d == Direction.Invalid)
            {
                return $"{Utils.Capitalize(direction)}? What kind of bonkers direction is that?!\r\n";
            }

            AdjoiningRoom ar = Peggy.CurrentRoom.GetAdjoiningRoom(d);
            if (ar == null)
            {
                return $"There is nothing to the {Utils.Capitalize(direction)}.\r\n";
            }
            if (!_rooms.ContainsKey(ar.Id))
            {
                throw new InvalidOperationException($"The adjoining room {ar.Id} is not in the list of rooms.");
            }

            // New room exists, get the new room object so we have access to all the properties.
            Room newRoom = _rooms[ar.Id];

            if (!string.IsNullOrEmpty(ar.RequiresId))
            {
                if (!Peggy.InventoryHasItem(ar.RequiresId))
                {
                    // TODO: we should print the Friendly name for the item, but iterating
                    // through all rooms and items is a slow way. We could build a Dictionary
                    // of item ID -> item Name mappings when we parse the map.
                    return $"The {ar.RequiresId} is required to get into the {newRoom.FriendlyName}.\r\n";
                }
                else
                {
                    Item requiredItem = Peggy.GetItemFromInventoryWithoutRemoving(ar.RequiresId);
                    ar.SatisfyRequiredItem(requiredItem);
                }
            }

            return Peggy.SetCurrentRoom(newRoom);
        }

        public string Look()
        {
            return Peggy.Look();
        }

        public string LookInContainer(string containerName)
        {
            return Peggy.LookInContainer(containerName);
        }

        public string TalkToCharacter(string characterName)
        {
            return Peggy.TalkToCharacter(characterName);
        }

        public string TakeItemFrom(string itemName, string takeFrom)
        {
            return Peggy.TakeItemFrom(itemName, takeFrom);
        }

        public string DropItemIn(string itemName, string dropIn)
        {
            return Peggy.DropItemIn(itemName, dropIn);
        }

        public string GiveItemTo(string itemName, string giveTo)
        {
            return Peggy.GiveItemTo(itemName, giveTo);
        }

        public string LookInInventory()
        {
            return Peggy.LookInInventory();
        }
    }
}
