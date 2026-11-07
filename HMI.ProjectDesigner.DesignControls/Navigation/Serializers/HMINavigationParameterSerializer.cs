// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using HMI.ProjectDesigner.DesignControls.Navigation.ViewModels;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.Navigation.Serializers
{
	public class HMINavigationParameterSerializer : CommonControlSerializer
	{
		public HMINavigationParameterSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMINavigationParameterViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel target, System.Version version)
		{
			HMINavigationParameterViewModel wFNavigationParameterViewModel = target as HMINavigationParameterViewModel;

			base.DeserializeCollections(propertyCollectionsElement, wFNavigationParameterViewModel, version);

			this.DeserializeNavigationParameter(propertyCollectionsElement, wFNavigationParameterViewModel, wFNavigationParameterViewModel.Parameters, version);
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
		{
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeCollections(source);
			HMINavigationParameterViewModel wFNavigationParameterViewModel = source as HMINavigationParameterViewModel;
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				this.GetSerializedNavigationParameters(wFNavigationParameterViewModel.Parameters)
			});
		}
	}
}
