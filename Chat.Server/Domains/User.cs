using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Chat.Core.Interfaces;
using Chat.Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Domains
{
    public class User: DomainBase
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTimeOffset CreateTime { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset LastLoginTime { get; set; }
        public ICollection<UserChatroom> UserChatrooms { get; set; }

        IClientService _clientService;
        [NotMapped]
        internal IClientService ClientService
        {
            get => _clientService;
            set
            {
				_logger?.LogInformation($"Set client.");
                if(_clientService != null)
                    _logger?.LogWarning($"Already has a connection, it will be reset.");
				_clientService = value;
            }
        }

        internal async Task NewMessageAsync (ChatMessage message)
        {
            await _clientService?.NewMessageAsync(message);
        }

        internal Task<List<ChatMessage>> GetMessagesAfter (DateTimeOffset time)
        {
            return _context.Messages.Where(m => m.CreateTime >= time).ToListAsync();
        }

        internal Task<List<long>> GetChatrooomIds ()
        {
            return _context.Set<UserChatroom>()
                           .Where(uc => uc.UserId == Id)
                           .Select(uc => uc.ChatroomId)
                           .ToListAsync();
        }

        public override string ToString()
        {
            return string.Format("[User: Id={0}, Username={1}]", Id, Username);
        }
    }
}
