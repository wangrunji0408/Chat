using System;
using Chat.Server.Domains.Entities;
using Chat.Server.Domains.Events;
using Chat.Server.Domains.Repositories;
using Chat.Server.Domains.Services;
using Chat.Server.Infrastructures;
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
        readonly MessageService _messageService;

        readonly IServiceProvider _provider;
        readonly IEventBus _eventBus;

        public Server(IServiceProvider serviceProvider)
        {
            _provider = serviceProvider;
            _logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("Chat.Server");
            _context = serviceProvider.GetRequiredService<ServerDbContext>();
            _context.Database.EnsureCreated();
            try {_context.Database.Migrate();}
            catch(Exception e)
            {
                _logger?.LogError(e, "An error occurred when database migrate. It's normal when using InMemory database.");
            }
            _eventBus = serviceProvider.GetRequiredService<IEventBus>();
            var eventLogger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("Chat.Server.Events");
            _eventBus.GetEventStream<IDomainEvent>().Subscribe(e => eventLogger.LogInformation(e.ToString()));
            
            _userRepo = new UserRepository(serviceProvider);
            _chatroomRepo = new ChatroomRepository(serviceProvider);
            _messageRepo = new MessageRepository(serviceProvider);

            _userService = new UserService(serviceProvider);
            _messageService = new MessageService(serviceProvider);
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
