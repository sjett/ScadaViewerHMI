// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Serialization;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Serializers
{
	public abstract class HMIAnimatedStatesControlBaseSerializer : HMIStatesControlBaseSerializer
	{
		public HMIAnimatedStatesControlBaseSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) 
			: base(serializerFactory, container, projectManagerService)
		{
		}
		protected override void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel target, System.Version version)
		{
			base.DeserializeCollections(propertyCollectionsElement, target, version);

			HMIAnimatedStatesControlBaseViewModel wFAnimatedStatesControlBaseViewModel = target as HMIAnimatedStatesControlBaseViewModel;
			this.DeserializeAnimatedStateConditions(propertyCollectionsElement, wFAnimatedStatesControlBaseViewModel, wFAnimatedStatesControlBaseViewModel.AnimatedStates, version);
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
		{
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeCollections(source);

			HMIAnimatedStatesControlBaseViewModel? wFAnimatedStatesControlBaseViewModel = source as HMIAnimatedStatesControlBaseViewModel;
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				this.GetSerializedAnimatedStateConditions(wFAnimatedStatesControlBaseViewModel.AnimatedStates)
			});
		}
	}
}
