// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using HMI.ProjectDesigner.DesignControls.Alarms.ViewModels;
using HMI.ProjectDesigner.DesignControls.Serializers;
using HMI.Common.Browsers.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Serialization;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Alarms.Serializers
{
	public class HMIAlarmOnlineSerializer : CommonControlSerializer
	{
		public HMIAlarmOnlineSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIAlarmOnlineViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject properties, ICommonControlViewModel target, System.Version version)
		{
			HMIAlarmOnlineViewModel node = target as HMIAlarmOnlineViewModel;
			base.DeserializeProperties(properties, node, version);

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

			node.RefreshInterval = ((int?)properties["RefreshInterval"]) ?? node.RefreshInterval;

			node.ServerSortOrder = properties["ServerSortOrder"]?.ToObject<ServerSortOrder?>() ?? node.ServerSortOrder;

			node.MaxAlarmRows = ((int?)properties["MaxAlarmRows"]) ?? node.MaxAlarmRows;

			node.ShowDateTimeMilliseconds = ((bool?)properties["ShowDateTimeMilliseconds"]) ?? node.ShowDateTimeMilliseconds;

			node.ShowHeader = ((bool?)properties["ShowHeader"]) ?? node.ShowHeader;

			node.ShowScrollBars = ((bool?)properties["ShowScrollBars"]) ?? node.ShowScrollBars;

			node.EnablePopup = ((bool?)properties["EnablePopup"]) ?? node.EnablePopup;

			node.Filter = properties["Filter"]?.ToObject<AlarmVisibility?>() ?? node.Filter;

            node.FilterAlarmGroupsByUser = ((bool?)properties["FilterAlarmGroupsByUser"]) ?? node.FilterAlarmGroupsByUser;

			node.MinimumPriority = ((int?)properties["MinimumPriority"]) ?? node.MinimumPriority;

			node.MaximumPriority = ((int?)properties["MaximumPriority"]) ?? node.MaximumPriority;

			node.HornPath = ((string?)properties["HornPath"]) ?? node.HornPath;

			node.Loop = ((bool?)properties["Loop"]) ?? node.Loop;

            node.WriteSecure = ((bool?)properties["WriteSecure"]) ?? node.WriteSecure;

            node.UseColumnWidthsProperties = ((bool?)properties["UseColumnWidthsProperties"]) ?? node.UseColumnWidthsProperties;

            node.AutoSizeColumns = ((bool?)properties["AutoSizeColumns"]) ?? node.AutoSizeColumns;

            node.ActiveWidth = ((double?)properties["ActiveWidth"]) ?? node.ActiveWidth;

            node.SystemTimeWidth = ((double?)properties["SystemTimeWidth"]) ?? node.SystemTimeWidth;

            node.GoneWidth = ((double?)properties["GoneWidth"]) ?? node.GoneWidth;

            node.AcknowledgedWidth = ((double?)properties["AcknowledgedWidth"]) ?? node.AcknowledgedWidth;

            node.TextWidth = ((double?)properties["TextWidth"]) ?? node.TextWidth;

            node.GroupWidth = ((double?)properties["GroupWidth"]) ?? node.GroupWidth;

            node.TypeWidth = ((double?)properties["TypeWidth"]) ?? node.TypeWidth;

            node.SignalNameWidth = ((double?)properties["SignalNameWidth"]) ?? node.SignalNameWidth;

            node.OpcItemWidth = ((double?)properties["OpcItemWidth"]) ?? node.OpcItemWidth;

            node.PriorityWidth = ((double?)properties["PriorityWidth"]) ?? node.PriorityWidth;

            node.PriorityColorWidth = ((double?)properties["PriorityColorWidth"]) ?? node.PriorityColorWidth;

            node.AcknowledgedWithCommentWidth = ((double?)properties["AcknowledgedWithCommentWidth"]) ?? node.AcknowledgedWithCommentWidth;

            node.UseColumnOrderProperties = ((bool?)properties["UseColumnOrderProperties"]) ?? node.UseColumnOrderProperties;

            node.ActiveOrder = ((int?)properties["ActiveOrder"]) ?? node.ActiveOrder;

            node.SystemTimeOrder = ((int?)properties["SystemTimeOrder"]) ?? node.SystemTimeOrder;

            node.GoneOrder = ((int?)properties["GoneOrder"]) ?? node.GoneOrder;

            node.AcknowledgedOrder = ((int?)properties["AcknowledgedOrder"]) ?? node.AcknowledgedOrder;

            node.TextOrder = ((int?)properties["TextOrder"]) ?? node.TextOrder;

            node.GroupOrder = ((int?)properties["GroupOrder"]) ?? node.GroupOrder;

            node.TypeOrder = ((int?)properties["TypeOrder"]) ?? node.TypeOrder;

            node.SignalNameOrder = ((int?)properties["SignalNameOrder"]) ?? node.SignalNameOrder;

            node.OpcItemOrder = ((int?)properties["OpcItemOrder"]) ?? node.OpcItemOrder;

            node.PriorityOrder = ((int?)properties["PriorityOrder"]) ?? node.PriorityOrder;

            node.PriorityColorOrder = ((int?)properties["PriorityColorOrder"]) ?? node.PriorityColorOrder;

            node.AcknowledgedWithCommentOrder = ((int?)properties["AcknowledgedWithCommentOrder"]) ?? node.AcknowledgedWithCommentOrder;
            
		}
		protected override void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel target, System.Version version)
		{
			HMIAlarmOnlineViewModel wFAlarmOnlineViewModel = target as HMIAlarmOnlineViewModel;
			base.DeserializeCollections(propertyCollectionsElement, wFAlarmOnlineViewModel, version);

			this.DeserializeRowColors(propertyCollectionsElement, wFAlarmOnlineViewModel, wFAlarmOnlineViewModel.RowColorDefinitions, version);
			this.DeserializeAlarmGroupFilter(propertyCollectionsElement, wFAlarmOnlineViewModel, wFAlarmOnlineViewModel.GroupFilters, version);
			this.DeserializeAlarmTypeFilter(propertyCollectionsElement, wFAlarmOnlineViewModel, wFAlarmOnlineViewModel.TypeFilters, version);
			this.DeserializePriorityColor(propertyCollectionsElement, wFAlarmOnlineViewModel, wFAlarmOnlineViewModel.PriorityColorDefinitions, version);
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIAlarmOnlineViewModel wFAlarmOnlineViewModel = source as HMIAlarmOnlineViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFAlarmOnlineViewModel);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("ActiveAlarmBackground", wFAlarmOnlineViewModel.ActiveAlarmBackground?.ToString()),
				new JProperty("ActiveAlarmForeground", wFAlarmOnlineViewModel.ActiveAlarmForeground?.ToString()),
				new JProperty("InactiveAlarmBackground", wFAlarmOnlineViewModel.InactiveAlarmBackground?.ToString()),
				new JProperty("InactiveAlarmForeground", wFAlarmOnlineViewModel.InactiveAlarmForeground?.ToString()),
				new JProperty("AcknowledgedAlarmBackground", wFAlarmOnlineViewModel.AcknowledgedAlarmBackground?.ToString()),
				new JProperty("AcknowledgedAlarmForeground", wFAlarmOnlineViewModel.AcknowledgedAlarmForeground?.ToString()),
				new JProperty("AcknowledgedAndGoneAlarmBackground", wFAlarmOnlineViewModel.AcknowledgedAndGoneAlarmBackground?.ToString()),
				new JProperty("AcknowledgedAndGoneAlarmForeground", wFAlarmOnlineViewModel.AcknowledgedAndGoneAlarmForeground?.ToString()),
				new JProperty("RefreshInterval", wFAlarmOnlineViewModel.RefreshInterval),
				new JProperty("ServerSortOrder", wFAlarmOnlineViewModel.ServerSortOrder),
				new JProperty("MaxAlarmRows", wFAlarmOnlineViewModel.MaxAlarmRows),
				new JProperty("ShowDateTimeMilliseconds", wFAlarmOnlineViewModel.ShowDateTimeMilliseconds),
				new JProperty("ShowHeader", wFAlarmOnlineViewModel.ShowHeader),
				new JProperty("ShowScrollBars", wFAlarmOnlineViewModel.ShowScrollBars),
				new JProperty("EnablePopup", wFAlarmOnlineViewModel.EnablePopup),
				new JProperty("Filter", wFAlarmOnlineViewModel.Filter),
				new JProperty("FilterAlarmGroupsByUser", wFAlarmOnlineViewModel.FilterAlarmGroupsByUser),
				new JProperty("MinimumPriority", wFAlarmOnlineViewModel.MinimumPriority),
				new JProperty("MaximumPriority", wFAlarmOnlineViewModel.MaximumPriority),
				new JProperty("HornPath", wFAlarmOnlineViewModel.HornPath),
				new JProperty("Loop", wFAlarmOnlineViewModel.Loop),
				new JProperty("WriteSecure", wFAlarmOnlineViewModel.WriteSecure),
				new JProperty("UseColumnWidthsProperties", wFAlarmOnlineViewModel.UseColumnWidthsProperties),
				new JProperty("AutoSizeColumns", wFAlarmOnlineViewModel.AutoSizeColumns),
				new JProperty("ActiveWidth", wFAlarmOnlineViewModel.ActiveWidth),
				new JProperty("SystemTimeWidth", wFAlarmOnlineViewModel.SystemTimeWidth),
				new JProperty("GoneWidth", wFAlarmOnlineViewModel.GoneWidth),
				new JProperty("AcknowledgedWidth", wFAlarmOnlineViewModel.AcknowledgedWidth),
				new JProperty("TextWidth", wFAlarmOnlineViewModel.TextWidth),
				new JProperty("GroupWidth", wFAlarmOnlineViewModel.GroupWidth),
				new JProperty("TypeWidth", wFAlarmOnlineViewModel.TypeWidth),
				new JProperty("SignalNameWidth", wFAlarmOnlineViewModel.SignalNameWidth),
				new JProperty("OpcItemWidth", wFAlarmOnlineViewModel.OpcItemWidth),
				new JProperty("PriorityWidth", wFAlarmOnlineViewModel.PriorityWidth),
				new JProperty("PriorityColorWidth", wFAlarmOnlineViewModel.PriorityColorWidth),
				new JProperty("AcknowledgedWithCommentWidth", wFAlarmOnlineViewModel.AcknowledgedWithCommentWidth),
				new JProperty("UseColumnOrderProperties", wFAlarmOnlineViewModel.UseColumnOrderProperties),
				new JProperty("ActiveOrder", wFAlarmOnlineViewModel.ActiveOrder),
				new JProperty("SystemTimeOrder", wFAlarmOnlineViewModel.SystemTimeOrder),
				new JProperty("GoneOrder", wFAlarmOnlineViewModel.GoneOrder),
				new JProperty("AcknowledgedOrder", wFAlarmOnlineViewModel.AcknowledgedOrder),
				new JProperty("TextOrder", wFAlarmOnlineViewModel.TextOrder),
				new JProperty("GroupOrder", wFAlarmOnlineViewModel.GroupOrder),
				new JProperty("TypeOrder", wFAlarmOnlineViewModel.TypeOrder),
				new JProperty("SignalNameOrder", wFAlarmOnlineViewModel.SignalNameOrder),
				new JProperty("OpcItemOrder", wFAlarmOnlineViewModel.OpcItemOrder),
				new JProperty("PriorityOrder", wFAlarmOnlineViewModel.PriorityOrder),
				new JProperty("PriorityColorOrder", wFAlarmOnlineViewModel.PriorityColorOrder),
				new JProperty("AcknowledgedWithCommentOrder", wFAlarmOnlineViewModel.AcknowledgedWithCommentOrder)
			});
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
		{
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeCollections(source);
			HMIAlarmOnlineViewModel wFAlarmOnlineViewModel = source as HMIAlarmOnlineViewModel;
			System.Collections.Generic.List<JProperty> second = new System.Collections.Generic.List<JProperty>
			{
				this.GetSerializedRowColors(wFAlarmOnlineViewModel.RowColorDefinitions),
				this.GetSerializedAlarmGroupFilter(wFAlarmOnlineViewModel.GroupFilters),
				this.GetSerializedAlarmTypeFilter(wFAlarmOnlineViewModel.TypeFilters),
				this.GetSerializedPriorityColor(wFAlarmOnlineViewModel.PriorityColorDefinitions)
			};
			return first.Concat(second);
		}
	}
}
