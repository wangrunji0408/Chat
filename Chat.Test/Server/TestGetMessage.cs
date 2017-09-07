using System;
using System.Threading.Tasks;
using Chat.Core.Models;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Xunit;

namespace Chat.Test.Server
{
    public class TestGetMessage: TestServerBase
    {
        public TestGetMessage()
        {
            SetupAsync().Wait();
        }

        async Task SetupAsync()
        {
            await server.NewChatroomAsync(new long[] {1});
            await server.ReceiveNewMessageAsync(new ChatMessage
            {
                ChatroomId = 1,
                SenderId = 1,
                Content = new Content{Text = "r1u1"}
            });
            t0 = DateTimeOffset.Now;
            await server.ReceiveNewMessageAsync(new ChatMessage
            {
                ChatroomId = 1,
                SenderId = 2,
                Content = new Content{Text = "r1u2"}
            });
            await server.ReceiveNewMessageAsync(new ChatMessage
            {
                ChatroomId = 2,
                SenderId = 1,
                Content = new Content{Text = "r2u1"}
            });
        }

        private DateTimeOffset t0;
        
        [Fact]
        public async Task Case1()
        {
            var list = await server.GetMessagesAsync(new GetMessagesRequest
            {
                SenderId = 1,
                ChatroomId = 1,
                Count = 3
            });
            Assert.Equal(3, list.Count);
            Assert.Contains(list, m => m.Content.PeopleEnter?.PeopleId == 2);
            Assert.Contains(list, m => m.Content.Text == "r1u1");
            Assert.Contains(list, m => m.Content.Text == "r1u2");
        }
        
        [Fact]
        public async Task NotInRoom()
        {
            var list = await server.GetMessagesAsync(new GetMessagesRequest
            {
                SenderId = 2,
                ChatroomId = 2,
                Count = int.MaxValue
            });
            Assert.Empty(list);
        }
        
        [Fact]
        public async Task AfterTime()
        {
            var list = await server.GetMessagesAsync(new GetMessagesRequest
            {
                SenderId = 1,
                ChatroomId = 1,
                AfterTimeUnixMs = t0.ToUnixTimeMilliseconds(),
                Count = int.MaxValue
            });
            Assert.Equal(1, list.Count);
            Assert.Contains(list, m => m.Content.Text == "r1u2");
        }
    }
}