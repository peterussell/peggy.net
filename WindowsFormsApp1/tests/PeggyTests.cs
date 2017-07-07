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
    public class PeggyTests
    {
        private Item _goodTestItem;
        private Item _badTestItem;
        private Room _testRoom;
        private Container _testContainer;
        private Character _testCharacter;

        public PeggyTests()
        {
            _goodTestItem = new Item("good-item-id", "A Good Item", "I am a good item.", "good-item-requires-id");
            _badTestItem = new Item("bad-item-id", "A Bad Item", "I am a bad item.", "bad-item-requires-id");
            _testRoom = new Room("test-room", "Test Room", "A test room.");
            _testContainer = new Container("Test Container", "I am a test container.");
            _testCharacter = new Character("Test Character", "I am a test character.");
        }

        [Fact]
        public void TestSetCurrentRoom()
        {
            Room r = new Room("room-id", "Room Name", "I am a room description.");
            Peggy p = new Peggy();

            p.SetCurrentRoom(r);
            Assert.Equal("room-id", p.CurrentRoom.Id);
            Assert.Equal("Room Name", p.CurrentRoom.FriendlyName);
            Assert.Equal("I am a room description.", p.CurrentRoom.Description);
        }

        [Fact]
        public void TestSetCurrentRoomWithNullValueThrowsException()
        {
            Peggy p = new Peggy();
            Assert.Throws<ArgumentNullException>(() => p.SetCurrentRoom(null));
        }

        [Fact]
        public void TestAddItemToAndRemoveItemFromInventory()
        {
            Peggy p = new Peggy();

            p.AddItemToInventory(_goodTestItem);
            Assert.True(p.InventoryHasItems());

            Item removedItem = p.RemoveItemFromInventoryById(_goodTestItem.Id);
            Assert.Equal(_goodTestItem, removedItem);
        }

        [Fact]
        public void TestAddInvalidItemToInventory()
        {
            Peggy p = new Peggy();
            Assert.Throws<ArgumentNullException>(() => p.AddItemToInventory(null));
        }

        [Fact]
        public void TestRemoveInvalidItemFromInventory()
        {
            Peggy p = new Peggy();

            // Add an item to the inventory just to verify it's working.
            p.AddItemToInventory(_goodTestItem);
            Assert.True(p.InventoryHasItems());

            // Now try and remove an invalid item from the inventory.
            p.RemoveItemFromInventory(_badTestItem);
            Assert.True(p.InventoryHasItems());
        }

        [Fact]
        public void TestRemoveInvalidItemFromInventoryByItemId()
        {
            Peggy p = new Peggy();
            p.AddItemToInventory(_goodTestItem);

            Item badItem = p.RemoveItemFromInventoryById("not-in-the-inventory");
            Assert.Null(badItem);
        }

        [Fact]
        public void TestInventoryHasItems()
        {
            Peggy p = new Peggy();
            Assert.False(p.InventoryHasItems());

            p.AddItemToInventory(_goodTestItem);
            Assert.True(p.InventoryHasItems());
        }

        [Fact]
        public void TestLook()
        {
            Room r = _testRoom;
            Container c1 = _testContainer;
            Character c2 = _testCharacter;
            r.AddContainer(c1);
            r.AddCharacter(c2);

            Peggy p = new Peggy();
            p.SetCurrentRoom(r);
            string res = p.Look();

            string expected = "You are in the Test Room, you see...\r\n" +
                               " - Test Container\r\n - Test Character\r\n";
            Assert.Equal(expected, res);
        }
    }
}
