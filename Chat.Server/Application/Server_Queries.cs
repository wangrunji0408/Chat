﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Chat.Server.Domains.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Chat.Server
{
	using Core.Interfaces;
	using Core.Models;
	using Domains;

	public partial class Server
	{
		public async Task<List<ChatMessage>> GetMessagesAsync(GetMessagesRequest request)
		{
			var chatroom = await _chatroomRepo.FindByIdAsync(request.ChatroomId);
			var user = await _userRepo.FindByIdAsync(request.SenderId);
			if(chatroom == null || user == null || !chatroom.Contains(user) || request.Count <= 0)
				return new List<ChatMessage>();
			
			return await _messageRepo.Query()
				.Where(m => m.TimeUnixMs >= request.AfterTimeUnixMs)
				.Where(m => m.ChatroomId == request.ChatroomId)
				.OrderByDescending(m => m.TimeUnixMs)
				.Take(request.Count)
				.ToListAsync();
		}

        public Task<User> FindUserAsync (long id)
        {
            return _userRepo.FindByIdAsync(id);
        }

        public Task<List<string>> GetChatroomsStringAsync ()
        {
            return _chatroomRepo.GetAllAsync().Select(c => c.ToString()).ToList();
        }

		public async Task<string> GetChatroomStringAsync(long id)
		{
            var room = await _chatroomRepo.GetByIdAsync(id);
            return room?.ToString() ?? "null";
		}

		public Task<List<string>> GetUsersStringAsync()
		{
            return _userRepo.GetAllAsync().Select(c => c.ToString()).ToList();
		}

		public async Task<string> GetUserStringAsync(long id)
		{
            var room = await _userRepo.GetByIdAsync(id);
			return room?.ToString() ?? "null";
		}

		public Task<List<ChatMessage>> GetRecentMessagesAsync(long chatroomId, int count)
		{
            return _messageRepo.Query()
						   .Where(m => m.ChatroomId == chatroomId)
						   .OrderByDescending(m => m.TimeUnixMs)
						   .Take(count)
						   .ToListAsync();
		}

		public async Task<PeopleInfo> GetPeopleInfoAsync(long userId, long targetId)
		{
			var user = await _userRepo.GetByIdAsync(userId);
			var target = await _userRepo.GetByIdAsync(targetId);
			return user.GetPeopleInfo(target);
		}
		
		public async Task<ChatroomInfo> GetChatroomInfoAsync(long userId, long roomId)
		{
			var user = await _userRepo.GetByIdAsync(userId);
			var room = await _chatroomRepo.GetByIdAsync(roomId);
			return user.GetChatroomInfo(room);
		}
	}
}
