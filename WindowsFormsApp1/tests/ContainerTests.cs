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
    public class ContainerTests
    {
        [Theory]
        [InlineData("My Container", "This is a test container.")]
        public void TestCreateNewContainer(string name, string desc)
        {
            Container c = new Container(name, desc);
            Assert.Equal(name, c.Name);
            Assert.Equal(desc, c.Description);
            Assert.False(c.HasItems());
        }

        [Theory]
        [InlineData("test-item", "Test Item", "Description of a test item.")]
        public void TestAddItemToContainer(string id, string name, string desc)
        {
            Item i = new Item(id, name, desc);
            Container c = new Container("a", "b");
            c.AddItem(i);
            Assert.True(c.HasItems());
        }

        [Theory]
        [InlineData("test-get-item", "Test Get Item", "Description of a test get item.")]
        public void TestGetItemFromContainer(string id, string name, string desc)
        {
            Item i = new Item(id, name, desc);
            Container c = new Container("a", "b");
            c.AddItem(i);
            Item i2 = c.RemoveItem(id);
            Assert.Equal(i, i2);
            Assert.False(c.HasItems());
        }

        [Fact]
        public void TestLookInContainerWithRequiredItemAndPeggyHasItem()
        {
            Item ri = new Item("required-item", "Required Item", "This item is required.");
            Item ci = new Item("contained-item", "Contained Item", "I am an item in a container.");
            Container c = new Container("container-with-required-item", "Container with Required Item");
            Room r = new Room("room", "Room", "A room.");
            c.AddItem(ci);
            c.AddRequiredItemById(ri.Id);
            r.AddContainer(c);

            Peggy p = new Peggy();
            p.AddItemToInventory(ri);
            p.SetCurrentRoom(r);
            string res = p.LookInContainer(c.Name);

            Assert.Equal("You peer into the container-with-required-item and see...\r\n" +
                         " - Contained Item\r\n",
                         res);
        }

        [Fact]
        public void TestLookInContainerWithRequiredItemAndPeggyDoesNotHaveItem()
        {
            Item ri = new Item("required-item", "Required Item", "This item is required.");
            Item ci = new Item("contained-item", "Contained Item", "I am an item in a container.");
            Container c = new Container("container-with-required-item", "Container with Required Item");
            Room r = new Room("room", "Room", "A room.");
            c.AddItem(ci);
            c.AddRequiredItemById(ri.Id);
            r.AddContainer(c);

            Peggy p = new Peggy();
            p.SetCurrentRoom(r);
            string res = p.LookInContainer(c.Name);

            Assert.Equal($"You need the {ri.Id} to open the {c.Name}.\r\n", res);
        }

        [Fact]
        public void TestLoadContainerFromJson()
        {
            string json = @"{
                              'name': 'A TV Cabinet',
                              'description': 'A faux-modern oversized TV cabinet covered in suspicious scratches.',
                              'requires': 'rusty-key',
                              'items': [{
                                'id': 'article-3',
                                'name': 'Article - left edge',
                                'description': 'The left edge of the missing article.',
                                'requires': 'shagpile-wonder'
                              }]
                            }";
            Container c = new Container(JObject.Parse(json));
            Assert.Equal("A TV Cabinet", c.Name);
            Assert.Equal("A faux-modern oversized TV cabinet covered in suspicious scratches.", c.Description);
            Assert.Equal("rusty-key", c.RequiresId);
            Assert.True(c.HasItems());

            Item i = c.RemoveItem("article-3");
            Assert.NotNull(i);
            Assert.Equal("article-3", i.Id);
            Assert.Equal("Article - left edge", i.Name);
            Assert.Equal("The left edge of the missing article.", i.Description);
            Assert.Equal("shagpile-wonder", i.RequiresId);
        }
    }
}
