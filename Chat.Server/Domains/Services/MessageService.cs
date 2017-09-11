using System;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
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
            _eventBus.GetEventStream<ChatroomEvent>()
                .Select(ChatroomEventMapper.Map)
                .Where(m => m?.Content != null)
                .Subscribe(async m => await StoreMessage(m));
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

        async Task StoreMessage(ChatMessage message)
        {
            _messageRepo.Add(message);
            await _messageRepo.SaveChangesAsync();
        }
    }
}