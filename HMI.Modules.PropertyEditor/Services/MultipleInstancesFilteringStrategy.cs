// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using HMI.Modules.PropertyEditor.Models;

namespace HMI.Modules.PropertyEditor.Services
{


    public class MultipleInstancesFilteringStrategy : IFilteringStrategy
    {
		private readonly IActionMonitor _actionMonitor;
		
		public MultipleInstancesFilteringStrategy(IActionMonitor actionMonitor)
		{
            _actionMonitor = actionMonitor;
		}
		public IEnumerable<IProperty> Filter(object[] instances, IEnumerable<IProperty> properties)
		{
			var propertiesByName = GroupCompatibleProperties(instances, properties);
			IEnumerable<string> compatiblePropertyNames = GetCompatiblePropertyNames(propertiesByName);
			return compatiblePropertyNames.Select(name => new MultipleInstanceProperty(propertiesByName[name].ToList(), this._actionMonitor)).ToList();
		}
        private IEnumerable<string> GetCompatiblePropertyNames(IEnumerable<KeyValuePair<string, IGrouping<string, IProperty>>> propertiesByName)
        {
            var groupsWithFirstProperty = propertiesByName.Select(group => (group, firstProperty: group.Value.First()));

            var compatibleGroups = groupsWithFirstProperty.Where(item =>
                item.group.Value.All(property => property.IsCompatibileWith(item.firstProperty)));

            return compatibleGroups.Select(item => item.group.Key);
        }

        private IDictionary<string, IGrouping<string, IProperty>> GroupCompatibleProperties(object[] instances, IEnumerable<IProperty> properties)
        {

            IEnumerable<IProperty> eligibleProperties = properties
				.Where(property => !property.GetAttributes<SingleInstanceEditableAttribute>().Any<SingleInstanceEditableAttribute>() && !property.IsCollection);

            IEnumerable<IGrouping<string, IProperty>> propertiesPresentOnAllInstances = eligibleProperties
				.GroupBy(property => property.Name)
				.Where(group => group.Count() == instances.Length);

            return propertiesPresentOnAllInstances.ToDictionary(group => group.Key);
        }
		

	}
}
