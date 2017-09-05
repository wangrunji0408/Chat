using System;
using System.Threading.Tasks;

namespace Chat.Core.Interfaces
{
    using Models;

    public interface IClientService
    {
        Task InformNewMessageAsync(ChatMessage message);
        // TODO Server send disconnection to client.
        //Task Disconnect();
    }
}
