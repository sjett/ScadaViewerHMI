// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
using HMI.ProjectDesigner.DesignControls.General.ViewModels;

namespace HMI.ProjectDesigner.DesignControls.General.Serializers
{
	public class HMILogbookSerializer : CommonControlSerializer
	{
		public HMILogbookSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMILogbookViewModel(Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, Version version)
		{
			base.DeserializeProperties(properties, target, version);
			HMILogbookViewModel viewModel = target as HMILogbookViewModel;

			viewModel.InitialConfiguration = (string?)properties["InitialConfiguration"] ?? viewModel.InitialConfiguration;

            viewModel.ClientSideConfiguration = (bool?)properties["ClientSideConfiguration"] ?? viewModel.ClientSideConfiguration;

			if ((string?)properties["ScrollbarBackground"] != null)
				viewModel.ScrollbarBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["ScrollbarBackground"]!)).GetAsFrozen();

            if ((string?)properties["ScrollbarForeground"] != null)
                viewModel.ScrollbarForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["ScrollbarForeground"]!)).GetAsFrozen();

            if ((string?)properties["DialogsBackground"] != null)
                viewModel.DialogsBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["DialogsBackground"]!)).GetAsFrozen();

            if ((string?)properties["DialogsForeground"] != null)
                viewModel.DialogsForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["DialogsForeground"]!)).GetAsFrozen();

            if ((string?)properties["ButtonBackground"] != null)
                viewModel.ButtonBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["ButtonBackground"]!)).GetAsFrozen();

            if ((string?)properties["ButtonForeground"] != null)
                viewModel.ButtonForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["ButtonForeground"]!)).GetAsFrozen();

            if ((string?)properties["HeaderBackground"] != null)
                viewModel.HeaderBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["HeaderBackground"]!)).GetAsFrozen();

            if ((string?)properties["TopicForeground"] != null)
                viewModel.TopicForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["TopicForeground"]!)).GetAsFrozen();

            if ((string?)properties["TimeFilterForeground"] != null)
                viewModel.TimeFilterForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["TimeFilterForeground"]!)).GetAsFrozen();

            if ((string?)properties["SenderBackground"] != null)
                viewModel.SenderBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["SenderBackground"]!)).GetAsFrozen();

            if ((string?)properties["SenderForeground"] != null)
                viewModel.SenderForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["SenderForeground"]!)).GetAsFrozen();

            if ((string?)properties["TimeStampForeground"] != null)
                viewModel.TimeStampForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["TimeStampForeground"]!)).GetAsFrozen();

            if ((string?)properties["SubjectBackground"] != null)
                viewModel.SubjectBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["SubjectBackground"]!)).GetAsFrozen();

            if ((string?)properties["SubjectForeground"] != null)
                viewModel.SubjectForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["SubjectForeground"]!)).GetAsFrozen();

            if ((string?)properties["BodyBackground"] != null)
                viewModel.BodyBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["BodyBackground"]!)).GetAsFrozen();

            if ((string?)properties["BodyForeground"] != null)
                viewModel.BodyForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["BodyForeground"]!)).GetAsFrozen();

            if ((string?)properties["FooterBackground"] != null)
                viewModel.FooterBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["FooterBackground"]!)).GetAsFrozen();

            if ((string?)properties["FooterForeground"] != null)
                viewModel.FooterForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["FooterForeground"]!)).GetAsFrozen();

            if ((string?)properties["ToolBarBackground"] != null)
                viewModel.ToolBarBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["ToolBarBackground"]!)).GetAsFrozen();

		}
		protected override void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel node, Version version)
		{
			base.DeserializeCollections(propertyCollectionsElement, node, version);
			HMILogbookViewModel wFLogbookViewModel = node as HMILogbookViewModel;
			DeserializeLogbookSecurityPolicies(propertyCollectionsElement, node, wFLogbookViewModel.SecurityPolicies, version);
		}
		public void DeserializeLogbookSecurityPolicies(JObject? propertyCollectionsElement, IExplorerNodeViewModel node, ICollection<LogbookSecurityPolicy> securityPolicies, Version version)
		{
			var xElement = propertyCollectionsElement["SecurityPolicies"]?.ToArray();
			if (xElement == null)
				return;

			Dictionary<LogbookSecuredButtons, LogbookSecurityPolicy> dictionary = new Dictionary<LogbookSecuredButtons, LogbookSecurityPolicy>();
			ISerializer serializer = SerializerFactory.GetSerializer<LogbookSecurityPolicy>();

			foreach (var current in xElement)
			{
				if ((string?)current["ItemElementName"] != "LogbookSecurityPolicy")
					continue;

                LogbookSecurityPolicy? logbookSecurityPolicy = serializer.Deserialize(node, (JObject)current, version) as LogbookSecurityPolicy;
				if (logbookSecurityPolicy != null)
				{
					dictionary[logbookSecurityPolicy.SecuredButton] = logbookSecurityPolicy;
				}
			}
			foreach (LogbookSecurityPolicy existingPolicy in securityPolicies)
			{
				LogbookSecurityPolicy deserializedPolicy;
				if (dictionary.TryGetValue(existingPolicy.SecuredButton, out deserializedPolicy))
				{
					deserializedPolicy.DeepCopyFields(existingPolicy);
				}
			}
		}
		protected override IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			IEnumerable<JProperty> first = base.SerializeProperties(source);
			List<JProperty> list = new List<JProperty>();
			HMILogbookViewModel wFLogbookViewModel = source as HMILogbookViewModel;
			list.Add(new JProperty("InitialConfiguration", wFLogbookViewModel.InitialConfiguration));
			list.Add(new JProperty("ClientSideConfiguration", wFLogbookViewModel.ClientSideConfiguration));
			list.Add(new JProperty("ScrollbarBackground", wFLogbookViewModel.ScrollbarBackground?.ToString()));
			list.Add(new JProperty("ScrollbarForeground", wFLogbookViewModel.ScrollbarForeground?.ToString()));
			list.Add(new JProperty("DialogsBackground", wFLogbookViewModel.DialogsBackground?.ToString()));
			list.Add(new JProperty("DialogsForeground", wFLogbookViewModel.DialogsForeground?.ToString()));
			list.Add(new JProperty("ButtonBackground", wFLogbookViewModel.ButtonBackground?.ToString()));
			list.Add(new JProperty("ButtonForeground", wFLogbookViewModel.ButtonForeground?.ToString()));
			list.Add(new JProperty("HeaderBackground", wFLogbookViewModel.HeaderBackground?.ToString()));
			list.Add(new JProperty("TopicForeground", wFLogbookViewModel.TopicForeground?.ToString()));
			list.Add(new JProperty("TimeFilterForeground", wFLogbookViewModel.TimeFilterForeground?.ToString()));
			list.Add(new JProperty("SenderBackground", wFLogbookViewModel.SenderBackground?.ToString()));
			list.Add(new JProperty("SenderForeground", wFLogbookViewModel.SenderForeground?.ToString()));
			list.Add(new JProperty("TimeStampForeground", wFLogbookViewModel.TimeStampForeground?.ToString()));
			list.Add(new JProperty("SubjectBackground", wFLogbookViewModel.SubjectBackground?.ToString()));
			list.Add(new JProperty("SubjectForeground", wFLogbookViewModel.SubjectForeground?.ToString()));
			list.Add(new JProperty("BodyBackground", wFLogbookViewModel.BodyBackground?.ToString()));
			list.Add(new JProperty("BodyForeground", wFLogbookViewModel.BodyForeground?.ToString()));
			list.Add(new JProperty("FooterBackground", wFLogbookViewModel.FooterBackground?.ToString()));
			list.Add(new JProperty("FooterForeground", wFLogbookViewModel.FooterForeground?.ToString()));
			list.Add(new JProperty("ToolBarBackground", wFLogbookViewModel.ToolBarBackground?.ToString()));
			return first.Concat(list);
		}
		protected override IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
		{
			IEnumerable<JProperty> first = base.SerializeCollections(source);
			HMILogbookViewModel wFLogbookViewModel = source as HMILogbookViewModel;

			return first.Concat(new List<JProperty>
			{
				GetSerializedLogbookSecurityPolicies(wFLogbookViewModel.SecurityPolicies)
			});
		}
		private JProperty GetSerializedLogbookSecurityPolicies(IEnumerable<LogbookSecurityPolicy> securityPolicies)
		{
			var xElement = new List<JObject>();
			foreach (LogbookSecurityPolicy current in securityPolicies)
			{
				ISerializer serializer = SerializerFactory.GetSerializer(current.GetType());
				xElement.Add(serializer.Serialize(current));
			}
			return new JProperty("LogbookSecurityPolicy", xElement);
		}
	}
}
