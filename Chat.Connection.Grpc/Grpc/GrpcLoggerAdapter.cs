using System;
using Grpc.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Chat.Connection.Grpc
{
    class GrpcLoggerAdapter : global::Grpc.Core.Logging.ILogger
    {
        readonly ILoggerFactory _msfactory;
        Microsoft.Extensions.Logging.ILogger _mslogger;

        public GrpcLoggerAdapter (Microsoft.Extensions.Logging.ILoggerFactory loggerFactory)
        {
            _msfactory = loggerFactory;
        }

        GrpcLoggerAdapter (Microsoft.Extensions.Logging.ILogger logger)
        {
            _mslogger = logger;
        }

        public void Debug(string message)
        {
            _mslogger.LogDebug(message);
        }

        public void Debug(string format, params object[] formatArgs)
        {
            _mslogger.LogDebug(string.Format(format, formatArgs));
        }

        public void Error(string message)
        {
            _mslogger.LogError(message);
        }

        public void Error(string format, params object[] formatArgs)
        {
            _mslogger.LogError(string.Format(format, formatArgs));
        }

        public void Error(Exception exception, string message)
        {
            _mslogger.LogError(exception, message);
        }

        public global::Grpc.Core.Logging.ILogger ForType<T>()
        {
            return new GrpcLoggerAdapter(_msfactory.CreateLogger<T>());
        }

        public void Info(string message)
        {
            _mslogger.LogInformation(message);
        }

        public void Info(string format, params object[] formatArgs)
        {
            _mslogger.LogInformation(string.Format(format, formatArgs));
        }

        public void Warning(string message)
        {
            _mslogger.LogWarning(message);
        }

        public void Warning(string format, params object[] formatArgs)
        {
            _mslogger.LogWarning(string.Format(format, formatArgs));
        }

        public void Warning(Exception exception, string message)
        {
            _mslogger.LogWarning(exception, message);
        }
    }
}