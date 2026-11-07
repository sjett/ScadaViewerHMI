// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;

namespace HMI.Modules.PropertyEditor.Services
{
    public interface IFilteringStrategy
    {
        IEnumerable<IProperty> Filter(object[] instances, IEnumerable<IProperty> properties);
    }
}
