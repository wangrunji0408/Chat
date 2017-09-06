using System;
using System.Threading.Tasks;
using Chat.Core.Models;
using Chat.Server.Domains.Events;
using Chat.Server.Domains.Events.Chatroom;
using Chat.Server.Domains.Repositories;
using Chat.Server.Infrastructures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Domains.Services
{
    class MessageService
    {
        readonly ILogger _logger;
        readonly MessageRepository _messageRepo;

        public MessageService(IServiceProvider provider)
        {
            _logger = provider.GetService<ILoggerFactory>()?
                .CreateLogger<MessageService>();
            _messageRepo = new MessageRepository(provider);

            var eventBus = provider.GetRequiredService<IEventBus>();
            eventBus.GetEventStream<UserEnterChatroomEvent>()
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
        }
    }
}