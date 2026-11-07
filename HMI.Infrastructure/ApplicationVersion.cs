// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;

namespace HMI.Infrastructure
{
    public static class ApplicationVersion
    {
        private static readonly Lazy<Version> var = new Lazy<Version>(() => typeof(ApplicationVersion).Assembly.GetName().Version);

        public static Version Current
        {
            get
            {
                return var.Value;
            }
        }
    }
}

