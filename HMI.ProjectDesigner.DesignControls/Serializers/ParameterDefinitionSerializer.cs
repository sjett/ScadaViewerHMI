// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.Serializers
{
	public class ParameterDefinitionSerializer : BaseSerializer
	{
		public ParameterDefinitionSerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		public override JObject Serialize(object source)
		{
			ParameterDefinition parameterDefinition = source as ParameterDefinition;
			string name = source.GetType().Name;

			return new JObject(
					new JProperty("LocalName", name),
					new JProperty("Id", parameterDefinition.Id),
					new JProperty("Name", parameterDefinition.Name),
					new JProperty("DefaultValue", parameterDefinition.DefaultValue ?? string.Empty)
				);
		}
		protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, System.Version version)
		{
			string localName = (string)root["LocalName"];
			if (localName == null)
			{
				throw new System.NotSupportedException();
			}

			ParameterDefinition definition = localName switch
			{
				"SignalParameterDefinition" => new SignalParameterDefinition(base.Container, ancestorNode),
				"SymbolicTextParameterDefinition" => new SymbolicTextParameterDefinition(base.Container, ancestorNode),
				"AlarmGroupParameterDefinition" => new AlarmGroupParameterDefinition(base.Container, ancestorNode),
				"AlarmTypeParameterDefinition" => new AlarmTypeParameterDefinition(base.Container, ancestorNode),
				_ => throw new System.NotSupportedException()
			};
			definition.Id = System.Guid.NewGuid();
			definition.Name = ((string?)root["Name"]) ?? definition.Name;
			definition.Id = ((Guid?)root["Id"]) ?? definition.Id;
			definition.DefaultValue = ((string?)root["DefaultValue"]) ?? definition.DefaultValue;
			return definition;
		}

		//protected override XElement UpgradeXml(XElement root, System.Version version)
		//{
		//	if (version < new System.Version(2, 0))
		//	{
		//		root = base.UpgradeXml(root, version);
		//		string signalParameterDefinitionUpdate_v1_0__to__v2_ = SerializationUpdateTemplates.SignalParameterDefinitionUpdate_v1_0__to__v2_0;
		//		root = base.TransformXmlElement(root, signalParameterDefinitionUpdate_v1_0__to__v2_, version);
		//	}
		//	return root;
		//}

	}
}
