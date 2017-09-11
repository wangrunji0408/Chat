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
            await Task.Delay(100);
            var t0 = DateTimeOffset.Now;
            await client1.SendTextMessage("Message2", GlobalChatroomId);
            await client1.SendTextMessage("Message3", GlobalChatroomId);
            var messages = await client1.GetMessages(new GetMessagesRequest
            {
                ChatroomId = GlobalChatroomId,
                AfterTimeUnixMs = t0.ToUnixTimeMilliseconds(),
                Count = 3
            });
            Assert.Equal(2, messages.Count);
            Assert.Contains(messages, m => m.Content.Text == "Message2");
            Assert.Contains(messages, m => m.Content.Text == "Message3");
        }

        [Fact]
        public async Task GetPeopleInfo()
        {
            var info = await client1.GetPeopleInfo(2);
            Assert.Equal(2, info.Id);
            Assert.Equal("user2", info.Username);
        }
    }
}