using System;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Models;
using Chat.Server.Domains.Entities;
using Chat.Server.Domains.Events.Chatroom;
using Chat.Server.Domains.Events.User;
using Chat.Server.Domains.Repositories;
using Chat.Server.Infrastructure.EntityFramework;
using Chat.Server.Infrastructure.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Domains.Services
{
    public class UserService
    {
	    private readonly ILogger _logger;
	    private readonly IUserRepository _userRepo;
	    private readonly IChatroomRepository _chatroomRepo;
	    private readonly IEventBus _eventBus;

        public UserService(IServiceProvider provider)
        {
            _logger = provider.GetService<ILoggerFactory>()?
                              .CreateLogger<UserService>();
	        _userRepo = provider.GetRequiredService<IUserRepository>();
	        _chatroomRepo = provider.GetRequiredService<IChatroomRepository>();
	        _eventBus = provider.GetRequiredService<IEventBus>();
	        
	        _eventBus.GetEventStream<UserSignupEvent>()
		        .Subscribe(async e => await AddNewUserToGlobalChatroom(e));
        }
	    
	    async Task AddNewUserToGlobalChatroom(UserSignupEvent e)
	    {
		    var globalRoom = await _chatroomRepo.GetByIdAsync(1);
		    var user = await _userRepo.GetByIdAsync(e.UserId);
		    globalRoom.AddPeople(user);
		    _chatroomRepo.Update(globalRoom);
		    await _chatroomRepo.SaveChangesAsync();
	    }

		public async Task<LoginResponse> LoginAsync(LoginRequest request)
		{
            var user = await _userRepo.FindByIdAsync(request.UserId);
			if (user == null)
				return new LoginResponse { Status = LoginResponse.Types.Status.NoSuchUser };
            var response = user.Login(request);
            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();
			_eventBus.Publish(new UserLoginEvent(user.Id));
            return response;
		}

		public async Task<SignupResponse> SignupAsync(SignupRequest request)
		{
			if (!StringCheckService.CheckUsername(request.Username, out var reason))
				return new SignupResponse
				{
					Status = SignupResponse.Types.Status.UsernameFormatWrong,
					Detail = reason
				};
			if (!StringCheckService.CheckPassword(request.Password, out reason))
				return new SignupResponse
				{
					Status = SignupResponse.Types.Status.PasswordFormatWrong,
					Detail = reason
				};
            if (await _userRepo.ContainsUsernameAsync(request.Username))
				return new SignupResponse
				{
					Status = SignupResponse.Types.Status.UsernameExist
				};

			var user = new User(request.Username, request.Password);

			_userRepo.Add(user);
			await _userRepo.SaveChangesAsync();

			_eventBus.Publish(new UserSignupEvent(user.Id));

			return new SignupResponse
			{
				Status = SignupResponse.Types.Status.Success,
				UserId = user.Id
			};
		}
    }
}
