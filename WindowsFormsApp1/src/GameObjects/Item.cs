using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeggyTheGameApp.src.GameObjects
{
    public class Item
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string RequiresId { get; private set; }

        private const string _idKey = "id";
        private const string _nameKey = "name";
        private const string _descriptionKey = "description";
        private const string _requiresKey = "requires";

        public Item(string id, string name, string desc, string requires = "")
        {
            Id = id;
            Name = name;
            Description = desc;
            RequiresId = requires;
        }

        public Item(JObject jItem)
        {
            Id = (string)jItem[_idKey];
            Name = (string)jItem[_nameKey];
            Description = (string)jItem[_descriptionKey];
            RequiresId = (string)jItem[_requiresKey];
        }
    }
}
