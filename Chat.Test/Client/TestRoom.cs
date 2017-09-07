using System.Threading.Tasks;
using Xunit;

namespace Chat.Test.Client
{
    public abstract class TestRoom<TSetup> : TestClientBase<TSetup>
        where TSetup : SetupBase, new()
    {
        [Fact]
        public async Task SendMessage()
        {
            var recv1 = false;
            var recv2 = false;
            var message = "Hello";
            client1.NewMessage += (sender, e) => recv1 |= e.SenderId == 2 && e.Content.Text == message;
            client2.NewMessage += (sender, e) => recv2 |= e.SenderId == 2 && e.Content.Text == message;

            await client2.SendTextMessage(message, GlobalChatroomId);
            await Task.Delay(1000); // wait for server forwarding the message
            Assert.True(recv1);
            Assert.True(recv2);
        }
    }
}