using System;
namespace Chat.Core.Interfaces
{
    public interface ILoginService
    {
        IServerService Login(long userId);
    }
}
