using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Domains.Entities
{
    public abstract class DomainBase
    {
        public long Id { get; internal set; }

        private ILogger _logger;
        protected ILogger Logger => _logger ??
            (_logger = _provider.GetService<ILoggerFactory>()?
                               .CreateLogger(LoggerName));
        protected virtual string LoggerName
        {
            get
            {
                var type = GetType();
                var id = (long)type.GetProperty("Id").GetValue(this);
                var className = type.FullName;
                return $"{className} {id}";
            }
        }

        protected IServiceProvider _provider;

        public void SetServices (IServiceProvider provider)
        {
            _provider = provider;
        }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
                return false;
            return Id == ((DomainBase) obj).Id;
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() ^ Id.GetHashCode();
        }
    }
}
