// SPDX-License-Identifier: GPL-3.0-only
using Telerik.Windows.Data;

namespace HMI.Infrastructure.Services
{
    public class FilterDescriptorFactory : IFilterDescriptorFactory
    {
        public IFilterDescriptor GetFilterDescriptor(string serverFilter, string connectorFilter, string groupFilter, string partFilter)
        {
            var filter = new CompositeFilterDescriptor
            {
                LogicalOperator = FilterCompositionLogicalOperator.And
            };
            AddServerFilter(filter, serverFilter);
            AddConnectorFilter(filter, connectorFilter);
            AddGroupFilter(filter, groupFilter);
            AddSignalFilter(filter, partFilter);
            return filter;
        }
        private static void AddServerFilter(CompositeFilterDescriptor filter, string serverName)
        {
            if (!string.IsNullOrEmpty(serverName))
            {
                filter.FilterDescriptors.Add(new FilterDescriptor("ServerName", FilterOperator.IsEqualTo, serverName, false));
            }
        }
        private static void AddConnectorFilter(CompositeFilterDescriptor filter, string connectorName)
        {
            if (!string.IsNullOrEmpty(connectorName))
            {
                filter.FilterDescriptors.Add(new FilterDescriptor("ConnectorName", FilterOperator.IsEqualTo, connectorName, false));
            }
        }
        private static void AddGroupFilter(CompositeFilterDescriptor filter, string groupName)
        {
            if (!string.IsNullOrEmpty(groupName))
            {
                filter.FilterDescriptors.Add(new FilterDescriptor("GroupName", FilterOperator.IsEqualTo, groupName, false));
            }
        }
        private static void AddSignalFilter(CompositeFilterDescriptor filter, string signalNamePrefix)
        {
            if (!string.IsNullOrEmpty(signalNamePrefix))
            {
                filter.FilterDescriptors.Add(new FilterDescriptor("SignalName", FilterOperator.StartsWith, signalNamePrefix, false));
            }
        }
    }
}
