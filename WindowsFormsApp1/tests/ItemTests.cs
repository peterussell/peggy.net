using Newtonsoft.Json.Linq;
using PeggyTheGameApp.src.GameObjects;
using PeggyTheGameApp.src.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PeggyTheGameApp.tests
{
    public class ItemTests
    {
        [Fact]
        public void TestLoadItemFromJson()
        {
            string json = @"{ 
                              'id': 'article-3',
                              'name': 'Article - left edge',
                              'description': 'The left edge of the missing article.',
                              'requires': 'glittery-key'
                            }";
            Item i = new Item(JObject.Parse(json));
            Assert.Equal("article-3", i.Id);
            Assert.Equal("Article - left edge", i.Name);
            Assert.Equal("The left edge of the missing article.", i.Description);
            Assert.Equal("glittery-key", i.RequiresId);
        }
    }
}