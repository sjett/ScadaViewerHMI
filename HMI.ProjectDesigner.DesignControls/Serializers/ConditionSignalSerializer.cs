// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.Infrastructure.Serialization
{
	public class ConditionSignalSerializer : BaseSerializer
	{
		public ConditionSignalSerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		public override JObject Serialize(object source)
		{
			ConditionSignal conditionSignal = source as ConditionSignal;

			return new JObject(
					new JProperty("ItemElementName","ConditionSignalItem"), 
					new JProperty("Properties", new JObject(
						new JProperty("Attributes", new JObject(
							new JProperty("ParameterName", conditionSignal.ParameterName),
							new JProperty("SignalName", conditionSignal.SignalName)
						)))));
		}
		protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, System.Version version)
		{
			var xElement = root["Properties"]!["Attributes"]!;

			ConditionSignal conditionSignal = new ConditionSignal(base.Container, ancestorNode);

			conditionSignal.ParameterName = ((string?)xElement["ParameterName"]) ?? conditionSignal.ParameterName;

            conditionSignal.SignalName = ((string?)xElement["SignalName"]) ?? conditionSignal.SignalName;

			return conditionSignal;
		}
	}
}
