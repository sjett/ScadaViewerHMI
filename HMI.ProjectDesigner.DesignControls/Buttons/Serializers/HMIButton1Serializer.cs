// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure;
using HMI.Infrastructure.Converters;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Buttons.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Buttons.Serializers
{
	public class HMIButton1Serializer : CommonControlSerializer
	{
		public HMIButton1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIButton1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject properties, ICommonControlViewModel target, System.Version version)
		{
			HMIButton1ViewModel node = target as HMIButton1ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.SecurityAuthorizationName = ((string?)properties["SecurityAuthorizationName"]) ?? node.SecurityAuthorizationName;

			node.SecurityDenyAccessBehaviour = properties["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? node.SecurityDenyAccessBehaviour;

            node.Value = ((double?)properties["Value"]) ?? node.Value;

            node.WriteSecure = ((bool?)properties["WriteSecure"]) ?? node.WriteSecure;

            node.SignalName = ((string?)properties["SignalName"]) ?? node.SignalName;

            node.ObjectID = ((string?)properties["ObjectID"]) ?? node.ObjectID;

            node.SignalPrefix = properties["SignalPrefix"]?.ToObject<SignalPrefix?>() ?? node.SignalPrefix;

            node.EnableMask = ((int?)properties["EnableMask"]) ?? node.EnableMask;

            node.EnableSignalName = ((string?)properties["EnableSignalName"]) ?? node.EnableSignalName;

            node.VisibilityMask = ((int?)properties["VisibilityMask"]) ?? node.VisibilityMask;

            node.VisibilitySignalName = ((string?)properties["VisibilitySignalName"]) ?? node.VisibilitySignalName;

            node.SymbolicText = ((string?)properties["SymbolicText"]) ?? node.SymbolicText;

			if((string?)properties["Background"] != null)
				node.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Background"]!)).GetAsFrozen();

            if ((string?)properties["Foreground"] != null)
                node.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Foreground"]!)).GetAsFrozen();

            node.UseLocalColors = ((bool?)properties["UseLocalColors"]) ?? node.UseLocalColors;

            node.FontFamily = ((string?)properties["FontFamily"]) ?? node.FontFamily;

            node.FontSize = ((double?)properties["FontSize"]) ?? node.FontSize;

            node.FontStretch = ((string?)properties["FontStretch"]) ?? node.FontStretch;

            node.FontStyle = ((string?)properties["FontStyle"]) ?? node.FontStyle;

            node.FontWeight = ((string?)properties["FontWeight"]) ?? node.FontWeight;

            if (properties["BorderBrush"] != null)
                node.BorderBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["BorderBrush"]!)).GetAsFrozen();

            if (properties["BorderThickness"] != null)
                node.BorderThickness = (Thickness)new ThicknessToStringConverter().ConvertBack((string)properties["BorderThickness"]!, null, null, null);

            if (properties["Padding"] != null)
                node.Padding = (Thickness)new ThicknessToStringConverter().ConvertBack((string)properties["Padding"]!, null, null, null);

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIButton1ViewModel wFButton1ViewModel = source as HMIButton1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFButton1ViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("Foreground", wFButton1ViewModel.Foreground?.ToString()),
				new JProperty("Background", wFButton1ViewModel.Background?.ToString()),
				new JProperty("UseLocalColors", wFButton1ViewModel.UseLocalColors),
				new JProperty("SecurityAuthorizationName", wFButton1ViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFButton1ViewModel.SecurityDenyAccessBehaviour),
				new JProperty("Value", wFButton1ViewModel.Value),
				new JProperty("WriteSecure", wFButton1ViewModel.WriteSecure),
				new JProperty("SignalName", wFButton1ViewModel.SignalName),
				new JProperty("ObjectID", wFButton1ViewModel.ObjectID),
				new JProperty("SignalPrefix", wFButton1ViewModel.SignalPrefix),
				new JProperty("EnableMask", wFButton1ViewModel.EnableMask),
				new JProperty("EnableSignalName", wFButton1ViewModel.EnableSignalName),
				new JProperty("VisibilityMask", wFButton1ViewModel.VisibilityMask),
				new JProperty("VisibilitySignalName", wFButton1ViewModel.VisibilitySignalName),
				new JProperty("SymbolicText", wFButton1ViewModel.SymbolicText),
				new JProperty("FontFamily", wFButton1ViewModel.FontFamily),
				new JProperty("FontSize", wFButton1ViewModel.FontSize),
				new JProperty("FontStretch", wFButton1ViewModel.FontStretch),
				new JProperty("FontStyle", wFButton1ViewModel.FontStyle),
				new JProperty("FontWeight", wFButton1ViewModel.FontWeight),
				new JProperty("BorderBrush", wFButton1ViewModel.BorderBrush?.ToString()),
				new JProperty("BorderThickness", wFButton1ViewModel.BorderThickness.ToString()),
				new JProperty("Padding", wFButton1ViewModel.Padding.ToString())
			});
		}
	}
}
