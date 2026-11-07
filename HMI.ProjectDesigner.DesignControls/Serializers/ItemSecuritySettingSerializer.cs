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
	public abstract class ItemSecuritySettingSerializer : BaseSerializer
	{
		protected abstract string TagName
		{
			get;
		}
		public ItemSecuritySettingSerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		public override JObject Serialize(object source)
		{
			ItemSecuritySetting item = source as ItemSecuritySetting;
			return new JObject(
				new JProperty("ItemElementName", this.TagName), 
				new JProperty("Properties", new JObject(
					new JProperty("Attributes", new JObject([..this.GetSerializedAttributes(item)]))
					)));
		}
		protected virtual System.Collections.Generic.ICollection<JProperty> GetSerializedAttributes(ItemSecuritySetting item)
		{
			return new System.Collections.Generic.List<JProperty>(new JProperty[]
			{
				new JProperty("IsVisible", item.IsVisible),
				new JProperty("SecurityDenyAccessBehavior", item.SecurityDenyAccessBehavior),
				new JProperty("SecurityAuthorizationName", item.SecurityAuthorizationName)
			});
		}
		protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, System.Version version)
		{
			object result;
			try
			{
				var attributesElement = root["Properties"]!["Attributes"];
				ItemSecuritySetting itemSecuritySetting = this.CreateItem(base.Container, ancestorNode);

				this.DeserializeItemAttributes((JObject)attributesElement!, itemSecuritySetting);
				result = itemSecuritySetting;
			}
			catch
			{
				result = null;
			}
			return result;
		}
		protected abstract ItemSecuritySetting CreateItem(IServiceProvider container, IExplorerNodeViewModel ancestorNode);
		protected virtual void DeserializeItemAttributes(JObject attributesElement, ItemSecuritySetting item)
		{
			item.IsVisible = ((bool?)attributesElement["IsVisible"]) ?? item.IsVisible;

			item.SecurityDenyAccessBehavior = attributesElement["SecurityDenyAccessBehavior"]?.ToObject<SecurityDenyAccessBehavior?>() ?? item.SecurityDenyAccessBehavior;


            item.SecurityAuthorizationName = ((string?)attributesElement["SecurityAuthorizationName"]) ?? item.SecurityAuthorizationName;

		}
	}
}
