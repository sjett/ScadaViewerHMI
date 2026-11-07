// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.Infrastructure.Serialization
{
	public static class ConditionSignalSerializationExtensions
	{
		public static void DeserializeConditionSignals(this BaseSerializer serializer, JObject propertyCollectionsElement, IExplorerNodeViewModel node, System.Collections.Generic.ICollection<ConditionSignal> conditionSignals, System.Version version)
		{
			var xElement = propertyCollectionsElement["ConditionSignals"];
			if (xElement == null)
				return;

			conditionSignals.Clear();
			ISerializer serializer2 = serializer.SerializerFactory.GetSerializer<ConditionSignal>();

			foreach (var current in xElement)
			{
				if (((string?)current["ItemElementName"]) != "ConditionSignalItem")
					continue;

				ConditionSignal? conditionSignal = serializer2.Deserialize(node, (JObject)current, version) as ConditionSignal;
				if (conditionSignal != null)
				{
					conditionSignals.Add(conditionSignal);
				}
			}
		}
		public static JProperty GetSerializedConditionSignals(this BaseSerializer serializer, System.Collections.Generic.ICollection<ConditionSignal> conditionSignals)
		{
			var xElement = new List<JObject>();
			foreach (ConditionSignal current in conditionSignals)
			{
				ISerializer serializer2 = serializer.SerializerFactory.GetSerializer(current.GetType());
				xElement.Add(serializer2.Serialize(current));
			}
			return new JProperty("ConditionSignals", xElement);
		}
	}
}
