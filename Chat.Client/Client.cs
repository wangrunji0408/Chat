using System;
using Chat.Core.Interfaces;
using Chat.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Client
{
    public class Client
    {
        private readonly ILogger _logger;
        private readonly ILoginService _loginService;

        private IServerService _serverService;

        public Client(long userId, IServiceProvider serviceProvider)
        {
            UserId = userId;
            _loginService = serviceProvider.GetRequiredService<ILoginService>();
            _logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger($"Client {UserId}");
        }

        public long UserId { get; }

        private IServerService ServerService
        {
            get => _serverService ??
                   throw new NullReferenceException("The client has not login. ServerService is null.");
            set => _serverService = value;
        }

        public void SendTextMessage(string text)
        {
            var message = new ChatMessage
            {
                SenderId = UserId,
                Content = new Content {Text = text}
            };
            ServerService.SendMessage(message);
            _logger?.LogInformation($"Send message.");
        }

        public void InformNewMessage(ChatMessage message)
        {
            _logger?.LogInformation($"New message from {message.SenderId}.\n{message.Content.Text}");
            NewMessage?.Invoke(this, message);
        }

        public event EventHandler<ChatMessage> NewMessage;

        public bool Login(string password)
        {
            var request = new LoginRequest
            {
                UserId = (int) UserId,
                Password = password
            };
            _serverService = _loginService.Login(request);
            return _serverService != null;
        }
    }
}