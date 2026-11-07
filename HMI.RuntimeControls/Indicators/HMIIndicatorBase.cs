// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HMI.Runtime.Core;
using HMI.MathExpressions;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
using HMI.Runtime.Shared.Dialogs;
using HMI.MathExpressions.Compilation;
using HMI.Runtime.Shared.Common.Converters;
namespace HMI.RuntimeControls.Indicators
{
    /// <summary>




    /// </summary>
    [
        TemplateVisualState(GroupName = "CommonStates", Name = "State2"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State7"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Normal"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State1"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State5"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State4"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State6"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State8"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State3")]
	public abstract class HMIIndicatorBase : Control, IDisposable
	{
		protected const string VisualStateGroupCommonStates = "CommonStates";
		protected const string VisualStateNormal = "Normal";
		protected const string VisualState1 = "State1";
		protected const string VisualState2 = "State2";
		protected const string VisualState3 = "State3";
		protected const string VisualState4 = "State4";
		protected const string VisualState5 = "State5";
		protected const string VisualState6 = "State6";
		protected const string VisualState7 = "State7";
		protected const string VisualState8 = "State8";

		private DialogManager dialogManager;
		private IHMIConnector connector;
		private ObservableCollection<ConditionSignalItem> observableConditionSignalItems;
		private ObservableCollection<ParameterItem> observableParameterItems;
		private Dictionary<string, HMIConditionSignal> conditionParameters;
		private Dictionary<string, string> dictSignalValueCache;

		private ExpressionCompiler? conditionRule1Exp;
		private ExpressionCompiler? conditionRule2Exp;
		private ExpressionCompiler? conditionRule3Exp;
		private ExpressionCompiler? conditionRule4Exp;
		private ExpressionCompiler? conditionRule5Exp;
		private ExpressionCompiler? conditionRule6Exp;
		private ExpressionCompiler? conditionRule7Exp;
		private ExpressionCompiler? conditionRule8Exp;

		private double stateSignalValue1;
		private double stateSignalValue2;
		private double stateSignalValue3;
		private double stateSignalValue4;
		private double stateSignalValue5;
		private double stateSignalValue6;
		private double stateSignalValue7;
		private double stateSignalValue8;

		private OnSignalChangedDelegate conditionSignalChangedHandler;
		private OnSignalChangedDelegate stateSignal1ChangedHandler;
		private OnSignalChangedDelegate stateSignal2ChangedHandler;
		private OnSignalChangedDelegate stateSignal3ChangedHandler;
		private OnSignalChangedDelegate stateSignal4ChangedHandler;
		private OnSignalChangedDelegate stateSignal5ChangedHandler;
		private OnSignalChangedDelegate stateSignal6ChangedHandler;
		private OnSignalChangedDelegate stateSignal7ChangedHandler;
		private OnSignalChangedDelegate stateSignal8ChangedHandler;
		private OnSignalChangedDelegate visibilitySignalChangedHandler;

		private bool canShowControlFromVisibilitySignal;
		private bool isSecurityAvailable;
		private bool isAuthorized;

