// SPDX-License-Identifier: GPL-3.0-only
//using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;

namespace HMI.Infrastructure.Services.Logging
{
    public class LoggerFactory : ILoggerFactory
    {
        private ICollection<IRuntimeLogger> _runtimeLoggers = new List<IRuntimeLogger>();
        public ICollection<IRuntimeLogger> RuntimeLoggers
        {
            get
            {
                return this._runtimeLoggers;
            }
        }
    }
}
