using System;
namespace Chat.Core.Interfaces
{
    using Models;

    public interface IClientService
    {
        void NewMessage(ChatMessage message);
    }
}
