using System.Threading.Tasks;
using Chat.Core.Models;
using Xunit;

namespace Chat.Test.Client
{
    public abstract class TestRoom<TSetup> : TestClientBase<TSetup>
        where TSetup : SetupBase, new()
    {
        [Fact]
        public async Task InformNewMessage()
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
        public async Task InformPeopleEntered()
        {
            var room = await server.GetChatroomApplication(0, 0)
                                   .NewChatroomAsync(new long[] {1});
            var recv = false;
            client1.NewMessage += (sender, e) =>
                recv |= e.SenderId == 0 && e.ChatroomId == room.ChatroomId
                        && e.Content.PeopleEnter.PeopleId == 2;
            await room.AddPeopleAsync(2);
            await Task.Delay(100);
            Assert.True(recv);
        }
        
        [Fact]
        public async Task InformPeopleLeft()
        {
            var room = await server.GetChatroomApplication(0, 0)
                                   .NewChatroomAsync(new long[] {1, 2});
            var recv = false;
            client1.NewMessage += (sender, e) =>
                recv |= e.SenderId == 0 && e.ChatroomId == room.ChatroomId
                        && e.Content.PeopleLeave.PeopleId == 2;
            await room.RemovePeopleAsync(2);
            await Task.Delay(100);
            Assert.True(recv);
        }
    }
}