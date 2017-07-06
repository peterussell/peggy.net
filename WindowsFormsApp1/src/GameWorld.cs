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
        private Peggy _peggy;
        private Dictionary<string, Room> _rooms;
        private string _startRoomId;

        public GameWorld()
        {
            _rooms = new Dictionary<string, Room>();
            _peggy = new Peggy();
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
            _peggy.SetCurrentRoom(_rooms[_startRoomId]);
        }

        public string MovePeggy(string direction)
        {
            Direction d = Utils.GetDirectionFromString(direction);
            if (d == Direction.Invalid)
            {
                return $"{Utils.Capitalize(direction)}? What kind of bonkers direction is that?!\r\n";
            }

            AdjoiningRoom ar = _peggy.CurrentRoom.GetAdjoiningRoom(d);
            if (ar == null)
            {
                return $"There is nothing to the {Utils.Capitalize(direction)}.\r\n";
            }
            if (!_rooms.ContainsKey(ar.Id))
            {
                throw new InvalidOperationException($"The adjoining room {ar.Id} is not in the list of rooms.");
            }

            // TODO: if there are required items, check Peggy has them before allowing the transition.

            return _peggy.SetCurrentRoom(_rooms[ar.Id]);
        }

        public string Look()
        {
            return _peggy.Look();
        }

        public string LookInContainer(string containerName)
        {
            return _peggy.LookInContainer(containerName);
        }

        public string TalkToCharacter(string characterName)
        {
            return _peggy.TalkToCharacter(characterName);
        }

        public string TakeItemFrom(string itemName, string takeFrom)
        {
            return _peggy.TakeItemFrom(itemName, takeFrom);
        }
    }
}
