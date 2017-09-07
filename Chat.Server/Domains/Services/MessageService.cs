using System;
using System.Threading.Tasks;
using Chat.Core.Models;
using Chat.Server.Domains.Events;
using Chat.Server.Domains.Events.Chatroom;
using Chat.Server.Domains.Repositories;
using Chat.Server.Infrastructure.EntityFramework;
using Chat.Server.Infrastructure.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Domains.Services
{
    class MessageService
    {
        readonly ILogger _logger;
        readonly IMessageRepository _messageRepo;
        private readonly IEventBus _eventBus;

        public MessageService(IServiceProvider provider)
        {
            _logger = provider.GetService<ILoggerFactory>()?
                .CreateLogger<MessageService>();
            _messageRepo = provider.GetRequiredService<IMessageRepository>();

            _eventBus = provider.GetRequiredService<IEventBus>();
            _eventBus.GetEventStream<UserEnterChatroomEvent>()
                .Subscribe(async e => await OnUserEnterChatroomEvent(e));
        }

        async Task OnUserEnterChatroomEvent(UserEnterChatroomEvent @event)
        {
            var cm = new ChatMessage
            {
                ChatroomId = @event.ChatroomId,
                SenderId = 0,
                TimeUnix = @event.Time.ToUnixTimeSeconds(),
                Content = new Content
                {
                    PeopleEnter = new Content.Types.PeopleEnter { PeopleId = @event.UserId }
                }
            };
            _messageRepo.Add(cm);
            await _messageRepo.SaveChangesAsync();
            
            _eventBus.Publish(new NewMessageEvent(cm));
        }
    }
}