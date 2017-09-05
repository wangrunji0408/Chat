﻿using System;
namespace Chat.Connection.Local
{
    using System.Threading.Tasks;
    using Core.Interfaces;
    using Core.Models;
    using Server;

    class LocalLoginService: ILoginService
    {
        internal Client.Client Client { get; set; }

		readonly Server _server;
		public LocalLoginService(Server server)
		{
			_server = server;
		}

        public async Task<IServerService> LoginAsync(LoginRequest request)
        {
            await Task.CompletedTask;
            var response = await _server.LoginAsync(request);
            if (response.Status != LoginResponse.Types.Status.Success)
                throw new Exception($"Failed to login: {response.Detail}");
            var userId = request.UserId;
            var client = new LocalClientService(Client);
            _server.SetUserClient(userId, client);
            return new LocalServerService(_server, userId);
        }

        public Task<SignupResponse> SignupAsync(SignupRequest request)
        {
            return _server.SignupAsync(request);
        }
    }
}
