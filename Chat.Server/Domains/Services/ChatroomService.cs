using System;
using System.Threading.Tasks;
using Chat.Server.Domains.Events.User;
using Chat.Server.Domains.Repositories;
using Chat.Server.Infrastructures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Domains.Services
{
    public class ChatroomService
    {
        readonly ILogger _logger;
        readonly ChatroomRepository _chatroomRepo;
        private readonly IEventBus _eventBus;

        public ChatroomService(IServiceProvider provider)
        {
            _logger = provider.GetService<ILoggerFactory>()?
                .CreateLogger<ChatroomService>();
            _chatroomRepo = new ChatroomRepository(provider);
            _eventBus = provider.GetRequiredService<IEventBus>();
        }
    }
}