// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Media;
namespace HMI.Infrastructure.Serialization
{
	public class StateConditionSerializer : BaseSerializer
	{
		protected virtual string ItemElementName
		{
			get
			{
				return "HMIStateCondition";
			}
		}
		public StateConditionSerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		public override JObject Serialize(object source)
		{
			StateCondition? stateCondition = source as StateCondition;

			return new JObject(
					new JProperty("ItemElementName", this.ItemElementName),
					new JProperty("Properties", new JObject(
						new JProperty("Attributes", this.GetSerializedAttributes(source)),
						new JProperty("Collections", new JObject(
							new JProperty("ConditionParameters", new JObject(
								new JProperty("HMIStateConditionParameter", new JObject(
									new JProperty("Properties", new JObject(
										new JProperty("Attributes", new JObject(
											new JProperty("ParameterName", "$Input1"),
											new JProperty("SignalName", stateCondition.SignalName)
											)))))))))))));
		}
		protected virtual JObject GetSerializedAttributes(object source)
		{
			StateCondition stateCondition = source as StateCondition;
			return new JObject
			{
				new JProperty("Background", stateCondition.Background?.ToString()),
				new JProperty("AlternateBackground", stateCondition.AlternateBackground?.ToString()),
				new JProperty("Condition", stateCondition.Condition),
				new JProperty("Foreground", stateCondition.Foreground?.ToString()),
				new JProperty("AlternateForeground", stateCondition.AlternateForeground?.ToString()),
				new JProperty("IsBlinking", stateCondition.IsBlinking),
				new JProperty("BlinkingFrequency", stateCondition.BlinkingFrequency),
				new JProperty("StateName", stateCondition.StateName),
				new JProperty("ComparatorInternal", stateCondition.Comparator),
				new JProperty("SignalValueInternal", stateCondition.SignalValue),
				new JProperty("IsVisible", stateCondition.IsVisible),
				new JProperty("IsEnabled", stateCondition.IsEnabled)
			};
		}
		protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, System.Version version)
		{
			StateCondition state = this.CreateNewItem(ancestorNode);

			var container = root["Properties"] as JObject;

			var attributesElement = container!["Attributes"] as JObject;
			this.DeserializeAttributes(attributesElement!, state, root);

			var signalname = ((string?)container?["Collections"]?["ConditionParameters"]?["HMIStateConditionParameter"]?["Properties"]?["Attributes"]?["SignalName"]);
			state.SignalName = signalname ?? state.SignalName;

			return state;
		}
		protected virtual StateCondition CreateNewItem(IExplorerNodeViewModel ancestorNode)
		{
			return new StateCondition(base.Container, ancestorNode);
		}
		protected virtual void DeserializeAttributes(JObject attributesElement, StateCondition state, JObject root)
		{
			if ((string?)attributesElement["Background"] != null)
				state.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString(((string)attributesElement["Background"]!))).GetAsFrozen();

			if ((string?)attributesElement["AlternateBackground"] != null)
				state.AlternateBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString(((string)attributesElement["AlternateBackground"]!))).GetAsFrozen();

            state.Condition = ((string?)attributesElement["Condition"]) ?? state.Condition;

            if ((string?)attributesElement["Foreground"] != null)
                state.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString(((string)attributesElement["Foreground"]!))).GetAsFrozen();

            if ((string?)attributesElement["AlternateForeground"] != null)
                state.AlternateForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString(((string)attributesElement["AlternateForeground"]!))).GetAsFrozen();

            state.IsBlinking = ((bool?)attributesElement["IsBlinking"]) ?? state.IsBlinking;

			state.IsVisible = ((bool?)attributesElement["IsVisible"]) ?? state.IsVisible;

			state.IsEnabled = ((bool?)attributesElement["IsEnabled"]) ?? state.IsEnabled;

			state.BlinkingFrequency = attributesElement["BlinkingFrequency"]?.ToObject<StateBlinkingFrequency?>() ?? state.BlinkingFrequency;

			state.StateName = ((string?)attributesElement["StateName"]) ?? state.StateName;


			if (attributesElement["ComparatorInternal"] == null)
			{
				attributesElement = root["MetadataProperties"]?["Attributes"] as JObject;
			}

			state.Comparator = attributesElement["ComparatorInternal"]?.ToObject<StatesConditionComparator?>() ?? state.Comparator;

			state.SignalValue = ((string?)attributesElement["SignalValueInternal"]) ?? state.SignalValue;
		}

		//protected override JObject UpgradeXml(JObject root, System.Version version)
		//{
		//	if (version < new System.Version(2, 0))
		//	{
		//		root = base.UpgradeXml(root, version);
		//		string stateConditionUpdate_v1_0__to__v2_ = SerializationUpdateTemplates.StateConditionUpdate_v1_0__to__v2_0;
		//		root = base.TransformXmlElement(root, stateConditionUpdate_v1_0__to__v2_, version);
		//	}
		//	return root;
		//}
	}
}
