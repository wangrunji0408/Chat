using System;
using System.Threading.Tasks;
using Chat.Server.Domains.Entities;
using Chat.Server.Domains.Events.User;
using Chat.Server.Domains.Repositories;
using Chat.Server.Infrastructure.EntityFramework;
using Chat.Server.Infrastructure.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Domains.Services
{
    class ChatroomService
    {
        readonly ILogger _logger;
        readonly IChatroomRepository _chatroomRepo;
        private readonly IEventBus _eventBus;

        public ChatroomService(IServiceProvider provider)
        {
            _logger = provider.GetService<ILoggerFactory>()?
                .CreateLogger<ChatroomService>();
            _chatroomRepo = provider.GetRequiredService<IChatroomRepository>();
            _eventBus = provider.GetRequiredService<IEventBus>();
        }
        
        /// <summary>
        /// 确保创建1号全局聊天室
        /// </summary>
        internal void EnsureGlobalChatroomCreated ()
        {
            if (_chatroomRepo.ContainsIdAsync(1).Result)
                return;
            var chatroom = new Chatroom("Global Chatroom");

            _chatroomRepo.Add(chatroom);
            _chatroomRepo.SaveChanges();
        }
    }
}