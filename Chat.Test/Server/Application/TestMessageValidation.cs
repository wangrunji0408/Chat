using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Models;
using Xunit;

namespace Chat.Test.Server
{
    public class TestMessageValidation: TestServerBase
    {
        [Theory]
        [MemberData(nameof(GetValidMessages))]
        public async Task ValidMessage(ChatMessage message)
        {
            var response = await server.ReceiveNewMessageAsync(message);
            Assert.True(response.Success);
        }
        
        [Theory]
        [MemberData(nameof(GetInvalidMessages))]
        public async Task InvalidMessage(ChatMessage message)
        {
            var response = await server.ReceiveNewMessageAsync(message);
            Assert.False(response.Success);
        }

        public static IEnumerable<object[]> GetInvalidMessages()
            => InvalidMessages.Select(m => new object[] {m});
        
        public static IEnumerable<object[]> GetValidMessages()
            => ValidMessages.Select(m => new object[] {m});
        
        private static readonly IEnumerable<ChatMessage> InvalidMessages = new[]
        {
            new ChatMessage
            {
                ChatroomId = 0, // !
                SenderId = 1,
                Content = new Content {Text = "hello"}
            },
            new ChatMessage
            {
                ChatroomId = 1,
                SenderId = 0, // ! 
                Content = new Content {Text = "hello"}
            },
            new ChatMessage
            {
                ChatroomId = 1,
                SenderId = 0,
                Content = new Content() // ! No content.
            },
            new ChatMessage
            {
                ChatroomId = 1,
                SenderId = 1,
                Content = new Content // !
                {
                    PeopleEnter = new Content.Types.PeopleEnter {PeopleId = 1}
                }
            },
            new ChatMessage
            {
                ChatroomId = 1,
                SenderId = 1,
                Content = new Content // !
                {
                    PeopleLeave = new Content.Types.PeopleLeave {PeopleId = 2}
                }
            }
        };

        private static readonly IEnumerable<ChatMessage> ValidMessages = new[]
        {
            new ChatMessage
            {
                ChatroomId = 1,
                SenderId = 1,
                Content = new Content {Text = "hello"}
            }
        };
    }
}