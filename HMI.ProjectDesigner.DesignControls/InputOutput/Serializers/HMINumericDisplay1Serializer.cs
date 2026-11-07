// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels;
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Serialization;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.InputOutput.Serializers
{
	public class HMINumericDisplay1Serializer : CommonControlSerializer
	{
		public HMINumericDisplay1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMINumericDisplay1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMINumericDisplay1ViewModel node = target as HMINumericDisplay1ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.Precision = ((int?)properties["Precision"]) ?? node.Precision;

            node.Unit = ((string?)properties["Unit"]) ?? node.Unit;

            node.FontFamily = ((string?)properties["FontFamily"]) ?? node.FontFamily;

            node.FontSize = ((double?)properties["FontSize"]) ?? node.FontSize;

            node.FontStretch = ((string?)properties["FontStretch"]) ?? node.FontStretch;

            node.FontStyle = ((string?)properties["FontStyle"]) ?? node.FontStyle;

            node.FontWeight = ((string?)properties["FontWeight"]) ?? node.FontWeight;

            node.HorizontalContentAlignment = properties["HorizontalContentAlignment"]?.ToObject<HorizontalAlignment?>() ?? node.HorizontalContentAlignment;

            node.VerticalContentAlignment = properties["VerticalContentAlignment"]?.ToObject<VerticalAlignment?>() ?? node.VerticalContentAlignment;

            node.SecurityAuthorizationName = ((string?)properties["SecurityAuthorizationName"]) ?? node.SecurityAuthorizationName;

            node.SecurityDenyAccessBehaviour = properties["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? node.SecurityDenyAccessBehaviour;

            node.VisibilitySignalName = ((string?)properties["VisibilitySignalName"]) ?? node.VisibilitySignalName;

            node.VisibilityMask = ((int?)properties["VisibilityMask"]) ?? node.VisibilityMask;

            node.BinaryMask = ((bool?)properties["BinaryMask"]) ?? node.BinaryMask;

            node.ConditionRule1 = ((string?)properties["ConditionRule1"]) ?? node.ConditionRule1;
            node.ConditionRule2 = ((string?)properties["ConditionRule2"]) ?? node.ConditionRule2;
            node.ConditionRule3 = ((string?)properties["ConditionRule3"]) ?? node.ConditionRule3;
            node.ConditionRule4 = ((string?)properties["ConditionRule4"]) ?? node.ConditionRule4;

            node.MaskSignal1 = ((int?)properties["MaskSignal1"]) ?? node.MaskSignal1;
            node.MaskSignal2 = ((int?)properties["MaskSignal2"]) ?? node.MaskSignal2;
            node.MaskSignal3 = ((int?)properties["MaskSignal3"]) ?? node.MaskSignal3;
            node.MaskSignal4 = ((int?)properties["MaskSignal4"]) ?? node.MaskSignal4;

			if (((string?)properties["Foreground"]) != null)
				node.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Foreground"]!)).GetAsFrozen();

            node.LimitSignalName1 = ((string?)properties["LimitSignalName1"]) ?? node.LimitSignalName1;
            node.LimitSignalName2 = ((string?)properties["LimitSignalName2"]) ?? node.LimitSignalName2;
            node.LimitSignalName3 = ((string?)properties["LimitSignalName3"]) ?? node.LimitSignalName3;
            node.LimitSignalName4 = ((string?)properties["LimitSignalName4"]) ?? node.LimitSignalName4;

            node.SignalName = ((string?)properties["SignalName"]) ?? node.SignalName;

            node.ObjectID = ((string?)properties["ObjectID"]) ?? node.ObjectID;

            node.SignalPrefix = properties["SignalPrefix"]?.ToObject<SignalPrefix?>() ?? node.SignalPrefix;

		}
		protected override void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel target, System.Version version)
		{
			HMINumericDisplay1ViewModel wFNumericDisplay1ViewModel = target as HMINumericDisplay1ViewModel;
			base.DeserializeCollections(propertyCollectionsElement, wFNumericDisplay1ViewModel, version);
			this.DeserializeConditionSignals(propertyCollectionsElement, wFNumericDisplay1ViewModel, wFNumericDisplay1ViewModel.ConditionSignalsContents, version);
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMINumericDisplay1ViewModel wFNumericDisplay1ViewModel = source as HMINumericDisplay1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFNumericDisplay1ViewModel);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("Precision", wFNumericDisplay1ViewModel.Precision),
				new JProperty("Unit", wFNumericDisplay1ViewModel.Unit),
				new JProperty("FontFamily", wFNumericDisplay1ViewModel.FontFamily),
				new JProperty("FontSize", wFNumericDisplay1ViewModel.FontSize),
				new JProperty("FontStretch", wFNumericDisplay1ViewModel.FontStretch),
				new JProperty("FontStyle", wFNumericDisplay1ViewModel.FontStyle),
				new JProperty("FontWeight", wFNumericDisplay1ViewModel.FontWeight),
				new JProperty("HorizontalContentAlignment", wFNumericDisplay1ViewModel.HorizontalContentAlignment),
				new JProperty("VerticalContentAlignment", wFNumericDisplay1ViewModel.VerticalContentAlignment),
				new JProperty("SecurityAuthorizationName", wFNumericDisplay1ViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFNumericDisplay1ViewModel.SecurityDenyAccessBehaviour),
				new JProperty("VisibilitySignalName", wFNumericDisplay1ViewModel.VisibilitySignalName),
				new JProperty("VisibilityMask", wFNumericDisplay1ViewModel.VisibilityMask),
				new JProperty("BinaryMask", wFNumericDisplay1ViewModel.BinaryMask),
				new JProperty("ConditionRule1", wFNumericDisplay1ViewModel.ConditionRule1),
				new JProperty("ConditionRule2", wFNumericDisplay1ViewModel.ConditionRule2),
				new JProperty("ConditionRule3", wFNumericDisplay1ViewModel.ConditionRule3),
				new JProperty("ConditionRule4", wFNumericDisplay1ViewModel.ConditionRule4),
				new JProperty("MaskSignal1", wFNumericDisplay1ViewModel.MaskSignal1),
				new JProperty("MaskSignal2", wFNumericDisplay1ViewModel.MaskSignal2),
				new JProperty("MaskSignal3", wFNumericDisplay1ViewModel.MaskSignal3),
				new JProperty("MaskSignal4", wFNumericDisplay1ViewModel.MaskSignal4),
				new JProperty("LimitSignalName1", wFNumericDisplay1ViewModel.LimitSignalName1),
				new JProperty("LimitSignalName2", wFNumericDisplay1ViewModel.LimitSignalName2),
				new JProperty("LimitSignalName3", wFNumericDisplay1ViewModel.LimitSignalName3),
				new JProperty("LimitSignalName4", wFNumericDisplay1ViewModel.LimitSignalName4),
				new JProperty("SignalName", wFNumericDisplay1ViewModel.SignalName),
				new JProperty("ObjectID", wFNumericDisplay1ViewModel.ObjectID),
				new JProperty("SignalPrefix", wFNumericDisplay1ViewModel.SignalPrefix),
				new JProperty("Foreground", wFNumericDisplay1ViewModel.Foreground?.ToString())
			});
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
		{
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeCollections(source);
			HMINumericDisplay1ViewModel wFNumericDisplay1ViewModel = source as HMINumericDisplay1ViewModel;

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				this.GetSerializedConditionSignals(wFNumericDisplay1ViewModel.ConditionSignalsContents)
			});
		}
	}
}
