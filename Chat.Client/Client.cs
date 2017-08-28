using System;
using System.Threading.Tasks;
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

        public Client(IServiceProvider serviceProvider)
        {
            _loginService = serviceProvider.GetRequiredService<ILoginService>();
            _logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger($"Client {UserId}");
        }

        public long UserId { get; internal set; }
        internal string Password { get; set; }

        private IServerService ServerService
        {
            get => _serverService ??
                   throw new NullReferenceException("The client has not login. ServerService is null.");
            set => _serverService = value;
        }

        public async Task SendTextMessage(string text)
        {
			var message = new ChatMessage
            {
                SenderId = UserId,
                Content = new Content {Text = text}
            };
            await ServerService.SendMessageAsync(message);
            _logger?.LogInformation($"Sent message.");
        }

        public void InformNewMessage(ChatMessage message)
        {
            _logger?.LogInformation($"New message from {message.SenderId}.\n{message.Content.Text}");
            NewMessage?.Invoke(this, message);
        }

        public event EventHandler<ChatMessage> NewMessage;

        public async Task<bool> Login()
        {
            var request = new LoginRequest
            {
                UserId = UserId,
                Password = Password
            };
            _serverService = await _loginService.LoginAsync(request);
            return _serverService != null;
        }
    }
}