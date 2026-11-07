// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Serialization;
using Newtonsoft.Json.Linq;
using HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels;

namespace HMI.ProjectDesigner.DesignControls.InputOutput.Serializers
{
	public class HMIStateDisplay1Serializer : CommonControlSerializer
	{
		public HMIStateDisplay1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIStateDisplay1ViewModel(Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, Version version)
		{
			HMIStateDisplay1ViewModel node = target as HMIStateDisplay1ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.ObjectID = (string?)properties["ObjectID"] ?? node.ObjectID;

			node.SignalPrefix = properties["SignalPrefix"]?.ToObject<SignalPrefix?>() ?? node.SignalPrefix;

            node.SecurityAuthorizationName = (string?)properties["SecurityAuthorizationName"] ?? node.SecurityAuthorizationName;

            node.SecurityDenyAccessBehaviour = properties["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? node.SecurityDenyAccessBehaviour;

            node.VisibilityMask = (int?)properties["VisibilityMask"] ?? node.VisibilityMask;

            node.VisibilitySignalName = (string?)properties["VisibilitySignalName"] ?? node.VisibilitySignalName;

            node.BinaryMask = (bool?)properties["BinaryMask"] ?? node.BinaryMask;

            node.ConditionRule1 = (string?)properties["ConditionRule1"] ?? node.ConditionRule1;
            node.ConditionRule2 = (string?)properties["ConditionRule2"] ?? node.ConditionRule2;
            node.ConditionRule3 = (string?)properties["ConditionRule3"] ?? node.ConditionRule3;
            node.ConditionRule4 = (string?)properties["ConditionRule4"] ?? node.ConditionRule4;
            node.ConditionRule5 = (string?)properties["ConditionRule5"] ?? node.ConditionRule5;
            node.ConditionRule6 = (string?)properties["ConditionRule6"] ?? node.ConditionRule6;
            node.ConditionRule7 = (string?)properties["ConditionRule7"] ?? node.ConditionRule7;
            node.ConditionRule8 = (string?)properties["ConditionRule8"] ?? node.ConditionRule8;

            node.MaskSignal1 = (int?)properties["MaskSignal1"] ?? node.MaskSignal1;
            node.MaskSignal2 = (int?)properties["MaskSignal2"] ?? node.MaskSignal2;
            node.MaskSignal3 = (int?)properties["MaskSignal3"] ?? node.MaskSignal3;
            node.MaskSignal4 = (int?)properties["MaskSignal4"] ?? node.MaskSignal4;
            node.MaskSignal5 = (int?)properties["MaskSignal5"] ?? node.MaskSignal5;
            node.MaskSignal6 = (int?)properties["MaskSignal6"] ?? node.MaskSignal6;
            node.MaskSignal7 = (int?)properties["MaskSignal7"] ?? node.MaskSignal7;
            node.MaskSignal8 = (int?)properties["MaskSignal8"] ?? node.MaskSignal8;

            node.StateSignalName1 = (string?)properties["StateSignalName1"] ?? node.StateSignalName1;
            node.StateSignalName2 = (string?)properties["StateSignalName2"] ?? node.StateSignalName2;
            node.StateSignalName3 = (string?)properties["StateSignalName3"] ?? node.StateSignalName3;
            node.StateSignalName4 = (string?)properties["StateSignalName4"] ?? node.StateSignalName4;
            node.StateSignalName5 = (string?)properties["StateSignalName5"] ?? node.StateSignalName5;
            node.StateSignalName6 = (string?)properties["StateSignalName6"] ?? node.StateSignalName6;
            node.StateSignalName7 = (string?)properties["StateSignalName7"] ?? node.StateSignalName7;
            node.StateSignalName8 = (string?)properties["StateSignalName8"] ?? node.StateSignalName8;

            node.SymbolicTextState1 = (string?)properties["SymbolicTextState1"] ?? node.SymbolicTextState1;
            node.SymbolicTextState2 = (string?)properties["SymbolicTextState2"] ?? node.SymbolicTextState2;
            node.SymbolicTextState3 = (string?)properties["SymbolicTextState3"] ?? node.SymbolicTextState3;
            node.SymbolicTextState4 = (string?)properties["SymbolicTextState4"] ?? node.SymbolicTextState4;
            node.SymbolicTextState5 = (string?)properties["SymbolicTextState5"] ?? node.SymbolicTextState5;
            node.SymbolicTextState6 = (string?)properties["SymbolicTextState6"] ?? node.SymbolicTextState6;
            node.SymbolicTextState7 = (string?)properties["SymbolicTextState7"] ?? node.SymbolicTextState7;
            node.SymbolicTextState8 = (string?)properties["SymbolicTextState8"] ?? node.SymbolicTextState8;

            node.RegisterSymbolicTextState = (bool?)properties["RegisterSymbolicTextState"] ?? node.RegisterSymbolicTextState;

            node.SymbolicTextNormalState = (string?)properties["SymbolicTextNormalState"] ?? node.SymbolicTextNormalState;

            node.UseLocalColors = (bool?)properties["UseLocalColors"] ?? node.UseLocalColors;

			if ((string?)properties["BackgroundLocal"] != null)
				node.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["BackgroundLocal"]!)).GetAsFrozen();

