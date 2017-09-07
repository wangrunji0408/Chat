using System;
using Chat.Core.Models;
using Chat.Server.Infrastructure.EntityFramework;

namespace Chat.Server.Domains.Repositories
{
    public class MessageRepository: EFRepository<ChatMessage>
	{
		public MessageRepository(IServiceProvider provider)
            : base(provider)
        {
		}
    }
}
