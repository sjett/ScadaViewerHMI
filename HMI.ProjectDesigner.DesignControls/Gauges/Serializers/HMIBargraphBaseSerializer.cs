// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Linq;
using HMI.ProjectDesigner.DesignControls.Gauges.ViewModels;
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.Gauges.Serializers
{
	public class HMIBargraphBaseSerializer : CommonControlSerializer
	{
		public HMIBargraphBaseSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIBargraphBaseViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIBargraphBaseViewModel node = target as HMIBargraphBaseViewModel;
			base.DeserializeProperties(properties, node, version);

			node.ObjectID = ((string?)properties["ObjectID"]) ?? node.ObjectID;

            node.SignalPrefix = properties["SignalPrefix"]?.ToObject<SignalPrefix?>() ?? node.SignalPrefix;

            node.SignalName = ((string?)properties["SignalName"]) ?? node.SignalName;

            node.ShowScale = ((bool?)properties["Scale"]) ?? node.ShowScale;

            node.RangeMin = ((double?)properties["RangeMin"]) ?? node.RangeMin;

            node.RangeMax = ((double?)properties["RangeMax"]) ?? node.RangeMax;

            node.SecurityAuthorizationName = ((string?)properties["SecurityAuthorizationName"]) ?? node.SecurityAuthorizationName;

            node.SecurityDenyAccessBehaviour = properties["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? node.SecurityDenyAccessBehaviour;

            node.VisibilitySignalName = ((string?)properties["VisibilitySignalName"]) ?? node.VisibilitySignalName;

            node.VisibilityMask = ((int?)properties["VisibilityMask"]) ?? node.VisibilityMask;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIBargraphBaseViewModel wFBargraphBaseViewModel = source as HMIBargraphBaseViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFBargraphBaseViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("ObjectID", wFBargraphBaseViewModel.ObjectID),
				new JProperty("SignalPrefix", wFBargraphBaseViewModel.SignalPrefix),
				new JProperty("SignalName", wFBargraphBaseViewModel.SignalName),
				new JProperty("Scale", wFBargraphBaseViewModel.ShowScale),
				new JProperty("RangeMin", wFBargraphBaseViewModel.RangeMin),
				new JProperty("RangeMax", wFBargraphBaseViewModel.RangeMax),
				new JProperty("SecurityAuthorizationName", wFBargraphBaseViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFBargraphBaseViewModel.SecurityDenyAccessBehaviour),
				new JProperty("VisibilitySignalName", wFBargraphBaseViewModel.VisibilitySignalName),
				new JProperty("VisibilityMask", wFBargraphBaseViewModel.VisibilityMask)
			});
		}
	}
}
