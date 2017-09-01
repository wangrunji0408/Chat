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

    public class Server
    {
        readonly ILogger _logger;
        readonly ServerDbContext _context;

        public Server(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("Server");
            _context = serviceProvider.GetRequiredService<ServerDbContext>();
            _context.Database.EnsureCreated();
            try {_context.Database.Migrate();}
            catch(Exception e)
            {
                _logger?.LogError(e, "An error occurred when database migrate.");
            }
        }

        public LoginResponse Login (LoginRequest request)
        {
            var user = _context.Find<User>(request.UserId);
            if(user == null)
                return new LoginResponse { Status = LoginResponse.Types.Status.NoSuchUser };
            if (user.Password != request.Password)
                return new LoginResponse { Status = LoginResponse.Types.Status.WrongPassword };
            _logger?.LogInformation($"User {request.UserId} login.");
            return new LoginResponse 
            {
                Status = LoginResponse.Types.Status.Success
            };
        }

        public SignupResponse Signup (SignupRequest request)
        {
            if (!StringCheckService.CheckUsername(request.Username, out var reason))
                return new SignupResponse 
                {
                    Status = SignupResponse.Types.Status.UsernameFormatWrong,
                    Detail = reason
                };
            if(!StringCheckService.CheckPassword(request.Password, out reason))
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

        public User GetUserNullable (long userId)
        {
            return _context.Find<User>(userId);
        }

        User GetUser (long userId)
        {
            return GetUserNullable(userId)
                ?? throw new InvalidOperationException($"User {userId} does not exist.");
        }

        public void ClearDatabase ()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _context.Database.Migrate();
        }

        public void SetUserClient (long userId, IClientService client)
        {
            _logger?.LogInformation($"User {userId} set client.");
            var user = GetUser(userId);
            if(user.ClientService != null)
                _logger?.LogWarning($"User {userId} already has a connection, it will be reset.");
            user.ClientService = client;
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            _logger?.LogInformation($"New message from user {message.SenderId}.");
            message.Time = DateTimeOffset.Now.ToString();
            _context.Add(message);
            await _context.SaveChangesAsync();
            var ids = await _context.Users.Select(u => u.Id).ToListAsync();
            var forwarding = Task.WhenAll(ids.Select(id => _context.Users.Find(id).NewMessageAsync(message)));
            // TODO store message for offline users
		}

        public Task<List<ChatMessage>> GetMessages(GetMessagesRequest request)
		{
            var user = GetUser(request.UserId);
            var time = DateTimeOffset.FromUnixTimeSeconds(request.AfterTimeUnix);
            return user.GetMessagesAfter(time, _context.Messages);
		}

        public Task<List<ChatMessage>> GetRecentMessages (int count)
        {
            return _context.Messages.OrderByDescending(m => m.Time).Take(count).ToListAsync();
        }
    }
}
