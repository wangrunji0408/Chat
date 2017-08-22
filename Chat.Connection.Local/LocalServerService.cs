﻿using System;
using System.Collections.Generic;

namespace Chat.Connection.Local
{
    using Core.Interfaces;
    using Server;

    public class LocalServerService: IServerService
    {
        readonly long _userId;
        readonly Server _server;
        LocalServerService(Server server, long userId)
        {
            _server = server;
            _userId = userId;
        }

        public void SendMessage(string message)
        {
            _server.SendMessage(_userId, message);
        }

		static Dictionary<string, Server> servers
			= new Dictionary<string, Server>();

		public static void Register(Server server, string name)
		{
            servers.Add(name, server);
		}

		public static LocalServerService GetService(string serverName, long userId)
		{
            if (!servers.TryGetValue(serverName, out var server))
				throw new KeyNotFoundException($"Can not find LocalServerService. Server \"{serverName}\" has not register.");
            return new LocalServerService(server, userId);
		}

        public static void Clear ()
        {
            servers.Clear();
        }
    }
}
