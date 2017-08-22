using System;
using System.Collections.Generic;

namespace Chat.Connection.Local
{
    using Core.Interfaces;
    using Client;

    public class LocalClientService : IClientService
    {
        readonly Client _client;
        LocalClientService (Client client)
        {
            _client = client;
        }

        public void NewMessage(long senderId, string message)
        {
            _client.InformNewMessage(senderId, message);
        }

        static Dictionary<long, LocalClientService> clients 
            = new Dictionary<long, LocalClientService>();

        public static void Register(Client client)
        {
            clients.Add(client.UserId, new LocalClientService(client));
        }

        public static LocalClientService GetService(long userId)
        {
            if (!clients.TryGetValue(userId, out var service))
                throw new KeyNotFoundException($"Can not find LocalClientService. User {userId} has not register.");
            return service;
        }

        public static void Clear ()
        {
            clients.Clear();
        }
    }
}
