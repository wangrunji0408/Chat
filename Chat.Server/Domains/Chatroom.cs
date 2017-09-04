﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Domains
{
    public class Chatroom: DomainBase
    {
        public string Name { get; set; }
		public DateTimeOffset CreateTime { get; private set; } = DateTimeOffset.Now;
        internal ICollection<UserChatroom> UserChatrooms { get; set; } = new HashSet<UserChatroom>();

        [NotMapped]
        public IEnumerable<long> UserIds => UserChatrooms.Select(uc => uc.UserId);

        public void NewMessage (ChatMessage message)
        {
			if (message.ChatroomId != Id)
				throw new InvalidOperationException($"Message is not for this Chatroom.");
			if (!UserIds.Contains(message.SenderId))
				throw new InvalidOperationException($"User {message.SenderId} is not in Chatroom {message.ChatroomId}.");
		}

        public void NewPeople (long userId)
        {
            if (UserIds.Contains(userId))
                throw new InvalidOperationException($"User {userId} already exist in chatroom {Id}.");

			var uc = new UserChatroom
            {
                UserId = userId,
                ChatroomId = Id
            };
            UserChatrooms.Add(uc);

            _logger?.LogInformation($"User {userId} entered.");
        }

        public override string ToString()
        {
            return string.Format("[Chatroom: Id={0}, Name={1}, CreateTime={2}, \n\tUserIds={3}]", 
                                 Id, Name, CreateTime, 
                                 string.Join(",", UserIds.Select(i => i.ToString()) ));
        }
    }
}
