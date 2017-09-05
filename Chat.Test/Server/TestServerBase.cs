using System;
using Chat.Server;
using Chat.Core.Models;
using Xunit;

namespace Chat.Test
{
    public abstract class TestServerBase
    {
        protected Server.Server server;

        protected TestServerBase()
        {
            server = new ServerBuilder().UseInMemory().Build();
        }
    }
}
