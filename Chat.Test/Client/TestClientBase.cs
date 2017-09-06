using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Test
{
    using Client;
    using Server;
    using Core.Interfaces;
    using Core.Models;
    using Connection.Local;
    using Connection.Grpc;

    public abstract class TestClientBase<TSetup>
    	where TSetup: SetupBase, new()
    {
        protected ILoginService loginService;
        protected ClientBuilder clientBuilder;
        protected Server server;

	    protected const long GlobalChatroomId = 1;

        public TestClientBase ()
        {
            var setup = new TSetup();
	        loginService = setup.loginService;
	        clientBuilder = setup.clientBuilder;
	        server = setup.server;
        }
    }
}
