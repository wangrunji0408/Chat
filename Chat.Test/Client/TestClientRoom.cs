using System;
using System.Threading.Tasks;
using Chat.Core.Models;
using Xunit;

namespace Chat.Test
{
    public abstract class TestClientRoom<TSetup>: TestClientBase<TSetup>
        where TSetup: SetupBase, new()
    {
	    private Client.Client client1;
	    private Client.Client client2;
	    
	    public TestClientRoom()
	    {
		    loginService.SignupAsync(new SignupRequest { Username = "user1", Password = "123456" }).Wait();
		    loginService.SignupAsync(new SignupRequest { Username = "user2", Password = "123456" }).Wait();
		    client1 = clientBuilder.SetUser(1, "123456").Build();
		    client2 = clientBuilder.SetUser(2, "123456").Build();
		    client1.Login().Wait();
		    client2.Login().Wait();
	    }
	    
        [Fact]
		public async Task SendMessage()
		{
            bool recv1 = false;
            bool recv2 = false;
            string message = "Hello";
            client1.NewMessage += (sender, e) => recv1 |= e.SenderId == 2 && e.Content.Text == message;
			client2.NewMessage += (sender, e) => recv2 |= e.SenderId == 2 && e.Content.Text == message;
			
            await client2.SendTextMessage(message, GlobalChatroomId);
            await Task.Delay(1000); // wait for server forwarding the message
			Assert.True(recv1);
			Assert.True(recv2);
		}

		[Fact]
		public async Task GetMessageAfter()
		{
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
		    Assert.Equal(new PeopleInfo
		    {
			    Id = 2,
			    Username = "user2"
		    }, await client1.GetPeopleInfo(2));
	    }
	    
	    [Fact]
	    public async Task GetChatroomInfo()
	    {
		    Assert.Equal(new ChatroomInfo
		    {
			    Id = GlobalChatroomId,
			    Name = "Global Chatroom",
			    PeopleIds = { 1, 2 }
		    }, await client1.GetChatroomInfo(GlobalChatroomId));
	    }
    }
	
	public class TestClientRoom_Local: TestClientRoom<LocalSetup> {}
	public class TestClientRoom_GrpcLocal: TestClientRoom<GrpcLocalSetup> {}
}