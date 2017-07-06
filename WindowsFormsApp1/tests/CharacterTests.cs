using Newtonsoft.Json.Linq;
using PeggyTheGameApp.src.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PeggyTheGameApp.tests
{
    public class CharacterTests
    {
        [Theory]
        [InlineData("My Container", "This is a test container.")]
        public void TestCreateNewCharacter(string name, string desc)
        {
            Character c = new Character(name, desc);
            Assert.Equal(name, c.Name);
            Assert.Equal(desc, c.Description);
            Assert.False(c.HasItems());
        }

        [Theory]
        [InlineData("test-item", "Test Item", "Description of a test item.")]
        public void TestAddItemToCharacter(string id, string name, string desc)
        {
            Item i = new Item(id, name, desc);
            Character c = new Character("a", "b");
            c.AddItem(i);
            Assert.True(c.HasItems());
        }

        [Theory]
        [InlineData("test-get-item", "Test Get Item", "Description of a test get item.")]
        public void TestGetItemFromCharacter(string id, string name, string desc)
        {
            Item i = new Item(id, name, desc);
            Character c = new Character("a", "b");
            c.AddItem(i);
            Item i2 = c.GetItem(id);
            Assert.Equal(i, i2);
            Assert.False(c.HasItems());
        }

        [Fact]
        public void TestLoadCharacterFromJson()
        {
            string json = @"{
                              'name': 'Archibald',
                              'description': 'A hulking giant of a woman with frazzled hair.',
                              'response': 'You look like you could do with some help, here - have a key.',
                              'items': [{
                                'id': 'article-3',
                                'name': 'Article - left edge',
                                'description': 'The left edge of the missing article.',
                                'requires': 'shagpile-wonder'
                              }]
                            }";
            Character c = new Character(JObject.Parse(json));
            Assert.Equal("Archibald", c.Name);
            Assert.Equal("A hulking giant of a woman with frazzled hair.", c.Description);
            Assert.Equal("You look like you could do with some help, here - have a key.", c.TalkTo());
            Assert.True(c.HasItems());

            Item i = c.GetItem("article-3");
            Assert.NotNull(i);
            Assert.Equal("article-3", i.Id);
            Assert.Equal("Article - left edge", i.Name);
            Assert.Equal("The left edge of the missing article.", i.Description);
            Assert.Equal("shagpile-wonder", i.RequiresId);
        }
    }
}
