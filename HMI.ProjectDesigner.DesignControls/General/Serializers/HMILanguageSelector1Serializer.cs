// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Linq;

using HMI.ProjectDesigner.DesignControls.General.ViewModels;
using HMI.Infrastructure;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.General.Serializers
{
	public class HMILanguageSelector1Serializer : CommonControlSerializer
	{
		public HMILanguageSelector1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMILanguageSelector1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMILanguageSelector1ViewModel node = target as HMILanguageSelector1ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.SecurityAuthorizationName = ((string?)properties["SecurityAuthorizationName"]) ?? node.SecurityAuthorizationName;

            node.SecurityDenyAccessBehaviour = properties["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? node.SecurityDenyAccessBehaviour;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMILanguageSelector1ViewModel wFLanguageSelector1ViewModel = source as HMILanguageSelector1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFLanguageSelector1ViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("SecurityAuthorizationName", wFLanguageSelector1ViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFLanguageSelector1ViewModel.SecurityDenyAccessBehaviour)
			});
		}
	}
}
