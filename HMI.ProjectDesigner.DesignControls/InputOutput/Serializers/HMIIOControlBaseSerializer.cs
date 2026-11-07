// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.InputOutput.Serializers
{
	public abstract class HMIIOControlBaseSerializer : CommonControlSerializer
	{
		public HMIIOControlBaseSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIIOControlBaseViewModel node = target as HMIIOControlBaseViewModel;
			base.DeserializeProperties(properties, node, version);

			node.ObjectID = ((string?)properties["ObjectID"]) ?? node.ObjectID;

			node.SignalPrefix = properties["SignalPrefix"]?.ToObject<SignalPrefix?>() ?? node.SignalPrefix;

            node.SignalName = ((string?)properties["SignalName"]) ?? node.SignalName;

            node.SecurityAuthorizationName = ((string?)properties["SecurityAuthorizationName"]) ?? node.SecurityAuthorizationName;

            node.SecurityDenyAccessBehaviour = properties["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? node.SecurityDenyAccessBehaviour;

            node.VisibilitySignalName = ((string?)properties["VisibilitySignalName"]) ?? node.VisibilitySignalName;

            node.VisibilityMask = ((int?)properties["VisibilityMask"]) ?? node.VisibilityMask;

            node.FontFamily = ((string?)properties["FontFamily"]) ?? node.FontFamily;

            node.FontSize = ((double?)properties["FontSize"]) ?? node.FontSize;

            node.FontStretch = ((string?)properties["FontStretch"]) ?? node.FontStretch;

            node.FontStyle = ((string?)properties["FontStyle"]) ?? node.FontStyle;

            node.FontWeight = ((string?)properties["FontWeight"]) ?? node.FontWeight;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIIOControlBaseViewModel wFIOControlBaseViewModel = source as HMIIOControlBaseViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFIOControlBaseViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("ObjectID", wFIOControlBaseViewModel.ObjectID),
				new JProperty("SignalPrefix", wFIOControlBaseViewModel.SignalPrefix),
				new JProperty("SignalName", wFIOControlBaseViewModel.SignalName),
				new JProperty("SecurityAuthorizationName", wFIOControlBaseViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFIOControlBaseViewModel.SecurityDenyAccessBehaviour),
				new JProperty("VisibilitySignalName", wFIOControlBaseViewModel.VisibilitySignalName),
				new JProperty("VisibilityMask", wFIOControlBaseViewModel.VisibilityMask),
				new JProperty("FontFamily", wFIOControlBaseViewModel.FontFamily),
				new JProperty("FontSize", wFIOControlBaseViewModel.FontSize),
				new JProperty("FontStretch", wFIOControlBaseViewModel.FontStretch),
				new JProperty("FontStyle", wFIOControlBaseViewModel.FontStyle),
				new JProperty("FontWeight", wFIOControlBaseViewModel.FontWeight)
			});
		}
	}
}
