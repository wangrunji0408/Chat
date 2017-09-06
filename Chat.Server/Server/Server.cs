using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Chat.Server
{
    using Core.Interfaces;
    using Core.Models;
    using Domains;
    using Repositories;

    public partial class Server
    {
        readonly ILogger _logger;

        readonly ServerDbContext _context;
        readonly UserRepository _userRepo;
        readonly ChatroomRepository _chatroomRepo;
        readonly MessageRepository _messageRepo;

        readonly UserService _userService;

        public Server(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("Chat.Server");
            _context = serviceProvider.GetRequiredService<ServerDbContext>();
            _context.Database.EnsureCreated();
            try {_context.Database.Migrate();}
            catch(Exception e)
            {
                _logger?.LogError(e, "An error occurred when database migrate. It's normal when using InMemory database.");
            }

            _userRepo = new UserRepository(serviceProvider);
            _chatroomRepo = new ChatroomRepository(serviceProvider);
            _messageRepo = new MessageRepository(serviceProvider);

            _userService = new UserService(serviceProvider);
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
