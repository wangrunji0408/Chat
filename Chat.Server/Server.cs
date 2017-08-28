using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server
{
    using Core.Interfaces;
    using Core.Models;
    using Domains;

    public class Server
    {
        Dictionary<long, IClientService> clients = new Dictionary<long, IClientService>();
        Dictionary<long, User> users = new Dictionary<long, User>();
        Dictionary<string, long> usernameToId = new Dictionary<string, long>();
        readonly ILogger _logger;

        public Server(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("Server");
        }

        public LoginResponse Login (LoginRequest request)
        {
            if(!users.ContainsKey(request.UserId))
                return new LoginResponse { Status = LoginResponse.Types.Status.NoSuchUser };
            var user = users[request.UserId];
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
            if (usernameToId.ContainsKey(request.Username))
                return new SignupResponse { Status = SignupResponse.Types.Status.UsernameExist };
            var userId = users.Count + 1;
			var user = new User
			{
                Id = userId,
				Username = request.Username,
				Password = request.Password
			};
			users.Add(user.Id, user);
            usernameToId.Add(user.Username, user.Id);
            _logger?.LogInformation($"New user {userId} signup.");
            return new SignupResponse
			{
                Status = SignupResponse.Types.Status.Success,
                UserId = userId
			};
        }

        public User GetUser (long userId)
        {
            users.TryGetValue(userId, out var user);
            return user;
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
