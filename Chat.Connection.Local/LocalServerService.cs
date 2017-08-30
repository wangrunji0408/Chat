using System;
using System.Collections.Generic;

namespace Chat.Connection.Local
{
    using System.Threading.Tasks;
    using Core.Interfaces;
    using Core.Models;
    using Server;

    class LocalServerService : IServerService
    {
        readonly long _userId;
        readonly Server _server;
        internal LocalServerService(Server server, long userId)
        {
            _server = server;
            _userId = userId;
        }

        public IAsyncEnumerable<ChatMessage> GetMessageAfter(DateTimeOffset time)
        {
            throw new NotImplementedException();
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            await _server.SendMessageAsync(message);
        }
    }
}
