using Chat.Server;
using Chat.Client;
using Chat.Core.Interfaces;

namespace Chat.Connection.Grpc
{
    public static class GrpcConnectionExtension
    {
        public static ServerBuilder UseGrpc(this ServerBuilder builder, string host, int port)
        {
            return builder.SetConnection(new GrpcServerConnectionBuilder(host, port));
        }
        public static ClientBuilder UseGrpc(this ClientBuilder builder, string serverAddress, string host, int port)
		{
            return builder.SetConnection(new GrpcClientConnectionBuilder(serverAddress, host, port));
		}
        public static ILoginService GetLoginService (string serverAddress)
        {
            return new GrpcServerServiceClient(serverAddress);
        }
    }
}
