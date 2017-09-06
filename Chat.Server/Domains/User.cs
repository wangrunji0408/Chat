﻿using System;
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
        public string Username { get; private set; }
        public string Password { get; private set; }
        public DateTimeOffset CreateTime { get; private set; } = DateTimeOffset.Now;
        public DateTimeOffset LastLoginTime { get; private set; }
        internal ICollection<UserChatroom> UserChatrooms { get; private set; }
	    internal ICollection<UserRelationship> UserRelationships { get; private set; }

        [NotMapped]
        public IEnumerable<long> ChatroomIds => UserChatrooms.Select(uc => uc.ChatroomId);

	    [NotMapped]
	    public IEnumerable<long> FriendIds =>
		    UserRelationships.Where(r => r.IsFriend).Select(r => r.ToUserId);

        IClientService _clientService;

        internal void SetClientService (IClientService clientService)
        {
			_logger?.LogInformation($"Set client service.");
			if (_clientService != null)
				_logger?.LogWarning($"Already has a connection, it will be reset.");
            _clientService = clientService;
        }

	    private User()
	    {
		    
	    }

	    internal User(string username, string password)
	    {
		    Username = username;
		    Password = password;
	    }

	    internal LoginResponse Login (LoginRequest request)
        {
			if (Password != request.Password)
				return new LoginResponse { Status = LoginResponse.Types.Status.WrongPassword };

			LastLoginTime = DateTimeOffset.Now;

			_logger?.LogInformation($"Login.");
			return new LoginResponse
			{
				Status = LoginResponse.Types.Status.Success
			};
        }

        internal async Task ReceiveNewMessageAsync (ChatMessage message)
        {
            if (_clientService == null)
            {
                _logger.LogWarning("Receive new message. But ClientService is null.");
                return;
            }
            await _clientService.InformNewMessageAsync(message);
        }

        public override string ToString()
        {
            return string.Format("[User: Id={0}, Username={1}, Friends={2}]", Id, Username,
	            FriendIds.ToJsonString());
        }

	    internal PeopleInfo GetPeopleInfo(User target)
	    {
		    return new PeopleInfo
		    {
			    Id = target.Id,
			    Username = target.Username
		    };
	    }

	    internal UserRelationship GetOrAddRelationshipWith (User target)
	    {
		    var relationship = UserRelationships.FirstOrDefault(r => r.ToUserId == target.Id);
		    if (relationship == null)
		    {
			    relationship = new UserRelationship(this, target);
			    UserRelationships.Add(relationship);
			    _logger?.LogInformation($"New relationship with user {target.Id}");
		    }
		    return relationship;
	    }
    }
}
