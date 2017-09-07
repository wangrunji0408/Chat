using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Interfaces;
using Chat.Server.Domains.Events.Chatroom;
using Chat.Server.Domains.Repositories;
using Chat.Server.Infrastructure.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Domains.Services
{
    class UserClientService
    {
        private Dictionary<long, IClientService> dict
            = new Dictionary<long, IClientService>();

        private readonly ILogger _logger;
        private readonly IEventBus _eventBus;
        private readonly IChatroomRepository _chatroomRepo;

        public UserClientService(IServiceProvider provider)
        {
            _logger = provider.GetService<ILoggerFactory>()?
                .CreateLogger<UserClientService>();
            _chatroomRepo = provider.GetRequiredService<IChatroomRepository>();
            _eventBus = provider.GetRequiredService<IEventBus>();
            
            _eventBus.GetEventStream<NewMessageEvent>()
                .Subscribe(async e => await ForwardMessage(e));
        }
        
        internal IClientService this[long userId]
        {
            get => dict.ContainsKey(userId) ? dict[userId] : null;
            set
            {
                if (dict.ContainsKey(userId))
                {
                    _logger?.LogWarning($"User {userId} already has a connection, it will be reset.");
                    dict[userId] = value;
                }
                else
                    dict.Add(userId, value);
                _logger?.LogInformation($"User {userId} set client service.");
            }
        }
        
        async Task ForwardMessage(NewMessageEvent e)
        {
            var chatroom = await _chatroomRepo.GetByIdAsync(e.ChatroomId);
            await Task.WhenAll(chatroom.UserIds.Select(async id =>
            {
                var client = this[id];
                if(client == null)
                    _logger.LogInformation($"User {id} is offline.");
                else
                    await client.InformNewMessageAsync(e.Message);                
            }));
        }
    }
}