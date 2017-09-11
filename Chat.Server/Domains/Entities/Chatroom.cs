using System;
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
        public long CreatorId { get; private set; }
        public bool IsP2P => P2PUser1Id != 0 && P2PUser2Id != 0;
        internal long P2PUser1Id { get; set; }
        internal long P2PUser2Id { get; set; }
        public bool IsActive { get; set; } = true;
        internal ICollection<UserChatroom> UserChatrooms { get; set; } = new HashSet<UserChatroom>();

        [NotMapped]
        public IEnumerable<long> UserIds => UserChatrooms.Select(uc => uc.UserId);

        private Chatroom()
        {
        }

        internal Chatroom(string name, long creatorId)
        {
            Name = name;
            CreatorId = creatorId;
        }

        internal void NewMessage(ChatMessage message)
        {
            EnsureActive();
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

        internal void AddPeople(long userId, long operatorId)
        {
            EnsureActive();
            EnsureAdmin(operatorId);
            if (UserIds.Contains(userId))
                throw new InvalidOperationException($"User {userId} already exist in chatroom {Id}.");
            if(IsP2P)
                throw new InvalidOperationException($"Can't add people to P2P chatroom.");

            var uc = new UserChatroom
            {
                UserId = userId,
                ChatroomId = Id
            };
            UserChatrooms.Add(uc);
            
            _provider.GetRequiredService<IEventBus>().Publish(
                new UserEnteredChatroomEvent(Id, userId));
        }

        internal bool Contains(User user)
        {
            return UserChatrooms.Any(uc => uc.UserId == user.Id);
        }

        public override string ToString()
        {
            return string.Format("[Chatroom: Id={0}, Name={1}, CreateTime={2}, P2P={3}, UserIds={4}]",
                Id, Name, CreateTime, IsP2P, UserIds.ToJsonString());
        }

        public void RemovePeople(long userId, long operatorId)
        {
            EnsureActive();
            EnsureAdmin(operatorId);
            if(IsP2P)
                throw new InvalidOperationException($"Can't remove people from P2P chatroom.");
            var uc = UserChatrooms.FirstOrDefault(r => r.UserId == userId);
            if (uc == null)
                throw new InvalidOperationException($"User {userId} doesn't exist in chatroom {Id}.");
            UserChatrooms.Remove(uc);
            
            _provider.GetRequiredService<IEventBus>().Publish(
                new UserLeftChatroomEvent(Id, userId));
        }

        public void Quit(long userId)
        {
            EnsureActive();
            if(IsP2P)
                throw new InvalidOperationException($"Can't quit from P2P chatroom.");
            var uc = UserChatrooms.FirstOrDefault(r => r.UserId == userId);
            if (uc == null)
                throw new InvalidOperationException($"User {userId} doesn't exist in chatroom {Id}.");
            UserChatrooms.Remove(uc);

            _provider.GetRequiredService<IEventBus>().Publish(
                new UserLeftChatroomEvent(Id, userId));
        }

        void EnsureActive()
        {
            if(!IsActive)
                throw new InvalidOperationException($"Chatroom {Id} is not active.");
        }

        void EnsureAdmin(long userId)
        {
            if(userId != 0)
                throw new InvalidOperationException($"Permission denied.");
        }

        public void DismissBy(long userId)
        {
            EnsureActive();
            EnsureAdmin(userId);
            IsActive = false;
            _provider.GetRequiredService<IEventBus>().Publish(
                new DismissedEvent(Id, userId));
        }
    }
}