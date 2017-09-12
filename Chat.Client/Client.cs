﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Chat.Core.Interfaces;
using Chat.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Client
{
    public class Client
    {
        private readonly ILogger _logger;
        private readonly ILoginService _loginService;

        private IServerService _serverService;

        public Client(IServiceProvider serviceProvider)
        {
            _loginService = serviceProvider.GetRequiredService<ILoginService>();
            _logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger($"Client {UserId}");
        }

        public long UserId { get; internal set; }
        internal string Password { get; set; }

        private IServerService ServerService
        {
            get => _serverService ??
                   throw new NullReferenceException("The client has not login. ServerService is null.");
            set => _serverService = value;
        }

        public Chatroom GetChatroom(long roomId)
        {
            return new Chatroom()
            {
                UserId = UserId,
                RoomId = roomId,
                ServerService = _serverService
            };
        }

        public void InformNewMessage(ChatMessage message)
        {
            _logger?.LogInformation($"New message from {message.SenderId}.\n{message.Content.Text}");
            NewMessage?.Invoke(this, message);
        }

        public event EventHandler<ChatMessage> NewMessage;

        public async Task<bool> Login()
        {
            var request = new LoginRequest
            {
                UserId = UserId,
                Password = Password
            };
            _serverService = await _loginService.LoginAsync(request);
            return _serverService != null;
        }

        public Task<List<ChatMessage>> GetMessages (GetMessagesRequest request)
        {
            request.SenderId = UserId;
            return _serverService.GetMessages(request).ToList();
        }
        
        public async Task<PeopleInfo> GetPeopleInfo(long peopleId)
        {
            var request = new GetPeopleInfoRequest
            {
                SenderId = UserId,
                TargetId = peopleId
            };
            var response = await _serverService.GetPeopleInfo(request);
            return response.PeopleInfo;
        }

        public Func<MakeFriendRequest, Task<MakeFriendResponse>> MakeFriendHandler;

        public Task<MakeFriendResponse> MakeFriend(long targetId, string greeting = "")
        {
            var request = new MakeFriendRequest
            {
                SenderId = UserId,
                TargetId = targetId,
                Greeting = greeting ?? ""
            };
            return _serverService.MakeFriend(request);
        }

        public IAsyncEnumerable<GetDataResponse> GetDataAsync(GetDataRequest request)
        {
            return _serverService.GetData(request);
        }
    }
}