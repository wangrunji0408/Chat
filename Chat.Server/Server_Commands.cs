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
		public Task<LoginResponse> LoginAsync(LoginRequest request)
		{
			return _userService.LoginAsync(request);
		}

		public Task<SignupResponse> SignupAsync(SignupRequest request)
		{
			return _userService.SignupAsync(request);
		}

		public async Task<Chatroom> NewChatroomAsync(IEnumerable<long> peopleIds)
		{
			var peoples = peopleIds.Select(id => _userRepo.GetByIdAsync(id).Result);
			var chatroom = new Chatroom();
			foreach (var user in peoples)
				chatroom.NewPeople(user);
            _chatroomRepo.Add(chatroom);
            await _chatroomRepo.SaveChangesAsync();
			return chatroom;
		}

		public async Task AddPeopleToChatroom(long chatroomId, long userId)
		{
			var chatroom = await _chatroomRepo.GetByIdAsync(chatroomId);
			var people = await _userRepo.GetByIdAsync(userId);
			chatroom.NewPeople(people);
			await _chatroomRepo.SaveChangesAsync();
		}

		public void ClearDatabase()
		{
			_context.Database.EnsureDeleted();
			_context.Database.EnsureCreated();
			_context.Database.Migrate();
		}

		public void SetUserClient(long userId, IClientService client)
		{
            _userRepo.GetByIdAsync(userId).Result
                     .SetClientService(client);
		}

		/// <summary>
		/// 收到一条新消息
		/// 保存消息，通知相应聊天室，再通知聊天室的所有用户
		/// </summary>
		public async Task ReceiveNewMessageAsync(ChatMessage message)
		{
            _logger?.LogInformation($"New message from user {message.SenderId}.");
			message.TimeUnix = DateTimeOffset.Now.ToUnixTimeSeconds();

            var chatroom = await _chatroomRepo.GetByIdAsync(message.ChatroomId);
            chatroom.NewMessage(message);

            _messageRepo.Add(message);
            await _messageRepo.SaveChangesAsync();

            await Task.WhenAll(chatroom.UserIds.Select(async id => 
            {
                var user = await _userRepo.GetByIdAsync(id);
                await user.ReceiveNewMessageAsync(message);
            }));
		}
	}
}
