using System;
using System.Collections.Generic;

namespace Chat.Connection.Local
{
    using Core.Interfaces;
    using Core.Models;
    using Client;
    using System.Threading.Tasks;

    class LocalClientService : IClientService
    {
        readonly Client _client;
        public LocalClientService (Client client)
        {
            _client = client;
        }

        public async Task NewMessageAsync(ChatMessage message)
        {
            await Task.CompletedTask;
            _client.InformNewMessage(message);
        }
    }
}
