using System;
using Chat.Client;
using Chat.Core.Interfaces;

namespace Chat.Connection.Local
{
    class LocalClientConnectionBuilder: ClientConnectionBuilder
    {
        readonly Server.Server _server;
        public LocalClientConnectionBuilder(Server.Server server)
        {
            _server = server;
        }

        LocalLoginService lls;

        public override void After(Client.Client client, IServiceProvider provider)
        {
            lls.Client = client;
		}

        public override ILoginService Before()
        {
            return lls = new LocalLoginService(_server);
        }
    }
}
