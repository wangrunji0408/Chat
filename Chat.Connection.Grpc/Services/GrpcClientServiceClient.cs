using System.Threading.Tasks;
using Chat.Core.Interfaces;
using Chat.Core.Models;
using Grpc.Core;

namespace Chat.Connection.Grpc
{
    public class GrpcClientServiceClient: ChatClientService.ChatClientServiceClient, IClientService
    {
        public GrpcClientServiceClient(string target) : 
            base(new Channel(target, ChannelCredentials.Insecure))
        {
        }

        public async Task NewMessageAsync(ChatMessage message)
        {
            var request = new SendMessageRequest{Message = message};
            await base.NewMessageAsync(request);
        }
    }
}