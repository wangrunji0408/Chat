using System;
namespace Chat.Core.Interfaces
{
    public interface IClientService
    {
        void NewMessage(long senderId, string message);
    }
}
