using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using Chat.Server.Application;
using Chat.Server.Domains.Entities;
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
			return _identityService.LoginAsync(request);
		}

		public Task<SignupResponse> SignupAsync(SignupRequest request)
		{
			return _identityService.SignupAsync(request);
		}

		public void SetUserClient(long userId, IClientService client)
		{
			_userClientService[userId] = client;
		}
		
		public async Task<MakeFriendResponse> MakeFriendsAsync(MakeFriendRequest request)
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

		public ChatroomApplication GetChatroomApplication(long roomId, long operatorId)
		{
			return new ChatroomApplication(_provider)
			{
				ChatroomId = roomId,
				OperatorId = operatorId
			};
		}
		
		public async Task<SendMessageResponse> ReceiveNewMessageAsync(ChatMessage message)
        {
            try
            {
	            var ca = GetChatroomApplication(message.ChatroomId, message.SenderId);
                switch (message.Content.ContentCase)
                {
                    case Content.ContentOneofCase.Text:
                    case Content.ContentOneofCase.Image:
                    case Content.ContentOneofCase.File:
                        await ca.NewMessageAsync(message);
                        break;
                        
                    case Content.ContentOneofCase.New:
                        var newArgs = message.Content.New;
                        ca = await ca.NewChatroomAsync(newArgs.PeopleIds, newArgs.Name);
                        return new SendMessageResponse{Success = true, ChatroomId = ca.ChatroomId};
                        
                    case Content.ContentOneofCase.Dismiss:
                        await ca.DismissAsync();
                        break;
                        
                    case Content.ContentOneofCase.AddPeople:
                        var addArgs = message.Content.AddPeople;
                        await ca.AddPeoplesAsync(addArgs.PeopleIds);
                        break;
                        
                    case Content.ContentOneofCase.RemovePeople:
                        var removeArgs = message.Content.RemovePeople;
                        await ca.RemovePeoplesAsync(removeArgs.PeopleIds);
                        break;
                        
                    case Content.ContentOneofCase.Apply:
                        break;
                    case Content.ContentOneofCase.Quit:
	                    await ca.QuitAsync();
                        break;

                    case Content.ContentOneofCase.Announce:
                        break;
                    case Content.ContentOneofCase.SetPeoperty:
	                    var spArgs = message.Content.SetPeoperty;
	                    if (spArgs.Key == "Name")
		                    await ca.ChangeName(spArgs.Value);
	                    else
	                    	throw new ArgumentException("PropertyName");
	                    break;
                    case Content.ContentOneofCase.Withdraw:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
	            _logger.LogWarning(e, $"Invalid message: {message}\nThrows: {e.Message}");
                return new SendMessageResponse
                {
                    Success = false,
                    Detail = e.Message
                };
            }
            return new SendMessageResponse {Success = true};
        }
	}
}
