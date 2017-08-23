using System;
using System.Threading.Tasks;
using Chat.Core.Models;

namespace Chat.Core.Interfaces
{
    public interface ILoginService
    {
        Task<IServerService> LoginAsync(LoginRequest request);
        Task<SignupResponse> SignupAsync(SignupRequest request);
    }
}
