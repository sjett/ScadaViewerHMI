// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Primitives.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.Primitives.Serializers
{
	public class HMIRectangle1Serializer : HMIPrimitiveSerializer
	{
        public HMIRectangle1Serializer(ISerializerFactory serializerFactory, IControlFactory controlFactory, IServiceProvider container, IProjectManagerService projectManagerService) 
			: base(serializerFactory, controlFactory, container, projectManagerService)
        {
        }
        protected override ICommonControlViewModel CreateNode(Type controlType, IExplorerNodeViewModel parentNode)
        {
            return new HMIRectangle1ViewModel(base.Container, parentNode, controlType);
        }

        protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, Version version)
		{
			HMIRectangle1ViewModel node = target as HMIRectangle1ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.RadiusX = ((double?)properties["RadiusX"]) ?? node.RadiusX;

			node.RadiusY = ((double?)properties["RadiusY"]) ?? node.RadiusY;

		}
		protected override IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIRectangle1ViewModel wFRectangle1ViewModel = source as HMIRectangle1ViewModel;
            IEnumerable<JProperty> first = base.SerializeProperties(wFRectangle1ViewModel);

			return first.Concat(new List<JProperty>
            {
				new JProperty("RadiusX", wFRectangle1ViewModel.RadiusX),
				new JProperty("RadiusY", wFRectangle1ViewModel.RadiusY)
			});
		}
		//protected override XElement UpgradeXml(XElement root, Version version)
		//{
		//	if (version < new Version(3, 3, 12, 0))
		//	{
		//		root = base.UpgradeXml(root, version);
		//		string wFRectangle1Update_v1_0__to__v3_3_12_ = UpgradeTemplates.HMIRectangle1Update_v1_0__to__v3_3_12_0;
		//		root = base.TransformXmlElement(root, wFRectangle1Update_v1_0__to__v3_3_12_, version);
		//	}
		//	return base.UpgradeXml(root, version);
		//}

	}
}
