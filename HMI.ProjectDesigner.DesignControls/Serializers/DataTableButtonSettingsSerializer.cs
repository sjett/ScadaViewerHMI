// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.Infrastructure.Serialization
{
	public class DataTableButtonSettingsSerializer : ButtonSettingsSerializer
	{
		protected override string TagName
		{
			get
			{
				return "DataTableButtonSetting";
			}
		}
		public DataTableButtonSettingsSerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		protected override ItemSecuritySetting CreateItem(IServiceProvider container, IExplorerNodeViewModel ancestorNode)
		{
			return new DataTableButtonSettings(container, ancestorNode);
		}
		protected override void DeserializeItemAttributes(JObject attributesElement, ItemSecuritySetting item)
		{
			base.DeserializeItemAttributes(attributesElement, item);
			DataTableButtonSettings dataTableItem = item as DataTableButtonSettings;

			dataTableItem.Button = attributesElement["Button"]?.ToObject<DataTableButtons?>() ?? dataTableItem.Button;
            
		}
		protected override System.Collections.Generic.ICollection<JProperty> GetSerializedAttributes(ItemSecuritySetting item)
		{
			System.Collections.Generic.ICollection<JProperty> serializedAttributes = base.GetSerializedAttributes(item);
			DataTableButtonSettings dataTableButtonSettings = item as DataTableButtonSettings;
			serializedAttributes.Add(new JProperty("Button", dataTableButtonSettings.Button));
			return serializedAttributes;
		}
	}
}
