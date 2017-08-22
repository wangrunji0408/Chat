using System;
namespace Chat.Core.Interfaces
{
    public interface ILoginListener
    {
        event EventHandler<(long userId, IClientService service)> NewUserLogin;
    }
}
