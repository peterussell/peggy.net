using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeggyTheGameApp.src.GameObjects
{
    public class Character : ContainerBase
    {
        private string _response;

        private const string _nameKey = "name";
        private const string _descriptionKey = "description";
        private const string _responseKey = "response";
        private const string _itemsKey = "items";

        public Character(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public Character(JObject jCharacter)
        {
            Name = (string)jCharacter[_nameKey];
            Description = (string)jCharacter[_descriptionKey];
            _response = (string)jCharacter[_responseKey];

            foreach (JObject jItem in JArray.Parse(jCharacter[_itemsKey].ToString()))
            {
                Item i = new Item(jItem);
                Items.Add(i.Id, i);
            }
        }

        public string TalkTo()
        {
            if (!string.IsNullOrEmpty(_response))
            {
                string[] actions = { "shudders", "laughs", "dances", "winks", "kicks and imaginary can" };
                Random rnd = new Random(DateTime.Now.Millisecond);
                int actionIndex = rnd.Next(0, actions.Length);

                string res = $"{Name} {actions[actionIndex]} and says...\r\n\"{_response}\"\r\n";
                return res;
            }
            return "I have nothing to say to you.\r\n";
        }
    }
}
