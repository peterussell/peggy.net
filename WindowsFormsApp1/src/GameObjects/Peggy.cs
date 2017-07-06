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

        private List<Item> _inventory;
        
        public Peggy()
        {
            _inventory = new List<Item>();
        }

        public string SetCurrentRoom(Room newRoom)
        {
            CurrentRoom = newRoom;
            return $"Moved to the {CurrentRoom.FriendlyName}.\r\n";
        }

        public void AddItemToInventory(Item i)
        {
            _inventory.Add(i);
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

        public string TakeItemFrom(string itemName, string takeFrom)
        {
            // Get the Container or Character who has the item
            ContainerBase matchContainer = CurrentRoom.GetContainerByName(takeFrom);
            if (matchContainer == null)
            {
                // Try the Characters
                matchContainer = CurrentRoom.GetCharacterByName(takeFrom);
                if (matchContainer == null)
                {
                    // No Containers or Characters found
                    return $"You can't take {itemName} from {takeFrom}, {takeFrom} isn't in here.\r\n";
                }
            }

            Item removedItem = matchContainer.RemoveItemByName(itemName);
            if (removedItem != null)
            {
                AddItemToInventory(removedItem);
                return $"{itemName} has been added to your inventory.\r\n";
            }
            return $"{takeFrom} doesn't have {itemName}.\r\n";
        }
    }
}
