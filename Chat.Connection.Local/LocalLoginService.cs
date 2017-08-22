using System;
namespace Chat.Connection.Local
{
    using Core.Interfaces;
    using Server;

    public class LocalLoginService: ILoginService
    {
		readonly Server _server;
		public LocalLoginService(Server server)
		{
			_server = server;
		}

        public IServerService Login(long userId)
        {
            var client = LocalClientService.GetService(userId);
            _server.UserLogin(userId, client);
            return new LocalServerService(_server, userId);
        }
    }
}
