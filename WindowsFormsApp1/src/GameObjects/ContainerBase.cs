using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeggyTheGameApp.src.GameObjects
{
    public abstract class ContainerBase
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        protected Dictionary<string, Item> Items { get; private set; }

        public ContainerBase()
        {
            Items = new Dictionary<string, Item>();
        }

        public bool HasItems()
        {
            return Items.Count > 0;
        }

        public void AddItem(Item i)
        {
            if (i == null)
            {
                throw new ArgumentNullException("Cannot add a null Item to " + Name);
            }
            Items.Add(i.Id, i);
        }

        public bool HasItem(string id)
        {
            return Items.ContainsKey(id);
        }

        public Item GetItemWithoutRemoving(string name)
        {
            Item matchingItem = null;
            foreach (KeyValuePair<string, Item> kv in Items)
            {
                if (kv.Value.Name.ToLower().Equals(name.ToLower()))
                {
                    matchingItem = kv.Value;
                    return matchingItem;
                }
            }
            return null;
        }

        public Item RemoveItem(string id)
        {
            if (!HasItem(id))
            {
                return null;
            }
            Item retItem = Items[id];
            Items.Remove(id);
            return retItem;
        }

        public Item RemoveItemByName(string name)
        {
            Item matchingItem = GetItemWithoutRemoving(name);
            if (matchingItem != null)
            {
                Items.Remove(matchingItem.Id);
            }
            return matchingItem;
        }

        public bool OwnsRequiredItem(string requiresId)
        {
            return Items.Keys.Contains(requiresId);
        }
    }
}