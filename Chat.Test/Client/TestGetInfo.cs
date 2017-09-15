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
        public async Task GetMessageAfter()
        {
            var room = client1.GetChatroom(GlobalChatroomId);
            await room.SendTextAsync("Message1");
            await Task.Delay(100);
            var t0 = DateTimeOffset.Now;
            await room.SendTextAsync("Message2");
            await room.SendTextAsync("Message3");
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