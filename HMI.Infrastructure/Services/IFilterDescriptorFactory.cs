// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Data;

namespace HMI.Infrastructure.Services
{
    public interface IFilterDescriptorFactory
    {
        IFilterDescriptor GetFilterDescriptor(string serverFilter, string connectorFilter, string groupFilter, string partFilter);
    }
}
