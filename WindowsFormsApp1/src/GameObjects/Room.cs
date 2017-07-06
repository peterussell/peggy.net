using Newtonsoft.Json.Linq;
using PeggyTheGameApp.src.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeggyTheGameApp.src.GameObjects
{
    public class Room
    {
        public string Id { get; private set; }
        public string FriendlyName { get; private set; }
        public string Description { get; private set; }
        public bool IsStartRoom { get; private set; }

        private List<Container> _containers;
        private List<Character> _characters;
        private Dictionary<Direction, AdjoiningRoom> _adjoiningRooms;

        private const string _isStartRoomKey = "is-start-room";
        private const string _friendlyNameKey = "friendly-name";
        private const string _descriptionKey = "description";
        private const string _containersKey = "containers";
        private const string _charactersKey = "characters";
        private const string _adjoiningRoomsKey = "adjoining-rooms";

        public Room(string id)
        {
            Id = id;
            _containers = new List<Container>();
            _characters = new List<Character>();
            _adjoiningRooms = new Dictionary<Direction, AdjoiningRoom>();
        }

        public Room(string id, string friendlyName, string description) : this(id)
        {
            FriendlyName = friendlyName;
            Description = description;
        }

        public Room(string id, JObject jRoom) : this(id)
        {
            // Is Start Room
            JToken jIsStartRoom = jRoom[_isStartRoomKey];
            if (jIsStartRoom != null && ((bool)jIsStartRoom) == true)
            {
                IsStartRoom = true;
            }

            // Friendly Name
            JToken jFriendlyName = jRoom[_friendlyNameKey];
            if (jFriendlyName != null && ((string)jFriendlyName) != "")
            {
                FriendlyName = (string)jFriendlyName;
            }
            else
            {
                FriendlyName = "Room of No Name";
            }

            // Description
            JToken jDescription = jRoom[_descriptionKey];
            if (jDescription != null)
            {
                Description = (string)jDescription;
            }

            // Containers
            JToken jContainers = jRoom[_containersKey];
            if (jContainers != null) {
                foreach (JObject jContainer in JArray.Parse(jContainers.ToString()))
                {
                    _containers.Add(new Container(jContainer));
                }
            }

            // Characters
            JToken jCharacters = jRoom[_charactersKey];
            if (jCharacters != null)
            {
                foreach (JObject jCharacter in JArray.Parse(jCharacters.ToString()))
                {
                    _characters.Add(new Character(jCharacter));
                }
            }

            // Adjoining Rooms
            JToken jAdjoiningRooms = jRoom[_adjoiningRoomsKey];
            if (jAdjoiningRooms != null)
            {
                foreach (var jar in JObject.Parse(jAdjoiningRooms.ToString()))
                {
                    string adjDir = jar.Key;
                    string adjId = (string)jar.Value["id"];
                    string adjReq = (string)jar.Value["requires"];

                    if (adjId != null)
                    {
                        AddAdjoiningRoom(adjDir, adjId, adjReq);
                    }
                }
            }
        }

        public void AddContainer(Container c)
        {
            _containers.Add(c);
        }

        public void AddCharacter(Character c)
        {
            _characters.Add(c);
        }

        public string Look()
        {
            string res = $"You are in the {FriendlyName}, ";
            if (_containers.Count == 0 && _characters.Count == 0)
            {
                return res += "which is devoid of anything interesting.\r\n";
            }

            res += "you see...\r\n";
            foreach (Container c in _containers)
            {
                res += " - " + c.Name + "\r\n";
            }
            foreach (Character c in _characters)
            {
                res += " - " + c.Name + "\r\n";
            }
            return res;
        }

        public string LookInContainer(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                return "Look in where?\r\n";
            }

            Container match = _containers.Find(c => c.Name.ToLower().Equals(containerName));
            if (match == null)
            {
                return $"There's no {containerName} you can look in here.\r\n";
            }
            return match.LookIn();
        }

        public string TalkToCharacter(string characterName)
        {
            string charNotFoundText = $"There's no one called {characterName} in here.\r\n";
            if (string.IsNullOrEmpty(characterName))
            {
                return charNotFoundText;
            }

            Character match = _characters.Find(c => c.Name.ToLower().Equals(characterName.ToLower()));
            if (match == null)
            {
                return charNotFoundText;
            }
            return match.TalkTo();
        }

        public Container GetContainerByName(string containerName)
        {
            return _containers.Find(c => c.Name.ToLower().Equals(containerName.ToLower()));
        }

        public Character GetCharacterByName(string characterName)
        {
            return _characters.Find(c => c.Name.ToLower().Equals(characterName.ToLower()));
        }

        public void AddAdjoiningRoom(string direction, string id, string requiresId="")
        {
            if (string.IsNullOrEmpty(direction))
            {
                throw new ArgumentException("No direction provided for adjoining room " + id);
            }

            Direction d = Utils.GetDirectionFromString(direction);

            if (d == Direction.Invalid)
            {
                throw new ArgumentException("Invalid direction '" + direction + "' for adjoining room " + id);
            }
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("No room id provided for adjoining room. This room: " + 
                                            FriendlyName + ", direction " + direction);
            }
            AdjoiningRoom adjRoom = new AdjoiningRoom(id, requiresId);

            if (_adjoiningRooms.Keys.Contains(d))
            {
                throw new InvalidOperationException("Unable to add adjoining room, " + FriendlyName +
                                                    "already has a room to the " + direction);
            }
            _adjoiningRooms.Add(d, adjRoom);
        }

        public AdjoiningRoom GetAdjoiningRoom(Direction d)
        {
            if (!_adjoiningRooms.ContainsKey(d))
            {
                return null;
            }
            return _adjoiningRooms[d];
        }
    }

    public class AdjoiningRoom
    {
        public string Id { get; private set; }
        public string RequiresId { get; private set; }

        public AdjoiningRoom(string id, string requiresId)
        {
            Id = id;
            RequiresId = requiresId;
        }
    }
}
