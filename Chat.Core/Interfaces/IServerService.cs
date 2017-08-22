using System;

namespace Chat.Core.Interfaces
{
    public interface IServerService
    {
        void SendMessage(string message);
    }
}
