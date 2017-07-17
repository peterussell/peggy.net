using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeggyTheGameApp.src.GameObjects
{
    public class Container : ContainerBase
    {
        public string RequiresId { get; private set; }

        private const string _nameKey = "name";
        private const string _descriptionKey = "description";
        private const string _requiresKey = "requires";
        private const string _itemsKey = "items";

        public Container(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public Container(JObject jContainer)
        {
            Name = (string)jContainer[_nameKey];
            Description = (string)jContainer[_descriptionKey];
            RequiresId = (string)jContainer[_requiresKey];

            foreach (JObject jItem in JArray.Parse(jContainer[_itemsKey].ToString()))
            {
                Item i = new Item(jItem);
                Items.Add(i.Id, i);
            }
        }

        public string LookIn()
        {
            if (!HasItems())
            {
                return $"There's nothing in the {Name}.\r\n";
            }

            string res = $"You peer into the {Name} and see...\r\n";
            foreach (Item i in Items.Values)
            {
                res += $" - {i.Name}\r\n";
            }
            return res;
        }

        public void AddRequiredItemById(string requiredItemId)
        {
            RequiresId = requiredItemId;
        }
    }
}
