using System.Threading.Tasks;
using Chat.Core.Models;
using Xunit;

namespace Chat.Test.Client
{
    public abstract class TestRoom<TSetup> : TestClientBase<TSetup>
        where TSetup : SetupBase, new()
    {
        public TestRoom()
        {
            server.MakeFriendsAsync(1, 2).Wait();
        }
        
        [Fact]
        public async Task CreateRoom()
        {
            var room = await client1.GetChatroom(0).NewChatroom(new long[] {1, 2}, "Room");
            var info = await room.GetInfoAsync();
            Assert.Equal(room.RoomId, info.Id);
            Assert.Equal("Room", info.Name);
            Assert.Equal(new long[] {1,2}, info.PeopleIds);
        }
        
        [Fact]
        public async Task SendMessage()
        {
            var recv1 = false;
            var recv2 = false;
            var message = "Hello";
            client1.NewMessage += (sender, e) => recv1 |= e.SenderId == 2 && e.Content.Text == message;
            client2.NewMessage += (sender, e) => recv2 |= e.SenderId == 2 && e.Content.Text == message;

            await client2.GetChatroom(GlobalChatroomId).SendTextAsync(message);
            await Task.Delay(100); // wait for server forwarding the message
            Assert.True(recv1);
            Assert.True(recv2);
        }

        [Fact]
        public async Task AddPeople()
        {
            var room = await client1.GetChatroom(0).NewChatroom(new long[] {1}, "Room");
            var recv = false;
            client2.NewMessage += (sender, e) =>
                recv |= e.SenderId == 0 && e.ChatroomId == room.RoomId
                        && e.Content.PeopleEnter.PeopleId == 2;
            await room.AddPeoplesAsync(new long[] {2});
            Assert.True(recv);
            var info = await room.GetInfoAsync();
            Assert.Contains(2, info.PeopleIds);
        }
        
        [Fact]
        public async Task RemovePeople()
        {
            var room = await client1.GetChatroom(0).NewChatroom(new long[] {1, 2}, "Room");
            var recv = false;
            client1.NewMessage += (sender, e) =>
                recv |= e.SenderId == 0 && e.ChatroomId == room.RoomId
                        && e.Content.PeopleLeave.PeopleId == 2;
            await room.RemovePeoplesAsync(new long[] {2});
            Assert.True(recv);
            var info = await room.GetInfoAsync();
            Assert.DoesNotContain(2, info.PeopleIds);
        }
        
        [Fact]
        public async Task Quit()
        {
            var room = await client1.GetChatroom(0).NewChatroom(new long[] {1, 2}, "Room");
            var recv = false;
            client1.NewMessage += (sender, e) =>
                recv |= e.SenderId == 0 && e.ChatroomId == room.RoomId
                        && e.Content.PeopleLeave.PeopleId == 2;
            await client2.GetChatroom(room.RoomId).QuitAsync();
            Assert.True(recv);
            var info = await room.GetInfoAsync();
            Assert.DoesNotContain(2, info.PeopleIds);
        }
        
        [Fact]
        public async Task ChangeName()
        {
            const string newName = "NewValue";
            var room = await client1.GetChatroom(0).NewChatroom(new long[] {1, 2}, "Room");
            var recv = false;
            client1.NewMessage += (sender, e) =>
                recv |= e.SenderId == 0 && e.ChatroomId == room.RoomId
                        && e.Content.SetPeoperty.Key == "Name"
                        && e.Content.SetPeoperty.Value == newName;
            
            await room.ChangeNameAsync(newName);
            Assert.True(recv);
            
            var info = await room.GetInfoAsync();
            Assert.Equal(newName, info.Name);
        }
    }
}