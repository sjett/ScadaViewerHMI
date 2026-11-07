// SPDX-License-Identifier: GPL-3.0-only
//using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using HMI.Common.Instrumentation.ErrorHandling;

namespace HMI.Infrastructure.Services.Logging
{
    public class CompositeLoggingService : IRuntimeLogger, ILoggingService//, ILogger
    {
        private IEnumerable<IRuntimeLogger> runtimeLoggers;
        private string parameters;
        //private LoggerVerbosity loggerVerbosity_0;
        public string Parameters
        {
            get
            {
                return this.parameters;
            }
            set
            {
                this.parameters = value;
            }
        }
        public CompositeLoggingService(ILoggerFactory loggerFactory)
        {
            Contract.Requires<ArgumentNullException>(loggerFactory != null);
            runtimeLoggers = loggerFactory.RuntimeLoggers;
            Contract.Assert(runtimeLoggers != null);
        }
        public void LogException(Exception exception)
        {
            foreach (IRuntimeLogger runtimeLogger in runtimeLoggers)
            {
                runtimeLogger.LogException(exception);
            }
        }
    }
}
