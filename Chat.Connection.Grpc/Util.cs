using System;
using System.Net;
using System.Net.Sockets;

namespace Chat.Connection.Grpc
{
    static class Util
    {
		// https://stackoverflow.com/questions/138043/find-the-next-tcp-port-in-net
        public static int FreeTcpPort()
		{
			var l = new TcpListener(IPAddress.Loopback, 0);
			l.Start();
			int port = ((IPEndPoint)l.LocalEndpoint).Port;
			l.Stop();
			return port;
		}
    }
}
