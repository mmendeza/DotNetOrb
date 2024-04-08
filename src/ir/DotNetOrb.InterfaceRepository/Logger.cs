// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace DotNetOrb.InterfaceRepository
{
    public class Logger : Core.ILogger
    {
        public static ILoggerFactory LoggerFactory { get; set; }

        private ILogger logger;

        public string Name { get; private set; }

        public bool IsDebugEnabled => logger.IsEnabled(LogLevel.Debug);

        public bool IsInfoEnabled => logger.IsEnabled(LogLevel.Information);

        public bool IsWarnEnabled => logger.IsEnabled(LogLevel.Warning);

        public bool IsErrorEnabled => logger.IsEnabled(LogLevel.Error);

        public bool IsFatalEnabled => logger.IsEnabled(LogLevel.Critical);

        public Logger(string name)
        {
            Name = name;
            logger = LoggerFactory.CreateLogger(name);
        }

        public Logger(Type type) : this(type.FullName)
        {
        }

        public void Debug(object message)
        {
            logger.LogDebug(message.ToString());
        }

        public void Debug(object message, Exception exception)
        {
            logger.LogDebug(exception, message.ToString());
        }

        public void DebugFormat(string format, params object[] args)
        {
            logger.LogDebug(format, args);
        }

        public void Error(object message)
        {
            logger.LogError(message.ToString());
        }

        public void Error(object message, Exception exception)
        {
            logger.LogError(exception, message.ToString());
        }

        public void ErrorFormat(string format, params object[] args)
        {
            logger.LogError(format, args);
        }

        public void Fatal(object message)
        {
            logger.LogCritical(message.ToString());
        }

        public void Fatal(object message, Exception exception)
        {
            logger.LogCritical(exception, message.ToString());
        }

        public void FatalFormat(string format, params object[] args)
        {
            logger.LogCritical(format, args);
        }

        public void Info(object message)
        {
            logger.LogInformation(message.ToString());
        }

        public void Info(object message, Exception exception)
        {
            logger.LogInformation(exception, message.ToString());
        }

        public void InfoFormat(string format, params object[] args)
        {
            logger.LogInformation(format, args);
        }

        public void Warn(object message)
        {
            logger.LogWarning(message.ToString());
        }

        public void Warn(object message, Exception exception)
        {
            logger.LogWarning(exception, message.ToString());
        }

        public void WarnFormat(string format, params object[] args)
        {
            logger.LogWarning(format, args);
        }
    }
}
