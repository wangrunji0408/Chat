using System;
using Chat.Server;

namespace Chat.Connection.Grpc
{
    class GrpcServerConnectionBuilder: ServerConnectionBuilder
    {
        readonly string _host;
        readonly int _port;

        public GrpcServerConnectionBuilder(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public override void After(Server.Server server, IServiceProvider provider)
        {
			var ss = new GrpcServerServiceImpl(server, provider, _host, _port);
		}
    }
}
