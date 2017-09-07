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
            _eventBus.GetEventStream<UserEnteredChatroomEvent>()
                .Subscribe(OnUserEnterChatroomEvent);
            _eventBus.GetEventStream<UserLeftChatroomEvent>()
                .Subscribe(OnUserLeftChatroomEvent);
            
            _eventBus.GetEventStream<NewMessageEvent>()
                .Subscribe(async e => await StoreMessage(e));
        }

        internal static bool IsValid(ChatMessage m)
        {
            if (m.SenderId != 0)
            {
                switch (m.Content.ContentCase)
                {
                    case Content.ContentOneofCase.PeopleEnter:
                        return false;
                    case Content.ContentOneofCase.PeopleLeave:
                        return false;
                }
            }
            return true;
        }

        async Task StoreMessage(NewMessageEvent e)
        {
            _messageRepo.Add(e.Message);
            await _messageRepo.SaveChangesAsync();
        }

        void OnUserEnterChatroomEvent(UserEnteredChatroomEvent @event)
        {
            var cm = new ChatMessage
            {
                ChatroomId = @event.ChatroomId,
                SenderId = 0,
                TimeUnixMs = @event.Time.ToUnixTimeMilliseconds(),
                Content = new Content
                {
                    PeopleEnter = new Content.Types.PeopleEnter { PeopleId = @event.UserId }
                }
            };
            _eventBus.Publish(new NewMessageEvent(cm));
        }
        
        void OnUserLeftChatroomEvent(UserLeftChatroomEvent @event)
        {
            var cm = new ChatMessage
            {
                ChatroomId = @event.ChatroomId,
                SenderId = 0,
                TimeUnixMs = @event.Time.ToUnixTimeMilliseconds(),
                Content = new Content
                {
                    PeopleLeave = new Content.Types.PeopleLeave { PeopleId = @event.UserId }
                }
            };            
            _eventBus.Publish(new NewMessageEvent(cm));
        }
    }
}