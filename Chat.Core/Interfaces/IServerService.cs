using System;
using System.Threading.Tasks;
using Chat.Core.Models;

namespace Chat.Core.Interfaces
{
    public interface IServerService
    {
        Task SendMessageAsync(ChatMessage message);
    }
}
