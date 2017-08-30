using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Chat.Core.Models;

namespace Chat.Core.Interfaces
{
    public interface IServerService
    {
        Task SendMessageAsync(ChatMessage message);
        IAsyncEnumerable<ChatMessage> GetMessageAfter(DateTimeOffset time);
    }
}
