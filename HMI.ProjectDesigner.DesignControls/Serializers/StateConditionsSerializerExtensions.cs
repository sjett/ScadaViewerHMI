// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.Infrastructure.Serialization
{
	public static class StateConditionsSerializerExtensions
	{
		public static JProperty GetSerializedStateConditions(this BaseSerializer serializer, System.Collections.Generic.ICollection<StateCondition> stateConditions)
		{
			var xElement = new List<JObject>();
			foreach (StateCondition current in stateConditions)
			{
				ISerializer serializer2 = serializer.SerializerFactory.GetSerializer(current.GetType());
				xElement.Add(serializer2.Serialize(current));
			}

			return  new JProperty("States", xElement);
		}
		public static void DeserializeStateConditions(this BaseSerializer serializer, JObject propertyCollectionsElement, IExplorerNodeViewModel node, System.Collections.Generic.ICollection<StateCondition> states, System.Version version)
		{
			var xElement = propertyCollectionsElement["States"]?.ToArray();
			if (xElement == null)
				return;

			states.Clear();
			ISerializer serializer2 = serializer.SerializerFactory.GetSerializer<StateCondition>();
			foreach (var current in xElement)
            {
                if (((string?)current["ItemElementName"]) != "HMIStateCondition")
                    continue;

                StateCondition? stateCondition = serializer2.Deserialize(node, (JObject)current, version) as StateCondition;
				if (stateCondition != null)
				{
					states.Add(stateCondition);
				}
			}
		}
	}
}
