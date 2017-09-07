using System;
using Chat.Server.Domains.Entities;
using Chat.Server.Domains.Events;
using Chat.Server.Domains.Repositories;
using Chat.Server.Domains.Services;
using Chat.Server.Infrastructure.EntityFramework;
using Chat.Server.Infrastructure.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Chat.Server
{
    public partial class Server
    {
        readonly ILogger _logger;

        readonly ServerDbContext _context;
        readonly UserRepository _userRepo;
        readonly ChatroomRepository _chatroomRepo;
        readonly MessageRepository _messageRepo;

        readonly UserService _userService;
        readonly ChatroomService _chatroomService;
        readonly MessageService _messageService;

        readonly IServiceProvider _provider;
        readonly IEventBus _eventBus;

        public Server(IServiceProvider provider)
        {
            _provider = provider;
            _logger = provider.GetService<ILoggerFactory>()?.CreateLogger("Chat.Server");
            _context = provider.GetRequiredService<ServerDbContext>();
            _context.Database.EnsureCreated();
            try {_context.Database.Migrate();}
            catch(Exception e)
            {
                _logger?.LogError(e, "An error occurred when database migrate. It's normal when using InMemory database.");
            }
            _eventBus = provider.GetRequiredService<IEventBus>();
            var eventLogger = provider.GetService<ILoggerFactory>()?.CreateLogger("Chat.Server.Events");
            _eventBus.GetEventStream<DomainEvent>().Subscribe(e => eventLogger.LogInformation(e.ToString()));
            
            _userRepo = new UserRepository(provider);
            _chatroomRepo = new ChatroomRepository(provider);
            _messageRepo = new MessageRepository(provider);

            _userService = new UserService(provider);
            _chatroomService = new ChatroomService(provider);
            _messageService = new MessageService(provider);
            EnsureGlobalChatroomCreated();
        }

        /// <summary>
        /// 确保创建1号全局聊天室
        /// </summary>
        void EnsureGlobalChatroomCreated ()
        {
            if (_chatroomRepo.ContainsIdAsync(1).Result)
                return;
            var chatroom = new Chatroom("Global Chatroom");

            _chatroomRepo.Add(chatroom);
            _chatroomRepo.SaveChanges();
        }
    }
}
