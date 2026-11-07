// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.Models;
using HMI.Infrastructure.Presentation;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Serialization;
using HMI.ProjectDesigner.DesignControls.Alarms;
using HMI.ProjectDesigner.DesignControls.Alarms.ViewModels;
using HMI.ProjectDesigner.DesignControls.Serializers;
using Newtonsoft.Json.Linq;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Alarms.Serializers
{
	public class HMIAlarmViewerSerializer : CommonControlSerializer
	{
		public HMIAlarmViewerSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIAlarmViewerViewModel(Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, Version version)
		{
			HMIAlarmViewerViewModel node = target as HMIAlarmViewerViewModel;
			base.DeserializeProperties(properties, node, version);

			node.RefreshInterval = (int?)properties["RefreshInterval"] ?? node.RefreshInterval;

            node.HornPath = (string?)properties["HornPath"] ?? node.HornPath;

            node.Loop = (bool?)properties["HornPath"] ?? node.Loop;

            node.WriteSecure = (bool?)properties["WriteSecure"] ?? node.WriteSecure;

            node.EnableNavigation = (bool?)properties["EnableNavigation"] ?? node.EnableNavigation;

            node.EnablePopup = (bool?)properties["EnablePopup"] ?? node.EnablePopup;

            node.ShowAlarmGroupsFilterTree = (bool?)properties["ShowAlarmGroupsFilterTree"] ?? node.ShowAlarmGroupsFilterTree;

            node.ShowAlarmTypesFilterTree = (bool?)properties["ShowAlarmTypesFilterTree"] ?? node.ShowAlarmTypesFilterTree;

            node.ShowStatusBar = (bool?)properties["ShowStatusBar"] ?? node.ShowStatusBar;

            node.ShowHeader = (bool?)properties["ShowHeader"] ?? node.ShowHeader;

            node.ShowScrollbars = (bool?)properties["ShowScrollBars"] ?? node.ShowScrollbars;

            node.ShowDateTimeMilliseconds = (bool?)properties["ShowDateTimeMilliseconds"] ?? node.ShowDateTimeMilliseconds;

            node.ShowMultilineTextValues = (bool?)properties["ShowMultilineTextValues"] ?? node.ShowMultilineTextValues;

            node.ShowTotalAlarmCount = (bool?)properties["ShowTotalAlarmCount"] ?? node.ShowTotalAlarmCount;

            node.ShowActiveAlarmCount = (bool?)properties["ShowActiveAlarmCount"] ?? node.ShowActiveAlarmCount;

            node.ShowUnacknowledgedAlarmCount = (bool?)properties["ShowUnacknowledgedAlarmCount"] ?? node.ShowUnacknowledgedAlarmCount;

            node.AlarmCountDisplayMode = properties["AlarmCountDisplayMode"]?.ToObject<AlarmCountDisplayMode?>() ?? node.AlarmCountDisplayMode;

			if(properties["OfflineInterval"] != null)
			{
				node.OfflineInterval = new Infrastructure.Presentation.Range
				{
					dateRange = properties["OfflineInterval"]!["dateRange"]?.ToObject<DateRange?>() ?? DateRange.None,
					startDate = (DateTime?)properties["OfflineInterval"]!["startDate"] ?? DateTime.MinValue,
					endDate = (DateTime?)properties["OfflineInterval"]!["endDate"],
					number = (int?)properties["OfflineInterval"]!["endDate"] ?? 0
                };
            }

            node.Offline = (bool?)properties["Offline"] ?? node.Offline;

            node.ShowGroupsInRuntimeFilter = (bool?)properties["ShowGroupsInRuntimeFilter"] ?? node.ShowGroupsInRuntimeFilter;

            node.GroupsFilter = (string?)properties["GroupsFilter"] ?? node.GroupsFilter;

            node.FilterGroupsByUser = (bool?)properties["FilterGroupsByUser"] ?? node.FilterGroupsByUser;

            node.ShowTypesInRuntimeFilter = (bool?)properties["ShowTypesInRuntimeFilter"] ?? node.ShowTypesInRuntimeFilter;

            node.TypesFilter = (string?)properties["TypesFilter"] ?? node.TypesFilter;

            node.ShowMaximumCountInRuntimeFilter = (bool?)properties["ShowMaximumCountInRuntimeFilter"] ?? node.ShowMaximumCountInRuntimeFilter;

            node.MaximumCount = (int?)properties["MaximumCount"] ?? node.MaximumCount;

            node.ShowServerSortOrderInRuntimeFilter = (bool?)properties["ShowServerSortOrderInRuntimeFilter"] ?? node.ShowServerSortOrderInRuntimeFilter;

            node.ServerSortOrder = properties["ServerSortOrder"]?.ToObject<ServerSortOrder?>() ?? node.ServerSortOrder;

            node.ShowAlarmVisibilityInRuntimeFilter = (bool?)properties["ShowAlarmVisibilityInRuntimeFilter"] ?? node.ShowAlarmVisibilityInRuntimeFilter;

            node.AlarmVisibility = properties["AlarmVisibility"]?.ToObject<AlarmStatusFilter?>() ?? node.AlarmVisibility;

            node.ShowPriorityInRuntimeFilter = (bool?)properties["ShowPriorityInRuntimefilter"] ?? node.ShowPriorityInRuntimeFilter;

            node.MinimumPriority = (int?)properties["MinimumPriority"] ?? node.MinimumPriority;

            node.MaximumPriority = (int?)properties["MaximumPriority"] ?? node.MaximumPriority;

            node.ShowTextFilterInRuntimeFilter = (bool?)properties["ShowTextFilterInRuntimeFilter"] ?? node.ShowTextFilterInRuntimeFilter;

            node.TextFilterColumn = properties["TextFilterColumn"]?.ToObject<FilterColumnType?>() ?? node.TextFilterColumn;

            node.TextFilter = (string?)properties["TextFilter"] ?? node.TextFilter;

			if((string?)properties["ActiveAlarmBackground"] != null)
				node.ActiveAlarmBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["ActiveAlarmBackground"]!)).GetAsFrozen();

            if ((string?)properties["ActiveAlarmForeground"] != null)
                node.ActiveAlarmForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["ActiveAlarmForeground"]!)).GetAsFrozen();

            if ((string?)properties["InactiveAlarmBackground"] != null)
                node.InactiveAlarmBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["InactiveAlarmBackground"]!)).GetAsFrozen();

            if ((string?)properties["InactiveAlarmForeground"] != null)
                node.InactiveAlarmForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["InactiveAlarmForeground"]!)).GetAsFrozen();

            if ((string?)properties["AcknowledgedAlarmBackground"] != null)
                node.AcknowledgedAlarmBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["AcknowledgedAlarmBackground"]!)).GetAsFrozen();

            if ((string?)properties["AcknowledgedAlarmForeground"] != null)
                node.AcknowledgedAlarmForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["AcknowledgedAlarmForeground"]!)).GetAsFrozen();

            if ((string?)properties["AcknowledgedAndGoneAlarmBackground"] != null)
                node.AcknowledgedAndGoneAlarmBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["AcknowledgedAndGoneAlarmBackground"]!)).GetAsFrozen();

            if ((string?)properties["AcknowledgedAndGoneAlarmForeground"] != null)
                node.AcknowledgedAndGoneAlarmForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["AcknowledgedAndGoneAlarmForeground"]!)).GetAsFrozen();

            if ((string?)properties["BadQualityAlarmBackground"] != null)
                node.BadQualityAlarmBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["BadQualityAlarmBackground"]!)).GetAsFrozen();

            if ((string?)properties["BadQualityAlarmForeground"] != null)
                node.BadQualityAlarmForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["BadQualityAlarmForeground"]!)).GetAsFrozen();

		}
		protected override void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel target, Version version)
		{
			HMIAlarmViewerViewModel wFAlarmViewerViewModel = target as HMIAlarmViewerViewModel;
			base.DeserializeCollections(propertyCollectionsElement, wFAlarmViewerViewModel, version);

			this.DeserializeRowColors(propertyCollectionsElement, wFAlarmViewerViewModel, wFAlarmViewerViewModel.RowColorDefinitions, version);
			this.DeserializeAlarmColumnSettings(propertyCollectionsElement, wFAlarmViewerViewModel, wFAlarmViewerViewModel.Columns, version);
			this.DeserializePriorityColor(propertyCollectionsElement, wFAlarmViewerViewModel, wFAlarmViewerViewModel.PriorityColorDefinitions, version);
			this.DeserializeSecurityPolicies(propertyCollectionsElement, wFAlarmViewerViewModel, wFAlarmViewerViewModel.SecurityPolicies, version);
		}
		protected override IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIAlarmViewerViewModel wFAlarmViewerViewModel = source as HMIAlarmViewerViewModel;
			IEnumerable<JProperty> first = base.SerializeProperties(wFAlarmViewerViewModel);

			List<JProperty> list =
            [
                new JProperty("RefreshInterval", wFAlarmViewerViewModel.RefreshInterval),
                new JProperty("HornPath", wFAlarmViewerViewModel.HornPath),
                new JProperty("Loop", wFAlarmViewerViewModel.Loop),
                new JProperty("WriteSecure", wFAlarmViewerViewModel.WriteSecure),
                new JProperty("EnableNavigation", wFAlarmViewerViewModel.EnableNavigation),
                new JProperty("EnablePopup", wFAlarmViewerViewModel.EnablePopup),
                new JProperty("ShowAlarmGroupsFilterTree", wFAlarmViewerViewModel.ShowAlarmGroupsFilterTree),
                new JProperty("ShowAlarmTypesFilterTree", wFAlarmViewerViewModel.ShowAlarmTypesFilterTree),
                new JProperty("ShowStatusBar", wFAlarmViewerViewModel.ShowStatusBar),
                new JProperty("ShowHeader", wFAlarmViewerViewModel.ShowHeader),
                new JProperty("ShowScrollBars", wFAlarmViewerViewModel.ShowScrollbars),
                new JProperty("ShowDateTimeMilliseconds", wFAlarmViewerViewModel.ShowDateTimeMilliseconds),
                new JProperty("ShowMultilineTextValues", wFAlarmViewerViewModel.ShowMultilineTextValues),
                new JProperty("ShowTotalAlarmCount", wFAlarmViewerViewModel.ShowTotalAlarmCount),
                new JProperty("ShowActiveAlarmCount", wFAlarmViewerViewModel.ShowActiveAlarmCount),
                new JProperty("ShowUnacknowledgedAlarmCount", wFAlarmViewerViewModel.ShowUnacknowledgedAlarmCount),
                new JProperty("AlarmCountDisplayMode", wFAlarmViewerViewModel.AlarmCountDisplayMode),
            ];

			var offlineInterval = wFAlarmViewerViewModel.OfflineInterval;
			var item = new JProperty("OfflineInterval", new JObject(
				new JProperty("dateRange", offlineInterval.dateRange),
				new JProperty("startDate", offlineInterval.startDate),
				new JProperty("endDate", offlineInterval.endDate),//nullabled
				new JProperty("number", offlineInterval.number)
			));
			list.Add(item);

			list.Add(new JProperty("Offline", wFAlarmViewerViewModel.Offline));
			list.Add(new JProperty("ShowGroupsInRuntimeFilter", wFAlarmViewerViewModel.ShowGroupsInRuntimeFilter));
			list.Add(new JProperty("GroupsFilter", wFAlarmViewerViewModel.GroupsFilter));
			list.Add(new JProperty("FilterGroupsByUser", wFAlarmViewerViewModel.FilterGroupsByUser));
			list.Add(new JProperty("ShowTypesInRuntimeFilter", wFAlarmViewerViewModel.ShowTypesInRuntimeFilter));
			list.Add(new JProperty("TypesFilter", wFAlarmViewerViewModel.TypesFilter));
			list.Add(new JProperty("ShowMaximumCountInRuntimeFilter", wFAlarmViewerViewModel.ShowMaximumCountInRuntimeFilter));
			list.Add(new JProperty("MaximumCount", wFAlarmViewerViewModel.MaximumCount));
			list.Add(new JProperty("ShowServerSortOrderInRuntimeFilter", wFAlarmViewerViewModel.ShowServerSortOrderInRuntimeFilter));
			list.Add(new JProperty("ServerSortOrder", wFAlarmViewerViewModel.ServerSortOrder));
			list.Add(new JProperty("ShowAlarmVisibilityInRuntimeFilter", wFAlarmViewerViewModel.ShowAlarmVisibilityInRuntimeFilter));
			list.Add(new JProperty("AlarmVisibility", wFAlarmViewerViewModel.AlarmVisibility));
			list.Add(new JProperty("ShowPriorityInRuntimefilter", wFAlarmViewerViewModel.ShowPriorityInRuntimeFilter));
			list.Add(new JProperty("MinimumPriority", wFAlarmViewerViewModel.MinimumPriority));
			list.Add(new JProperty("MaximumPriority", wFAlarmViewerViewModel.MaximumPriority));
			list.Add(new JProperty("ShowTextFilterInRuntimeFilter", wFAlarmViewerViewModel.ShowTextFilterInRuntimeFilter));
			list.Add(new JProperty("TextFilterColumn", wFAlarmViewerViewModel.TextFilterColumn));
			list.Add(new JProperty("TextFilter", wFAlarmViewerViewModel.TextFilter));

			list.Add(new JProperty("ActiveAlarmBackground", wFAlarmViewerViewModel.ActiveAlarmBackground?.ToString()));
			list.Add(new JProperty("ActiveAlarmForeground", wFAlarmViewerViewModel.ActiveAlarmForeground?.ToString()));
			list.Add(new JProperty("InactiveAlarmBackground", wFAlarmViewerViewModel.InactiveAlarmBackground?.ToString()));
			list.Add(new JProperty("InactiveAlarmForeground", wFAlarmViewerViewModel.InactiveAlarmForeground?.ToString()));
			list.Add(new JProperty("AcknowledgedAlarmBackground", wFAlarmViewerViewModel.AcknowledgedAlarmBackground?.ToString()));
			list.Add(new JProperty("AcknowledgedAlarmForeground", wFAlarmViewerViewModel.AcknowledgedAlarmForeground?.ToString()));
			list.Add(new JProperty("AcknowledgedAndGoneAlarmBackground", wFAlarmViewerViewModel.AcknowledgedAndGoneAlarmBackground?.ToString()));
			list.Add(new JProperty("AcknowledgedAndGoneAlarmForeground", wFAlarmViewerViewModel.AcknowledgedAndGoneAlarmForeground?.ToString()));
			list.Add(new JProperty("BadQualityAlarmBackground", wFAlarmViewerViewModel.BadQualityAlarmBackground?.ToString()));
			list.Add(new JProperty("BadQualityAlarmForeground", wFAlarmViewerViewModel.BadQualityAlarmForeground?.ToString()));
			return first.Concat(list);
		}
		protected override IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
		{
			IEnumerable<JProperty> first = base.SerializeCollections(source);
			HMIAlarmViewerViewModel wFAlarmViewerViewModel = source as HMIAlarmViewerViewModel;
			List<JProperty> second = new List<JProperty>
			{
				this.GetSerializedRowColors(wFAlarmViewerViewModel.RowColorDefinitions),
				this.GetSerializedAlarmColumnSettings(wFAlarmViewerViewModel.Columns),
				this.GetSerializedPriorityColor(wFAlarmViewerViewModel.PriorityColorDefinitions),
				this.GetSerializedSecurityPolicies(wFAlarmViewerViewModel.SecurityPolicies)
			};

			return first.Concat(second);
		}
	}
}
