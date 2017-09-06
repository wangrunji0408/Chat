using System;
using Chat.Core.Models;

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
