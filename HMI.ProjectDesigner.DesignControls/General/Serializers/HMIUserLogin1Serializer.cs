// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.General.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.General.Serializers
{
	public class HMIUserLogin1Serializer : CommonControlSerializer
	{
		public HMIUserLogin1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIUserLogin1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIUserLogin1ViewModel node = target as HMIUserLogin1ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.PopupModality = properties["PopupModality"]?.ToObject<DialogModality?>() ?? node.PopupModality;

			node.ShowConfirmation = ((bool?)properties["ShowConfirmation"]) ?? node.ShowConfirmation;

            node.InitialUserIsDomainUser = ((bool?)properties["InitialUserIsDomainuser"]) ?? node.InitialUserIsDomainUser;

            node.CanConnectWithDomainUser = ((bool?)properties["CanConnectWithDomainUser"]) ?? node.CanConnectWithDomainUser;

            node.AutoLoginAtStartup = properties["AutoLoginAtStartup"]?.ToObject<AutoLoginType?>() ?? node.AutoLoginAtStartup;

            node.AutoLoginAfterLogout = properties["AutoLoginAfterLogout"]?.ToObject<AutoLoginType?>() ?? node.AutoLoginAfterLogout;

            node.AlwaysShowErrorMessages = ((bool?)properties["AlwaysShowErrorMessages"]) ?? node.AlwaysShowErrorMessages;

            node.InitialUserName = ((string?)properties["InitialUserName"]) ?? node.InitialUserName;

            node.InitialUserPassword = ((string?)properties["InitialUserPassword"]) ?? node.InitialUserPassword;

            node.CanChangePassword = ((bool?)properties["CanChangePassword"]) ?? node.CanChangePassword;

            node.InitialUserRuntimeCheck = ((bool?)properties["InitialUserRuntimeCheck"]) ?? node.InitialUserRuntimeCheck;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIUserLogin1ViewModel wFUserLogin1ViewModel = source as HMIUserLogin1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFUserLogin1ViewModel);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("PopupModality", wFUserLogin1ViewModel.PopupModality),
				new JProperty("ShowConfirmation", wFUserLogin1ViewModel.ShowConfirmation),
				new JProperty("InitialUserIsDomainuser", wFUserLogin1ViewModel.InitialUserIsDomainUser),
				new JProperty("CanConnectWithDomainUser", wFUserLogin1ViewModel.CanConnectWithDomainUser),
				new JProperty("AutoLoginAtStartup", wFUserLogin1ViewModel.AutoLoginAtStartup),
				new JProperty("AutoLoginAfterLogout", wFUserLogin1ViewModel.AutoLoginAfterLogout),
				new JProperty("AlwaysShowErrorMessages", wFUserLogin1ViewModel.AlwaysShowErrorMessages),
				new JProperty("InitialUserName", wFUserLogin1ViewModel.InitialUserName),
				new JProperty("InitialUserPassword", wFUserLogin1ViewModel.InitialUserPassword),
				new JProperty("CanChangePassword", wFUserLogin1ViewModel.CanChangePassword),
				new JProperty("InitialUserRuntimeCheck", wFUserLogin1ViewModel.InitialUserRuntimeCheck)
			});
		}
	}
}
