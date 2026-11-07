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
namespace HMI.RuntimeControls.InputOutput
{
	[
		TemplatePart(Name = "OutputElement", Type = typeof(TextBlock)),
        TemplateVisualState(GroupName = "CommonStates", Name = "Normal"),
        TemplateVisualState(GroupName = "CommonStates", Name = "State1"),
        TemplateVisualState(GroupName = "CommonStates", Name = "State2"),
        TemplateVisualState(GroupName = "CommonStates", Name = "State3"),
        TemplateVisualState(GroupName = "CommonStates", Name = "State4"),
        TemplateVisualState(GroupName = "CommonStates", Name = "State5"),
        TemplateVisualState(GroupName = "CommonStates", Name = "State6"),
        TemplateVisualState(GroupName = "CommonStates", Name = "State7"),
        TemplateVisualState(GroupName = "CommonStates", Name = "State8")]
	public class HMIStateDisplay1 : Control, IDisposable
	{
		private const string ElementOutput = "OutputElement";

		private const string VisualStateGroupCommonStates = "CommonStates";
		private const string VisualStateNormal = "Normal";
		private const string VisualState1 = "State1";
		private const string VisualState2 = "State2";
		private const string VisualState3 = "State3";
		private const string VisualState4 = "State4";
		private const string VisualState5 = "State5";
		private const string VisualState6 = "State6";
		private const string VisualState7 = "State7";
		private const string VisualState8 = "State8";

		private const string KeyNormalState = "NormalState";
		private const string KeyState1 = "State1";
		private const string KeyState2 = "State2";
		private const string KeyState3 = "State3";
		private const string KeyState4 = "State4";
		private const string KeyState5 = "State5";
		private const string KeyState6 = "State6";
		private const string KeyState7 = "State7";
		private const string KeyState8 = "State8";

		private TextBlock tbOutputElement;
		private readonly DialogManager dialogManager;
		private IHMIConnector connector;

		private ObservableCollection<ConditionSignalItem> conditionSignals;
		private ObservableCollection<ParameterItem> popupParameters;
		private Dictionary<string, HMIConditionSignal> dictEquationCompilerParameters;
		private Dictionary<string, string> dictConditionSignals;

		private ExpressionCompiler equationCompiler1;
		private ExpressionCompiler equationCompiler2;
		private ExpressionCompiler equationCompiler3;
		private ExpressionCompiler equationCompiler4;
		private ExpressionCompiler equationCompiler5;
		private ExpressionCompiler equationCompiler6;
		private ExpressionCompiler equationCompiler7;
		private ExpressionCompiler equationCompiler8;

		private double stateSignal1Value;
		private double stateSignal2Value;
		private double stateSignal3Value;
		private double stateSignal4Value;
		private double stateSignal5Value;
		private double stateSignal6Value;
		private double stateSignal7Value;
		private double stateSignal8Value;

		private OnLanguageChangedDelegate delegateSymbolicTextTranslationsChanged;
		private OnSignalChangedDelegate delegateConditionSignalValueChanged;
		private OnSignalChangedDelegate delegateSignal1ValueChanged;
		private OnSignalChangedDelegate delegateSignal2ValueChanged;
		private OnSignalChangedDelegate delegateSignal3ValueChanged;
		private OnSignalChangedDelegate delegateSignal4ValueChanged;
		private OnSignalChangedDelegate delegateSignal5ValueChanged;
		private OnSignalChangedDelegate delegateSignal6ValueChanged;
		private OnSignalChangedDelegate delegateSignal7ValueChanged;
		private OnSignalChangedDelegate delegateSignal8ValueChanged;
		private OnSignalChangedDelegate delegateVisibilitySignalValueChanged;

		private Dictionary<string, SymbolicTextTranslation> dictTranslations;
		private Dictionary<string, SymbolicTextTranslation> dictStatesToTranslations;

		private bool canShowControlFromVisibilitySignal;
		private bool isSecurityAvailable;
		private bool isAuthorized;

