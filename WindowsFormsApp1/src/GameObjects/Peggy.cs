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
            CurrentRoom = newRoom ?? throw new ArgumentNullException("newRoom");
            return $"Moved to the {CurrentRoom.FriendlyName}.\r\n";
        }

        public void AddItemToInventory(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            _inventory.Add(item);
        }

        public Item RemoveItemFromInventoryById(string itemId)
        {
            Item match = _inventory.Find(i => i.Id.ToLower().Equals(itemId.ToLower()));
            if (match != null)
            {
                _inventory.Remove(match);
            }
            return match;
        }

        public void RemoveItemFromInventory(Item i)
        {
            _inventory.Remove(i);
        }

        public bool InventoryHasItems()
        {
            if (_inventory == null || _inventory.Count == 0)
            {
                return false;
            }
            return true;
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

        public string DropItemIn(string itemName, string dropIn)
        {
            // Check we have the item in our Inventory
            Item matchItem = _inventory.Find(i => i.Name.ToLower().Equals(itemName.ToLower()));
            if (matchItem == null)
            {
                return $"There's nothing in your inventory called {itemName}.\r\n";
            }

            // Check the room has a Container with the right name
            Container matchContainer = CurrentRoom.GetContainerByName(dropIn);
            if (matchContainer == null)
            {
                return $"You can't drop {itemName} in {dropIn}, {dropIn} isn't in here.\r\n";
            }

            RemoveItemFromInventory(matchItem);
            matchContainer.AddItem(matchItem);
            return $"Removed {itemName} from your inventory.\r\n";
        }

        public string GiveItemTo(string itemName, string giveTo)
        {
            // TODO: suspiciously similar to DropItemIn() - needs a refactor.

            // Check we have the item in our Inventory
            Item matchItem = _inventory.Find(i => i.Name.ToLower().Equals(itemName.ToLower()));
            if (matchItem == null)
            {
                return $"There's nothing in your inventory called {itemName}.\r\n";
            }

            // Check the room has a Container with the right name
            Character matchContainer = CurrentRoom.GetCharacterByName(giveTo);
            if (matchContainer == null)
            {
                return $"You can't give {itemName} to {giveTo}, {giveTo} isn't in here.\r\n";
            }

            RemoveItemFromInventory(matchItem);
            matchContainer.AddItem(matchItem);
            return $"Removed {itemName} from your inventory.\r\n";
        }
    }
}
