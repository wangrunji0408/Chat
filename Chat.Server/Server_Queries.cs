using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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
		public Task<List<ChatMessage>> GetMessages(GetMessagesRequest request)
		{
			return _context.Messages
				.Where(m => m.TimeUnix >= request.AfterTimeUnix)
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

		public Task<List<ChatMessage>> GetRecentMessages(long chatroomId, int count)
		{
            return _messageRepo.Query()
						   .Where(m => m.ChatroomId == chatroomId)
						   .OrderByDescending(m => m.TimeUnix)
						   .Take(count)
						   .ToListAsync();
		}
	}
}
