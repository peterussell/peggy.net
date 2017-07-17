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

        public Item GetItemFromInventoryWithoutRemoving(string itemId)
        {
            return _inventory.Find(i => i.Id.ToLower().Equals(itemId.ToLower()));
        }

        public Item RemoveItemFromInventoryById(string itemId)
        {
            Item match = GetItemFromInventoryWithoutRemoving(itemId);
            if (match != null)
            {
                RemoveItemFromInventory(match);
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

        public bool InventoryHasItem(string itemId)
        {
            Item match = _inventory.Find(i => i.Id.ToLower().Equals(itemId.ToLower()));
            return match != null;
        }

        public string Look()
        {
            return CurrentRoom.Look();
        }

        public string LookInContainer(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                return "Look in where?\r\n";
            }

            Container container = CurrentRoom.GetContainerByName(containerName);
            if (container == null)
            {
                return $"There's no {containerName} you can look in here.\r\n";
            }

            // If the container requires an item to open/use it, check Peggy has it.
            if (!string.IsNullOrEmpty(container.RequiresId) &&
                !InventoryHasItem(container.RequiresId)) {
                return $"You need the {container.RequiresId} to open the {container.Name}.\r\n";
            }

            return CurrentRoom.LookInContainer(containerName);
        }

        public string LookInInventory()
        {
            if (!InventoryHasItems())
            {
                return "There's nothing in your inventory. Get cracking!";
            }

            string res = "You cautiously lift the corner of your satchel, peek inside, and see...\r\n";
            foreach (Item i in _inventory)
            {
                res += $" - {i.Name}\r\n";
            }
            return res;
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

            Item item = matchContainer.GetItemWithoutRemoving(itemName);
            if (item == null)
            {
                return $"{takeFrom} doesn't have {itemName}.\r\n";
            }

            if (!string.IsNullOrEmpty(item.RequiresId))
            {
                if (!matchContainer.OwnsRequiredItem(item.RequiresId))
                {
                    return $"{matchContainer.Name} needs the {item.RequiresId} in return for the " +
                           $"{item.Name} (try \"give {item.RequiresId} to {matchContainer.Name}\").\r\n";
                }
                else
                {
                    
                }
            }
            Item removedItem = matchContainer.RemoveItemByName(itemName);
            AddItemToInventory(removedItem);
            return $"{itemName} has been added to your inventory.\r\n";
        }

        public string DropItemIn(string itemName, string dropIn)
        {
            if (string.IsNullOrEmpty(itemName))
            {
                throw new ArgumentException("itemName");
            }
            if (string.IsNullOrEmpty(dropIn))
            {
                throw new ArgumentException("dropIn");
            }
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
            if (string.IsNullOrEmpty(itemName))
            {
                throw new ArgumentException("itemName");
            }
            if (string.IsNullOrEmpty(giveTo))
            {
                throw new ArgumentException("dropIn");
            }

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
