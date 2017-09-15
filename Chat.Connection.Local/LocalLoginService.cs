using System;
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

        public async Task<IServerService> GetService(LoginResponse token)
        {
            if (!token.Success)
                throw new InvalidOperationException("Can't get service with failed login response.");
            var userId = token.UserId;
            var client = new LocalClientService(Client);
            _server.SetUserClient(userId, client);
            return new LocalServerService(_server, userId, token.Token);
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            return await _server.LoginAsync(request);
        }

        public Task<SignupResponse> SignupAsync(SignupRequest request)
        {
            return _server.SignupAsync(request);
        }
    }
}
