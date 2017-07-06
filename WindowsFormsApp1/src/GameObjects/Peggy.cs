using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeggyTheGameApp.src.GameObjects
{
    public class Peggy
    {
        public Room CurrentRoom { get; private set; }
        
        public string SetCurrentRoom(Room newRoom)
        {
            CurrentRoom = newRoom;
            return $"Moved to the {CurrentRoom.FriendlyName}.\r\n";
        }

        public string Look()
        {
            return CurrentRoom.Look();
        }

        public string LookInContainer(string containerName)
        {
            return CurrentRoom.LookInContainer(containerName);
        }

        public string TalkToCharacter(string characterName)
        {
            return CurrentRoom.TalkToCharacter(characterName);
        }
    }
}
