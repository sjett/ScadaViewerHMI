// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Buttons.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.Buttons.Serializers
{
	public class HMIStatesButton1Serializer : HMIStatesButtonBaseSerializer
	{
		public HMIStatesButton1Serializer(ISerializerFactory serializerFactory, IControlFactory controlFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, controlFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIStatesButton1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIStatesButton1ViewModel node = target as HMIStatesButton1ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.IconImage = ((string?)properties!["IconImage"]) ?? node.IconImage;

            node.Value = ((double?)properties["Value"]) ?? node.Value;
            
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIStatesButton1ViewModel wFStatesButton1ViewModel = source as HMIStatesButton1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFStatesButton1ViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("IconImage", wFStatesButton1ViewModel.IconImage),
				new JProperty("Value", wFStatesButton1ViewModel.Value)
			});
		}
	}
}
