// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Presentation.Serialization
{
	public class NavigationParameterSerializer : BaseSerializer
	{
		public NavigationParameterSerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		public override JObject Serialize(object source)
		{
			NavigationParameter navigationParameter = source as NavigationParameter;
			return new JObject(
					//new JProperty("ParameterItem", new JObject(
						new JProperty("Properties", new JObject(
							new JProperty("Attributes", new JObject(
								new JProperty("ParameterName", navigationParameter.Name),
								new JProperty("ParameterValue", navigationParameter.Value)
								)))));
		}
		protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, System.Version version)
		{
			var xElement = root["Properties"]?["Attributes"];
			NavigationParameter navigationParameter = new NavigationParameter(base.Container, ancestorNode);

			navigationParameter.Name = ((string?)xElement?["ParameterName"]) ?? navigationParameter.Name;

			navigationParameter.Value = ((string?)xElement?["ParameterValue"]) ?? navigationParameter.Value;

			return navigationParameter;
		}
	}
}
