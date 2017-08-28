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
        Dictionary<long, IClientService> clients = new Dictionary<long, IClientService>();
        readonly ILogger _logger;
        readonly ServerDbContext _context;

        public Server(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("Server");
            _context = serviceProvider.GetRequiredService<ServerDbContext>();
            _context.Database.EnsureCreated();
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
            bool exist = _context.Users.Any(u => u.Username == request.Username);
            if (exist)
                return new SignupResponse { Status = SignupResponse.Types.Status.UsernameExist };
            
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

        public User GetUser (long userId)
        {
            return _context.Find<User>(userId);
        }

        public void SetUserClient (long userId, IClientService client)
        {
            _logger?.LogInformation($"User {userId} set client.");
            if(clients.ContainsKey(userId))
            {
                _logger?.LogWarning($"User {userId} already has a connection, it will be reset.");
                clients[userId] = client;
            }
            else
                clients.Add(userId, client);
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            _logger?.LogInformation($"New message from user {message.SenderId}.");
            var forwarding = Task.WhenAll(clients.Values.Select(user => user.NewMessageAsync(message)));
            // TODO store message for offline users
		}
    }
}
