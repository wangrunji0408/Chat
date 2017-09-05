using System;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Models;
using Chat.Server.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Domains
{
    public class UserService
    {
        readonly ILogger _logger;
        readonly UserRepository _userRepo;
        readonly ChatroomRepository _chatroomRepo;

        public UserService(IServiceProvider provider)
        {
            _logger = provider.GetService<ILoggerFactory>()?
                              .CreateLogger<UserService>();
            _userRepo = new UserRepository(provider);
            _chatroomRepo = new ChatroomRepository(provider);
        }

		public async Task<LoginResponse> LoginAsync(LoginRequest request)
		{
            var user = await _userRepo.FindByIdAsync(request.UserId);
			if (user == null)
				return new LoginResponse { Status = LoginResponse.Types.Status.NoSuchUser };
            var response = user.Login(request);
            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();

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

			var user = new User
			{
				Username = request.Username,
				Password = request.Password
			};

			_userRepo.Add(user);
			await _userRepo.SaveChangesAsync();

            _logger?.LogInformation($"New user {user.Id} signup.");

            var globalRoom = await _chatroomRepo.FindByIdAsync(1);
            globalRoom.NewPeople(user.Id);
            _chatroomRepo.Update(globalRoom);
            await _chatroomRepo.SaveChangesAsync();

			return new SignupResponse
			{
				Status = SignupResponse.Types.Status.Success,
				UserId = user.Id
			};
		}
    }
}