		public static readonly DependencyProperty BinaryMaskProperty = DependencyProperty.Register("BinaryMask", typeof(bool), typeof(HMIIndicatorBase), new PropertyMetadata(false, new PropertyChangedCallback(HMIIndicatorBase.OnBinaryMaskPropertyChanged)));
		public static readonly DependencyProperty ConditionRule1Property = DependencyProperty.Register("ConditionRule1", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnConditionRule1PropertyChanged)));
		public static readonly DependencyProperty ConditionRule2Property = DependencyProperty.Register("ConditionRule2", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnConditionRule2PropertyChanged)));
		public static readonly DependencyProperty ConditionRule3Property = DependencyProperty.Register("ConditionRule3", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnConditionRule3PropertyChanged)));
		public static readonly DependencyProperty ConditionRule4Property = DependencyProperty.Register("ConditionRule4", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnConditionRule4PropertyChanged)));
		public static readonly DependencyProperty ConditionRule5Property = DependencyProperty.Register("ConditionRule5", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnConditionRule5PropertyChanged)));
		public static readonly DependencyProperty ConditionRule6Property = DependencyProperty.Register("ConditionRule6", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnConditionRule6PropertyChanged)));
		public static readonly DependencyProperty ConditionRule7Property = DependencyProperty.Register("ConditionRule7", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnConditionRule7PropertyChanged)));
		public static readonly DependencyProperty ConditionRule8Property = DependencyProperty.Register("ConditionRule8", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnConditionRule8PropertyChanged)));
		public static readonly DependencyProperty MaskSignal1Property = DependencyProperty.Register("MaskSignal1", typeof(int?), typeof(HMIIndicatorBase), new PropertyMetadata(null, new PropertyChangedCallback(HMIIndicatorBase.OnMaskSignal1PropertyChanged)));
		public static readonly DependencyProperty MaskSignal2Property = DependencyProperty.Register("MaskSignal2", typeof(int?), typeof(HMIIndicatorBase), new PropertyMetadata(null, new PropertyChangedCallback(HMIIndicatorBase.OnMaskSignal2PropertyChanged)));
		public static readonly DependencyProperty MaskSignal3Property = DependencyProperty.Register("MaskSignal3", typeof(int?), typeof(HMIIndicatorBase), new PropertyMetadata(null, new PropertyChangedCallback(HMIIndicatorBase.OnMaskSignal3PropertyChanged)));
		public static readonly DependencyProperty MaskSignal4Property = DependencyProperty.Register("MaskSignal4", typeof(int?), typeof(HMIIndicatorBase), new PropertyMetadata(null, new PropertyChangedCallback(HMIIndicatorBase.OnMaskSignal4PropertyChanged)));
		public static readonly DependencyProperty MaskSignal5Property = DependencyProperty.Register("MaskSignal5", typeof(int?), typeof(HMIIndicatorBase), new PropertyMetadata(null, new PropertyChangedCallback(HMIIndicatorBase.OnMaskSignal5PropertyChanged)));
		public static readonly DependencyProperty MaskSignal6Property = DependencyProperty.Register("MaskSignal6", typeof(int?), typeof(HMIIndicatorBase), new PropertyMetadata(null, new PropertyChangedCallback(HMIIndicatorBase.OnMaskSignal6PropertyChanged)));
		public static readonly DependencyProperty MaskSignal7Property = DependencyProperty.Register("MaskSignal7", typeof(int?), typeof(HMIIndicatorBase), new PropertyMetadata(null, new PropertyChangedCallback(HMIIndicatorBase.OnMaskSignal7PropertyChanged)));
		public static readonly DependencyProperty MaskSignal8Property = DependencyProperty.Register("MaskSignal8", typeof(int?), typeof(HMIIndicatorBase), new PropertyMetadata(null, new PropertyChangedCallback(HMIIndicatorBase.OnMaskSignal8PropertyChanged)));
		public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnObjectIDPropertyChanged)));
		public static readonly DependencyProperty PopupModalityProperty = DependencyProperty.Register("PopupModality", typeof(DialogModality), typeof(HMIIndicatorBase), new PropertyMetadata(DialogModality.Modeless, new PropertyChangedCallback(HMIIndicatorBase.OnPopupModalityPropertyChanged)));
		public static readonly DependencyProperty PopupSourceProperty = DependencyProperty.Register("PopupSource", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnPopupSourcePropertyChanged)));
		public static readonly DependencyProperty PopupTitleProperty = DependencyProperty.Register("PopupTitle", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnPopupTitlePropertyChanged)));
		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnSecurityAuthorizationNamePropertyChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMIIndicatorBase), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(HMIIndicatorBase.OnSecurityDenyAccessBehaviourPropertyChanged)));
		public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnServerNamePropertyChanged)));
		public static readonly DependencyProperty ShowPopupProperty = DependencyProperty.Register("ShowPopup", typeof(bool), typeof(HMIIndicatorBase), new PropertyMetadata(true, new PropertyChangedCallback(HMIIndicatorBase.OnShowPopupPropertyChanged)));
		public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnSignalPrefixPropertyChanged)));
		public static readonly DependencyProperty StateSignalName1Property = DependencyProperty.Register("StateSignalName1", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnStateSignalName1Changed)));
		public static readonly DependencyProperty StateSignalName2Property = DependencyProperty.Register("StateSignalName2", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnStateSignalName2Changed)));
		public static readonly DependencyProperty StateSignalName3Property = DependencyProperty.Register("StateSignalName3", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnStateSignalName3Changed)));
		public static readonly DependencyProperty StateSignalName4Property = DependencyProperty.Register("StateSignalName4", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnStateSignalName4Changed)));
		public static readonly DependencyProperty StateSignalName5Property = DependencyProperty.Register("StateSignalName5", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnStateSignalName5Changed)));
		public static readonly DependencyProperty StateSignalName6Property = DependencyProperty.Register("StateSignalName6", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnStateSignalName6Changed)));
		public static readonly DependencyProperty StateSignalName7Property = DependencyProperty.Register("StateSignalName7", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnStateSignalName7Changed)));
		public static readonly DependencyProperty StateSignalName8Property = DependencyProperty.Register("StateSignalName8", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnStateSignalName8Changed)));
		public static readonly DependencyProperty VisibilityMaskProperty = DependencyProperty.Register("VisibilityMask", typeof(int?), typeof(HMIIndicatorBase), new PropertyMetadata(null, new PropertyChangedCallback(HMIIndicatorBase.OnVisibilityMaskPropertyChanged)));
		public static readonly DependencyProperty VisibilitySignalNameProperty = DependencyProperty.Register("VisibilitySignalName", typeof(string), typeof(HMIIndicatorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorBase.OnVisibilitySignalNamePropertyChanged)));
		
		[Category("HMI")]
		public bool BinaryMask
		{
			get
			{
				return (bool)base.GetValue(HMIIndicatorBase.BinaryMaskProperty);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.BinaryMaskProperty, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule1
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.ConditionRule1Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.ConditionRule1Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule2
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.ConditionRule2Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.ConditionRule2Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule3
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.ConditionRule3Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.ConditionRule3Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule4
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.ConditionRule4Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.ConditionRule4Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule5
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.ConditionRule5Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.ConditionRule5Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule6
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.ConditionRule6Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.ConditionRule6Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule7
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.ConditionRule7Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.ConditionRule7Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule8
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.ConditionRule8Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.ConditionRule8Property, value);
			}
		}
		[Category("HMI")]
		public ObservableCollection<ConditionSignalItem> ConditionSignals
		{
			get
			{
				return this.observableConditionSignalItems;
			}
		}
		protected IHMIConnector Connector
		{
			get
			{
				return this.connector;
			}
		}
		protected bool IsAuthorized
		{
			get
			{
				return this.isAuthorized;
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal1
		{
			get
			{
				return (int?)base.GetValue(HMIIndicatorBase.MaskSignal1Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.MaskSignal1Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal2
		{
			get
			{
				return (int?)base.GetValue(HMIIndicatorBase.MaskSignal2Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.MaskSignal2Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal3
		{
			get
			{
				return (int?)base.GetValue(HMIIndicatorBase.MaskSignal3Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.MaskSignal3Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal4
		{
			get
			{
				return (int?)base.GetValue(HMIIndicatorBase.MaskSignal4Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.MaskSignal4Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal5
		{
			get
			{
				return (int?)base.GetValue(HMIIndicatorBase.MaskSignal5Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.MaskSignal5Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal6
		{
			get
			{
				return (int?)base.GetValue(HMIIndicatorBase.MaskSignal6Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.MaskSignal6Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal7
		{
			get
			{
				return (int?)base.GetValue(HMIIndicatorBase.MaskSignal7Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.MaskSignal7Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal8
		{
			get
			{
				return (int?)base.GetValue(HMIIndicatorBase.MaskSignal8Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.MaskSignal8Property, value);
			}
		}
		[Category("HMI")]
		public string ObjectID
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.ObjectIDProperty);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.ObjectIDProperty, value);
			}
		}
		[Category("HMI")]
		public DialogModality PopupModality
		{
			get
			{
				return (DialogModality)base.GetValue(HMIIndicatorBase.PopupModalityProperty);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.PopupModalityProperty, value);
			}
		}
		[Category("HMI")]
		public ObservableCollection<ParameterItem> PopupParameters
		{
			get
			{
				return this.observableParameterItems;
			}
		}
		[Category("HMI")]
		public string PopupSource
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.PopupSourceProperty);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.PopupSourceProperty, value);
			}
		}
		[Category("HMI")]
		public string PopupTitle
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.PopupTitleProperty);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.PopupTitleProperty, value);
			}
		}
		[Category("HMI")]
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMIIndicatorBase.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		[Category("HMI")]
		protected string ServerName
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.ServerNameProperty);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.ServerNameProperty, value);
			}
		}
		[Category("HMI")]
		public bool ShowPopup
		{
			get
			{
				return (bool)base.GetValue(HMIIndicatorBase.ShowPopupProperty);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.ShowPopupProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalPrefix
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.SignalPrefixProperty);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.SignalPrefixProperty, value);
			}
		}
		[Category("HMI")]
		public string StateSignalName1
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.StateSignalName1Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.StateSignalName1Property, value);
			}
		}
		[Category("HMI")]
		public string StateSignalName2
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.StateSignalName2Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.StateSignalName2Property, value);
			}
		}
		[Category("HMI")]
		public string StateSignalName3
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.StateSignalName3Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.StateSignalName3Property, value);
			}
		}
		[Category("HMI")]
		public string StateSignalName4
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.StateSignalName4Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.StateSignalName4Property, value);
			}
		}
		[Category("HMI")]
		public string StateSignalName5
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.StateSignalName5Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.StateSignalName5Property, value);
			}
		}
		[Category("HMI")]
		public string StateSignalName6
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.StateSignalName6Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.StateSignalName6Property, value);
			}
		}
		[Category("HMI")]
		public string StateSignalName7
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.StateSignalName7Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.StateSignalName7Property, value);
			}
		}
		[Category("HMI")]
		public string StateSignalName8
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.StateSignalName8Property);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.StateSignalName8Property, value);
			}
		}
		protected double StateSignalValue1
		{
			get
			{
				return this.stateSignalValue1;
			}
		}
		protected double StateSignalValue2
		{
			get
			{
				return this.stateSignalValue2;
			}
		}
		protected double StateSignalValue3
		{
			get
			{
				return this.stateSignalValue3;
			}
		}
		protected double StateSignalValue4
		{
			get
			{
				return this.stateSignalValue4;
			}
		}
		protected double StateSignalValue5
		{
			get
			{
				return this.stateSignalValue5;
			}
		}
		protected double StateSignalValue6
		{
			get
			{
				return this.stateSignalValue6;
			}
		}
		protected double StateSignalValue7
		{
			get
			{
				return this.stateSignalValue7;
			}
		}
		protected double StateSignalValue8
		{
			get
			{
				return this.stateSignalValue8;
			}
		}

		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? VisibilityMask
		{
			get
			{
				return (int?)base.GetValue(HMIIndicatorBase.VisibilityMaskProperty);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.VisibilityMaskProperty, value);
			}
		}
		[Category("HMI")]
		public string VisibilitySignalName
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorBase.VisibilitySignalNameProperty);
			}
			set
			{
				base.SetValue(HMIIndicatorBase.VisibilitySignalNameProperty, value);
			}
		}
		private static void OnBinaryMaskPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnConditionRule1PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnConditionRule2PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnConditionRule3PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnConditionRule4PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnConditionRule5PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnConditionRule6PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnConditionRule7PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnConditionRule8PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnMaskSignal1PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnMaskSignal2PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnMaskSignal3PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnMaskSignal4PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnMaskSignal5PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnMaskSignal6PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnMaskSignal7PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnMaskSignal8PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnObjectIDPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnPopupModalityPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnPopupSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnPopupTitlePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnSecurityAuthorizationNamePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnSecurityDenyAccessBehaviourPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnServerNamePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnShowPopupPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnSignalPrefixPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnStateSignalName1Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnStateSignalName2Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnStateSignalName3Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnStateSignalName4Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnStateSignalName5Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnStateSignalName6Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnStateSignalName7Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnStateSignalName8Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnVisibilityMaskPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnVisibilitySignalNamePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		protected HMIIndicatorBase()
		{
			base.DefaultStyleKey=(typeof(HMIIndicatorBase));
			this.observableConditionSignalItems = new ObservableCollection<ConditionSignalItem>();
			this.observableParameterItems = new ObservableCollection<ParameterItem>();
			this.conditionParameters = new Dictionary<string, HMIConditionSignal>(StringComparer.InvariantCultureIgnoreCase);
			this.dictSignalValueCache = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

			this.stateSignalValue1 = double.NaN;
			this.stateSignalValue2 = double.NaN;
			this.stateSignalValue3 = double.NaN;
			this.stateSignalValue4 = double.NaN;
			this.stateSignalValue5 = double.NaN;
			this.stateSignalValue6 = double.NaN;
			this.stateSignalValue7 = double.NaN;
			this.stateSignalValue8 = double.NaN;

			this.dialogManager = new DialogManager(null, null, null);
		}
		~HMIIndicatorBase()
		{
			this.Dispose(false);
		}
		public virtual void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void OnConditionSignalValueChanged(string signalName, object signalValue)
		{
			foreach (var conditionSignal in this.conditionParameters.Values)
			{
				if (conditionSignal.Name.Equals(signalName, StringComparison.InvariantCultureIgnoreCase))
				{
					conditionSignal.Value = HMINumericSignal.ConvertSignalValueToDouble(signalValue);
				}

				if (this.conditionRule1Exp != null)
				{
					this.conditionRule1Exp.SetVariable(conditionSignal.ParameterName, conditionSignal.Value);
				}
				if (this.conditionRule2Exp != null)
				{
					this.conditionRule2Exp.SetVariable(conditionSignal.ParameterName, conditionSignal.Value);
				}
				if (this.conditionRule3Exp != null)
				{
					this.conditionRule3Exp.SetVariable(conditionSignal.ParameterName, conditionSignal.Value);
				}
				if (this.conditionRule4Exp != null)
				{
					this.conditionRule4Exp.SetVariable(conditionSignal.ParameterName, conditionSignal.Value);
				}
				if (this.conditionRule5Exp != null)
				{
					this.conditionRule5Exp.SetVariable(conditionSignal.ParameterName, conditionSignal.Value);
				}
				if (this.conditionRule6Exp != null)
				{
					this.conditionRule6Exp.SetVariable(conditionSignal.ParameterName, conditionSignal.Value);
				}
				if (this.conditionRule7Exp != null)
				{
					this.conditionRule7Exp.SetVariable(conditionSignal.ParameterName, conditionSignal.Value);
				}
				if (this.conditionRule8Exp != null)
				{
					this.conditionRule8Exp.SetVariable(conditionSignal.ParameterName, conditionSignal.Value);
				}
			}
			this.ChangeVisualState();
		}
		private void OnStateSignal1Changed(string signalName, object signalValue)
		{
			this.stateSignalValue1 = this.ConvertSignalValueToDouble(signalValue);
			this.ChangeVisualState();
		}
		private void OnStateSignal2Changed(string signalName, object signalValue)
		{
			this.stateSignalValue2 = this.ConvertSignalValueToDouble(signalValue);
			this.ChangeVisualState();
		}
		private void OnStateSignal3Changed(string signalName, object signalValue)
		{
			this.stateSignalValue3 = this.ConvertSignalValueToDouble(signalValue);
			this.ChangeVisualState();
		}
		private void OnStateSignal4Changed(string signalName, object signalValue)
		{
			this.stateSignalValue4 = this.ConvertSignalValueToDouble(signalValue);
			this.ChangeVisualState();
		}
		private void OnStateSignal5Changed(string signalName, object signalValue)
		{
			this.stateSignalValue5 = this.ConvertSignalValueToDouble(signalValue);
			this.ChangeVisualState();
		}
		private void OnStateSignal6Changed(string signalName, object signalValue)
		{
			this.stateSignalValue6 = this.ConvertSignalValueToDouble(signalValue);
			this.ChangeVisualState();
		}
		private void OnStateSignal7Changed(string signalName, object signalValue)
		{
			this.stateSignalValue7 = this.ConvertSignalValueToDouble(signalValue);
			this.ChangeVisualState();
		}
		private void OnStateSignal8Changed(string signalName, object signalValue)
		{
			this.stateSignalValue8 = this.ConvertSignalValueToDouble(signalValue);
			this.ChangeVisualState();
		}
		private void OnVisibilitySignalValueChanged(string signalName, object signalValue)
		{
			double visibilitySignalValue = this.ConvertSignalValueToDouble(signalValue);
			this.canShowControlFromVisibilitySignal =
				!double.IsNaN(visibilitySignalValue) &&
				visibilitySignalValue != 0.0 &&
				(!this.VisibilityMask.HasValue ||
				 (Convert.ToInt64(visibilitySignalValue) & (long)this.VisibilityMask.Value) != (long)this.VisibilityMask.Value);
			this.ShowControl(this.canShowControlFromVisibilitySignal);
		}
		private void OnIndicatorMouseEnter(object sender, MouseEventArgs e)
		{
			if (string.IsNullOrEmpty(this.PopupSource))
			{
				return;
			}
			base.Cursor=(Cursors.Hand);
		}
		private void OnIndicatorMouseLeave(object sender, MouseEventArgs e)
		{
			base.Cursor=(Cursors.Arrow);
		}
		private void HandleUserContextChanged(string userName)
		{
			if (this.isSecurityAvailable)
			{
				this.isAuthorized = this.connector.CheckProjectAuthorization(this.SecurityAuthorizationName);
				switch (this.SecurityDenyAccessBehaviour)
				{
				case DenyAccessBehaviour.Disable:
					this.EnableControl(this.isAuthorized);
					break;
				case DenyAccessBehaviour.Hide:
					this.ShowControl(this.isAuthorized);
					break;
				default:
					throw new ArgumentOutOfRangeException("DenyAccessBehaviour");
				}
			}
			this.OnUserContextChanged(userName);
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			base.IsTabStop=(false);
			bool isInDesignMode = DesignerProperties.GetIsInDesignMode(this);
			this.resolvePlaceholders();
			if (!isInDesignMode && this.connector == null)
			{
                this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
                if (string.IsNullOrEmpty(this.SecurityAuthorizationName))
				{
					this.isAuthorized = true;
					this.isSecurityAvailable = false;
				}
				else
				{
					this.isAuthorized = false;
					this.isSecurityAvailable = true;
					switch (this.SecurityDenyAccessBehaviour)
					{
					case DenyAccessBehaviour.Disable:
						base.IsEnabled=(false);
						break;
					case DenyAccessBehaviour.Hide:
						base.Visibility= System.Windows.Visibility.Collapsed;
						break;
					default:
						throw new ArgumentOutOfRangeException("DenyAccessBehaviour");
					}
				}
				this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.HandleUserContextChanged);
				this.InitializeConditionRule1Expression();
				this.InitializeConditionRule2Expression();
				this.InitializeConditionRule3Expression();
				this.InitializeConditionRule4Expression();
				this.InitializeConditionRule5Expression();
				this.InitializeConditionRule6Expression();
				this.InitializeConditionRule7Expression();
				this.InitializeConditionRule8Expression();
				this.RegisterConditionSignals();

				if (!string.IsNullOrEmpty(this.StateSignalName1))
				{
					this.StateSignalName1 = this.SignalPrefix + this.StateSignalName1;
					this.StateSignalName1 = SignalNameBuilder.BuildSignalName(this.StateSignalName1, this);

					this.stateSignal1ChangedHandler = new OnSignalChangedDelegate(this.OnStateSignal1Changed);
					this.RegisterSignal(this.ServerName, this.StateSignalName1, this.stateSignal1ChangedHandler);
				}
				if (!string.IsNullOrEmpty(this.StateSignalName2))
				{
					this.StateSignalName2 = this.SignalPrefix + this.StateSignalName2;
					this.StateSignalName2 = SignalNameBuilder.BuildSignalName(this.StateSignalName2, this);
					this.stateSignal2ChangedHandler = new OnSignalChangedDelegate(this.OnStateSignal2Changed);
					this.RegisterSignal(this.ServerName, this.StateSignalName2, this.stateSignal2ChangedHandler);
				}
				if (!string.IsNullOrEmpty(this.StateSignalName3))
				{
					this.StateSignalName3 = this.SignalPrefix + this.StateSignalName3;
					this.StateSignalName3 = SignalNameBuilder.BuildSignalName(this.StateSignalName3, this);
					this.stateSignal3ChangedHandler = new OnSignalChangedDelegate(this.OnStateSignal3Changed);
					this.RegisterSignal(this.ServerName, this.StateSignalName3, this.stateSignal3ChangedHandler);
				}
				if (!string.IsNullOrEmpty(this.StateSignalName4))
				{
					this.StateSignalName4 = this.SignalPrefix + this.StateSignalName4;
					this.StateSignalName4 = SignalNameBuilder.BuildSignalName(this.StateSignalName4, this);
					this.stateSignal4ChangedHandler = new OnSignalChangedDelegate(this.OnStateSignal4Changed);
					this.RegisterSignal(this.ServerName, this.StateSignalName4, this.stateSignal4ChangedHandler);
				}
				if (!string.IsNullOrEmpty(this.StateSignalName5))
				{
					this.StateSignalName5 = this.SignalPrefix + this.StateSignalName5;
					this.StateSignalName5 = SignalNameBuilder.BuildSignalName(this.StateSignalName5, this);
					this.stateSignal5ChangedHandler = new OnSignalChangedDelegate(this.OnStateSignal5Changed);
					this.RegisterSignal(this.ServerName, this.StateSignalName5, this.stateSignal5ChangedHandler);
				}
				if (!string.IsNullOrEmpty(this.StateSignalName6))
				{
					this.StateSignalName6 = this.SignalPrefix + this.StateSignalName6;
					this.StateSignalName6 = SignalNameBuilder.BuildSignalName(this.StateSignalName6, this);
					this.stateSignal6ChangedHandler = new OnSignalChangedDelegate(this.OnStateSignal6Changed);
					this.RegisterSignal(this.ServerName, this.StateSignalName6, this.stateSignal6ChangedHandler);
				}
				if (!string.IsNullOrEmpty(this.StateSignalName7))
				{
					this.StateSignalName7 = this.SignalPrefix + this.StateSignalName7;
					this.StateSignalName7 = SignalNameBuilder.BuildSignalName(this.StateSignalName7, this);
					this.stateSignal7ChangedHandler = new OnSignalChangedDelegate(this.OnStateSignal7Changed);
					this.RegisterSignal(this.ServerName, this.StateSignalName7, this.stateSignal7ChangedHandler);
				}
				if (!string.IsNullOrEmpty(this.StateSignalName8))
				{
					this.StateSignalName8 = this.SignalPrefix + this.StateSignalName8;
					this.StateSignalName8 = SignalNameBuilder.BuildSignalName(this.StateSignalName8, this);
					this.stateSignal8ChangedHandler = new OnSignalChangedDelegate(this.OnStateSignal8Changed);
					this.RegisterSignal(this.ServerName, this.StateSignalName8, this.stateSignal8ChangedHandler);
				}
				if (!string.IsNullOrEmpty(this.VisibilitySignalName))
				{
					this.VisibilitySignalName = this.SignalPrefix + this.VisibilitySignalName;
					this.VisibilitySignalName = SignalNameBuilder.BuildSignalName(this.VisibilitySignalName, this);
					this.visibilitySignalChangedHandler = new OnSignalChangedDelegate(this.OnVisibilitySignalValueChanged);
					this.RegisterSignal(this.ServerName, this.VisibilitySignalName, this.visibilitySignalChangedHandler);
				}
				this.AddMouseEvents();
				return;
			}
		}
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);
			this.TryShowPopupDialog();
		}
		private bool TryShowPopupDialog()
		{
			if (this.ShowPopup && !string.IsNullOrEmpty(this.PopupSource))
			{
				try
				{
					this.dialogManager.Show(this, this.PopupSource, this.PopupParameters, this.PopupTitle, this.PopupModality);
				}
				catch (Exception ex)
				{
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
				return false;
			}
			return true;
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			this.dialogManager.Dispose();
			this.DisposeConnector();
			this.RemoveMouseEvents();
			this.StopStoryboards();
		}
		private void DisposeConnector()
		{
			if (this.connector == null)
			{
				return;
			}
			this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.HandleUserContextChanged);

			if (this.conditionSignalChangedHandler != null)
			{
				foreach (var current in this.dictSignalValueCache.Keys)
				{
					this.UnregisterSignal(this.ServerName, current, this.conditionSignalChangedHandler);
				}
			}
			if (this.stateSignal1ChangedHandler != null)
			{
				this.UnregisterSignal(this.ServerName, this.StateSignalName1, this.stateSignal1ChangedHandler);
			}
			if (this.stateSignal2ChangedHandler != null)
			{
				this.UnregisterSignal(this.ServerName, this.StateSignalName2, this.stateSignal2ChangedHandler);
			}
			if (this.stateSignal3ChangedHandler != null)
			{
				this.UnregisterSignal(this.ServerName, this.StateSignalName3, this.stateSignal3ChangedHandler);
			}
			if (this.stateSignal4ChangedHandler != null)
			{
				this.UnregisterSignal(this.ServerName, this.StateSignalName4, this.stateSignal4ChangedHandler);
			}
			if (this.stateSignal5ChangedHandler != null)
			{
				this.UnregisterSignal(this.ServerName, this.StateSignalName5, this.stateSignal5ChangedHandler);
			}
			if (this.stateSignal6ChangedHandler != null)
			{
				this.UnregisterSignal(this.ServerName, this.StateSignalName6, this.stateSignal6ChangedHandler);
			}
			if (this.stateSignal7ChangedHandler != null)
			{
				this.UnregisterSignal(this.ServerName, this.StateSignalName7, this.stateSignal7ChangedHandler);
			}
			if (this.stateSignal8ChangedHandler != null)
			{
				this.UnregisterSignal(this.ServerName, this.StateSignalName8, this.stateSignal8ChangedHandler);
			}
			if (this.visibilitySignalChangedHandler != null)
			{
				this.UnregisterSignal(this.ServerName, this.VisibilitySignalName, this.visibilitySignalChangedHandler);
			}
			this.connector.Dispose();
		}
		private void RemoveMouseEvents()
		{
			base.MouseEnter-=(new MouseEventHandler(this.OnIndicatorMouseEnter));
			base.MouseLeave-=(new MouseEventHandler(this.OnIndicatorMouseLeave));
		}
		protected virtual void OnUserContextChanged(string userName)
		{
		}
		private void resolvePlaceholders()
		{
			this.resolveParameterItemsPlaceholders();
			this.resolvePopupSourcePlaceholders();
			this.resolvePopupTitlePlaceholders();
		}
		private void resolveParameterItemsPlaceholders()
		{
			foreach (ParameterItem current in this.observableParameterItems)
			{
				current.ParameterName = StringPlaceholdersResolver.Resolve(current.ParameterName, this);
				current.ParameterValue = StringPlaceholdersResolver.Resolve(current.ParameterValue, this);
			}
		}
		private void resolvePopupSourcePlaceholders()
		{
			this.PopupSource = StringPlaceholdersResolver.Resolve(this.PopupSource, this);
		}
		private void resolvePopupTitlePlaceholders()
		{
			this.PopupTitle = StringPlaceholdersResolver.Resolve(this.PopupTitle, this);
		}
		private void EnableControl(bool enable)
		{
			if (!enable)
			{
				base.IsEnabled=(false);
				return;
			}
			if (this.isSecurityAvailable && !this.isAuthorized && this.SecurityDenyAccessBehaviour == DenyAccessBehaviour.Disable)
			{
				base.IsEnabled=(false);
				return;
			}
			base.IsEnabled=(true);
		}
		private void ShowControl(bool show)
		{
			if (!show)
			{
				base.Visibility= System.Windows.Visibility.Collapsed;
				return;
			}
			if (this.isSecurityAvailable && !this.isAuthorized && this.SecurityDenyAccessBehaviour == DenyAccessBehaviour.Hide)
			{
				base.Visibility= System.Windows.Visibility.Collapsed;
				return;
			}
			if (string.IsNullOrEmpty(this.VisibilitySignalName))
			{
				base.Visibility= System.Windows.Visibility.Visible;
				return;
			}
			if (this.canShowControlFromVisibilitySignal)
			{
				base.Visibility= System.Windows.Visibility.Visible;
				return;
			}
			base.Visibility= System.Windows.Visibility.Collapsed;
		}
		private bool convertStateValueToBool(double stateValue, int? maskValue)
		{
			bool result = false;
			if (!double.IsNaN(stateValue) && stateValue != 0.0)
			{
				if (maskValue.HasValue && maskValue.HasValue)
				{
					if (this.BinaryMask)
					{
						long num = Convert.ToInt64(stateValue);
						if ((num & (long)maskValue.Value) == (long)maskValue.Value)
						{
							result = true;
						}
					}
					else
					{
						if (stateValue >= (double)maskValue.Value)
						{
							result = true;
						}
					}
				}
				else
				{
					result = true;
				}
			}
			return result;
		}
		protected bool CanGoToState1()
		{
			if (this.conditionRule1Exp != null)
			{
				return this.CanGoToStateConditionRule1();
			}
			return this.convertStateValueToBool(this.stateSignalValue1, this.MaskSignal1);
		}
		protected bool CanGoToState2()
		{
			if (this.conditionRule2Exp != null)
			{
				return this.CanGoToStateConditionRule2();
			}
			return this.convertStateValueToBool(this.stateSignalValue2, this.MaskSignal2);
		}
		protected bool CanGoToState3()
		{
			if (this.conditionRule3Exp != null)
			{
				return this.CanGoToStateConditionRule3();
			}
			return this.convertStateValueToBool(this.stateSignalValue3, this.MaskSignal3);
		}
		protected bool CanGoToState4()
		{
			if (this.conditionRule4Exp != null)
			{
				return this.CanGoToStateConditionRule4();
			}
			return this.convertStateValueToBool(this.stateSignalValue4, this.MaskSignal4);
		}
		protected bool CanGoToState5()
		{
			if (this.conditionRule5Exp != null)
			{
				return this.CanGoToStateConditionRule5();
			}
			return this.convertStateValueToBool(this.stateSignalValue5, this.MaskSignal5);
		}
		protected bool CanGoToState6()
		{
			if (this.conditionRule6Exp != null)
			{
				return this.CanGoToStateConditionRule6();
			}
			return this.convertStateValueToBool(this.stateSignalValue6, this.MaskSignal6);
		}
		protected bool CanGoToState7()
		{
			if (this.conditionRule7Exp != null)
			{
				return this.CanGoToStateConditionRule7();
			}
			return this.convertStateValueToBool(this.stateSignalValue7, this.MaskSignal7);
		}
		protected bool CanGoToState8()
		{
			if (this.conditionRule8Exp != null)
			{
				return this.CanGoToStateConditionRule8();
			}
			return this.convertStateValueToBool(this.stateSignalValue8, this.MaskSignal8);
		}
		public bool CanGoToStateConditionRule1()
		{
			bool result = false;
			double num = double.NaN;
			try
			{
				num = this.conditionRule1Exp!.Evaluate();
			}
			catch (Exception ex)
			{
				num = double.NaN;
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
			if (!double.IsNaN(num) && num != 0.0)
			{
				result = true;
			}
			return result;
		}
		public bool CanGoToStateConditionRule2()
		{
			bool result = false;
			double num = double.NaN;
			try
			{
				num = this.conditionRule2Exp!.Evaluate();
			}
			catch (Exception ex)
			{
				num = double.NaN;
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
			if (!double.IsNaN(num) && num != 0.0)
			{
				result = true;
			}
			return result;
		}
		public bool CanGoToStateConditionRule3()
		{
			bool result = false;
			double num = double.NaN;
			try
			{
				num = this.conditionRule3Exp!.Evaluate();
			}
			catch (Exception ex)
			{
				num = double.NaN;
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
			if (!double.IsNaN(num) && num != 0.0)
			{
				result = true;
			}
			return result;
		}
		public bool CanGoToStateConditionRule4()
		{
			bool result = false;
			double num = double.NaN;
			try
			{
				num = this.conditionRule4Exp!.Evaluate();
			}
			catch (Exception ex)
			{
				num = double.NaN;
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
			if (!double.IsNaN(num) && num != 0.0)
			{
				result = true;
			}
			return result;
		}
		public bool CanGoToStateConditionRule5()
		{
			bool result = false;
			double num = double.NaN;
			try
			{
				num = this.conditionRule5Exp!.Evaluate();
			}
			catch (Exception ex)
			{
				num = double.NaN;
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
			if (!double.IsNaN(num) && num != 0.0)
			{
				result = true;
			}
			return result;
		}
		public bool CanGoToStateConditionRule6()
		{
			bool result = false;
			double num = double.NaN;
			try
			{
				num = this.conditionRule6Exp!.Evaluate();
			}
			catch (Exception ex)
			{
				num = double.NaN;
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
			if (!double.IsNaN(num) && num != 0.0)
			{
				result = true;
			}
			return result;
		}
		public bool CanGoToStateConditionRule7()
		{
			bool result = false;
			double num = double.NaN;
			try
			{
				num = this.conditionRule7Exp!.Evaluate();
			}
			catch (Exception ex)
			{
				num = double.NaN;
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
			if (!double.IsNaN(num) && num != 0.0)
			{
				result = true;
			}
			return result;
		}
		public bool CanGoToStateConditionRule8()
		{
			bool result = false;
			double num = double.NaN;
			try
			{
				num = this.conditionRule8Exp!.Evaluate();
			}
			catch (Exception ex)
			{
				num = double.NaN;
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
			if (!double.IsNaN(num) && num != 0.0)
			{
				result = true;
			}
			return result;
		}
		protected virtual void ChangeVisualState()
		{
			if (this.CanGoToState8())
			{
				VisualStateManager.GoToState(this, "State8", true);
				return;
			}
			if (this.CanGoToState7())
			{
				VisualStateManager.GoToState(this, "State7", true);
				return;
			}
			if (this.CanGoToState6())
			{
				VisualStateManager.GoToState(this, "State6", true);
				return;
			}
			if (this.CanGoToState5())
			{
				VisualStateManager.GoToState(this, "State5", true);
				return;
			}
			if (this.CanGoToState4())
			{
				VisualStateManager.GoToState(this, "State4", true);
				return;
			}
			if (this.CanGoToState3())
			{
				VisualStateManager.GoToState(this, "State3", true);
				return;
			}
			if (this.CanGoToState2())
			{
				VisualStateManager.GoToState(this, "State2", true);
				return;
			}
			if (this.CanGoToState1())
			{
				VisualStateManager.GoToState(this, "State1", true);
				return;
			}
			VisualStateManager.GoToState(this, "Normal", true);
		}
		private void RegisterConditionSignals()
		{
			foreach (ConditionSignalItem signalItem in this.observableConditionSignalItems)
			{
				if (!string.IsNullOrEmpty(signalItem.ParameterName) && !string.IsNullOrEmpty(signalItem.SignalName))
				{
					signalItem.ParameterName = signalItem.ParameterName.Trim();
					signalItem.SignalName = signalItem.SignalName.Trim();
					if (!string.IsNullOrEmpty(signalItem.ParameterName) && !string.IsNullOrEmpty(signalItem.SignalName))
					{
						signalItem.SignalName = this.SignalPrefix + signalItem.SignalName;
						signalItem.SignalName = SignalNameBuilder.BuildSignalName(signalItem.SignalName, this);
						if (!this.conditionParameters.ContainsKey(signalItem.ParameterName))
						{
							HMIConditionSignal conditionSignal = new HMIConditionSignal(signalItem.SignalName, signalItem.ParameterName);
							this.conditionParameters.Add(conditionSignal.ParameterName, conditionSignal);

							if (!this.dictSignalValueCache.ContainsKey(conditionSignal.Name))
							{
								this.dictSignalValueCache.Add(conditionSignal.Name, string.Empty);
							}
						}
					}
				}
			}

			if (this.dictSignalValueCache.Count > 0)
			{
				this.conditionSignalChangedHandler = new OnSignalChangedDelegate(this.OnConditionSignalValueChanged);
				foreach (var sigName in this.dictSignalValueCache.Keys)
				{
					this.RegisterSignal(this.ServerName, sigName, this.conditionSignalChangedHandler);
				}
			}
		}
		private void InitializeConditionRule1Expression()
		{
			if (!string.IsNullOrEmpty(this.ConditionRule1))
			{
				this.conditionRule1Exp = new ExpressionCompiler(this.ConditionRule1, true);
				try
				{
					this.conditionRule1Exp.Compile();
				}
				catch (Exception ex)
				{
					this.conditionRule1Exp = null;
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void InitializeConditionRule2Expression()
		{
			if (!string.IsNullOrEmpty(this.ConditionRule2))
			{
				this.conditionRule2Exp = new ExpressionCompiler(this.ConditionRule2, true);
				try
				{
					this.conditionRule2Exp.Compile();
				}
				catch (Exception ex)
				{
					this.conditionRule2Exp = null;
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void InitializeConditionRule3Expression()
		{
			if (!string.IsNullOrEmpty(this.ConditionRule3))
			{
				this.conditionRule3Exp = new ExpressionCompiler(this.ConditionRule3, true);
				try
				{
					this.conditionRule3Exp.Compile();
				}
				catch (Exception ex)
				{
					this.conditionRule3Exp = null;
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void InitializeConditionRule4Expression()
		{
			if (!string.IsNullOrEmpty(this.ConditionRule4))
			{
				this.conditionRule4Exp = new ExpressionCompiler(this.ConditionRule4, true);
				try
				{
					this.conditionRule4Exp.Compile();
				}
				catch (Exception ex)
				{
					this.conditionRule4Exp = null;
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void InitializeConditionRule5Expression()
		{
			if (!string.IsNullOrEmpty(this.ConditionRule5))
			{
				this.conditionRule5Exp = new ExpressionCompiler(this.ConditionRule5, true);
				try
				{
					this.conditionRule5Exp.Compile();
				}
				catch (Exception ex)
				{
					this.conditionRule5Exp = null;
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void InitializeConditionRule6Expression()
		{
			if (!string.IsNullOrEmpty(this.ConditionRule6))
			{
				this.conditionRule6Exp = new ExpressionCompiler(this.ConditionRule6, true);
				try
				{
					this.conditionRule6Exp.Compile();
				}
				catch (Exception ex)
				{
					this.conditionRule6Exp = null;
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void InitializeConditionRule7Expression()
		{
			if (!string.IsNullOrEmpty(this.ConditionRule7))
			{
				this.conditionRule7Exp = new ExpressionCompiler(this.ConditionRule7, true);
				try
				{
					this.conditionRule7Exp.Compile();
				}
				catch (Exception ex)
				{
					this.conditionRule7Exp = null;
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void InitializeConditionRule8Expression()
		{
			if (!string.IsNullOrEmpty(this.ConditionRule8))
			{
				this.conditionRule8Exp = new ExpressionCompiler(this.ConditionRule8, true);
				try
				{
					this.conditionRule8Exp.Compile();
				}
				catch (Exception ex)
				{
					this.conditionRule8Exp = null;
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private double ConvertSignalValueToDouble(object signalValue)
		{
			double result = double.NaN;
			try
			{
				result = double.Parse(signalValue.ToString());
			}
			catch
			{
				result = double.NaN;
			}
			return result;
		}
		protected void RegisterSignal(string serverName, string signalName, OnSignalChangedDelegate signalChangedDelegate)
		{
			try
			{
				this.connector.RegisterSignalChangedHandler(serverName, signalName, signalChangedDelegate);
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		protected void UnregisterSignal(string serverName, string signalName, OnSignalChangedDelegate signalChangedDelegate)
		{
			try
			{
				this.connector.UnregisterSignalChangedHandler(serverName, signalName, signalChangedDelegate);
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		private void AddMouseEvents()
		{
			base.MouseEnter+=(new MouseEventHandler(this.OnIndicatorMouseEnter));
			base.MouseLeave+=(new MouseEventHandler(this.OnIndicatorMouseLeave));
		}
	}
}
