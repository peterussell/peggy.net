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
    public class RoomTests
    {
        [Theory]
        [InlineData("room-one", "First Room", "This is the first room of the game")]
        public void TestCreateNewRoom(string id, string friendlyName, string desc)
        {
            Room r = new Room(id, friendlyName, desc);
            Assert.Equal(friendlyName, r.FriendlyName);
            Assert.Equal(desc, r.Description);
        }

        [Fact]
        public void TestAddContainerGetsPrintedInLook()
        {
            Room r = new Room("a", "b", "c");
            Container c = new Container("Container 1", "This is a container with no items.");
            r.AddContainer(c);
            Assert.Contains("Container 1", r.Look());
        }

        [Fact]
        public void TestAddCharacterGetsPrintedInLook( )
        {
            Room r = new Room("a", "b", "c");
            Character c = new Character("Bob Jones", "A non-descript person.");
            r.AddCharacter(c);
            Assert.Contains("Bob Jones", r.Look());
        }

        [Theory]
        [InlineData("north", Direction.North, "room1")]
        [InlineData("east", Direction.East, "room2")]
        [InlineData("south", Direction.South, "room3")]
        [InlineData("west", Direction.West, "room4")]
        public void TestAddAdjoiningRoom(string direction, Direction directionEnum, string id)
        {
            Room r = new Room("a", "b", "c");
            r.AddAdjoiningRoom(direction, id);
            AdjoiningRoom ar = r.GetAdjoiningRoom(directionEnum);
            Assert.Equal(id, ar.Id);
        }

        [Fact]
        public void TestGetNonExistantAdjoiningRoom()
        {
            Room r = new Room("a", "b", "c");
            AdjoiningRoom ar = r.GetAdjoiningRoom(Direction.South);
            Assert.Null(ar);
        }

        [Fact]
        public void TestAdjoiningRoomRequiredItem()
        {
            Room r = new Room("a", "b", "c");
            r.AddAdjoiningRoom("west", "room-with-requirement", "key-for-room");
            AdjoiningRoom ar = r.GetAdjoiningRoom(Direction.West);
            Assert.Equal("key-for-room", ar.RequiresId);
        }

        [Fact]
        public void TestAdjoiningRoomNoRequiredItem()
        {
            Room r = new Room("a", "b", "c");
            r.AddAdjoiningRoom("west", "room-with-requirement");
            AdjoiningRoom ar = r.GetAdjoiningRoom(Direction.West);
            Assert.Equal("", ar.RequiresId);
        }

        [Theory]
        [InlineData(@"{ 'is-start-room': true }", true)]
        [InlineData(@"{ 'is-start-room': false }", false)]
        [InlineData(@"{ 'friendly-name': 'Front Porch' }", false)]
        public void TestLoadJsonIsStartRoom(string json, bool expect)
        {
            Room r = new Room("test", JObject.Parse(json));
            Assert.Equal(expect, r.IsStartRoom);
        }

        [Theory]
        [InlineData(@"{ 'friendly-name': 'Test Room' }", "Test Room")]
        [InlineData(@"{ 'friendly-name': '' }", "Room of No Name")]
        public void TestLoadJsonFriendlyName(string json, string expect)
        {
            Room r = new Room("test", JObject.Parse(json));
            Assert.Equal(expect, r.FriendlyName);
        }

        [Theory]
        [InlineData(@"{ 'description': 'I am Philius, descriptor of rooms!' }", "I am Philius, descriptor of rooms!")]
        [InlineData(@"{ 'description': '' }", "")]
        public void TestLoadJsonDescription(string json, string expect)
        {
            Room r = new Room("test", JObject.Parse(json));
            Assert.Equal(expect, r.Description);
        }
    }
}