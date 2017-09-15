using System;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Models;
using Chat.Server.Domains.Entities;
using Chat.Server.Domains.Events.Chatroom;
using Chat.Server.Domains.Events.User;
using Chat.Server.Domains.Repositories;
using Chat.Server.Infrastructure.EventBus;
using Chat.Server.Infrastructure.Identity;
using Google.Protobuf;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Domains.Services
{
    public class IdentityService: DomainService
    {
	    private readonly ILogger _logger;
	    private readonly IUserRepository _userRepo;
	    private readonly IChatroomRepository _chatroomRepo;
	    private readonly IEventBus _eventBus;
	    private readonly UserManager<ChatIdentityUser> _userManager;
	    private readonly SignInManager<ChatIdentityUser> _signInManager;

        public IdentityService(IServiceProvider provider)
        {
            _logger = provider.GetService<ILoggerFactory>()?
                              .CreateLogger<IdentityService>();
	        _userManager = provider.GetRequiredService<UserManager<ChatIdentityUser>>();
	        _signInManager = provider.GetRequiredService<SignInManager<ChatIdentityUser>>();
	        _userRepo = provider.GetRequiredService<IUserRepository>();
	        _chatroomRepo = provider.GetRequiredService<IChatroomRepository>();
	        _eventBus = provider.GetRequiredService<IEventBus>();
	        
	        Subcriptions.Add(_eventBus.GetEventStream<UserSignupEvent>()
		        .Subscribe(async e => await AddNewUserToGlobalChatroom(e)));
	        Subcriptions.Add(_eventBus.GetEventStream<UserEnteredEvent>()
		        .Subscribe(async e =>
		        {
			        var room = await _chatroomRepo.GetByIdAsync(e.ChatroomId);
			        var user = await _userRepo.GetByIdAsync(e.UserId);
			        user.UserChatrooms.Add(room.UserChatrooms.First(uc => uc.UserId == e.UserId));
			        await _userRepo.SaveChangesAsync();
		        }));
	        Subcriptions.Add(_eventBus.GetEventStream<UserLeftEvent>()
		        .Subscribe(async e =>
		        {
			        var room = await _chatroomRepo.GetByIdAsync(e.ChatroomId);
			        var user = await _userRepo.GetByIdAsync(e.UserId);
			        user.UserChatrooms.Remove(user.UserChatrooms.First(uc => uc.ChatroomId == e.ChatroomId));
			        await _userRepo.SaveChangesAsync();
		        }));
//	        EnsureAddAdmin().Wait();
        }

	    async Task EnsureAddAdmin()
	    {
		    if(await _userManager.FindByNameAsync("Admin") != null)
			    return;
		    await _userManager.CreateAsync(new ChatIdentityUser("Admin"), "admin12345");
		    var admin = await _userManager.FindByNameAsync("Admin");
		    if(admin == null)
			    throw new Exception("Failed to create admin.");
//		    if(admin.Id != 0)
//			    throw new Exception($"Admin's ID is not 0, is {admin.Id}.");
	    }
	    
	    async Task AddNewUserToGlobalChatroom(UserSignupEvent e)
	    {
		    var globalRoom = await _chatroomRepo.GetByIdAsync(1);
		    globalRoom.AddPeople(e.UserId, operatorId: 0);
		    _chatroomRepo.Update(globalRoom);
		    await _chatroomRepo.SaveChangesAsync();
	    }

	    public async Task<bool> VertifyTokenAsync(long userId, string token)
	    {
		    var iuser = await _userManager.FindByIdAsync(userId.ToString());
		    return await _userManager.VerifyUserTokenAsync(
			    iuser, TokenOptions.DefaultProvider, "login", token);
	    }

		public async Task<LoginResponse> LoginAsync(LoginRequest request)
		{
			var iuser = await _userManager.FindByNameAsync(request.Username);
			if(iuser == null)
				return new LoginResponse
				{
					Success = false,
					Detail = "No such user"
				};
			var result = await _signInManager.CheckPasswordSignInAsync(iuser, request.Password, false);
			if(!result.Succeeded)
				return new LoginResponse
				{
					Success = false,
					Detail = "Wrong password"
				};
			
			var token = await _userManager.GenerateUserTokenAsync(
				iuser, TokenOptions.DefaultProvider, "login");
			
			_eventBus.Publish(new UserLoginEvent{UserId = iuser.Id});
            return new LoginResponse
            {
	            Success = true, 
	            UserId = iuser.Id,
	            Token = token
            };
		}

		public async Task<SignupResponse> SignupAsync(SignupRequest request)
		{
			var iuser = new ChatIdentityUser(request.Username);
			var result = await _userManager.CreateAsync(iuser, request.Password);
			if(!result.Succeeded)
				return new SignupResponse
				{
					Success = false,
					Detail = string.Join("\n", result.Errors.Select(e => e.Description))
				};
			iuser = await _userManager.FindByNameAsync(request.Username);
			var user = new User(iuser.Id, iuser.UserName);
			_userRepo.Add(user);
			await _userRepo.SaveChangesAsync();

			_eventBus.Publish(new UserSignupEvent{UserId = user.Id});

			return new SignupResponse
			{
				Success = true,
				UserId = user.Id
			};
		}
    }
}
