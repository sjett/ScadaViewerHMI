// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMI.Infrastructure.Services
{
    public interface IErrorHandlingService
    {
        void HandleWarning(string messageFormat, params object[] arguments);
        void HandleError(string messageFormat, params object[] arguments);
    }
}
