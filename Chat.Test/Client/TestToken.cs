using System;
using System.Threading.Tasks;
using Chat.Connection.Grpc;
using Grpc.Core;
using Xunit;

namespace Chat.Test.Client
{
    public abstract class TestToken<TSetup> : TestClientBase<TSetup>
        where TSetup : SetupBase, new()
    {
        [Fact]
        public async Task TokenEmpty()
        {
            await client1.GetPeopleInfo(1);
            
            dynamic ss = client1._serverService;
            ss.SetToken("empty token");
            
            await Assert.ThrowsAnyAsync<Exception>(async () =>
                await client1.GetPeopleInfo(1));
        }
    }
}