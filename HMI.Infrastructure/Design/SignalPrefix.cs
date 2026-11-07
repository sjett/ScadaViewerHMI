// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMI.Infrastructure.Design
{
    public enum SignalPrefix
    {
        None,
        ObjectName,
        ObjectName_PageSignalPrefix,
        PageSignalPrefix,
        PageSignalPrefix_ObjectName
    }
}
