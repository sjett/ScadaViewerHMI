// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Media;

namespace HMI.Infrastructure.Serialization
{
	public class AnimatedStateConditionSerializer : StateConditionSerializer
	{
		protected override string ItemElementName
		{
			get
			{
				return "HMIAnimatedStateCondition";
			}
		}
		public AnimatedStateConditionSerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		protected override StateCondition CreateNewItem(IExplorerNodeViewModel ancestorNode)
		{
			return new AnimatedStateCondition(base.Container, ancestorNode);
		}
		protected override JObject GetSerializedAttributes(object source)
		{
			var serializedAttributes = base.GetSerializedAttributes(source);

			AnimatedStateCondition animatedStateCondition = source as AnimatedStateCondition;

			serializedAttributes.Add(new JProperty("AnimatedFill", animatedStateCondition.AnimatedFill?.ToString()));
            serializedAttributes.Add(new JProperty("AlternateAnimatedFill", animatedStateCondition.AlternateAnimatedFill?.ToString()));
		
			return serializedAttributes;
		}
		protected override void DeserializeAttributes(JObject attributesElement, StateCondition state, JObject root)
		{
			base.DeserializeAttributes(attributesElement, state, root);

			AnimatedStateCondition animatedState = state as AnimatedStateCondition;

			if((string?)attributesElement["AnimatedFill"] != null)
				animatedState!.AnimatedFill = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)attributesElement["AnimatedFill"]!)).GetAsFrozen();

			if((string?)attributesElement["AlternateAnimatedFill"] != null)
				animatedState!.AlternateAnimatedFill = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)attributesElement["AlternateAnimatedFill"]!)).GetAsFrozen();

        }
	}
}
