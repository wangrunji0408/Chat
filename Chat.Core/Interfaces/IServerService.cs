using System;
using Chat.Core.Models;

namespace Chat.Core.Interfaces
{
    public interface IServerService
    {
        void SendMessage(ChatMessage message);
    }
}
