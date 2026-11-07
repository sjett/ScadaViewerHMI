// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMI.Infrastructure.Controls
{
    public interface IStaticSymbol
    {
        string ContentString
        {
            get;
            set;
        }
    }
}
