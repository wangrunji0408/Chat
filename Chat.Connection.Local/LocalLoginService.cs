using System;
namespace Chat.Connection.Local
{
    using Core.Interfaces;
    using Server;

    public class LocalLoginService: ILoginService, ILoginListener
    {
        public event EventHandler<(long userId, IClientService service)> NewUserLogin;

        public string ServerName { get; set; }

        public IServerService Login(long userId)
        {
            var client = LocalClientService.GetService(userId);
			if (NewUserLogin != null)
                NewUserLogin(this, (userId, client));
            return LocalServerService.GetService(ServerName, userId);
        }
    }
}
