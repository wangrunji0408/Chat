using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Core.Models;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Domains
{
    public class Chatroom: DomainBase
    {
        public long Id { get; set; }
        public string Name { get; set; }
		public DateTimeOffset CreateTime { get; set; } = DateTimeOffset.Now;
        public ICollection<UserChatroom> UserChatrooms { get; set; }
    }
}
