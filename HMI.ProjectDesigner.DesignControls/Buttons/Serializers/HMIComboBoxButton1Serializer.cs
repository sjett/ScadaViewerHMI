// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure;
using HMI.Infrastructure.Converters;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Serialization;
using HMI.ProjectDesigner.DesignControls.Buttons.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Buttons.Serializers
{
	public class HMIComboBoxButton1Serializer : CommonControlSerializer
	{
		public HMIComboBoxButton1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIComboBoxButton1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIComboBoxButton1ViewModel node = target as HMIComboBoxButton1ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.StateSignalName = ((string?)properties["StateSignalName"]) ?? node.StateSignalName;

            node.WriteSignalName = ((string?)properties["WriteSignalName"]) ?? node.WriteSignalName;

            node.ObjectID = ((string?)properties["ObjectID"]) ?? node.ObjectID;

			node.SignalPrefix = properties["SignalPrefix"]?.ToObject<SignalPrefix?>() ?? node.SignalPrefix;

			node.EnableMask = ((int?)properties["EnableMask"]) ?? node.EnableMask;

            node.EnableSignalName = ((string?)properties["EnableSignalName"]) ?? node.EnableSignalName;

            node.VisibilityMask = ((int?)properties["VisibilityMask"]) ?? node.VisibilityMask;

            node.VisibilitySignalName = ((string?)properties["VisibilitySignalName"]) ?? node.VisibilitySignalName;

            node.SecurityAuthorizationName = ((string?)properties["SecurityAuthorizationName"]) ?? node.SecurityAuthorizationName;

			node.SecurityDenyAccessBehaviour = properties["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? node.SecurityDenyAccessBehaviour;

            node.WriteSecure = ((bool?)properties["WriteSecure"]) ?? node.WriteSecure;

            node.IsTabStop = ((bool?)properties["IsTabStop"]) ?? node.IsTabStop;

			if((string?)properties["BorderBrush"] != null)
				node.BorderBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["BorderBrush"]!)).GetAsFrozen();

            if ((string?)properties["BorderThickness"] != null)
                node.BorderThickness = (Thickness)new ThicknessToStringConverter().ConvertBack((string)properties["BorderThickness"]!, null, null, null);

            node.FontFamily = ((string?)properties["FontFamily"]) ?? node.FontFamily;

            node.FontSize = ((double?)properties["FontSize"]) ?? node.FontSize;

            node.FontStretch = ((string?)properties["FontStretch"]) ?? node.FontStretch;

            node.FontStyle = ((string?)properties["FontStyle"]) ?? node.FontStyle;

            node.FontWeight = ((string?)properties["FontWeight"]) ?? node.FontWeight;

            if ((string?)properties["Background"] != null)
                node.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Background"]!)).GetAsFrozen();

            if ((string?)properties["Foreground"] != null)
                node.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Foreground"]!)).GetAsFrozen();

            node.UseLocalColors = ((bool?)properties["UseLocalColors"]) ?? node.UseLocalColors;

		}
		protected override void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel target, System.Version version)
		{
			HMIComboBoxButton1ViewModel wFComboBoxButton1ViewModel = target as HMIComboBoxButton1ViewModel;
			base.DeserializeCollections(propertyCollectionsElement, wFComboBoxButton1ViewModel, version);

			this.DeserializeComboBoxItem(propertyCollectionsElement, wFComboBoxButton1ViewModel, wFComboBoxButton1ViewModel.WriteItems, version);
			this.DeserializeStateConditions(propertyCollectionsElement, wFComboBoxButton1ViewModel, wFComboBoxButton1ViewModel.States, version);
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIComboBoxButton1ViewModel wFComboBoxButton1ViewModel = source as HMIComboBoxButton1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFComboBoxButton1ViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("StateSignalName", wFComboBoxButton1ViewModel.StateSignalName),
				new JProperty("WriteSignalName", wFComboBoxButton1ViewModel.WriteSignalName),
				new JProperty("ObjectID", wFComboBoxButton1ViewModel.ObjectID),
				new JProperty("SignalPrefix", wFComboBoxButton1ViewModel.SignalPrefix),
				new JProperty("EnableMask", wFComboBoxButton1ViewModel.EnableMask),
				new JProperty("EnableSignalName", wFComboBoxButton1ViewModel.EnableSignalName),
				new JProperty("VisibilityMask", wFComboBoxButton1ViewModel.VisibilityMask),
				new JProperty("VisibilitySignalName", wFComboBoxButton1ViewModel.VisibilitySignalName),
				new JProperty("SecurityAuthorizationName", wFComboBoxButton1ViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFComboBoxButton1ViewModel.SecurityDenyAccessBehaviour),
				new JProperty("WriteSecure", wFComboBoxButton1ViewModel.WriteSecure),
				new JProperty("IsTabStop", wFComboBoxButton1ViewModel.IsTabStop),
				new JProperty("BorderBrush", wFComboBoxButton1ViewModel.BorderBrush?.ToString()),
				new JProperty("BorderThickness", wFComboBoxButton1ViewModel.BorderThickness.ToString()),
				new JProperty("FontFamily", wFComboBoxButton1ViewModel.FontFamily),
				new JProperty("FontSize", wFComboBoxButton1ViewModel.FontSize),
				new JProperty("FontStretch", wFComboBoxButton1ViewModel.FontStretch),
				new JProperty("FontStyle", wFComboBoxButton1ViewModel.FontStyle),
				new JProperty("FontWeight", wFComboBoxButton1ViewModel.FontWeight),
				new JProperty("Background", wFComboBoxButton1ViewModel.Background?.ToString()),
				new JProperty("Foreground", wFComboBoxButton1ViewModel.Foreground?.ToString()),
				new JProperty("UseLocalColors", wFComboBoxButton1ViewModel.UseLocalColors)
			});
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
		{
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeCollections(source);
			HMIComboBoxButton1ViewModel wFComboBoxButton1ViewModel = source as HMIComboBoxButton1ViewModel;

			System.Collections.Generic.List<JProperty> second = new System.Collections.Generic.List<JProperty>
			{
				this.GetSerializedComboBoxItem(wFComboBoxButton1ViewModel.WriteItems),
				this.GetSerializedStateConditions(wFComboBoxButton1ViewModel.States)
			};

			return first.Concat(second);
		}
	}
}
