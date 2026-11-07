// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Isa.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Isa.Serializers
{
	
	public class HMIISAFlap4Serializer : HMIISABaseSerializer
	{
		public HMIISAFlap4Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIISAFlap4ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIISAFlap4ViewModel node = target as HMIISAFlap4ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.SignalName = ((string?)properties["SignalName"]) ?? node.SignalName;

            node.MinValue = ((double?)properties["MinValue"]) ?? node.MinValue;

            node.MaxValue = ((double?)properties["MaxValue"]) ?? node.MaxValue;

            node.MinAngle = ((double?)properties["MinAngle"]) ?? node.MinAngle;

            node.MaxAngle = ((double?)properties["MaxAngle"]) ?? node.MaxAngle;

            node.OffsetAngle = ((double?)properties["OffsetAngle"]) ?? node.OffsetAngle;


		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIISAFlap4ViewModel wFISAFlap4ViewModel = source as HMIISAFlap4ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFISAFlap4ViewModel);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("SignalName", wFISAFlap4ViewModel.SignalName),
				new JProperty("MinValue", wFISAFlap4ViewModel.MinValue),
				new JProperty("MaxValue", wFISAFlap4ViewModel.MaxValue),
				new JProperty("MinAngle", wFISAFlap4ViewModel.MinAngle),
				new JProperty("MaxAngle", wFISAFlap4ViewModel.MaxAngle),
				new JProperty("OffsetAngle", wFISAFlap4ViewModel.OffsetAngle)
			});
		}
	}
}
