// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Alarms.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Alarms.Serializers
{
	public class HMIMachineAvailability1Serializer : HMIAlarmStatisticBaseSerializer
	{
		public HMIMachineAvailability1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIMachineAvailability1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject properties, ICommonControlViewModel target, System.Version version)
		{
			HMIMachineAvailability1ViewModel node = target as HMIMachineAvailability1ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.ObjectID = ((string?)properties["ObjectID"]) ?? node.ObjectID;

			node.SignalPrefix = properties["SignalPrefix"]?.ToObject<SignalPrefix?>() ?? node.SignalPrefix;

            node.SignalName = ((string?)properties["SignalName"]) ?? node.SignalName;
            
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIMachineAvailability1ViewModel wFMachineAvailability1ViewModel = source as HMIMachineAvailability1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFMachineAvailability1ViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("ObjectID", wFMachineAvailability1ViewModel.ObjectID),
				new JProperty("SignalPrefix", wFMachineAvailability1ViewModel.SignalPrefix),
				new JProperty("SignalName", wFMachineAvailability1ViewModel.SignalName)
			});
		}
	}
}
