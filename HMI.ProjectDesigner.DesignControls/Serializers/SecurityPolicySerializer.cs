// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Serialization;
using HMI.ProjectDesigner.DesignControls.Models;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Serializers
{
	public class SecurityPolicySerializer : ItemSecuritySettingSerializer
	{
		protected override string TagName
		{
			get
			{
				return "SecurityPolicy";
			}
		}
		public SecurityPolicySerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		protected override ItemSecuritySetting CreateItem(IServiceProvider container, IExplorerNodeViewModel ancestorNode)
		{
			return new SecurityPolicy(container, ancestorNode);
		}
		protected override void DeserializeItemAttributes(JObject attributesElement, ItemSecuritySetting item)
		{
			base.DeserializeItemAttributes(attributesElement, item);
			SecurityPolicy securityPolicy = item as SecurityPolicy;

			securityPolicy.Action = attributesElement["Button"]?.ToObject<AlarmViewerSecuredAction?>() ?? securityPolicy.Action;
            
		}
		protected override System.Collections.Generic.ICollection<JProperty> GetSerializedAttributes(ItemSecuritySetting item)
		{
			System.Collections.Generic.ICollection<JProperty> serializedAttributes = base.GetSerializedAttributes(item);
			SecurityPolicy securityPolicy = item as SecurityPolicy;

			serializedAttributes.Add(new JProperty("Button", securityPolicy.Action));
			return serializedAttributes;
		}
	}
}
