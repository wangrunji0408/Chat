using System;
using Chat.Client;
using Chat.Core.Interfaces;

namespace Chat.Connection.Grpc
{
    class GrpcClientConnectionBuilder : ClientConnectionBuilder
    {
        readonly string _serverAddress;
        readonly string _host;
        readonly int _port;

        public GrpcClientConnectionBuilder(string serverAddress, string host, int port)
        {
            _serverAddress = serverAddress;
            _host = host;
            _port = port;
        }

        GrpcServerServiceClient ssc;

        public override void After(Client.Client client, IServiceProvider provider)
        {
            var cs = new GrpcClientServiceImpl(client, provider, _host, _port);
			ssc.ClientService = cs;
        }

        public override ILoginService Before()
        {
            ssc = new GrpcServerServiceClient(_serverAddress);
            return ssc;
        }
    }
}
