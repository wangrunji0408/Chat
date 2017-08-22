using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server
{
    using Core.Interfaces;

    public class Server
    {
        Dictionary<long, IClientService> clients = new Dictionary<long, IClientService>();
        readonly ILogger _logger;

        public Server(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("Server");
        }

        public void UserLogin (long userId, IClientService client)
        {
            _logger?.LogInformation($"User {userId} login.");
            clients.Add(userId, client);
        }

        public void SendMessage(long userId, string message)
        {
            _logger?.LogTrace($"New message from user {userId}.");
			foreach (var pair in clients)
                pair.Value.NewMessage(userId, message);
        }
    }
}
