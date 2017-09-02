using System;
using System.Linq;
using Chat.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Domains
{
    public class UserService
    {
        readonly ILogger _logger;
        readonly ServerDbContext _context;

        public UserService(IServiceProvider provider)
        {
            _logger = provider.GetService<ILoggerFactory>()?
                              .CreateLogger<UserService>();
            _context = provider.GetRequiredService<ServerDbContext>();
        }

		public LoginResponse Login(LoginRequest request)
		{
			var user = _context.Find<User>(request.UserId);
			if (user == null)
				return new LoginResponse { Status = LoginResponse.Types.Status.NoSuchUser };
			if (user.Password != request.Password)
				return new LoginResponse { Status = LoginResponse.Types.Status.WrongPassword };

			user.LastLoginTime = DateTimeOffset.Now;
			_context.Update(user);
			_context.SaveChanges();

            _context.Find<Chatroom>(1L).NewPeople(user.Id).Wait();

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
			if (_context.Users.Any(u => u.Username == request.Username))
				return new SignupResponse
				{
					Status = SignupResponse.Types.Status.UsernameExist
				};

			var user = new User
			{
				Username = request.Username,
				Password = request.Password
			};

			_context.Add(user);
			_context.SaveChanges();

			_logger?.LogInformation($"New user {user.Id} signup.");
			return new SignupResponse
			{
				Status = SignupResponse.Types.Status.Success,
				UserId = user.Id
			};
		}
    }
}