            if ((string?)properties["ForegroundLocal"] != null)
                node.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["ForegroundLocal"]!)).GetAsFrozen();

            node.FontFamily = (string?)properties["FontFamily"] ?? node.FontFamily;

            node.FontSize = (double?)properties["FontSize"] ?? node.FontSize;

            node.FontStretch = (string?)properties["FontStretch"] ?? node.FontStretch;

            node.FontStyle = (string?)properties["FontStyle"] ?? node.FontStyle;

            node.FontWeight = (string?)properties["FontWeight"] ?? node.FontWeight;

		}
		protected override void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel target, Version version)
		{
			HMIStateDisplay1ViewModel wFStateDisplay1ViewModel = target as HMIStateDisplay1ViewModel;
			base.DeserializeCollections(propertyCollectionsElement, wFStateDisplay1ViewModel, version);
			this.DeserializeConditionSignals(propertyCollectionsElement, wFStateDisplay1ViewModel, wFStateDisplay1ViewModel.ConditionSignals, version);
		}
		protected override IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIStateDisplay1ViewModel wFStateDisplay1ViewModel = source as HMIStateDisplay1ViewModel;
			IEnumerable<JProperty> first = base.SerializeProperties(wFStateDisplay1ViewModel);
			return first.Concat(new List<JProperty>
			{
				new JProperty("ObjectID", wFStateDisplay1ViewModel.ObjectID),
				new JProperty("SignalPrefix", wFStateDisplay1ViewModel.SignalPrefix),
				new JProperty("SecurityAuthorizationName", wFStateDisplay1ViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFStateDisplay1ViewModel.SecurityDenyAccessBehaviour),
				new JProperty("VisibilitySignalName", wFStateDisplay1ViewModel.VisibilitySignalName),
				new JProperty("VisibilityMask", wFStateDisplay1ViewModel.VisibilityMask),
				new JProperty("BinaryMask", wFStateDisplay1ViewModel.BinaryMask),

				new JProperty("ConditionRule1", wFStateDisplay1ViewModel.ConditionRule1),
				new JProperty("ConditionRule2", wFStateDisplay1ViewModel.ConditionRule2),
				new JProperty("ConditionRule3", wFStateDisplay1ViewModel.ConditionRule3),
				new JProperty("ConditionRule4", wFStateDisplay1ViewModel.ConditionRule4),
				new JProperty("ConditionRule5", wFStateDisplay1ViewModel.ConditionRule5),
				new JProperty("ConditionRule6", wFStateDisplay1ViewModel.ConditionRule6),
				new JProperty("ConditionRule7", wFStateDisplay1ViewModel.ConditionRule7),
				new JProperty("ConditionRule8", wFStateDisplay1ViewModel.ConditionRule8),

				new JProperty("MaskSignal1", wFStateDisplay1ViewModel.MaskSignal1),
				new JProperty("MaskSignal2", wFStateDisplay1ViewModel.MaskSignal2),
				new JProperty("MaskSignal3", wFStateDisplay1ViewModel.MaskSignal3),
				new JProperty("MaskSignal4", wFStateDisplay1ViewModel.MaskSignal4),
				new JProperty("MaskSignal5", wFStateDisplay1ViewModel.MaskSignal5),
				new JProperty("MaskSignal6", wFStateDisplay1ViewModel.MaskSignal6),
				new JProperty("MaskSignal7", wFStateDisplay1ViewModel.MaskSignal7),
				new JProperty("MaskSignal8", wFStateDisplay1ViewModel.MaskSignal8),

				new JProperty("StateSignalName1", wFStateDisplay1ViewModel.StateSignalName1),
				new JProperty("StateSignalName2", wFStateDisplay1ViewModel.StateSignalName2),
				new JProperty("StateSignalName3", wFStateDisplay1ViewModel.StateSignalName3),
				new JProperty("StateSignalName4", wFStateDisplay1ViewModel.StateSignalName4),
				new JProperty("StateSignalName5", wFStateDisplay1ViewModel.StateSignalName5),
				new JProperty("StateSignalName6", wFStateDisplay1ViewModel.StateSignalName6),
				new JProperty("StateSignalName7", wFStateDisplay1ViewModel.StateSignalName7),
				new JProperty("StateSignalName8", wFStateDisplay1ViewModel.StateSignalName8),

				new JProperty("SymbolicTextState1", wFStateDisplay1ViewModel.SymbolicTextState1),
				new JProperty("SymbolicTextState2", wFStateDisplay1ViewModel.SymbolicTextState2),
				new JProperty("SymbolicTextState3", wFStateDisplay1ViewModel.SymbolicTextState3),
				new JProperty("SymbolicTextState4", wFStateDisplay1ViewModel.SymbolicTextState4),
				new JProperty("SymbolicTextState5", wFStateDisplay1ViewModel.SymbolicTextState5),
				new JProperty("SymbolicTextState6", wFStateDisplay1ViewModel.SymbolicTextState6),
				new JProperty("SymbolicTextState7", wFStateDisplay1ViewModel.SymbolicTextState7),
				new JProperty("SymbolicTextState8", wFStateDisplay1ViewModel.SymbolicTextState8),

				new JProperty("RegisterSymbolicTextState", wFStateDisplay1ViewModel.RegisterSymbolicTextState),
				new JProperty("SymbolicTextNormalState", wFStateDisplay1ViewModel.SymbolicTextNormalState),
				new JProperty("UseLocalColors", wFStateDisplay1ViewModel.UseLocalColors),
				new JProperty("BackgroundLocal", wFStateDisplay1ViewModel.Background?.ToString()),
				new JProperty("ForegroundLocal", wFStateDisplay1ViewModel.Foreground?.ToString()),
				new JProperty("FontFamily", wFStateDisplay1ViewModel.FontFamily),
				new JProperty("FontSize", wFStateDisplay1ViewModel.FontSize),
				new JProperty("FontStretch", wFStateDisplay1ViewModel.FontStretch),
				new JProperty("FontStyle", wFStateDisplay1ViewModel.FontStyle),
				new JProperty("FontWeight", wFStateDisplay1ViewModel.FontWeight)
			});
		}
		protected override IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
		{
			IEnumerable<JProperty> first = base.SerializeCollections(source);
			HMIStateDisplay1ViewModel wFStateDisplay1ViewModel = source as HMIStateDisplay1ViewModel;

			return first.Concat(new List<JProperty>
			{
				this.GetSerializedConditionSignals(wFStateDisplay1ViewModel.ConditionSignals)
			});
		}
	}
}
