// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Serialization
{
	public class LogbookSecurityPolicySerializer : ItemSecuritySettingSerializer
	{
		protected override string TagName
		{
			get
			{
				return "LogbookSecurityPolicy";
			}
		}
		public LogbookSecurityPolicySerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		protected override ItemSecuritySetting CreateItem(IServiceProvider container, IExplorerNodeViewModel ancestorNode)
		{
			return new LogbookSecurityPolicy(container, ancestorNode);
		}
		protected override void DeserializeItemAttributes(JObject attributesElement, ItemSecuritySetting item)
		{
			base.DeserializeItemAttributes(attributesElement, item);
			LogbookSecurityPolicy logbookSecurityPolicy = item as LogbookSecurityPolicy;

			logbookSecurityPolicy.SecuredButton = attributesElement["SecuredButton"]?.ToObject<LogbookSecuredButtons?>() ?? logbookSecurityPolicy.SecuredButton;
            
		}
		protected override System.Collections.Generic.ICollection<JProperty> GetSerializedAttributes(ItemSecuritySetting item)
		{
			System.Collections.Generic.ICollection<JProperty> serializedAttributes = base.GetSerializedAttributes(item);

			LogbookSecurityPolicy logbookSecurityPolicy = item as LogbookSecurityPolicy;
			serializedAttributes.Add(new JProperty("SecuredButton", logbookSecurityPolicy.SecuredButton));
			return serializedAttributes;
		}
	}
}
