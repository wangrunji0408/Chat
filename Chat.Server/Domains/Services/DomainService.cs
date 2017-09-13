using System;
using System.Reactive.Disposables;

namespace Chat.Server.Domains.Services
{
    public abstract class DomainService : IDisposable
    {
        protected readonly CompositeDisposable Subcriptions = new CompositeDisposable();

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Subcriptions.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}