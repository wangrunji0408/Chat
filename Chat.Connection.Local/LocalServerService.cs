using System;
using System.Collections.Generic;
using System.Linq;

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


        public IAsyncEnumerable<ChatMessage> GetMessages(GetMessagesRequest request)
        {
            return _server.GetMessages(request)
						  .ToAsyncEnumerable()
						  .SelectMany(list => list.ToAsyncEnumerable());
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            await _server.ReceiveNewMessageAsync(message);
        }
    }
}
