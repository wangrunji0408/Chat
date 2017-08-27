using Chat.Client;
using Chat.Core.Interfaces;
using Chat.Server;

namespace Chat.Connection.Local
{
    public static class LocalConnectionExtension
    {
        public static ServerBuilder UseLocal(this ServerBuilder builder)
        {
            return builder;
        }
        public static ClientBuilder UseLocal(this ClientBuilder builder, Server.Server server)
        {
            return builder.SetConnection(new LocalClientConnectionBuilder(server));
        }
        public static ILoginService GetLoginService(Server.Server server)
        {
            return new LocalLoginService(server);
        }
    }
}
