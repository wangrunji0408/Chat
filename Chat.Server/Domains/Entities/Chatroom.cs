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
        internal ICollection<UserChatroom> UserChatrooms { get; private set; } = new HashSet<UserChatroom>();

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
            if(GetUserChatroom(message.SenderId).IsBlocked)
                throw new InvalidOperationException($"User {message.SenderId} is blocked.");
            if (message.ChatroomId != Id)
                throw new ArgumentException($"Message is not for this Chatroom.");
            if (!UserIds.Contains(message.SenderId))
                throw new ArgumentException(
                    $"User {message.SenderId} is not in Chatroom {message.ChatroomId}.");
            if(!MessageService.IsValid(message))
                throw new ArgumentException("Invalid message.");
            
            message.TimeUnixMs = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            _provider.GetRequiredService<IEventBus>().Publish(
                new NewMessageEvent{ChatroomId = Id, OperatorId = message.SenderId, Message = message});
        }

        internal void AddPeople(long userId, long operatorId)
        {
            EnsureActive();
            EnsureNotP2P();
            EnsureAdmin(operatorId);
            if (UserIds.Contains(userId))
                throw new InvalidOperationException($"User {userId} already exist in chatroom {Id}.");

            var uc = new UserChatroom
            {
                UserId = userId,
                ChatroomId = Id
            };
            UserChatrooms.Add(uc);
            
            _provider.GetRequiredService<IEventBus>().Publish(
                new UserEnteredEvent{ChatroomId = Id, UserId = userId, OperatorId = operatorId});
        }

        internal bool Contains(User user)
        {
            return UserChatrooms.Any(uc => uc.UserId == user.Id);
        }

        public override string ToString()
        {
            return string.Format("[Chatroom: Id={0}, Name={1}, CreateTime={2}, P2P={3}, Users={{ \n\t{4} }}]",
                Id, Name, CreateTime, IsP2P, string.Join(",\n\t", UserChatrooms));
        }

        internal void RemovePeople(long userId, long operatorId)
        {
            EnsureActive();
            EnsureNotP2P();
            EnsureAdmin(operatorId);

            var uc = GetUserChatroom(userId);
            if (uc == null)
                throw new InvalidOperationException($"User {userId} doesn't exist in chatroom {Id}.");
            UserChatrooms.Remove(uc);
            
            _provider.GetRequiredService<IEventBus>().Publish(
                new UserLeftEvent{ChatroomId = Id, UserId = userId, OperatorId = operatorId});
        }

        internal void Quit(long userId)
        {
            EnsureActive();
            EnsureNotP2P();

            var uc = GetUserChatroom(userId);
            if (uc == null)
                throw new InvalidOperationException($"User {userId} doesn't exist in chatroom {Id}.");
            UserChatrooms.Remove(uc);

            _provider.GetRequiredService<IEventBus>().Publish(
                new UserLeftEvent{ChatroomId = Id, UserId = userId, OperatorId = userId});
        }

        void EnsureActive()
        {
            if(!IsActive)
                throw new InvalidOperationException($"Chatroom {Id} is not active.");
        }

        void EnsureAdmin(long userId)
        {
            if(userId != 0 && GetUserChatroom(userId).Role != UserChatroom.UserRole.Admin)
                throw new InvalidOperationException($"Permission denied.");
        }

        void EnsureNotP2P()
        {
            if(IsP2P)
                throw new InvalidOperationException($"Is P2P chatroom.");
        }

        internal UserChatroom GetUserChatroom(long userId)
        {
            return UserChatrooms.FirstOrDefault(uc => uc.UserId == userId);
        }

        internal void DismissBy(long userId)
        {
            EnsureActive();
            EnsureNotP2P();
            EnsureAdmin(userId);
            
            IsActive = false;
            _provider.GetRequiredService<IEventBus>().Publish(
                new DismissedEvent{ChatroomId = Id, OperatorId = userId});
        }

        internal void SetRole(long userId, UserChatroom.UserRole roleEnum, long operatorId)
        {
            EnsureActive();
            EnsureNotP2P();
            EnsureAdmin(operatorId);
            
            GetUserChatroom(userId).Role = roleEnum;
            _provider.GetRequiredService<IEventBus>().Publish(
                new SetRoleEvent{ChatroomId = Id, OperatorId = operatorId, UserId = userId, NewRole = roleEnum.ToString()});
        }

        internal void SetName(string value, long operatorId)
        {
            EnsureActive();
            EnsureNotP2P();
            EnsureAdmin(operatorId);
            
            if (Name == value)
                return;
            Name = value;
            _provider.GetRequiredService<IEventBus>().Publish(
                new PropertyChangedEvent
                {
                    ChatroomId = Id, 
                    OperatorId = operatorId, 
                    Key = "Name", 
                    Value = value
                });
        }
    }
}