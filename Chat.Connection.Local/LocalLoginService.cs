using System;
namespace Chat.Connection.Local
{
    using System.Threading.Tasks;
    using Core.Interfaces;
    using Core.Models;
    using Server;

    public class LocalLoginService: ILoginService
    {
		readonly Server _server;
		public LocalLoginService(Server server)
		{
			_server = server;
		}

        public async Task<IServerService> LoginAsync(LoginRequest request)
        {
            var response = _server.Login(request);
            if (response.Status != LoginResponse.Types.Status.Success)
                throw new Exception($"Failed to login: {response.Detail}");
            var userId = request.UserId;
            var client = LocalClientService.GetService(userId);
            _server.SetUserClient(userId, client);
            return new LocalServerService(_server, userId);
        }

        public async Task<SignupResponse> SignupAsync(SignupRequest request)
        {
            return _server.Signup(request);
        }
    }
}
