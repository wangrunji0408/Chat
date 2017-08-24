using System;
using Chat.Server;

namespace Chat.Connection.Grpc
{
    class GrpcServerConnectionBuilder: ServerConnectionBuilder
    {
        readonly int _port;
        public GrpcServerConnectionBuilder(int port)
        {
            _port = port;
        }

        public override void After(Server.Server server, IServiceProvider provider)
        {
			var ss = new GrpcServerServiceImpl(server, provider, _port);
		}
    }
}
