﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Chat.Core.Models;
using Chat.Server.Domains.Events;
using Chat.Server.Domains.Events.Chatroom;
using Chat.Server.Domains.Services;
using Chat.Server.Infrastructure;
using Chat.Server.Infrastructure.EventBus;
using Google.Protobuf.Reflection;
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
            if (message.ChatroomId != Id)
                throw new ArgumentException($"Message is not for this Chatroom.");
            if (!UserIds.Contains(message.SenderId))
                throw new ArgumentException(
                    $"User {message.SenderId} is not in Chatroom {message.ChatroomId}.");
            if(!MessageService.IsValid(message))
                throw new ArgumentException("Invalid message.");
            
            message.TimeUnixMs = DateTimeOffset.Now.ToUnixTimeMilliseconds();
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
                new UserEnteredChatroomEvent(Id, user.Id));
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

        public void RemovePeople(User user)
        {
            var uc = UserChatrooms.FirstOrDefault(r => r.UserId == user.Id);
            if (uc == null)
                throw new InvalidOperationException($"User {user} doesn't exist in chatroom {Id}.");
            UserChatrooms.Remove(uc);
            
            _provider.GetRequiredService<IEventBus>().Publish(
                new UserLeftChatroomEvent(Id, user.Id));
        }
    }
}