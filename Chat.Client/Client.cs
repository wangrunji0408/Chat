using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Client
{
    using Core.Interfaces;
    using Core.Models;

    public class Client
    {
        public long UserId { get; private set; }

        public void SendMessage(string message)
        {
            ServerService.SendMessage(message);
			_logger?.LogInformation($"Send message.");

		}

        public void InformNewMessage(long senderId, string message)
        {
            _logger?.LogInformation($"New message from {senderId}.\n{message}");
            NewMessage?.Invoke(this, new NewMessageContent { SenderId = senderId, Message = message });
        }

        public event EventHandler<NewMessageContent> NewMessage; 

        IServerService _serverService;
        ILoginService _loginService;
        ILogger _logger;

        IServerService ServerService
        {
            get => _serverService ?? throw new NullReferenceException("The client has not login. ServerService is null.");
            set => _serverService = value;
        }

        public Client(long userId, IServiceProvider serviceProvider)
        {
            UserId = userId;
            _loginService = serviceProvider.GetRequiredService<ILoginService>();
            _logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger($"Client {UserId}");
        }

        public bool Login ()
        {
            ServerService = _loginService.Login(UserId);
            return ServerService != null;
        }
    }
}
