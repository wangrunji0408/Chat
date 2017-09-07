using System;
using System.Threading.Tasks;
using Chat.Core.Models;
using Xunit;

namespace Chat.Test.Client
{
    public abstract class TestGetInfo<TSetup> : TestClientBase<TSetup>
        where TSetup : SetupBase, new()
    {
        [Fact]
        public async Task GetChatroomInfo()
        {
            Assert.Equal(new ChatroomInfo
            {
                Id = GlobalChatroomId,
                Name = "Global Chatroom",
                PeopleIds = {1, 2}
            }, await client1.GetChatroomInfo(GlobalChatroomId));
        }

        [Fact]
        public async Task GetMessageAfter()
        {
            await client1.SendTextMessage("Message1", GlobalChatroomId);
            await Task.Delay(1000);
            var t0 = DateTimeOffset.Now;
            await client1.SendTextMessage("Message2", GlobalChatroomId);
            await client1.SendTextMessage("Message3", GlobalChatroomId);
            var messages = await client1.GetMessages(new GetMessagesRequest
            {
                AfterTimeUnix = t0.ToUnixTimeSeconds()
            });
            Assert.Null(messages.Find(m => m.Content.Text == "Message1"));
            Assert.NotNull(messages.Find(m => m.Content.Text == "Message2"));
            Assert.NotNull(messages.Find(m => m.Content.Text == "Message3"));
        }

        [Fact]
        public async Task GetPeopleInfo()
        {
            Assert.Equal(new PeopleInfo
            {
                Id = 2,
                Username = "user2"
            }, await client1.GetPeopleInfo(2));
        }
    }
}