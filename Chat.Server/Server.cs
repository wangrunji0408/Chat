﻿using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server
{
    using Core.Interfaces;
    using Core.Models;

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
            return new LoginResponse 
            {
                Status = LoginResponse.Types.Status.Success
            };
        }

        public SignupResponse SignUp (SignupRequest request)
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
            return new SignupResponse
			{
                Status = SignupResponse.Types.Status.Success,
                UserId = userId
			};
        }

        public void SetUserClient (long userId, IClientService client)
        {
            _logger?.LogInformation($"User {userId} set client.");
            clients.Add(userId, client);
        }

        public void SendMessage(ChatMessage message)
        {
            _logger?.LogTrace($"New message from user {message.SenderId}.");
			foreach (var pair in clients)
                pair.Value.NewMessage(message);
        }
    }
}
