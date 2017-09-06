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

    public abstract class TestClient_Base
    {
        protected ILoginService loginService;
        protected ClientBuilder clientBuilder;
        protected Server server;

	    private const long GlobalChatroomId = 1;

        public TestClient_Base ()
        {
            
        }

	    [Fact]
        public async Task Login()
        {
            await loginService.SignupAsync(new SignupRequest{Username = "user1", Password = "123456"});
            var client = clientBuilder.SetUser(1, "123456").Build();
            await client.Login();
        }

	    [Fact]
        public async Task Login_WrongPassword()
	    {
			await loginService.SignupAsync(new SignupRequest { Username = "user1", Password = "123456" });
			var client = clientBuilder.SetUser(1, "654321").Build();
            await Assert.ThrowsAsync<Exception>(client.Login);
	    }

		[Fact]
		public async Task SendMessage()
		{
			await loginService.SignupAsync(new SignupRequest { Username = "user1", Password = "123456" });
			await loginService.SignupAsync(new SignupRequest { Username = "user2", Password = "123456" });
			var client1 = clientBuilder.SetUser(1, "123456").Build();
			var client2 = clientBuilder.SetUser(2, "123456").Build();
            bool recv1 = false;
            bool recv2 = false;
            string message = "Hello";
            client1.NewMessage += (sender, e) => recv1 |= e.SenderId == 2 && e.Content.Text == message;
			client2.NewMessage += (sender, e) => recv2 |= e.SenderId == 2 && e.Content.Text == message;
			await client1.Login();
			await client2.Login();
            await client2.SendTextMessage(message, GlobalChatroomId);
            await Task.Delay(1000); // wait for server forwarding the message
			Assert.True(recv1);
			Assert.True(recv2);
		}

		[Fact]
		public async Task GetMessageAfter()
		{
			await loginService.SignupAsync(new SignupRequest { Username = "user1", Password = "123456" });
			var client1 = clientBuilder.SetUser(1, "123456").Build();
            await client1.Login();
			await client1.SendTextMessage("Message1", GlobalChatroomId);
            await Task.Delay(1000);
            var t0 = DateTimeOffset.Now;
			await client1.SendTextMessage("Message2", GlobalChatroomId);
			await client1.SendTextMessage("Message3", GlobalChatroomId);
            var messages = await client1.GetMessages(new GetMessagesRequest
            {
                AfterTimeUnix = t0.ToUnixTimeSeconds()
            });
            Assert.Null(messages.Find(m => m.Content.Text == "Message1"));
			Assert.NotNull(messages.Find(m => m.Content.Text == "Message2"));
            Assert.NotNull(messages.Find(m => m.Content.Text == "Message3"));
		}
	    
	    [Fact]
	    public async Task GetPeopleInfo()
	    {
		    await loginService.SignupAsync(new SignupRequest { Username = "user1", Password = "123456" });
		    await loginService.SignupAsync(new SignupRequest { Username = "user2", Password = "123456" });
		    var client1 = clientBuilder.SetUser(1, "123456").Build();
		    await client1.Login();
		    Assert.Equal(new PeopleInfo
		    {
			    Id = 2,
			    Username = "user2"
		    }, await client1.GetPeopleInfo(2));
	    }
	    
	    [Fact]
	    public async Task GetChatroomInfo()
	    {
		    await loginService.SignupAsync(new SignupRequest { Username = "user1", Password = "123456" });
		    await loginService.SignupAsync(new SignupRequest { Username = "user2", Password = "123456" });
		    var client1 = clientBuilder.SetUser(1, "123456").Build();
		    await client1.Login();
		    Assert.Equal(new ChatroomInfo
		    {
			    Id = GlobalChatroomId,
			    Name = "Global Chatroom",
			    PeopleIds = { 1, 2 }
		    }, await client1.GetChatroomInfo(GlobalChatroomId));
	    }
    }

    public class TestClient_Local: TestClient_Base
    {
        public TestClient_Local()
        {
            var serverBuilder = new ServerBuilder().UseLocal().UseInMemory();
			server = serverBuilder.Build();
            loginService = LocalConnectionExtension.GetLoginService(server);
			clientBuilder = new ClientBuilder().UseLocal(server);
        }
    }

	public class TestClient_Grpc_Local : TestClient_Base
	{
		public TestClient_Grpc_Local()
		{
            const string host = "localhost";
            const int port = 8080;
            string target = $"{host}:{port}";

            var serverBuilder = new ServerBuilder().UseGrpc(host, port).UseInMemory();
            clientBuilder = new ClientBuilder().UseGrpc(target, host: "localhost", port: 0);
            loginService = GrpcConnectionExtension.GetLoginService(target);
            server = serverBuilder.Build();
		}
	}

	//public class TestClient_Grpc_Remote : TestClient_Base
	//{
	//	public TestClient_Grpc_Remote()
	//	{
 //           const string target = "192.168.31.23:8080";
 //           const string localIP = "192.168.31.2";
	//		loginService = GrpcConnectionExtension.GetLoginService(target);
	//		clientBuilder = new ClientBuilder().UseGrpc(target, host: localIP, port: 0);
	//	}
	//}
}
