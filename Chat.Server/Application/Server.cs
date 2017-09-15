using System;
using Chat.Server.Domains.Events;
using Chat.Server.Domains.Repositories;
using Chat.Server.Domains.Services;
using Chat.Server.Infrastructure.EntityFramework;
using Chat.Server.Infrastructure.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Chat.Server
{
    public partial class Server
    {
        readonly ILogger _logger;

        readonly IUserRepository _userRepo;
        readonly IChatroomRepository _chatroomRepo;
        readonly IMessageRepository _messageRepo;

        readonly IdentityService _identityService;
        readonly ChatroomService _chatroomService;
        readonly MessageService _messageService;
        private readonly UserClientService _userClientService;

        readonly IServiceProvider _provider;
        readonly IEventBus _eventBus;

        public Server(IServiceProvider provider)
        {
            _provider = provider;
            _logger = provider.GetService<ILoggerFactory>()?.CreateLogger("Chat.Server");
            
            _eventBus = provider.GetRequiredService<IEventBus>();
            
            var eventLogger = provider.GetService<ILoggerFactory>()?.CreateLogger("Chat.Server.Events");
            _eventBus.GetEventStream<DomainEvent>().Subscribe(e => eventLogger.LogInformation(
                e.GetType().Name + " " + JsonConvert.SerializeObject(e)));
            
            _userRepo = provider.GetRequiredService<IUserRepository>();
            _chatroomRepo = provider.GetRequiredService<IChatroomRepository>();
            _messageRepo = provider.GetRequiredService<IMessageRepository>();

            _identityService = provider.GetRequiredService<IdentityService>();
            _chatroomService = provider.GetRequiredService<ChatroomService>();
            _messageService = provider.GetRequiredService<MessageService>();
            _userClientService = provider.GetRequiredService<UserClientService>();
            
            _chatroomService.EnsureGlobalChatroomCreated();
        }
    }
}
