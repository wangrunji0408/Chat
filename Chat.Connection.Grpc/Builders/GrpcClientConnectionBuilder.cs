using System;
using Chat.Client;
using Chat.Core.Interfaces;

namespace Chat.Connection.Grpc
{
    class GrpcClientConnectionBuilder : ClientConnectionBuilder
    {
        readonly string _serverAddress;
        //readonly int _port;

        public GrpcClientConnectionBuilder(string serverAddress)
        {
            _serverAddress = serverAddress;
            //_port = port;
        }

        GrpcServerServiceClient ssc;

        public override void After(Client.Client client, IServiceProvider provider)
        {
            var cs = new GrpcClientServiceImpl(client, provider, port: 0);
			ssc.ClientService = cs;
        }

        public override ILoginService Before()
        {
            ssc = new GrpcServerServiceClient(_serverAddress);
            return ssc;
        }
    }
}
