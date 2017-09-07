﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Chat.Core.Models;
using Chat.Server.Domains.Events;
using Chat.Server.Domains.Events.Chatroom;
using Chat.Server.Infrastructure;
using Chat.Server.Infrastructure.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Domains.Entities
{
    public class Chatroom : DomainBase
    {
        public string Name { get; private set; }
        public DateTimeOffset CreateTime { get; private set; } = DateTimeOffset.Now;
        internal ICollection<UserChatroom> UserChatrooms { get; set; } = new HashSet<UserChatroom>();

        [NotMapped]
        public IEnumerable<long> UserIds => UserChatrooms.Select(uc => uc.UserId);

        private Chatroom()
        {
        }

        internal Chatroom(string name = "")
        {
            Name = name;
        }

        internal void NewMessage(ChatMessage message)
        {
            message.TimeUnix = DateTimeOffset.Now.ToUnixTimeSeconds();
            if (message.ChatroomId != Id)
                throw new InvalidOperationException($"Message is not for this Chatroom.");
            if (!UserIds.Contains(message.SenderId))
                throw new InvalidOperationException(
                    $"User {message.SenderId} is not in Chatroom {message.ChatroomId}.");
            _provider.GetRequiredService<IEventBus>().Publish(
                new NewMessageEvent(message));
        }

        internal void NewPeople(User user)
        {
            if (UserIds.Contains(user.Id))
                throw new InvalidOperationException($"User {user} already exist in chatroom {Id}.");

            var uc = new UserChatroom
            {
                UserId = user.Id,
                ChatroomId = Id
            };
            UserChatrooms.Add(uc);
            
            _provider.GetRequiredService<IEventBus>().Publish(
                new UserEnterChatroomEvent(Id, user.Id));
        }

        internal bool Contains(User user)
        {
            return UserChatrooms.Any(uc => uc.UserId == user.Id);
        }

        public override string ToString()
        {
            return string.Format("[Chatroom: Id={0}, Name={1}, CreateTime={2}, UserIds={3}]",
                Id, Name, CreateTime, UserIds.ToJsonString());
        }
    }
}