// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Primitives.ViewModels;
using HMI.ProjectDesigner.DesignControls.Serializers;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Primitives.Serializers
{
	public abstract class HMIPrimitiveSerializer : HMIStaticStatesControlBaseSerializer
	{
		public HMIPrimitiveSerializer(ISerializerFactory serializerFactory, IControlFactory controlFactory, IServiceProvider container, IProjectManagerService projectManagerService) 
			: base(serializerFactory, container, projectManagerService)
		{
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, Version version)
		{
			HMIPrimitiveBaseViewModel node = target as HMIPrimitiveBaseViewModel;
			base.DeserializeProperties(properties, node, version);

			node.LineThickness = ((double?)properties!["LineThickness"]) ?? node.LineThickness;
		}
		protected override IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIPrimitiveBaseViewModel wFPrimitiveBaseViewModel = source as HMIPrimitiveBaseViewModel;
            IEnumerable<JProperty> first = base.SerializeProperties(wFPrimitiveBaseViewModel);

			return first.Concat(new List<JProperty>
            {
				new JProperty("LineThickness", wFPrimitiveBaseViewModel.LineThickness)
			});
		}
	}
}
