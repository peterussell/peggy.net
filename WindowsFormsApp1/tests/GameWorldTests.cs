using Newtonsoft.Json.Linq;
using PeggyTheGameApp.src;
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
    public class GameWorldTests
    {
        private Room _r1;
        private Room _r2;
        private Item _requiredItem;
        private Item _randomItem;

        public GameWorldTests()
        {
            _r1 = new Room("r1", "First Room", "The first room.");
            _r2 = new Room("r2", "Second Room", "The second room.");
            _requiredItem = new Item("required-item", "Required Item", "I am the required item.");
            _randomItem = new Item("random-item", "Random Item", "I am certainly not a required item.");
        }

        [Fact]
        public void TestMovePeggyWithNoRequiredItem()
        {
            Room r1 = _r1;
            Room r2 = _r2;
            r1.AddAdjoiningRoom("north", r2.Id);
            GameWorld gw = new GameWorld();
            gw.AddRoom(r1.Id, r1);
            gw.AddRoom(r2.Id, r2);

            gw.MovePeggyToRoom(r1);
            Assert.Equal(r1.Id, gw.Peggy.CurrentRoom.Id);

            string res = gw.MovePeggy("north");
            Assert.Equal(r2.Id, gw.Peggy.CurrentRoom.Id);
            Assert.Equal($"Moved to the {r2.FriendlyName}.\r\n", res);
        }

        [Fact]
        public void TestMovePeggyToRoomWhichRequiresAnItemAndPeggyHasTheItem()
        {
            Room r1 = _r1;
            Room r2 = _r2;
            Item requiredItem = _requiredItem;
            r1.AddAdjoiningRoom("north", r2.Id, requiredItem.Id);
            GameWorld gw = new GameWorld();
            gw.AddRoom(r1.Id, r1);
            gw.AddRoom(r2.Id, r2);

            gw.Peggy.AddItemToInventory(requiredItem);
            gw.MovePeggyToRoom(r1);

            // If Peggy tries moving a room which requires an item (eg. a key), and
            // Peggy _does_ have that item, 4 things should happen...
            string res = gw.MovePeggy("north");

            // 1. Peggy moves to the new room.
            Assert.Equal(r2.Id, gw.Peggy.CurrentRoom.Id);

            // 2. The item has been 'used', ie. Peggy no longer has the item.
            Assert.True(!gw.Peggy.InventoryHasItems());

            // 3. The the adjoining room is now 'open', ie. no longer requires an item
            //    to move into it.
            Assert.Equal("", r1.GetAdjoiningRoom(Direction.North).RequiresId);

            // 4. MovePeggy() returns a string stating success.
            Assert.Equal($"Moved to the {r2.FriendlyName}.\r\n", res);
        }

        [Fact]
        public void TestMovePeggyToRoomWhichRequiresItemAndPeggyDoesntHaveTheItem()
        {
            Room r1 = _r1;
            Room r2 = _r2;
            Item requiredItem = _requiredItem;
            r1.AddAdjoiningRoom("north", r2.Id, requiredItem.Id);
            GameWorld gw = new GameWorld();
            gw.AddRoom(r1.Id, r1);
            gw.AddRoom(r2.Id, r2);

            gw.Peggy.AddItemToInventory(_randomItem);
            gw.MovePeggyToRoom(r1);

            // If Peggy tries moving a room which requires an item (eg. a key), and
            // Peggy _does not_ have that item, 4 things should happen...
            string res = gw.MovePeggy("north");

            // 1. Peggy stays in the original room.
            Assert.Equal(r1.Id, gw.Peggy.CurrentRoom.Id);

            // 2. Any items in Peggy's inventory are still there.
            Assert.True(gw.Peggy.InventoryHasItems());

            // 3. The adjoining room still has the required item.
            Assert.Equal(requiredItem.Id, r1.GetAdjoiningRoom(Direction.North).RequiresId);

            // 4. MovePeggy() returns a failure message.
            // (TODO: this needs to be updated when we use the requiredItem friendly name).
            Assert.Equal($"The {requiredItem.Id} is required to get into the {r2.FriendlyName}.\r\n", res);
        }
    }
}
