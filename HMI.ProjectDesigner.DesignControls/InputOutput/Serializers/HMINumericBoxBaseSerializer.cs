// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.InputOutput.Serializers
{
	public abstract class HMINumericBoxBaseSerializer : HMITextBoxBaseSerializer
	{
		public HMINumericBoxBaseSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMINumericBoxBaseViewModel node = target as HMINumericBoxBaseViewModel;
			base.DeserializeProperties(properties, node, version);

			node.Precision = ((int?)properties["Precision"]) ?? node.Precision;
            
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMINumericBoxBaseViewModel wFNumericBoxBaseViewModel = source as HMINumericBoxBaseViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFNumericBoxBaseViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("Precision", wFNumericBoxBaseViewModel.Precision)
			});
		}
	}
}
