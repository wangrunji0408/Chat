using Chat.Core.Models;
using Chat.Server;

namespace Chat.Test.Server
{
    public abstract class TestServerBase
    {
        protected Chat.Server.Server server;

        protected TestServerBase()
        {
            server = new ServerBuilder().UseInMemory().Build();
            // ReSharper disable once VirtualMemberCallInConstructor
            Signup2User();
        }

        protected virtual void Signup2User()
        {
            server.SignupAsync(new SignupRequest
            {
                Username = "user1",
                Password = "password"
            }).Wait();

            server.SignupAsync(new SignupRequest
            {
                Username = "user2",
                Password = "password"
            }).Wait();
            
            server.SignupAsync(new SignupRequest
            {
                Username = "user3",
                Password = "password"
            }).Wait();
        }
    }
}