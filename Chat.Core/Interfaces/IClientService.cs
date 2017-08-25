using System;
using System.Threading.Tasks;

namespace Chat.Core.Interfaces
{
    using Models;

    public interface IClientService
    {
        Task NewMessageAsync(ChatMessage message);
    }
}
