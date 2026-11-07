// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.Infrastructure.Serialization
{
	public static class AnimatedStateConditionsSerializerExtensions
	{
		public static void DeserializeAnimatedStateConditions(this BaseSerializer serializer, JObject propertyCollectionsElement, IExplorerNodeViewModel node, System.Collections.Generic.ICollection<AnimatedStateCondition> animatedStates, System.Version version)
		{
			var xElement = propertyCollectionsElement["AnimatedStates"]?.ToArray();
            if (xElement == null)
                return;

            animatedStates.Clear();
			ISerializer serializer2 = serializer.SerializerFactory.GetSerializer<AnimatedStateCondition>();
			foreach (var current in xElement)
			{
				if (((string?)current["ItemElementName"]) != "HMIAnimatedStateCondition")
					continue;

                AnimatedStateCondition? animatedStateCondition = serializer2.Deserialize(node, (JObject)current, version) as AnimatedStateCondition;
				if (animatedStateCondition != null)
				{
					animatedStates.Add(animatedStateCondition);
				}
			}
		}
		public static JProperty GetSerializedAnimatedStateConditions(this BaseSerializer serializer, System.Collections.Generic.ICollection<AnimatedStateCondition> animatedStateConditions)
		{
			var xElement = new List<JObject>();
			foreach (AnimatedStateCondition current in animatedStateConditions)
			{
				ISerializer serializer2 = serializer.SerializerFactory.GetSerializer(current.GetType());
				xElement.Add(serializer2.Serialize(current));
			}
			return new JProperty("AnimatedStates", xElement);
		}
	}
}
