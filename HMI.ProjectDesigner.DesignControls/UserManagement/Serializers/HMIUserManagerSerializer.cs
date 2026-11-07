// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using HMI.ProjectDesigner.DesignControls.UserManagement.ViewModels;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.UserManagement.Serializers
{
	public class HMIUserManagerSerializer : CommonControlSerializer
	{
		public HMIUserManagerSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIUserManagerViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel node, System.Version version)
		{
			base.DeserializeProperties(properties, node, version);
			HMIUserManagerViewModel target = node as HMIUserManagerViewModel;

			target.EditPasswordPolicyIsVisible = ((bool?)properties["EditPasswordPolicyIsVisible"]) ?? target.EditPasswordPolicyIsVisible;

            target.EditUserDefaultSettingsIsVisible = ((bool?)properties["EditUserDefaultSettingsIsVisible"]) ?? target.EditUserDefaultSettingsIsVisible;

			if (((string?)properties["DomainUserGridRowBackground"]) != null)
				target.DomainUserGridRowBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["DomainUserGridRowBackground"]!)).GetAsFrozen();

            if (((string?)properties["DomainUserGridRowForeground"]) != null)
                target.DomainUserGridRowForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["DomainUserGridRowForeground"]!)).GetAsFrozen();

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(source);
			HMIUserManagerViewModel wFUserManagerViewModel = source as HMIUserManagerViewModel;
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("EditPasswordPolicyIsVisible", wFUserManagerViewModel.EditPasswordPolicyIsVisible),
				new JProperty("EditUserDefaultSettingsIsVisible", wFUserManagerViewModel.EditUserDefaultSettingsIsVisible),
				new JProperty("DomainUserGridRowBackground", wFUserManagerViewModel.DomainUserGridRowBackground?.ToString()),
				new JProperty("DomainUserGridRowForeground", wFUserManagerViewModel.DomainUserGridRowForeground?.ToString())
			});
		}
	}
}
