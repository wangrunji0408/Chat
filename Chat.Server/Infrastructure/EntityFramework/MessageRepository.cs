using System;
using Chat.Core.Models;
using Chat.Server.Domains.Repositories;

namespace Chat.Server.Infrastructure.EntityFramework
{
    public class MessageRepository: EFRepository<ChatMessage>, IMessageRepository
    {
		public MessageRepository(IServiceProvider provider)
            : base(provider)
        {
		}
    }
}
