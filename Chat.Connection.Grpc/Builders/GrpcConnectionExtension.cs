using Chat.Server;
using Chat.Client;

namespace Chat.Connection.Grpc
{
    public static class GrpcConnectionExtension
    {
        public static ServerBuilder UseGrpc(this ServerBuilder builder, int port)
        {
            return builder.SetConnection(new GrpcServerConnectionBuilder(port));
        }
        public static ClientBuilder UseGrpc(this ClientBuilder builder, string serverAddress)
		{
            return builder.SetConnection(new GrpcClientConnectionBuilder(serverAddress));
		}
    }
}