		public static readonly DependencyProperty BinaryMaskProperty = DependencyProperty.Register("BinaryMask", typeof(bool), typeof(HMIStateDisplay1), new PropertyMetadata(false, new PropertyChangedCallback(HMIStateDisplay1.OnBinaryMaskPropertyChanged)));
		public static readonly DependencyProperty ConditionRule1Property = DependencyProperty.Register("ConditionRule1", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnConditionRule1PropertyChanged)));
		public static readonly DependencyProperty ConditionRule2Property = DependencyProperty.Register("ConditionRule2", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnConditionRule2PropertyChanged)));
		public static readonly DependencyProperty ConditionRule3Property = DependencyProperty.Register("ConditionRule3", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnConditionRule3PropertyChanged)));
		public static readonly DependencyProperty ConditionRule4Property = DependencyProperty.Register("ConditionRule4", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnConditionRule4PropertyChanged)));
		public static readonly DependencyProperty ConditionRule5Property = DependencyProperty.Register("ConditionRule5", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnConditionRule5PropertyChanged)));
		public static readonly DependencyProperty ConditionRule6Property = DependencyProperty.Register("ConditionRule6", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnConditionRule6PropertyChanged)));
		public static readonly DependencyProperty ConditionRule7Property = DependencyProperty.Register("ConditionRule7", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnConditionRule7PropertyChanged)));
		public static readonly DependencyProperty ConditionRule8Property = DependencyProperty.Register("ConditionRule8", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnConditionRule8PropertyChanged)));
		public static readonly DependencyProperty MaskSignal1Property = DependencyProperty.Register("MaskSignal1", typeof(int?), typeof(HMIStateDisplay1), new PropertyMetadata(null, new PropertyChangedCallback(HMIStateDisplay1.OnMaskSignal1PropertyChanged)));
		public static readonly DependencyProperty MaskSignal2Property = DependencyProperty.Register("MaskSignal2", typeof(int?), typeof(HMIStateDisplay1), new PropertyMetadata(null, new PropertyChangedCallback(HMIStateDisplay1.OnMaskSignal2PropertyChanged)));
		public static readonly DependencyProperty MaskSignal3Property = DependencyProperty.Register("MaskSignal3", typeof(int?), typeof(HMIStateDisplay1), new PropertyMetadata(null, new PropertyChangedCallback(HMIStateDisplay1.OnMaskSignal3PropertyChanged)));
		public static readonly DependencyProperty MaskSignal4Property = DependencyProperty.Register("MaskSignal4", typeof(int?), typeof(HMIStateDisplay1), new PropertyMetadata(null, new PropertyChangedCallback(HMIStateDisplay1.OnMaskSignal4PropertyChanged)));
		public static readonly DependencyProperty MaskSignal5Property = DependencyProperty.Register("MaskSignal5", typeof(int?), typeof(HMIStateDisplay1), new PropertyMetadata(null, new PropertyChangedCallback(HMIStateDisplay1.OnMaskSignal5PropertyChanged)));
		public static readonly DependencyProperty MaskSignal6Property = DependencyProperty.Register("MaskSignal6", typeof(int?), typeof(HMIStateDisplay1), new PropertyMetadata(null, new PropertyChangedCallback(HMIStateDisplay1.OnMaskSignal6PropertyChanged)));
		public static readonly DependencyProperty MaskSignal7Property = DependencyProperty.Register("MaskSignal7", typeof(int?), typeof(HMIStateDisplay1), new PropertyMetadata(null, new PropertyChangedCallback(HMIStateDisplay1.OnMaskSignal7PropertyChanged)));
		public static readonly DependencyProperty MaskSignal8Property = DependencyProperty.Register("MaskSignal8", typeof(int?), typeof(HMIStateDisplay1), new PropertyMetadata(null, new PropertyChangedCallback(HMIStateDisplay1.OnMaskSignal8PropertyChanged)));
		public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnObjectIDPropertyChanged)));
		public static readonly DependencyProperty PopupModalityProperty = DependencyProperty.Register("PopupModality", typeof(DialogModality), typeof(HMIStateDisplay1), new PropertyMetadata(DialogModality.Modeless, new PropertyChangedCallback(HMIStateDisplay1.OnPopupModalityPropertyChanged)));
		public static readonly DependencyProperty PopupSourceProperty = DependencyProperty.Register("PopupSource", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.PopupSourcePropertyChanged)));
		public static readonly DependencyProperty PopupTitleProperty = DependencyProperty.Register("PopupTitle", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.PopupTitlePropertyChanged)));
		public static readonly DependencyProperty RegisterSymbolicTextStateProperty = DependencyProperty.Register("RegisterSymbolicTextState", typeof(bool), typeof(HMIStateDisplay1), new PropertyMetadata(true, new PropertyChangedCallback(HMIStateDisplay1.OnRegisterSymbolicTextStatePropertyChanged)));
		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnSecurityAuthorizationNamePropertyChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMIStateDisplay1), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(HMIStateDisplay1.OnSecurityDenyAccessBehaviourPropertyChanged)));
		public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnServerNamePropertyChanged)));
		public static readonly DependencyProperty ShowPopupProperty = DependencyProperty.Register("ShowPopup", typeof(bool), typeof(HMIStateDisplay1), new PropertyMetadata(true, new PropertyChangedCallback(HMIStateDisplay1.ShowPopupPropertyChanged)));
		public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnSignalPrefixPropertyChanged)));
		public static readonly DependencyProperty StateSignalName1Property = DependencyProperty.Register("StateSignalName1", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnStateSignalName1PropertyChanged)));
		public static readonly DependencyProperty StateSignalName2Property = DependencyProperty.Register("StateSignalName2", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnStateSignalName2PropertyChanged)));
		public static readonly DependencyProperty StateSignalName3Property = DependencyProperty.Register("StateSignalName3", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnStateSignalName3PropertyChanged)));
		public static readonly DependencyProperty StateSignalName4Property = DependencyProperty.Register("StateSignalName4", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnStateSignalName4PropertyChanged)));
		public static readonly DependencyProperty StateSignalName5Property = DependencyProperty.Register("StateSignalName5", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnStateSignalName5PropertyChanged)));
		public static readonly DependencyProperty StateSignalName6Property = DependencyProperty.Register("StateSignalName6", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnStateSignalName6PropertyChanged)));
		public static readonly DependencyProperty StateSignalName7Property = DependencyProperty.Register("StateSignalName7", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnStateSignalName7PropertyChanged)));
		public static readonly DependencyProperty StateSignalName8Property = DependencyProperty.Register("StateSignalName8", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnStateSignalName8PropertyChanged)));
		public static readonly DependencyProperty SymbolicTextNormalStateProperty = DependencyProperty.Register("SymbolicTextNormalState", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnSymbolicTextNormalStatePropertyChanged)));
		public static readonly DependencyProperty SymbolicTextState1Property = DependencyProperty.Register("SymbolicTextState1", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnSymbolicTextState1PropertyChanged)));
		public static readonly DependencyProperty SymbolicTextState2Property = DependencyProperty.Register("SymbolicTextState2", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnSymbolicTextState2PropertyChanged)));
		public static readonly DependencyProperty SymbolicTextState3Property = DependencyProperty.Register("SymbolicTextState3", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnSymbolicTextState3PropertyChanged)));
		public static readonly DependencyProperty SymbolicTextState4Property = DependencyProperty.Register("SymbolicTextState4", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnSymbolicTextState4PropertyChanged)));
		public static readonly DependencyProperty SymbolicTextState5Property = DependencyProperty.Register("SymbolicTextState5", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnSymbolicTextState5PropertyChanged)));
		public static readonly DependencyProperty SymbolicTextState6Property = DependencyProperty.Register("SymbolicTextState6", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnSymbolicTextState6PropertyChanged)));
		public static readonly DependencyProperty SymbolicTextState7Property = DependencyProperty.Register("SymbolicTextState7", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnSymbolicTextState7PropertyChanged)));
		public static readonly DependencyProperty SymbolicTextState8Property = DependencyProperty.Register("SymbolicTextState8", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnSymbolicTextState8PropertyChanged)));
		public static readonly DependencyProperty VisibilityMaskProperty = DependencyProperty.Register("VisibilityMask", typeof(int?), typeof(HMIStateDisplay1), new PropertyMetadata(null, new PropertyChangedCallback(HMIStateDisplay1.OnVisibilityMaskPropertyChanged)));
		public static readonly DependencyProperty VisibilitySignalNameProperty = DependencyProperty.Register("VisibilitySignalName", typeof(string), typeof(HMIStateDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStateDisplay1.OnVisibilitySignalNamePropertyChanged)));
		
		[Category("HMI")]
		public bool BinaryMask
		{
			get
			{
				return (bool)base.GetValue(HMIStateDisplay1.BinaryMaskProperty);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.BinaryMaskProperty, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule1
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.ConditionRule1Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.ConditionRule1Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule2
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.ConditionRule2Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.ConditionRule2Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule3
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.ConditionRule3Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.ConditionRule3Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule4
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.ConditionRule4Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.ConditionRule4Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule5
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.ConditionRule5Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.ConditionRule5Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule6
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.ConditionRule6Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.ConditionRule6Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule7
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.ConditionRule7Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.ConditionRule7Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule8
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.ConditionRule8Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.ConditionRule8Property, value);
			}
		}
		[Category("HMI")]
		public ObservableCollection<ConditionSignalItem> ConditionSignals
		{
			get
			{
				return this.conditionSignals;
			}
		}
		private IHMIConnector Connector
		{
			get
			{
				return this.connector;
			}
		}
		private bool IsAuthorized
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
				return (int?)base.GetValue(HMIStateDisplay1.MaskSignal1Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.MaskSignal1Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal2
		{
			get
			{
				return (int?)base.GetValue(HMIStateDisplay1.MaskSignal2Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.MaskSignal2Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal3
		{
			get
			{
				return (int?)base.GetValue(HMIStateDisplay1.MaskSignal3Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.MaskSignal3Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal4
		{
			get
			{
				return (int?)base.GetValue(HMIStateDisplay1.MaskSignal4Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.MaskSignal4Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal5
		{
			get
			{
				return (int?)base.GetValue(HMIStateDisplay1.MaskSignal5Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.MaskSignal5Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal6
		{
			get
			{
				return (int?)base.GetValue(HMIStateDisplay1.MaskSignal6Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.MaskSignal6Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal7
		{
			get
			{
				return (int?)base.GetValue(HMIStateDisplay1.MaskSignal7Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.MaskSignal7Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal8
		{
			get
			{
				return (int?)base.GetValue(HMIStateDisplay1.MaskSignal8Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.MaskSignal8Property, value);
			}
		}
		[Category("HMI")]
		public string ObjectID
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.ObjectIDProperty);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.ObjectIDProperty, value);
			}
		}
		[Category("HMI")]
		public DialogModality PopupModality
		{
			get
			{
				return (DialogModality)base.GetValue(HMIStateDisplay1.PopupModalityProperty);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.PopupModalityProperty, value);
			}
		}
		[Category("HMI")]
		public ObservableCollection<ParameterItem> PopupParameters
		{
			get
			{
				return this.popupParameters;
			}
		}
		[Category("HMI")]
		public string PopupSource
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.PopupSourceProperty);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.PopupSourceProperty, value);
			}
		}
		[Category("HMI")]
		public string PopupTitle
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.PopupTitleProperty);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.PopupTitleProperty, value);
			}
		}
		[Category("HMI")]
		public bool RegisterSymbolicTextState
		{
			get
			{
				return (bool)base.GetValue(HMIStateDisplay1.RegisterSymbolicTextStateProperty);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.RegisterSymbolicTextStateProperty, value);
			}
		}
		[Category("HMI")]
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMIStateDisplay1.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		[Category("HMI")]
		private string ServerName
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.ServerNameProperty);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.ServerNameProperty, value);
			}
		}
		[Category("HMI")]
		public bool ShowPopup
		{
			get
			{
				return (bool)base.GetValue(HMIStateDisplay1.ShowPopupProperty);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.ShowPopupProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalPrefix
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.SignalPrefixProperty);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.SignalPrefixProperty, value);
			}
		}
		[Category("HMI")]
		public string StateSignalName1
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.StateSignalName1Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.StateSignalName1Property, value);
			}
		}
		[Category("HMI")]
		public string StateSignalName2
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.StateSignalName2Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.StateSignalName2Property, value);
			}
		}
		[Category("HMI")]
		public string StateSignalName3
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.StateSignalName3Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.StateSignalName3Property, value);
			}
		}
		[Category("HMI")]
		public string StateSignalName4
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.StateSignalName4Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.StateSignalName4Property, value);
			}
		}
		[Category("HMI")]
		public string StateSignalName5
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.StateSignalName5Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.StateSignalName5Property, value);
			}
		}
		[Category("HMI")]
		public string StateSignalName6
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.StateSignalName6Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.StateSignalName6Property, value);
			}
		}
		[Category("HMI")]
		public string StateSignalName7
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.StateSignalName7Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.StateSignalName7Property, value);
			}
		}
		[Category("HMI")]
		public string StateSignalName8
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.StateSignalName8Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.StateSignalName8Property, value);
			}
		}
		private double StateSignalValue1
		{
			get
			{
				return this.stateSignal1Value;
			}
		}
		private double StateSignalValue2
		{
			get
			{
				return this.stateSignal2Value;
			}
		}
		private double StateSignalValue3
		{
			get
			{
				return this.stateSignal3Value;
			}
		}
		private double StateSignalValue4
		{
			get
			{
				return this.stateSignal4Value;
			}
		}
		private double StateSignalValue5
		{
			get
			{
				return this.stateSignal5Value;
			}
		}
		private double StateSignalValue6
		{
			get
			{
				return this.stateSignal6Value;
			}
		}
		private double StateSignalValue7
		{
			get
			{
				return this.stateSignal7Value;
			}
		}
		private double StateSignalValue8
		{
			get
			{
				return this.stateSignal8Value;
			}
		}
		[Category("HMI")]
		public string SymbolicTextNormalState
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.SymbolicTextNormalStateProperty);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.SymbolicTextNormalStateProperty, value);
			}
		}
		[Category("HMI")]
		public string SymbolicTextState1
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.SymbolicTextState1Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.SymbolicTextState1Property, value);
			}
		}
		[Category("HMI")]
		public string SymbolicTextState2
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.SymbolicTextState2Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.SymbolicTextState2Property, value);
			}
		}
		[Category("HMI")]
		public string SymbolicTextState3
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.SymbolicTextState3Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.SymbolicTextState3Property, value);
			}
		}
		[Category("HMI")]
		public string SymbolicTextState4
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.SymbolicTextState4Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.SymbolicTextState4Property, value);
			}
		}
		[Category("HMI")]
		public string SymbolicTextState5
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.SymbolicTextState5Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.SymbolicTextState5Property, value);
			}
		}
		[Category("HMI")]
		public string SymbolicTextState6
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.SymbolicTextState6Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.SymbolicTextState6Property, value);
			}
		}
		[Category("HMI")]
		public string SymbolicTextState7
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.SymbolicTextState7Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.SymbolicTextState7Property, value);
			}
		}
		[Category("HMI")]
		public string SymbolicTextState8
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.SymbolicTextState8Property);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.SymbolicTextState8Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? VisibilityMask
		{
			get
			{
				return (int?)base.GetValue(HMIStateDisplay1.VisibilityMaskProperty);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.VisibilityMaskProperty, value);
			}
		}
		[Category("HMI")]
		public string VisibilitySignalName
		{
			get
			{
				return (string)base.GetValue(HMIStateDisplay1.VisibilitySignalNameProperty);
			}
			set
			{
				base.SetValue(HMIStateDisplay1.VisibilitySignalNameProperty, value);
			}
		}
		private static void OnBinaryMaskPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnConditionRule1PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnConditionRule2PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnConditionRule3PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnConditionRule4PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnConditionRule5PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnConditionRule6PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnConditionRule7PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnConditionRule8PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnMaskSignal1PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnMaskSignal2PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnMaskSignal3PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnMaskSignal4PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnMaskSignal5PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnMaskSignal6PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnMaskSignal7PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnMaskSignal8PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnObjectIDPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnPopupModalityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void PopupSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void PopupTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnRegisterSymbolicTextStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSecurityAuthorizationNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSecurityDenyAccessBehaviourPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnServerNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void ShowPopupPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSignalPrefixPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnStateSignalName1PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnStateSignalName2PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnStateSignalName3PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnStateSignalName4PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnStateSignalName5PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnStateSignalName6PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnStateSignalName7PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnStateSignalName8PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSymbolicTextNormalStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIStateDisplay1? wFStateDisplay = d as HMIStateDisplay1;
			if (wFStateDisplay != null && DesignerProperties.GetIsInDesignMode(wFStateDisplay) && wFStateDisplay.tbOutputElement != null)
			{
				wFStateDisplay.tbOutputElement.Text=((string)e.NewValue);
			}
		}
		private static void OnSymbolicTextState1PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSymbolicTextState2PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSymbolicTextState3PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSymbolicTextState4PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSymbolicTextState5PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSymbolicTextState6PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSymbolicTextState7PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSymbolicTextState8PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnVisibilityMaskPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnVisibilitySignalNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		public HMIStateDisplay1()
		{
			base.DefaultStyleKey=(typeof(HMIStateDisplay1));
			this.conditionSignals = new ObservableCollection<ConditionSignalItem>();
			this.popupParameters = new ObservableCollection<ParameterItem>();
			this.dictEquationCompilerParameters = new Dictionary<string, HMIConditionSignal>(StringComparer.InvariantCultureIgnoreCase);
			this.dictConditionSignals = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
			this.dictTranslations = new Dictionary<string, SymbolicTextTranslation>(StringComparer.InvariantCultureIgnoreCase);
			this.dictStatesToTranslations = new Dictionary<string, SymbolicTextTranslation>(StringComparer.InvariantCultureIgnoreCase);
			this.stateSignal1Value = double.NaN;
			this.stateSignal2Value = double.NaN;
			this.stateSignal3Value = double.NaN;
			this.stateSignal4Value = double.NaN;
			this.stateSignal5Value = double.NaN;
			this.stateSignal6Value = double.NaN;
			this.stateSignal7Value = double.NaN;
			this.stateSignal8Value = double.NaN;
			this.dialogManager = new DialogManager(null, null, null);
		}
		~HMIStateDisplay1()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void OnConditionSignalValueChangedHandler(string signalName, object signalValue)
		{
			foreach (var current in this.dictEquationCompilerParameters.Values)
			{
				if (current.Name.Equals(signalName, StringComparison.InvariantCultureIgnoreCase))
				{
					current.Value = HMINumericSignal.ConvertSignalValueToDouble(signalValue);
				}
				if (this.equationCompiler1 != null)
				{
					this.equationCompiler1.SetVariable(current.ParameterName, current.Value);
				}
				if (this.equationCompiler2 != null)
				{
					this.equationCompiler2.SetVariable(current.ParameterName, current.Value);
				}
				if (this.equationCompiler3 != null)
				{
					this.equationCompiler3.SetVariable(current.ParameterName, current.Value);
				}
				if (this.equationCompiler4 != null)
				{
					this.equationCompiler4.SetVariable(current.ParameterName, current.Value);
				}
				if (this.equationCompiler5 != null)
				{
					this.equationCompiler5.SetVariable(current.ParameterName, current.Value);
				}
				if (this.equationCompiler6 != null)
				{
					this.equationCompiler6.SetVariable(current.ParameterName, current.Value);
				}
				if (this.equationCompiler7 != null)
				{
					this.equationCompiler7.SetVariable(current.ParameterName, current.Value);
				}
				if (this.equationCompiler8 != null)
				{
					this.equationCompiler8.SetVariable(current.ParameterName, current.Value);
				}
			}

			this.ChangeVisualState();
		}
		private void OnStateSignal1ValueChangedHandler(string signalName, object signalValue)
		{
			this.stateSignal1Value = this.GetSignalValue(signalValue);
			this.ChangeVisualState();
		}
		private void OnStateSignal2ValueChangedHandler(string signalName, object signalValue)
		{
			this.stateSignal2Value = this.GetSignalValue(signalValue);
			this.ChangeVisualState();
		}
		private void OnStateSignal3ValueChangedHandler(string signalName, object signalValue)
		{
			this.stateSignal3Value = this.GetSignalValue(signalValue);
			this.ChangeVisualState();
		}
		private void OnStateSignal4ValueChangedHandler(string signalName, object signalValue)
		{
			this.stateSignal4Value = this.GetSignalValue(signalValue);
			this.ChangeVisualState();
		}
		private void OnStateSignal5ValueChangedHandler(string signalName, object signalValue)
		{
			this.stateSignal5Value = this.GetSignalValue(signalValue);
			this.ChangeVisualState();
		}
		private void OnStateSignal6ValueChangedHandler(string signalName, object signalValue)
		{
			this.stateSignal6Value = this.GetSignalValue(signalValue);
			this.ChangeVisualState();
		}
		private void OnStateSignal7ValueChangedHandler(string signalName, object signalValue)
		{
			this.stateSignal7Value = this.GetSignalValue(signalValue);
			this.ChangeVisualState();
		}
		private void OnStateSignal8ValueChangedHandler(string signalName, object signalValue)
		{
			this.stateSignal8Value = this.GetSignalValue(signalValue);
			this.ChangeVisualState();
		}
		private void OnVisibilitySignalValueChangedHandler(string signalName, object signalValue)
		{
			double visibilitySignalValue = this.GetSignalValue(signalValue);
			this.canShowControlFromVisibilitySignal =
				!double.IsNaN(visibilitySignalValue) &&
				visibilitySignalValue != 0.0 &&
				(!this.VisibilityMask.HasValue ||
				 (Convert.ToInt64(visibilitySignalValue) & (long)this.VisibilityMask.Value) != (long)this.VisibilityMask.Value);
			this.ShowControl(this.canShowControlFromVisibilitySignal);
		}
		private void OnSymbolicTextTranslationsChangedHandler(string[] symbolicTexts, string[] translationTexts)
		{
			try
			{
				for (int i = 0; i < symbolicTexts.Length; i++)
				{
					SymbolicTextTranslation symbolicTextTranslation = this.dictTranslations[symbolicTexts[i]];
					if (translationTexts[i] != null && !translationTexts[i].Equals("!null!", StringComparison.InvariantCultureIgnoreCase))
					{
						symbolicTextTranslation.Translation = translationTexts[i];
					}
					else
					{
						symbolicTextTranslation.Translation = symbolicTextTranslation.SymbolicText;
					}
				}
				this.ChangeVisualState();
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		private void HMIControl_MouseEnter(object sender, MouseEventArgs e)
		{
			if (string.IsNullOrEmpty(this.PopupSource))
			{
				return;
			}
			base.Cursor=(Cursors.Hand);
		}
		private void HMIControl_MouseLeave(object sender, MouseEventArgs e)
		{
			base.Cursor=(Cursors.Arrow);
		}
		private void connector_UserContextChanged(string userName)
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
			this.tbOutputElement = null;
			base.OnApplyTemplate();
			this.tbOutputElement = (base.GetTemplateChild("OutputElement") as TextBlock);
			base.IsTabStop=(false);
			bool isInDesignMode = DesignerProperties.GetIsInDesignMode(this);
			this.ResolvePopupPlaceholders();
			if (isInDesignMode)
			{
				if (this.tbOutputElement != null)
				{
					this.tbOutputElement.Text=(this.SymbolicTextNormalState);
				}
				return;
			}
			if (this.connector != null)
			{
				return;
			}
			this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
			if (this.tbOutputElement != null)
			{
				this.tbOutputElement.Text=(this.RegisterSymbolicTextState ? string.Empty : this.SymbolicTextNormalState);
			}
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
			this.RegisterSymbolicTexts();
			this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
			this.CreateEquationCompiler1();
			this.CreateEquationCompiler2();
			this.CreateEquationCompiler3();
			this.CreateEquationCompiler4();
			this.CreateEquationCompiler5();
			this.CreateEquationCompiler6();
			this.CreateEquationCompiler7();
			this.CreateEquationCompiler8();
			this.CreateAndRegisterConditionSignals();
			if (!string.IsNullOrEmpty(this.StateSignalName1))
			{
				this.StateSignalName1 = this.SignalPrefix + this.StateSignalName1;
				this.StateSignalName1 = SignalNameBuilder.BuildSignalName(this.StateSignalName1, this);
				this.delegateSignal1ValueChanged = new OnSignalChangedDelegate(this.OnStateSignal1ValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.StateSignalName1, this.delegateSignal1ValueChanged);
			}
			if (!string.IsNullOrEmpty(this.StateSignalName2))
			{
				this.StateSignalName2 = this.SignalPrefix + this.StateSignalName2;
				this.StateSignalName2 = SignalNameBuilder.BuildSignalName(this.StateSignalName2, this);
				this.delegateSignal2ValueChanged = new OnSignalChangedDelegate(this.OnStateSignal2ValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.StateSignalName2, this.delegateSignal2ValueChanged);
			}
			if (!string.IsNullOrEmpty(this.StateSignalName3))
			{
				this.StateSignalName3 = this.SignalPrefix + this.StateSignalName3;
				this.StateSignalName3 = SignalNameBuilder.BuildSignalName(this.StateSignalName3, this);
				this.delegateSignal3ValueChanged = new OnSignalChangedDelegate(this.OnStateSignal3ValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.StateSignalName3, this.delegateSignal3ValueChanged);
			}
			if (!string.IsNullOrEmpty(this.StateSignalName4))
			{
				this.StateSignalName4 = this.SignalPrefix + this.StateSignalName4;
				this.StateSignalName4 = SignalNameBuilder.BuildSignalName(this.StateSignalName4, this);
				this.delegateSignal4ValueChanged = new OnSignalChangedDelegate(this.OnStateSignal4ValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.StateSignalName4, this.delegateSignal4ValueChanged);
			}
			if (!string.IsNullOrEmpty(this.StateSignalName5))
			{
				this.StateSignalName5 = this.SignalPrefix + this.StateSignalName5;
				this.StateSignalName5 = SignalNameBuilder.BuildSignalName(this.StateSignalName5, this);
				this.delegateSignal5ValueChanged = new OnSignalChangedDelegate(this.OnStateSignal5ValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.StateSignalName5, this.delegateSignal5ValueChanged);
			}
			if (!string.IsNullOrEmpty(this.StateSignalName6))
			{
				this.StateSignalName6 = this.SignalPrefix + this.StateSignalName6;
				this.StateSignalName6 = SignalNameBuilder.BuildSignalName(this.StateSignalName6, this);
				this.delegateSignal6ValueChanged = new OnSignalChangedDelegate(this.OnStateSignal6ValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.StateSignalName6, this.delegateSignal6ValueChanged);
			}
			if (!string.IsNullOrEmpty(this.StateSignalName7))
			{
				this.StateSignalName7 = this.SignalPrefix + this.StateSignalName7;
				this.StateSignalName7 = SignalNameBuilder.BuildSignalName(this.StateSignalName7, this);
				this.delegateSignal7ValueChanged = new OnSignalChangedDelegate(this.OnStateSignal7ValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.StateSignalName7, this.delegateSignal7ValueChanged);
			}
			if (!string.IsNullOrEmpty(this.StateSignalName8))
			{
				this.StateSignalName8 = this.SignalPrefix + this.StateSignalName8;
				this.StateSignalName8 = SignalNameBuilder.BuildSignalName(this.StateSignalName8, this);
				this.delegateSignal8ValueChanged = new OnSignalChangedDelegate(this.OnStateSignal8ValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.StateSignalName8, this.delegateSignal8ValueChanged);
			}
			if (!string.IsNullOrEmpty(this.VisibilitySignalName))
			{
				this.VisibilitySignalName = this.SignalPrefix + this.VisibilitySignalName;
				this.VisibilitySignalName = SignalNameBuilder.BuildSignalName(this.VisibilitySignalName, this);
				this.delegateVisibilitySignalValueChanged = new OnSignalChangedDelegate(this.OnVisibilitySignalValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.VisibilitySignalName, this.delegateVisibilitySignalValueChanged);
			}
			this.RegisterMouseHandlers();
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
		private void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			this.dialogManager.Dispose();
			this.DisposeConnector();
			this.DisposeMouseHandlers();
			this.StopStoryboards();
		}
		private void DisposeConnector()
		{
			if (this.connector == null)
			{
				return;
			}
			this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
			if (this.delegateConditionSignalValueChanged != null)
			{
				using (Dictionary<string, string>.KeyCollection.Enumerator enumerator = this.dictConditionSignals.Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						this.UnregisterSignal(this.ServerName, current, this.delegateConditionSignalValueChanged);
					}
				}
			}
			if (this.delegateSignal1ValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.StateSignalName1, this.delegateSignal1ValueChanged);
			}
			if (this.delegateSignal2ValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.StateSignalName2, this.delegateSignal2ValueChanged);
			}
			if (this.delegateSignal3ValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.StateSignalName3, this.delegateSignal3ValueChanged);
			}
			if (this.delegateSignal4ValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.StateSignalName4, this.delegateSignal4ValueChanged);
			}
			if (this.delegateSignal5ValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.StateSignalName5, this.delegateSignal5ValueChanged);
			}
			if (this.delegateSignal6ValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.StateSignalName6, this.delegateSignal6ValueChanged);
			}
			if (this.delegateSignal7ValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.StateSignalName7, this.delegateSignal7ValueChanged);
			}
			if (this.delegateSignal8ValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.StateSignalName8, this.delegateSignal8ValueChanged);
			}
			if (this.delegateVisibilitySignalValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.VisibilitySignalName, this.delegateVisibilitySignalValueChanged);
			}
			if (this.delegateSymbolicTextTranslationsChanged != null)
			{
				this.connector.UnregisterLanguageChangedHandler(null, this.delegateSymbolicTextTranslationsChanged);
			}
			this.connector.Dispose();
		}
		private void DisposeMouseHandlers()
		{
			base.MouseEnter-=(new MouseEventHandler(this.HMIControl_MouseEnter));
			base.MouseLeave-=(new MouseEventHandler(this.HMIControl_MouseLeave));
		}
		private void RemoveFromUI(Control control)
		{
			string text = "LayoutRoot";
			Grid grid = base.GetTemplateChild(text) as Grid;
			if (grid != null && grid.Children.Contains(control))
			{
				grid.Children.Remove(control);
			}
		}
		private void OnUserContextChanged(string userName)
		{
		}
		private void ResolvePopupPlaceholders()
		{
			this.ResolvePopupParametersPlaceholders();
			this.ResolvePopupSourcePlaceholders();
			this.ResolvePopupTitlePlaceholders();
		}
		private void ResolvePopupParametersPlaceholders()
		{
			foreach (var current in this.popupParameters)
			{
				current.ParameterName = StringPlaceholdersResolver.Resolve(current.ParameterName, this);
				current.ParameterValue = StringPlaceholdersResolver.Resolve(current.ParameterValue, this);
			}
		}
		private void ResolvePopupSourcePlaceholders()
		{
			this.PopupSource = StringPlaceholdersResolver.Resolve(this.PopupSource, this);
		}
		private void ResolvePopupTitlePlaceholders()
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
		private bool CanGoToState(double stateSignalValue, int? maskSignal)
		{
			bool result = false;
			if (!double.IsNaN(stateSignalValue) && stateSignalValue != 0.0)
			{
				if (maskSignal.HasValue && maskSignal.HasValue)
				{
					if (this.BinaryMask)
					{
						long num = Convert.ToInt64(stateSignalValue);
						if ((num & (long)maskSignal.Value) == (long)maskSignal.Value)
						{
							result = true;
						}
					}
					else
					{
						if (stateSignalValue >= (double)maskSignal.Value)
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
		private bool CanGoToState1()
		{
			if (this.equationCompiler1 != null)
			{
				return this.CanGoToStateConditionRule1();
			}
			return this.CanGoToState(this.stateSignal1Value, this.MaskSignal1);
		}
		private bool CanGoToState2()
		{
			if (this.equationCompiler2 != null)
			{
				return this.CanGoToStateConditionRule2();
			}
			return this.CanGoToState(this.stateSignal2Value, this.MaskSignal2);
		}
		private bool CanGoToState3()
		{
			if (this.equationCompiler3 != null)
			{
				return this.CanGoToStateConditionRule3();
			}
			return this.CanGoToState(this.stateSignal3Value, this.MaskSignal3);
		}
		private bool CanGoToState4()
		{
			if (this.equationCompiler4 != null)
			{
				return this.CanGoToStateConditionRule4();
			}
			return this.CanGoToState(this.stateSignal4Value, this.MaskSignal4);
		}
		private bool CanGoToState5()
		{
			if (this.equationCompiler5 != null)
			{
				return this.CanGoToStateConditionRule5();
			}
			return this.CanGoToState(this.stateSignal5Value, this.MaskSignal5);
		}
		private bool CanGoToState6()
		{
			if (this.equationCompiler6 != null)
			{
				return this.CanGoToStateConditionRule6();
			}
			return this.CanGoToState(this.stateSignal6Value, this.MaskSignal6);
		}
		private bool CanGoToState7()
		{
			if (this.equationCompiler7 != null)
			{
				return this.CanGoToStateConditionRule7();
			}
			return this.CanGoToState(this.stateSignal7Value, this.MaskSignal7);
		}
		private bool CanGoToState8()
		{
			if (this.equationCompiler8 != null)
			{
				return this.CanGoToStateConditionRule8();
			}
			return this.CanGoToState(this.stateSignal8Value, this.MaskSignal8);
		}
		public bool CanGoToStateConditionRule1()
		{
			bool result = false;
			double num = double.NaN;
			try
			{
				num = this.equationCompiler1.Evaluate();
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
				num = this.equationCompiler2.Evaluate();
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
				num = this.equationCompiler3.Evaluate();
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
				num = this.equationCompiler4.Evaluate();
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
				num = this.equationCompiler5.Evaluate();
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
				num = this.equationCompiler6.Evaluate();
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
				num = this.equationCompiler7.Evaluate();
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
				num = this.equationCompiler8.Evaluate();
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
		private void ChangeVisualState()
		{
			if (this.CanGoToState8())
			{
				if (this.tbOutputElement != null)
				{
					this.tbOutputElement.Text=(this.dictStatesToTranslations["State8"].Translation);
				}
				VisualStateManager.GoToState(this, "State8", true);
				return;
			}
			if (this.CanGoToState7())
			{
				if (this.tbOutputElement != null)
				{
					this.tbOutputElement.Text=(this.dictStatesToTranslations["State7"].Translation);
				}
				VisualStateManager.GoToState(this, "State7", true);
				return;
			}
			if (this.CanGoToState6())
			{
				if (this.tbOutputElement != null)
				{
					this.tbOutputElement.Text=(this.dictStatesToTranslations["State6"].Translation);
				}
				VisualStateManager.GoToState(this, "State6", true);
				return;
			}
			if (this.CanGoToState5())
			{
				if (this.tbOutputElement != null)
				{
					this.tbOutputElement.Text=(this.dictStatesToTranslations["State5"].Translation);
				}
				VisualStateManager.GoToState(this, "State5", true);
				return;
			}
			if (this.CanGoToState4())
			{
				if (this.tbOutputElement != null)
				{
					this.tbOutputElement.Text=(this.dictStatesToTranslations["State4"].Translation);
				}
				VisualStateManager.GoToState(this, "State4", true);
				return;
			}
			if (this.CanGoToState3())
			{
				if (this.tbOutputElement != null)
				{
					this.tbOutputElement.Text=(this.dictStatesToTranslations["State3"].Translation);
				}
				VisualStateManager.GoToState(this, "State3", true);
				return;
			}
			if (this.CanGoToState2())
			{
				if (this.tbOutputElement != null)
				{
					this.tbOutputElement.Text=(this.dictStatesToTranslations["State2"].Translation);
				}
				VisualStateManager.GoToState(this, "State2", true);
				return;
			}
			if (this.CanGoToState1())
			{
				if (this.tbOutputElement != null)
				{
					this.tbOutputElement.Text=(this.dictStatesToTranslations["State1"].Translation);
				}
				VisualStateManager.GoToState(this, "State1", true);
				return;
			}
			if (this.tbOutputElement != null)
			{
				this.tbOutputElement.Text=(this.dictStatesToTranslations["NormalState"].Translation);
			}
			VisualStateManager.GoToState(this, "Normal", true);
		}
		private void CreateAndRegisterConditionSignals()
		{
			foreach (var current in this.conditionSignals)
			{
				if (!string.IsNullOrEmpty(current.ParameterName) && !string.IsNullOrEmpty(current.SignalName))
				{
					current.ParameterName = current.ParameterName.Trim();
					current.SignalName = current.SignalName.Trim();
					if (!string.IsNullOrEmpty(current.ParameterName) && !string.IsNullOrEmpty(current.SignalName))
					{
						current.SignalName = this.SignalPrefix + current.SignalName;
						current.SignalName = SignalNameBuilder.BuildSignalName(current.SignalName, this);
						if (!this.dictEquationCompilerParameters.ContainsKey(current.ParameterName))
						{
							HMIConditionSignal wFConditionSignal = new HMIConditionSignal(current.SignalName, current.ParameterName);
							this.dictEquationCompilerParameters.Add(wFConditionSignal.ParameterName, wFConditionSignal);
							if (!this.dictConditionSignals.ContainsKey(wFConditionSignal.Name))
							{
								this.dictConditionSignals.Add(wFConditionSignal.Name, string.Empty);
							}
						}
					}
				}
			}
			if (this.dictConditionSignals.Count > 0)
			{
				this.delegateConditionSignalValueChanged = new OnSignalChangedDelegate(this.OnConditionSignalValueChangedHandler);
				foreach (var conditionSignalName in this.dictConditionSignals.Keys)
				{
					this.RegisterSignal(this.ServerName, conditionSignalName, this.delegateConditionSignalValueChanged);
				}
			}
		}
		private void CreateEquationCompiler1()
		{
			if (!string.IsNullOrEmpty(this.ConditionRule1))
			{
				this.equationCompiler1 = new ExpressionCompiler(this.ConditionRule1, true);
				try
				{
					this.equationCompiler1.Compile();
				}
				catch (Exception ex)
				{
					this.equationCompiler1 = null;
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void CreateEquationCompiler2()
		{
			if (!string.IsNullOrEmpty(this.ConditionRule2))
			{
				this.equationCompiler2 = new ExpressionCompiler(this.ConditionRule2, true);
				try
				{
					this.equationCompiler2.Compile();
				}
				catch (Exception ex)
				{
					this.equationCompiler2 = null;
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void CreateEquationCompiler3()
		{
			if (!string.IsNullOrEmpty(this.ConditionRule3))
			{
				this.equationCompiler3 = new ExpressionCompiler(this.ConditionRule3, true);
				try
				{
					this.equationCompiler3.Compile();
				}
				catch (Exception ex)
				{
					this.equationCompiler3 = null;
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void CreateEquationCompiler4()
		{
			if (!string.IsNullOrEmpty(this.ConditionRule4))
			{
				this.equationCompiler4 = new ExpressionCompiler(this.ConditionRule4, true);
				try
				{
					this.equationCompiler4.Compile();
				}
				catch (Exception ex)
				{
					this.equationCompiler4 = null;
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void CreateEquationCompiler5()
		{
			if (!string.IsNullOrEmpty(this.ConditionRule5))
			{
				this.equationCompiler5 = new ExpressionCompiler(this.ConditionRule5, true);
				try
				{
					this.equationCompiler5.Compile();
				}
				catch (Exception ex)
				{
					this.equationCompiler5 = null;
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void CreateEquationCompiler6()
		{
			if (!string.IsNullOrEmpty(this.ConditionRule6))
			{
				this.equationCompiler6 = new ExpressionCompiler(this.ConditionRule6, true);
				try
				{
					this.equationCompiler6.Compile();
				}
				catch (Exception ex)
				{
					this.equationCompiler6 = null;
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void CreateEquationCompiler7()
		{
			if (!string.IsNullOrEmpty(this.ConditionRule7))
			{
				this.equationCompiler7 = new ExpressionCompiler(this.ConditionRule7, true);
				try
				{
					this.equationCompiler7.Compile();
				}
				catch (Exception ex)
				{
					this.equationCompiler7 = null;
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void CreateEquationCompiler8()
		{
			if (!string.IsNullOrEmpty(this.ConditionRule8))
			{
				this.equationCompiler8 = new ExpressionCompiler(this.ConditionRule8, true);
				try
				{
					this.equationCompiler8.Compile();
				}
				catch (Exception ex)
				{
					this.equationCompiler8 = null;
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private double GetSignalValue(object value)
		{
			double result = double.NaN;
			try
			{
				result = double.Parse(value.ToString());
			}
			catch
			{
				result = double.NaN;
			}
			return result;
		}
		private void RegisterSignal(string serverName, string signalName, OnSignalChangedDelegate signalChangedDelegate)
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
		private void UnregisterSignal(string serverName, string signalName, OnSignalChangedDelegate signalChangedDelegate)
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
		private void RegisterSymbolicTexts()
		{
			if (this.RegisterSymbolicTextState)
			{
				this.SymbolicTextNormalState = StringPlaceholdersResolver.Resolve(this.SymbolicTextNormalState, this);
				this.SymbolicTextState1 = StringPlaceholdersResolver.Resolve(this.SymbolicTextState1, this);
				this.SymbolicTextState2 = StringPlaceholdersResolver.Resolve(this.SymbolicTextState2, this);
				this.SymbolicTextState3 = StringPlaceholdersResolver.Resolve(this.SymbolicTextState3, this);
				this.SymbolicTextState4 = StringPlaceholdersResolver.Resolve(this.SymbolicTextState4, this);
				this.SymbolicTextState5 = StringPlaceholdersResolver.Resolve(this.SymbolicTextState5, this);
				this.SymbolicTextState6 = StringPlaceholdersResolver.Resolve(this.SymbolicTextState6, this);
				this.SymbolicTextState7 = StringPlaceholdersResolver.Resolve(this.SymbolicTextState7, this);
				this.SymbolicTextState8 = StringPlaceholdersResolver.Resolve(this.SymbolicTextState8, this);
			}
			SymbolicTextTranslation symbolicTextTranslation = new SymbolicTextTranslation(this.SymbolicTextNormalState, this.SymbolicTextNormalState);
			if (!this.dictTranslations.ContainsKey(symbolicTextTranslation.SymbolicText))
			{
				this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
			}
			this.dictStatesToTranslations.Add("NormalState", this.dictTranslations[symbolicTextTranslation.SymbolicText]);
			symbolicTextTranslation = new SymbolicTextTranslation(this.SymbolicTextState1, this.SymbolicTextState1);
			if (!this.dictTranslations.ContainsKey(symbolicTextTranslation.SymbolicText))
			{
				this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
			}
			this.dictStatesToTranslations.Add("State1", this.dictTranslations[symbolicTextTranslation.SymbolicText]);
			symbolicTextTranslation = new SymbolicTextTranslation(this.SymbolicTextState2, this.SymbolicTextState2);
			if (!this.dictTranslations.ContainsKey(symbolicTextTranslation.SymbolicText))
			{
				this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
			}
			this.dictStatesToTranslations.Add("State2", this.dictTranslations[symbolicTextTranslation.SymbolicText]);
			symbolicTextTranslation = new SymbolicTextTranslation(this.SymbolicTextState3, this.SymbolicTextState3);
			if (!this.dictTranslations.ContainsKey(symbolicTextTranslation.SymbolicText))
			{
				this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
			}
			this.dictStatesToTranslations.Add("State3", this.dictTranslations[symbolicTextTranslation.SymbolicText]);
			symbolicTextTranslation = new SymbolicTextTranslation(this.SymbolicTextState4, this.SymbolicTextState4);
			if (!this.dictTranslations.ContainsKey(symbolicTextTranslation.SymbolicText))
			{
				this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
			}
			this.dictStatesToTranslations.Add("State4", this.dictTranslations[symbolicTextTranslation.SymbolicText]);
			symbolicTextTranslation = new SymbolicTextTranslation(this.SymbolicTextState5, this.SymbolicTextState5);
			if (!this.dictTranslations.ContainsKey(symbolicTextTranslation.SymbolicText))
			{
				this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
			}
			this.dictStatesToTranslations.Add("State5", this.dictTranslations[symbolicTextTranslation.SymbolicText]);
			symbolicTextTranslation = new SymbolicTextTranslation(this.SymbolicTextState6, this.SymbolicTextState6);
			if (!this.dictTranslations.ContainsKey(symbolicTextTranslation.SymbolicText))
			{
				this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
			}
			this.dictStatesToTranslations.Add("State6", this.dictTranslations[symbolicTextTranslation.SymbolicText]);
			symbolicTextTranslation = new SymbolicTextTranslation(this.SymbolicTextState7, this.SymbolicTextState7);
			if (!this.dictTranslations.ContainsKey(symbolicTextTranslation.SymbolicText))
			{
				this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
			}
			this.dictStatesToTranslations.Add("State7", this.dictTranslations[symbolicTextTranslation.SymbolicText]);
			symbolicTextTranslation = new SymbolicTextTranslation(this.SymbolicTextState8, this.SymbolicTextState8);
			if (!this.dictTranslations.ContainsKey(symbolicTextTranslation.SymbolicText))
			{
				this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
			}
			this.dictStatesToTranslations.Add("State8", this.dictTranslations[symbolicTextTranslation.SymbolicText]);
			if (this.RegisterSymbolicTextState)
			{
				try
				{
					string[] array = new string[this.dictTranslations.Count];
					this.dictTranslations.Keys.CopyTo(array, 0);
					this.delegateSymbolicTextTranslationsChanged = new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler);
					this.connector.RegisterLanguageChangedHandler(array, this.delegateSymbolicTextTranslationsChanged);
				}
				catch (Exception ex)
				{
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void RegisterMouseHandlers()
		{
			base.MouseEnter+=(new MouseEventHandler(this.HMIControl_MouseEnter));
			base.MouseLeave+=(new MouseEventHandler(this.HMIControl_MouseLeave));
		}
	}
}
