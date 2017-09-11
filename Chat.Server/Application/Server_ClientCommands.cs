using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using Chat.Server.Infrastructure;
using Google.Protobuf;
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

		public void SetUserClient(long userId, IClientService client)
		{
			_userClientService[userId] = client;
		}

		public async Task<ChatroomResponse> ReceiveNewMessageAsync(ChatMessage message)
		{
			return await _chatroomService.HandleNewMessageAsync(message);
		}
		
		public async Task<MakeFriendResponse> MakeFriends(MakeFriendRequest request)
		{
			var user = await _userRepo.GetByIdAsync(request.SenderId);
			var target = await _userRepo.FindByIdAsync(request.TargetId);
			if(target == null)
				return new MakeFriendResponse{Status = MakeFriendResponse.Types.Status.UserNotExist};
			var response = await user.HandleMakeFriend(request, target);
			await _userRepo.SaveChangesAsync();
			return response;
		}

		public IAsyncEnumerable<GetDataResponse> GetDataAsync(GetDataRequest request)
		{
			BinaryReader reader;
			if (request.RandLength != 0)
				reader = DataService.TestData(request.Seed, request.RandLength);
			else
				reader = DataService.ReadFile(request.FileName);
			return DataService.GetDataAsync(reader);
		}
	}
}
