using System;
using System.Linq;
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

		public LoginResponse Login(LoginRequest request)
		{
            var user = _userRepo.FindByIdAsync(request.UserId).Result;
			if (user == null)
				return new LoginResponse { Status = LoginResponse.Types.Status.NoSuchUser };
			if (user.Password != request.Password)
				return new LoginResponse { Status = LoginResponse.Types.Status.WrongPassword };

			user.LastLoginTime = DateTimeOffset.Now;
            _userRepo.Update(user);
            _userRepo.SaveChanges();

			_logger?.LogInformation($"User {request.UserId} login.");
			return new LoginResponse
			{
				Status = LoginResponse.Types.Status.Success
			};
		}

		public SignupResponse Signup(SignupRequest request)
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
            if (_userRepo.ContainsUsernameAsync(request.Username).Result)
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
			_userRepo.SaveChanges();

            _chatroomRepo.FindByIdAsync(1).Result.NewPeople(user.Id);

			_logger?.LogInformation($"New user {user.Id} signup.");
			return new SignupResponse
			{
				Status = SignupResponse.Types.Status.Success,
				UserId = user.Id
			};
		}
    }
}
