// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Indicators.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Indicators.Serializers
{
	public abstract class HMIIndicatorDynamicSerializerBase : HMIIndicatorBaseSerializer
	{
		public HMIIndicatorDynamicSerializerBase(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIIndicatorDynamicViewModelBase wFIndicatorDynamicViewModelBase = source as HMIIndicatorDynamicViewModelBase;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFIndicatorDynamicViewModelBase);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("DefaultStateXamlPath", wFIndicatorDynamicViewModelBase.DefaultStateXamlPath),
				new JProperty("StateXamlPath1", wFIndicatorDynamicViewModelBase.StateXamlPath1),
				new JProperty("StateXamlPath2", wFIndicatorDynamicViewModelBase.StateXamlPath2),
				new JProperty("StateXamlPath3", wFIndicatorDynamicViewModelBase.StateXamlPath3),
				new JProperty("StateXamlPath4", wFIndicatorDynamicViewModelBase.StateXamlPath4),
				new JProperty("StateXamlPath5", wFIndicatorDynamicViewModelBase.StateXamlPath5),
				new JProperty("StateXamlPath6", wFIndicatorDynamicViewModelBase.StateXamlPath6),
				new JProperty("StateXamlPath7", wFIndicatorDynamicViewModelBase.StateXamlPath7),
				new JProperty("StateXamlPath8", wFIndicatorDynamicViewModelBase.StateXamlPath8)
			});
		}
		protected override void DeserializeProperties(JObject properties, ICommonControlViewModel target, System.Version version)
		{
			HMIIndicatorDynamicViewModelBase? node = target as HMIIndicatorDynamicViewModelBase;
			base.DeserializeProperties(properties, node, version);

			node.DefaultStateXamlPath = ((string?)properties["DefaultStateXamlPath"]) ?? node.DefaultStateXamlPath;

            node.StateXamlPath1 = ((string?)properties["StateXamlPath1"]) ?? node.StateXamlPath1;
            node.StateXamlPath2 = ((string?)properties["StateXamlPath2"]) ?? node.StateXamlPath2;
            node.StateXamlPath3 = ((string?)properties["StateXamlPath3"]) ?? node.StateXamlPath3;
            node.StateXamlPath4 = ((string?)properties["StateXamlPath4"]) ?? node.StateXamlPath4;
            node.StateXamlPath5 = ((string?)properties["StateXamlPath5"]) ?? node.StateXamlPath5;
            node.StateXamlPath6 = ((string?)properties["StateXamlPath6"]) ?? node.StateXamlPath6;
            node.StateXamlPath7 = ((string?)properties["StateXamlPath7"]) ?? node.StateXamlPath7;
            node.StateXamlPath8 = ((string?)properties["StateXamlPath8"]) ?? node.StateXamlPath8;

		}
	}
}
