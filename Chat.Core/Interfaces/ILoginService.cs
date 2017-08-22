using System;
using Chat.Core.Models;

namespace Chat.Core.Interfaces
{
    public interface ILoginService
    {
        IServerService Login(LoginRequest request);
        SignupResponse Signup(SignupRequest request);
    }
}
