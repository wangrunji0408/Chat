using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Server.Domains
{
    public abstract class DomainBase
    {
        protected ILogger _logger;
        protected virtual string LoggerName
        {
            get
            {
                var type = GetType();
                var id = (long)type.GetProperty("Id").GetValue(this);
                var className = type.Name;
                return $"{className} {id}";
            }
        }

        public void SetServices (IServiceProvider provider)
        {
            if(_logger == null)
            {
				_logger = provider.GetService<ILoggerFactory>()?
							  .CreateLogger(LoggerName);
				_logger?.LogTrace($"Created.");
            }
        }
    }
}
