// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Converters;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Primitives.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Primitives.Serializers
{
	public class HMIAnimatedRectangle1Serializer : HMIAnimatedPrimitiveSerializer
	{
        public HMIAnimatedRectangle1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService)
			: base(serializerFactory, container, projectManagerService)
        {
        }
        protected override ICommonControlViewModel CreateNode(Type controlType, IExplorerNodeViewModel parentNode)
        {
            return new HMIAnimatedRectangle1ViewModel(base.Container, parentNode, controlType);
        }
        protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, Version version)
		{
			HMIAnimatedRectangle1ViewModel node = target as HMIAnimatedRectangle1ViewModel;
			base.DeserializeProperties(properties, node, version);

            if ((string?)properties!["CornerRadius"] != null)
                node.CornerRadius = (CornerRadius)new CornerRadiusToStringConverter().ConvertBack((string)properties["CornerRadius"]!, null, null, null);

		}
		protected override IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIAnimatedRectangle1ViewModel wFAnimatedRectangle1ViewModel = source as HMIAnimatedRectangle1ViewModel;
            IEnumerable<JProperty> first = base.SerializeProperties(wFAnimatedRectangle1ViewModel);

			return first.Concat(new List<JProperty>
            {
				new JProperty("CornerRadius", wFAnimatedRectangle1ViewModel.CornerRadius.ToString())
			});
		}

	}
}
