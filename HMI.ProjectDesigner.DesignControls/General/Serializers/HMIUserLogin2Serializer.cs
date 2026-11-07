// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.General.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.General.Serializers
{
	public class HMIUserLogin2Serializer : CommonControlSerializer
	{
		public HMIUserLogin2Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIUserLogin2ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIUserLogin2ViewModel node = target as HMIUserLogin2ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.Password = ((string?)properties["Password"]) ?? node.Password;

            node.UserPrefix = properties["UserPrefix"]?.ToObject<UserPrefixType?>() ?? node.UserPrefix;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIUserLogin2ViewModel wFUserLogin2ViewModel = source as HMIUserLogin2ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFUserLogin2ViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("Password", wFUserLogin2ViewModel.Password),
				new JProperty("UserPrefix", wFUserLogin2ViewModel.UserPrefix)
			});
		}
	}
}
