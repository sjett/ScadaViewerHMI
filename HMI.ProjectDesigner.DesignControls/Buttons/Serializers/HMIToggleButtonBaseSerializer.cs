// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Linq;
using HMI.ProjectDesigner.DesignControls.Buttons.ViewModels;
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.Buttons.Serializers
{
	public abstract class HMIToggleButtonBaseSerializer : CommonControlSerializer
	{
		public HMIToggleButtonBaseSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override void DeserializeProperties(JObject properties, ICommonControlViewModel target, System.Version version)
		{
			HMIToggleButtonBaseViewModel node = target as HMIToggleButtonBaseViewModel;
			base.DeserializeProperties(properties, node, version);

			node.ObjectID = ((string?)properties["ObjectID"]) ?? node.ObjectID;

			node.SignalPrefix = properties["SignalPrefix"]?.ToObject<SignalPrefix?>() ?? node.SignalPrefix;

            node.SignalName = ((string?)properties["SignalName"]) ?? node.SignalName;

            node.UseSynchronizedToggling = ((bool?)properties["UseSynchronizedToggling"]) ?? node.UseSynchronizedToggling;

            node.Value1 = ((double?)properties["Value1"]) ?? node.Value1;

            node.Value2 = ((double?)properties["Value2"]) ?? node.Value2;

            node.WriteSecure = ((bool?)properties["WriteSecure"]) ?? node.WriteSecure;

            node.SecurityAuthorizationName = ((string?)properties["SecurityAuthorizationName"]) ?? node.SecurityAuthorizationName;

            node.SecurityDenyAccessBehaviour = properties["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? node.SecurityDenyAccessBehaviour;

            node.EnableMask = ((int?)properties["EnableMask"]) ?? node.EnableMask;

            node.EnableSignalName = ((string?)properties["EnableSignalName"]) ?? node.EnableSignalName;

            node.VisibilityMask = ((int?)properties["VisibilityMask"]) ?? node.VisibilityMask;

            node.VisibilitySignalName = ((string?)properties["VisibilitySignalName"]) ?? node.VisibilitySignalName;

            node.IsTipModeEnabled = ((bool?)properties["IsTipModeEnabled"]) ?? node.IsTipModeEnabled;

		}

		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIToggleButtonBaseViewModel wFToggleButtonBaseViewModel = source as HMIToggleButtonBaseViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFToggleButtonBaseViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("ObjectID", wFToggleButtonBaseViewModel.ObjectID),
				new JProperty("SignalPrefix", wFToggleButtonBaseViewModel.SignalPrefix),
				new JProperty("SignalName", wFToggleButtonBaseViewModel.SignalName),
				new JProperty("UseSynchronizedToggling", wFToggleButtonBaseViewModel.UseSynchronizedToggling),
				new JProperty("Value1", wFToggleButtonBaseViewModel.Value1),
				new JProperty("Value2", wFToggleButtonBaseViewModel.Value2),
				new JProperty("WriteSecure", wFToggleButtonBaseViewModel.WriteSecure),
				new JProperty("SecurityAuthorizationName", wFToggleButtonBaseViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFToggleButtonBaseViewModel.SecurityDenyAccessBehaviour),
				new JProperty("EnableMask", wFToggleButtonBaseViewModel.EnableMask),
				new JProperty("EnableSignalName", wFToggleButtonBaseViewModel.EnableSignalName),
				new JProperty("VisibilityMask", wFToggleButtonBaseViewModel.VisibilityMask),
				new JProperty("VisibilitySignalName", wFToggleButtonBaseViewModel.VisibilitySignalName),
				new JProperty("IsTipModeEnabled", wFToggleButtonBaseViewModel.IsTipModeEnabled)
			});
		}
	}
}
