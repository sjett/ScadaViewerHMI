// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;

namespace HMI.Modules.PropertyEditor.Services
{
	public class SingleInstanceFilteringStrategy : IFilteringStrategy
    {
		public IEnumerable<IProperty> Filter(object[] instances, IEnumerable<IProperty> properties)
		{
			return properties;
		}
	}
}
