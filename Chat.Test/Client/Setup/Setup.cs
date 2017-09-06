﻿using System;
using Chat.Client;
using Chat.Connection.Grpc;
using Chat.Connection.Local;
using Chat.Core.Interfaces;
using Chat.Server;

namespace Chat.Test
{
    public abstract class SetupBase
    {
        internal ILoginService loginService;
        internal ClientBuilder clientBuilder;
        internal Server.Server server;
    }
    
    
    public class LocalSetup: SetupBase
    {
        public LocalSetup()
        {
            var serverBuilder = new ServerBuilder().UseLocal().UseInMemory();
            server = serverBuilder.Build();
            loginService = LocalConnectionExtension.GetLoginService(server);
            clientBuilder = new ClientBuilder().UseLocal(server);
        }
    }

    public class GrpcLocalSetup : SetupBase
    {
        public GrpcLocalSetup()
        {
            const string host = "localhost";
            int port = new Random().Next(5000, 10000);
            string target = $"{host}:{port}";

            var serverBuilder = new ServerBuilder().UseGrpc(host, port).UseInMemory();
            clientBuilder = new ClientBuilder().UseGrpc(target, host: "localhost", port: 0);
            loginService = GrpcConnectionExtension.GetLoginService(target);
            server = serverBuilder.Build();
        }
    }
    
    public class GrpcRemoteSetup : SetupBase
    {
    	public GrpcRemoteSetup()
    	{
            const string target = "192.168.31.23:8080";
            const string localIP = "192.168.31.2";
    		loginService = GrpcConnectionExtension.GetLoginService(target);
    		clientBuilder = new ClientBuilder().UseGrpc(target, host: localIP, port: 0);
    	}
    }
}