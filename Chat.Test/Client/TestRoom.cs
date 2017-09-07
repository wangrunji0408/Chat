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

            await client2.SendTextMessage(message, GlobalChatroomId);
            await Task.Delay(100); // wait for server forwarding the message
            Assert.True(recv1);
            Assert.True(recv2);
        }

        [Fact]
        public async Task InformPeopleEntered()
        {
            var room = await server.NewChatroomAsync(new long[] {1});
            var recv = false;
            client1.NewMessage += (sender, e) =>
                recv |= e.SenderId == 0 && e.ChatroomId == 2
                        && e.Content.PeopleEnter.PeopleId == 2;
            await server.AddPeopleToChatroom(room.Id, userId: 2);
            await Task.Delay(100);
            Assert.True(recv);
        }
    }
}