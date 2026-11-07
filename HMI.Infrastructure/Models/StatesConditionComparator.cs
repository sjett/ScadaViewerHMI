// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMI.Infrastructure.Models
{
    public enum StatesConditionComparator
    {
        Different,
        Equal,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        AndWord,
        AndDWord
    }
}
