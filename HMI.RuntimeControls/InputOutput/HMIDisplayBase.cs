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
		TemplateVisualState(GroupName = "CommonStates", Name = "Limit3"),
		TemplateVisualState(GroupName = "CommonStates", Name = "Normal"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Limit4"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Limit1"),
		TemplateVisualState(GroupName = "CommonStates", Name = "Limit2")]
	public abstract class HMIDisplayBase : Control, IDisposable
	{
		protected const string ElementOutput = "OutputElement";
		protected const string VisualStateGroupCommonStates = "CommonStates";
		protected const string VisualStateNormal = "Normal";
		protected const string VisualStateLimit1 = "Limit1";
		protected const string VisualStateLimit2 = "Limit2";
		protected const string VisualStateLimit3 = "Limit3";
		protected const string VisualStateLimit4 = "Limit4";

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

		private OnSignalChangedDelegate delegateConditionSignalValueChanged;
		private OnSignalChangedDelegate delegateSignalValueChanged;
		private OnSignalChangedDelegate delegateLimitSignal1ValueChanged;
		private OnSignalChangedDelegate delegateLimitSignal2ValueChanged;
		private OnSignalChangedDelegate delegateLimitSignal3ValueChanged;
		private OnSignalChangedDelegate delegateLimitSignal4ValueChanged;
		private OnSignalChangedDelegate delegateVisibilitySignalValueChanged;

		private object signalValue;
		private double limitSignal1Value;
		private double limitSignal2Value;
		private double limitSignal3Value;
		private double limitSignal4Value;
		private bool canShowControlFromVisibilitySignal;
		private bool isSecurityAvailable;
		private bool isAuthorized;

		public static readonly DependencyProperty BinaryMaskProperty = DependencyProperty.Register("BinaryMask", typeof(bool), typeof(HMIDisplayBase), new PropertyMetadata(false, new PropertyChangedCallback(HMIDisplayBase.OnBinaryMaskPropertyChanged)));
		public static readonly DependencyProperty ConditionRule1Property = DependencyProperty.Register("ConditionRule1", typeof(string), typeof(HMIDisplayBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDisplayBase.OnConditionRule1PropertyChanged)));
		public static readonly DependencyProperty ConditionRule2Property = DependencyProperty.Register("ConditionRule2", typeof(string), typeof(HMIDisplayBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDisplayBase.OnConditionRule2PropertyChanged)));
		public static readonly DependencyProperty ConditionRule3Property = DependencyProperty.Register("ConditionRule3", typeof(string), typeof(HMIDisplayBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDisplayBase.OnConditionRule3PropertyChanged)));
		public static readonly DependencyProperty ConditionRule4Property = DependencyProperty.Register("ConditionRule4", typeof(string), typeof(HMIDisplayBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDisplayBase.OnConditionRule4PropertyChanged)));
		public static readonly DependencyProperty LimitSignalName1Property = DependencyProperty.Register("LimitSignalName1", typeof(string), typeof(HMIDisplayBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDisplayBase.OnLimitSignalName1PropertyChanged)));
		public static readonly DependencyProperty LimitSignalName2Property = DependencyProperty.Register("LimitSignalName2", typeof(string), typeof(HMIDisplayBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDisplayBase.OnLimitSignalName2PropertyChanged)));
		public static readonly DependencyProperty LimitSignalName3Property = DependencyProperty.Register("LimitSignalName3", typeof(string), typeof(HMIDisplayBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDisplayBase.OnLimitSignalName3PropertyChanged)));
		public static readonly DependencyProperty LimitSignalName4Property = DependencyProperty.Register("LimitSignalName4", typeof(string), typeof(HMIDisplayBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDisplayBase.OnLimitSignalName4PropertyChanged)));
		public static readonly DependencyProperty MaskSignal1Property = DependencyProperty.Register("MaskSignal1", typeof(int?), typeof(HMIDisplayBase), new PropertyMetadata(null, new PropertyChangedCallback(HMIDisplayBase.OnMaskSignal1PropertyChanged)));
		public static readonly DependencyProperty MaskSignal2Property = DependencyProperty.Register("MaskSignal2", typeof(int?), typeof(HMIDisplayBase), new PropertyMetadata(null, new PropertyChangedCallback(HMIDisplayBase.OnMaskSignal2PropertyChanged)));
		public static readonly DependencyProperty MaskSignal3Property = DependencyProperty.Register("MaskSignal3", typeof(int?), typeof(HMIDisplayBase), new PropertyMetadata(null, new PropertyChangedCallback(HMIDisplayBase.OnMaskSignal3PropertyChanged)));
		public static readonly DependencyProperty MaskSignal4Property = DependencyProperty.Register("MaskSignal4", typeof(int?), typeof(HMIDisplayBase), new PropertyMetadata(null, new PropertyChangedCallback(HMIDisplayBase.OnMaskSignal4PropertyChanged)));
		public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMIDisplayBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDisplayBase.OnObjectIDPropertyChanged)));
		public static readonly DependencyProperty PopupModalityProperty = DependencyProperty.Register("PopupModality", typeof(DialogModality), typeof(HMIDisplayBase), new PropertyMetadata(DialogModality.Modeless, new PropertyChangedCallback(HMIDisplayBase.OnPopupModalityPropertyChanged)));
		public static readonly DependencyProperty PopupSourceProperty = DependencyProperty.Register("PopupSource", typeof(string), typeof(HMIDisplayBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDisplayBase.PopupSourcePropertyChanged)));
		public static readonly DependencyProperty PopupTitleProperty = DependencyProperty.Register("PopupTitle", typeof(string), typeof(HMIDisplayBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDisplayBase.PopupTitlePropertyChanged)));
		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMIDisplayBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDisplayBase.OnSecurityAuthorizationNamePropertyChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMIDisplayBase), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(HMIDisplayBase.OnSecurityDenyAccessBehaviourPropertyChanged)));
		public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(HMIDisplayBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDisplayBase.OnServerNamePropertyChanged)));
		public static readonly DependencyProperty ShowPopupProperty = DependencyProperty.Register("ShowPopup", typeof(bool), typeof(HMIDisplayBase), new PropertyMetadata(true, new PropertyChangedCallback(HMIDisplayBase.ShowPopupPropertyChanged)));
		public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMIDisplayBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDisplayBase.OnSignalNamePropertyChanged)));
		public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMIDisplayBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDisplayBase.OnSignalPrefixPropertyChanged)));
		public static readonly DependencyProperty VisibilityMaskProperty = DependencyProperty.Register("VisibilityMask", typeof(int?), typeof(HMIDisplayBase), new PropertyMetadata(null, new PropertyChangedCallback(HMIDisplayBase.OnVisibilityMaskPropertyChanged)));
		public static readonly DependencyProperty VisibilitySignalNameProperty = DependencyProperty.Register("VisibilitySignalName", typeof(string), typeof(HMIDisplayBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDisplayBase.OnVisibilitySignalNamePropertyChanged)));
		
		[Category("HMI")]
		public bool BinaryMask
		{
			get
			{
				return (bool)base.GetValue(HMIDisplayBase.BinaryMaskProperty);
			}
			set
			{
				base.SetValue(HMIDisplayBase.BinaryMaskProperty, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule1
		{
			get
			{
				return (string)base.GetValue(HMIDisplayBase.ConditionRule1Property);
			}
			set
			{
				base.SetValue(HMIDisplayBase.ConditionRule1Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule2
		{
			get
			{
				return (string)base.GetValue(HMIDisplayBase.ConditionRule2Property);
			}
			set
			{
				base.SetValue(HMIDisplayBase.ConditionRule2Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule3
		{
			get
			{
				return (string)base.GetValue(HMIDisplayBase.ConditionRule3Property);
			}
			set
			{
				base.SetValue(HMIDisplayBase.ConditionRule3Property, value);
			}
		}
		[Category("HMI")]
		public string ConditionRule4
		{
			get
			{
				return (string)base.GetValue(HMIDisplayBase.ConditionRule4Property);
			}
			set
			{
				base.SetValue(HMIDisplayBase.ConditionRule4Property, value);
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
		[Category("HMI")]
		public string LimitSignalName1
		{
			get
			{
				return (string)base.GetValue(HMIDisplayBase.LimitSignalName1Property);
			}
			set
			{
				base.SetValue(HMIDisplayBase.LimitSignalName1Property, value);
			}
		}
		[Category("HMI")]
		public string LimitSignalName2
		{
			get
			{
				return (string)base.GetValue(HMIDisplayBase.LimitSignalName2Property);
			}
			set
			{
				base.SetValue(HMIDisplayBase.LimitSignalName2Property, value);
			}
		}
		[Category("HMI")]
		public string LimitSignalName3
		{
			get
			{
				return (string)base.GetValue(HMIDisplayBase.LimitSignalName3Property);
			}
			set
			{
				base.SetValue(HMIDisplayBase.LimitSignalName3Property, value);
			}
		}
		[Category("HMI")]
		public string LimitSignalName4
		{
			get
			{
				return (string)base.GetValue(HMIDisplayBase.LimitSignalName4Property);
			}
			set
			{
				base.SetValue(HMIDisplayBase.LimitSignalName4Property, value);
			}
		}
		protected double LimitSignalValue1
		{
			get
			{
				return this.limitSignal1Value;
			}
		}
		protected double LimitSignalValue2
		{
			get
			{
				return this.limitSignal2Value;
			}
		}
		protected double LimitSignalValue3
		{
			get
			{
				return this.limitSignal3Value;
			}
		}
		protected double LimitSignalValue4
		{
			get
			{
				return this.limitSignal4Value;
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal1
		{
			get
			{
				return (int?)base.GetValue(HMIDisplayBase.MaskSignal1Property);
			}
			set
			{
				base.SetValue(HMIDisplayBase.MaskSignal1Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal2
		{
			get
			{
				return (int?)base.GetValue(HMIDisplayBase.MaskSignal2Property);
			}
			set
			{
				base.SetValue(HMIDisplayBase.MaskSignal2Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal3
		{
			get
			{
				return (int?)base.GetValue(HMIDisplayBase.MaskSignal3Property);
			}
			set
			{
				base.SetValue(HMIDisplayBase.MaskSignal3Property, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? MaskSignal4
		{
			get
			{
				return (int?)base.GetValue(HMIDisplayBase.MaskSignal4Property);
			}
			set
			{
				base.SetValue(HMIDisplayBase.MaskSignal4Property, value);
			}
		}
		[Category("HMI")]
		public string ObjectID
		{
			get
			{
				return (string)base.GetValue(HMIDisplayBase.ObjectIDProperty);
			}
			set
			{
				base.SetValue(HMIDisplayBase.ObjectIDProperty, value);
			}
		}
		protected TextBlock OutputElement
		{
			get
			{
				return this.tbOutputElement;
			}
		}
		[Category("HMI")]
		public DialogModality PopupModality
		{
			get
			{
				return (DialogModality)base.GetValue(HMIDisplayBase.PopupModalityProperty);
			}
			set
			{
				base.SetValue(HMIDisplayBase.PopupModalityProperty, value);
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
				return (string)base.GetValue(HMIDisplayBase.PopupSourceProperty);
			}
			set
			{
				base.SetValue(HMIDisplayBase.PopupSourceProperty, value);
			}
		}
		[Category("HMI")]
		public string PopupTitle
		{
			get
			{
				return (string)base.GetValue(HMIDisplayBase.PopupTitleProperty);
			}
			set
			{
				base.SetValue(HMIDisplayBase.PopupTitleProperty, value);
			}
		}
		[Category("HMI")]
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMIDisplayBase.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMIDisplayBase.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMIDisplayBase.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMIDisplayBase.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		[Category("HMI")]
		protected string ServerName
		{
			get
			{
				return (string)base.GetValue(HMIDisplayBase.ServerNameProperty);
			}
			set
			{
				base.SetValue(HMIDisplayBase.ServerNameProperty, value);
			}
		}
		[Category("HMI")]
		public bool ShowPopup
		{
			get
			{
				return (bool)base.GetValue(HMIDisplayBase.ShowPopupProperty);
			}
			set
			{
				base.SetValue(HMIDisplayBase.ShowPopupProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalName
		{
			get
			{
				return (string)base.GetValue(HMIDisplayBase.SignalNameProperty);
			}
			set
			{
				base.SetValue(HMIDisplayBase.SignalNameProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalPrefix
		{
			get
			{
				return (string)base.GetValue(HMIDisplayBase.SignalPrefixProperty);
			}
			set
			{
				base.SetValue(HMIDisplayBase.SignalPrefixProperty, value);
			}
		}
		protected object SignalValue
		{
			get
			{
				return this.signalValue;
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? VisibilityMask
		{
			get
			{
				return (int?)base.GetValue(HMIDisplayBase.VisibilityMaskProperty);
			}
			set
			{
				base.SetValue(HMIDisplayBase.VisibilityMaskProperty, value);
			}
		}
		[Category("HMI")]
		public string VisibilitySignalName
		{
			get
			{
				return (string)base.GetValue(HMIDisplayBase.VisibilitySignalNameProperty);
			}
			set
			{
				base.SetValue(HMIDisplayBase.VisibilitySignalNameProperty, value);
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
		private static void OnLimitSignalName1PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnLimitSignalName2PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnLimitSignalName3PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnLimitSignalName4PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
		private static void OnObjectIDPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIDisplayBase wFDisplayBase = d as HMIDisplayBase;
			if (wFDisplayBase != null && DesignerProperties.GetIsInDesignMode(wFDisplayBase))
			{
				wFDisplayBase.UpdateOutputInDesignMode();
			}
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
		private static void OnSignalNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIDisplayBase wFDisplayBase = d as HMIDisplayBase;
			if (wFDisplayBase != null && DesignerProperties.GetIsInDesignMode(wFDisplayBase))
			{
				wFDisplayBase.UpdateOutputInDesignMode();
			}
		}
		private static void OnSignalPrefixPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIDisplayBase? wFDisplayBase = d as HMIDisplayBase;
			if (wFDisplayBase != null && DesignerProperties.GetIsInDesignMode(wFDisplayBase))
			{
				wFDisplayBase.UpdateOutputInDesignMode();
			}
		}
		private static void OnVisibilityMaskPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnVisibilitySignalNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		public HMIDisplayBase()
		{
			base.DefaultStyleKey=(typeof(HMIDisplayBase));
			this.conditionSignals = new ObservableCollection<ConditionSignalItem>();
			this.popupParameters = new ObservableCollection<ParameterItem>();
			this.dictEquationCompilerParameters = new Dictionary<string, HMIConditionSignal>(StringComparer.InvariantCultureIgnoreCase);
			this.dictConditionSignals = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
			this.signalValue = double.NaN;
			this.limitSignal1Value = double.NaN;
			this.limitSignal2Value = double.NaN;
			this.limitSignal3Value = double.NaN;
			this.limitSignal4Value = double.NaN;
			this.dialogManager = new DialogManager(null, null, null);
		}
		~HMIDisplayBase()
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
			using (Dictionary<string, HMIConditionSignal>.ValueCollection.Enumerator enumerator = this.dictEquationCompilerParameters.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HMIConditionSignal current = enumerator.Current;
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
				}
			}
			this.ChangeVisualState();
		}
		private void OnSignalValueChangedHandler(string signalName, object signalValue)
		{
			this.signalValue = signalValue;
			this.OnUpdateOutput();
			this.ChangeVisualState();
		}
		private void OnLimitSignal1ValueChangedHandler(string signalName, object signalValue)
		{
			this.limitSignal1Value = this.GetSignalValue(signalValue);
			this.ChangeVisualState();
		}
		private void OnLimitSignal2ValueChangedHandler(string signalName, object signalValue)
		{
			this.limitSignal2Value = this.GetSignalValue(signalValue);
			this.ChangeVisualState();
		}
		private void OnLimitSignal3ValueChangedHandler(string signalName, object signalValue)
		{
			this.limitSignal3Value = this.GetSignalValue(signalValue);
			this.ChangeVisualState();
		}
		private void OnLimitSignal4ValueChangedHandler(string signalName, object signalValue)
		{
			this.limitSignal4Value = this.GetSignalValue(signalValue);
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
				this.UpdateOutputInDesignMode();
				return;
			}
			if (this.connector != null)
			{
				return;
			}
			this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
			if (this.tbOutputElement != null)
			{
				this.tbOutputElement.Text=(string.Empty);
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
			this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
			this.CreateEquationCompiler1();
			this.CreateEquationCompiler2();
			this.CreateEquationCompiler3();
			this.CreateEquationCompiler4();
			this.CreateAndRegisterConditionSignals();
			if (!string.IsNullOrEmpty(this.SignalName))
			{
				this.SignalName = this.SignalPrefix + this.SignalName;
				this.SignalName = SignalNameBuilder.BuildSignalName(this.SignalName, this);
				this.delegateSignalValueChanged = new OnSignalChangedDelegate(this.OnSignalValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.SignalName, this.delegateSignalValueChanged);
			}
			if (!string.IsNullOrEmpty(this.LimitSignalName1))
			{
				this.LimitSignalName1 = this.SignalPrefix + this.LimitSignalName1;
				this.LimitSignalName1 = SignalNameBuilder.BuildSignalName(this.LimitSignalName1, this);
				this.delegateLimitSignal1ValueChanged = new OnSignalChangedDelegate(this.OnLimitSignal1ValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.LimitSignalName1, this.delegateLimitSignal1ValueChanged);
			}
			if (!string.IsNullOrEmpty(this.LimitSignalName2))
			{
				this.LimitSignalName2 = this.SignalPrefix + this.LimitSignalName2;
				this.LimitSignalName2 = SignalNameBuilder.BuildSignalName(this.LimitSignalName2, this);
				this.delegateLimitSignal2ValueChanged = new OnSignalChangedDelegate(this.OnLimitSignal2ValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.LimitSignalName2, this.delegateLimitSignal2ValueChanged);
			}
			if (!string.IsNullOrEmpty(this.LimitSignalName3))
			{
				this.LimitSignalName3 = this.SignalPrefix + this.LimitSignalName3;
				this.LimitSignalName3 = SignalNameBuilder.BuildSignalName(this.LimitSignalName3, this);
				this.delegateLimitSignal3ValueChanged = new OnSignalChangedDelegate(this.OnLimitSignal3ValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.LimitSignalName3, this.delegateLimitSignal3ValueChanged);
			}
			if (!string.IsNullOrEmpty(this.LimitSignalName4))
			{
				this.LimitSignalName4 = this.SignalPrefix + this.LimitSignalName4;
				this.LimitSignalName4 = SignalNameBuilder.BuildSignalName(this.LimitSignalName4, this);
				this.delegateLimitSignal4ValueChanged = new OnSignalChangedDelegate(this.OnLimitSignal4ValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.LimitSignalName4, this.delegateLimitSignal4ValueChanged);
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
		protected virtual void Dispose(bool disposing)
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
				foreach (string current in this.dictConditionSignals.Keys)
				{
					this.UnregisterSignal(this.ServerName, current, this.delegateConditionSignalValueChanged);
				}
			}
			if (this.delegateSignalValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.SignalName, this.delegateSignalValueChanged);
			}
			if (this.delegateLimitSignal1ValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.LimitSignalName1, this.delegateLimitSignal1ValueChanged);
			}
			if (this.delegateLimitSignal2ValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.LimitSignalName2, this.delegateLimitSignal2ValueChanged);
			}
			if (this.delegateLimitSignal3ValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.LimitSignalName3, this.delegateLimitSignal3ValueChanged);
			}
			if (this.delegateLimitSignal4ValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.LimitSignalName4, this.delegateLimitSignal4ValueChanged);
			}
			if (this.delegateVisibilitySignalValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.VisibilitySignalName, this.delegateVisibilitySignalValueChanged);
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
			Grid? grid = base.GetTemplateChild("LayoutRoot") as Grid;
			if (grid != null && grid.Children.Contains(control))
			{
				grid.Children.Remove(control);
			}
		}
		protected virtual void OnUserContextChanged(string userName)
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
			foreach (ParameterItem current in this.popupParameters)
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
		private bool CanGoToStateLimit(double limitSignalValue, int? maskSignal)
		{
			bool result = false;
			if (!double.IsNaN(limitSignalValue) && limitSignalValue != 0.0)
			{
				if (maskSignal.HasValue && maskSignal.HasValue)
				{
					if (this.BinaryMask)
					{
						long num = Convert.ToInt64(limitSignalValue);
						if ((num & (long)maskSignal.Value) == (long)maskSignal.Value)
						{
							result = true;
						}
					}
					else
					{
						if (limitSignalValue >= (double)maskSignal.Value)
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
		private bool CanGoToStateLimit1()
		{
			if (this.equationCompiler1 != null)
			{
				return this.CanGoToStateConditionRule1();
			}
			return this.CanGoToStateLimit(this.limitSignal1Value, this.MaskSignal1);
		}
		private bool CanGoToStateLimit2()
		{
			if (this.equationCompiler2 != null)
			{
				return this.CanGoToStateConditionRule2();
			}
			return this.CanGoToStateLimit(this.limitSignal2Value, this.MaskSignal2);
		}
		private bool CanGoToStateLimit3()
		{
			if (this.equationCompiler3 != null)
			{
				return this.CanGoToStateConditionRule3();
			}
			return this.CanGoToStateLimit(this.limitSignal3Value, this.MaskSignal3);
		}
		private bool CanGoToStateLimit4()
		{
			if (this.equationCompiler4 != null)
			{
				return this.CanGoToStateConditionRule4();
			}
			return this.CanGoToStateLimit(this.limitSignal4Value, this.MaskSignal4);
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
		private void ChangeVisualState()
		{
			if (this.CanGoToStateLimit4())
			{
				VisualStateManager.GoToState(this, "Limit4", true);
				return;
			}
			if (this.CanGoToStateLimit3())
			{
				VisualStateManager.GoToState(this, "Limit3", true);
				return;
			}
			if (this.CanGoToStateLimit2())
			{
				VisualStateManager.GoToState(this, "Limit2", true);
				return;
			}
			if (this.CanGoToStateLimit1())
			{
				VisualStateManager.GoToState(this, "Limit1", true);
				return;
			}
			VisualStateManager.GoToState(this, "Normal", true);
		}
		private void CreateAndRegisterConditionSignals()
		{
			foreach (ConditionSignalItem current in this.conditionSignals)
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
				using (Dictionary<string, string>.KeyCollection.Enumerator conditionSignalEnumerator = this.dictConditionSignals.Keys.GetEnumerator())
				{
					while (conditionSignalEnumerator.MoveNext())
					{
						string conditionSignalName = conditionSignalEnumerator.Current;
						this.RegisterSignal(this.ServerName, conditionSignalName, this.delegateConditionSignalValueChanged);
					}
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
		protected double GetSignalValue(object value)
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
		protected abstract void OnUpdateOutput();
		private void UpdateOutputInDesignMode()
		{
			if (this.tbOutputElement != null)
			{
				string text = string.Empty;
				if (!string.IsNullOrEmpty(this.SignalName))
				{
					text = this.SignalName;
					text = this.SignalPrefix + text;
					text = SignalNameBuilder.BuildSignalName(text, this);
				}
				this.tbOutputElement.Text=(text);
			}
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
		private void RegisterMouseHandlers()
		{
			base.MouseEnter+=(new MouseEventHandler(this.HMIControl_MouseEnter));
			base.MouseLeave+=(new MouseEventHandler(this.HMIControl_MouseLeave));
		}
	}
}
