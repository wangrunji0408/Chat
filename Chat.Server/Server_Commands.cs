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
		public LoginResponse Login(LoginRequest request)
		{
			return _userService.Login(request);
		}

		public SignupResponse Signup(SignupRequest request)
		{
			return _userService.Signup(request);
		}

		public async Task<Chatroom> NewChatroom(IEnumerable<long> peopleIds)
		{
			// TODO Check people exist
			var chatroom = new Chatroom();
			foreach (var id in peopleIds)
				chatroom.NewPeople(id);
            _chatroomRepo.Add(chatroom);
            await _chatroomRepo.SaveChangesAsync();
			return chatroom;
		}

		public void ClearDatabase()
		{
			_context.Database.EnsureDeleted();
			_context.Database.EnsureCreated();
			_context.Database.Migrate();
		}

		public void SetUserClient(long userId, IClientService client)
		{
            _userRepo.GetAsyncById(userId).Result
                     .ClientService = client;
		}

		/// <summary>
		/// 收到一条新消息
		/// 保存消息，通知相应聊天室，再通知聊天室的所有用户
		/// </summary>
		public async Task SendMessageAsync(ChatMessage message)
		{
            _logger?.LogInformation($"New message from user {message.SenderId}.");
            message.Time = DateTimeOffset.Now.ToString();

            var chatroom = await _chatroomRepo.FindByIdAsync(message.ChatroomId)
								   ?? throw new ArgumentException($"Chatroom {message.ChatroomId} does not exist.");
            chatroom.NewMessage(message);

            _context.Add(message);
            await _context.SaveChangesAsync();

            var forwarding = Task.WhenAll(chatroom.UserIds.Select(async id => (await _userRepo.FindByIdAsync(id)).NewMessageAsync(message)));
		}
	}
}
