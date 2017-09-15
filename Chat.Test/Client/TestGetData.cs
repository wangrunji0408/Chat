using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Chat.Core.Models;
using Chat.Server.Infrastructure;
using Xunit;

namespace Chat.Test.Client
{
    public abstract class TestGetData<TSetup>: TestClientBase<TSetup>
        where TSetup : SetupBase, new()
    {
        [Fact]
        public async Task GetTestData()
        {
            var responses = client1.GetDataAsync(new GetDataRequest
            {
                Seed = 8080,
                RandLength = 0x10000
            });
            var data = responses.ToEnumerable().SelectMany(r => r.Data).ToArray();
            var expected = DataService.TestData(8080, 0x10000).ReadBytes(0x10000);
            Assert.Equal(expected, data);
        }
    }
}