// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.Infrastructure.Serialization
{
	public class HMIComboBoxItemSerializer : BaseSerializer
	{
		public HMIComboBoxItemSerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		public override JObject Serialize(object source)
		{
			HMIComboBoxItem wFComboBoxItem = source as HMIComboBoxItem;

			return new JObject(
				new JProperty("ItemElementName", "HMIComboBoxItem"), 
				new JProperty("Properties", new JObject(
					new JProperty("Attributes", new JObject(
						new JProperty("SymbolicText", wFComboBoxItem.SymbolicText),
						new JProperty("Value", wFComboBoxItem.Value),
						new JProperty("Source", wFComboBoxItem.Source),
						new JProperty("Height", wFComboBoxItem.Height)
					)))));
		}
		protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, System.Version version)
		{
			var xElement = root["Properties"]!["Attributes"];
			HMIComboBoxItem comboBoxItem = new HMIComboBoxItem(base.Container, ancestorNode);

			comboBoxItem.SymbolicText = ((string?)xElement["SymbolicText"]) ?? comboBoxItem.SymbolicText;

            comboBoxItem.Value = ((int?)xElement["Value"]) ?? comboBoxItem.Value;

            comboBoxItem.Source = ((string?)xElement["Source"]) ?? comboBoxItem.Source;

            comboBoxItem.Height = ((int?)xElement["Height"]) ?? comboBoxItem.Height;

			return comboBoxItem;
		}
	}
}
