// SPDX-License-Identifier: GPL-3.0-only
//using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;

namespace HMI.Infrastructure.Services.Logging
{
    public interface ILoggerFactory
    {
        //ICollection<ILogger> BuildLoggers
        //{
        //    get;
        //}
        ICollection<IRuntimeLogger> RuntimeLoggers
        {
            get;
        }
    }
}
