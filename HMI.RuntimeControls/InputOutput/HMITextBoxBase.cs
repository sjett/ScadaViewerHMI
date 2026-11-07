// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
using HMI.Runtime.Shared.Common.Converters;
using HMI.Runtime.Shared.Dialogs;
using HMI.Runtime.Shared.KeyboardControl;
using HMI.Runtime.Shared.VirtualKeyboard;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DialogResult = HMI.Runtime.Shared.Dialogs.DialogResult;

namespace HMI.RuntimeControls.InputOutput
{
    /// <summary>




    /// </summary>
    [
        TemplatePart(Name = "FocusVisualElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "DisabledVisualElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "ReadOnlyVisualElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "RootElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "ContentElement", Type = typeof(FrameworkElement)), 
		TemplateVisualState(Name = "ReadOnly", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "Normal", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates"), 
		TemplateVisualState(Name = "Focused", GroupName = "FocusStates"), 
		TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
	public abstract class HMITextBoxBase : TextBox, IDisposable, IVirtualKeyboardAware
	{
        private const string SNInsufficientAuthorizations = "HMISN_SL_InsufficientAuthorizations";
        private const string SNInvalidPasswordDuringWriteSecureOperations = "HMISN_Ctrl_SL_InvalidPasswordDuringWriteSecureOperations";
        private const string SNWriteError = "HMISN_SL_WriteError";

		private WriteSecureDialogManager writeSecureDialogManager;
		private MessageBoxManager messageBoxManager;
		private IHMIConnector connector;

		private bool isInEditMode;
		private object signalValue;
        private bool hasUserChangedSignalValue;
		private bool canEnableControlFromEnableSignal;
		private bool canShowControlFromVisibilitySignal;
		private bool isSecurityAvailable;
		private bool isAuthorized;

		private OnLanguageChangedDelegate delegateSymbolicTextTranslationsChanged;
		private OnResultDelegate delegateWriteSignalResult;
		private OnSignalChangedDelegate delegateSignalValueChanged;
		private OnSignalChangedDelegate delegateEnableSignalValueChanged;
		private OnSignalChangedDelegate delegateVisibilitySignalValueChanged;
		private Dictionary<string, SymbolicTextTranslation> dictTranslations;
		private SynchronizationContext syncContextUserChangedSignalValue;

		private string serverNameToWrite;
		private string signalNameToWrite;
		private object signalValueToWrite;
        private Brush realBackground;
        private bool isFocused;
        private CultureInfo currentCultureInfo = CultureInfo.CurrentUICulture;

        public static readonly DependencyProperty EnableMaskProperty = DependencyProperty.Register("EnableMask", typeof(int?), typeof(HMITextBoxBase), new PropertyMetadata(null, new PropertyChangedCallback(HMITextBoxBase.OnEnableMaskPropertyChanged)));
		public static readonly DependencyProperty EnableSignalNameProperty = DependencyProperty.Register("EnableSignalName", typeof(string), typeof(HMITextBoxBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMITextBoxBase.OnEnableSignalNamePropertyChanged)));
		public static readonly DependencyProperty EditModeHintBackgroundProperty = DependencyProperty.Register("EditModeHintBackground", typeof(Brush), typeof(HMITextBoxBase), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 249, 217, 130))));
		public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMITextBoxBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMITextBoxBase.OnObjectIDPropertyChanged)));
		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMITextBoxBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMITextBoxBase.OnSecurityAuthorizationNamePropertyChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMITextBoxBase), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(HMITextBoxBase.OnSecurityDenyAccessBehaviourPropertyChanged)));
		public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(HMITextBoxBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMITextBoxBase.OnServerNamePropertyChanged)));
		public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMITextBoxBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMITextBoxBase.OnSignalNamePropertyChanged)));
		public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMITextBoxBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMITextBoxBase.OnSignalPrefixPropertyChanged)));
		public static readonly DependencyProperty VisibilityMaskProperty = DependencyProperty.Register("VisibilityMask", typeof(int?), typeof(HMITextBoxBase), new PropertyMetadata(null, new PropertyChangedCallback(HMITextBoxBase.OnVisibilityMaskPropertyChanged)));
		public static readonly DependencyProperty VisibilitySignalNameProperty = DependencyProperty.Register("VisibilitySignalName", typeof(string), typeof(HMITextBoxBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMITextBoxBase.OnVisibilitySignalNamePropertyChanged)));
		public static readonly DependencyProperty WriteSecureProperty = DependencyProperty.Register("WriteSecure", typeof(bool), typeof(HMITextBoxBase), new PropertyMetadata(false, new PropertyChangedCallback(HMITextBoxBase.OnWriteSecurePropertyChanged)));
		
		public event UserChangedSignalValueEventHandler UserChangedSignalValue;

		protected IHMIConnector Connector
		{
			get
			{
				return this.connector;
			}
		}

		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? EnableMask
		{
			get
			{
				return (int?)base.GetValue(HMITextBoxBase.EnableMaskProperty);
			}
			set
			{
				base.SetValue(HMITextBoxBase.EnableMaskProperty, value);
			}
		}
		[Category("HMI")]
		public string EnableSignalName
		{
			get
			{
				return (string)base.GetValue(HMITextBoxBase.EnableSignalNameProperty);
			}
			set
			{
				base.SetValue(HMITextBoxBase.EnableSignalNameProperty, value);
			}
		}
		protected bool IsAuthorized
		{
			get
			{
				return this.isAuthorized;
			}
		}
		public Brush EditModeHintBackground
		{
			get
			{
				return (Brush)base.GetValue(HMITextBoxBase.EditModeHintBackgroundProperty);
			}
			set
			{
				base.SetValue(HMITextBoxBase.EditModeHintBackgroundProperty, value);
			}
		}
		internal bool IsInEditMode
		{
			get
			{
				return this.isInEditMode;
			}
			set
			{
				bool flag = this.isInEditMode != value;
				this.isInEditMode = value;
				if (!base.IsEnabled || this.IsReadOnly)
				{
					this.isInEditMode = false;
				}
				if (flag)
				{
					this.OnIsInEditModeChanged();
				}
			}
		}
		public bool IsReadOnly
		{
			get
			{
				return base.IsReadOnly;
			}
			set
			{
				base.IsReadOnly=(value);
				if (base.IsReadOnly)
				{
					this.IsInEditMode = false;
				}
			}
		}
		[Category("HMI")]
		public string ObjectID
		{
			get
			{
				return (string)base.GetValue(HMITextBoxBase.ObjectIDProperty);
			}
			set
			{
				base.SetValue(HMITextBoxBase.ObjectIDProperty, value);
			}
		}
		[Category("HMI")]
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMITextBoxBase.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMITextBoxBase.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMITextBoxBase.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMITextBoxBase.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		[Category("HMI")]
		protected string ServerName
		{
			get
			{
				return (string)base.GetValue(HMITextBoxBase.ServerNameProperty);
			}
			set
			{
				base.SetValue(HMITextBoxBase.ServerNameProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalName
		{
			get
			{
				return (string)base.GetValue(HMITextBoxBase.SignalNameProperty);
			}
			set
			{
				base.SetValue(HMITextBoxBase.SignalNameProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalPrefix
		{
			get
			{
				return (string)base.GetValue(HMITextBoxBase.SignalPrefixProperty);
			}
			set
			{
				base.SetValue(HMITextBoxBase.SignalPrefixProperty, value);
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
				return (int?)base.GetValue(HMITextBoxBase.VisibilityMaskProperty);
			}
			set
			{
				base.SetValue(HMITextBoxBase.VisibilityMaskProperty, value);
			}
		}
		[Category("HMI")]
		public string VisibilitySignalName
		{
			get
			{
				return (string)base.GetValue(HMITextBoxBase.VisibilitySignalNameProperty);
			}
			set
			{
				base.SetValue(HMITextBoxBase.VisibilitySignalNameProperty, value);
			}
		}
		[Category("HMI")]
		public bool WriteSecure
		{
			get
			{
				return (bool)base.GetValue(HMITextBoxBase.WriteSecureProperty);
			}
			set
			{
				base.SetValue(HMITextBoxBase.WriteSecureProperty, value);
			}
		}

        /// <summary>

        /// </summary>
        public abstract VirtualKeyboardSettings VirtualKeyboardSettings
		{
			get;
		}

		protected CultureInfo CurrentCultureInfo
		{
			get
			{
				return this.currentCultureInfo;
			}
		}
		protected virtual bool VKActiveOnEditModeOnly
		{
			get
			{
				return false;
			}
		}
		private static void OnEnableMaskPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnEnableSignalNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnObjectIDPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
		private static void OnSignalNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMITextBoxBase wFTextBoxBase = d as HMITextBoxBase;
			if (wFTextBoxBase != null && DesignerProperties.GetIsInDesignMode(wFTextBoxBase))
			{
				wFTextBoxBase.UpdateOutputInDesignMode();
			}
		}
		protected virtual void UpdateOutputInDesignMode()
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(this.SignalName))
			{
				text = this.SignalName;
				text = this.SignalPrefix + text;
				text = SignalNameBuilder.BuildSignalName(text, this);
			}
			else
			{
				text = base.GetType().Name;
			}
			base.Text=(text);
		}
		private static void OnSignalPrefixPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnVisibilityMaskPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnVisibilitySignalNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnWriteSecurePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		public HMITextBoxBase()
		{
			this.dictTranslations = new Dictionary<string, SymbolicTextTranslation>(StringComparer.InvariantCultureIgnoreCase);
			this.messageBoxManager = new MessageBoxManager(null);
			this.writeSecureDialogManager = new WriteSecureDialogManager(new EventHandler<DialogClosedEventArgs>(this.WriteSecureDlg_DialogClosed));
		}
		~HMITextBoxBase()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void OnSignalValueChangedHandler(string signalName, object signalValue)
		{
			this.signalValue = signalValue;
			if (!this.IsInEditMode)
			{
				this.OnUpdateOutput();
			}
			if (this.hasUserChangedSignalValue)
			{
				UserChangedSignalValueEventArgs args = new UserChangedSignalValueEventArgs(this.SignalName, signalValue);
				this.syncContextUserChangedSignalValue.Post( obj =>
				{
					this.OnUserChangedSignalValue(args);
				}, null);
			}
			this.hasUserChangedSignalValue = false;
		}
		private void OnEnableSignalValueChangedHandler(string signalName, object signalValue)
		{
			double enableSignalValue = this.ConvertSignalValueToDouble(signalValue);
			this.canEnableControlFromEnableSignal =
				!double.IsNaN(enableSignalValue) &&
				enableSignalValue != 0.0 &&
				(!this.EnableMask.HasValue ||
				 (Convert.ToInt64(enableSignalValue) & (long)this.EnableMask.Value) != (long)this.EnableMask.Value);
			this.EnableControl(this.canEnableControlFromEnableSignal);
		}
		private void OnVisibilitySignalValueChangedHandler(string signalName, object signalValue)
		{
			double visibilitySignalValue = this.ConvertSignalValueToDouble(signalValue);
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
				this.currentCultureInfo = HMI.Runtime.Core.Helper.CultureInfoCache.GetCultureInfo(this.connector.GetLanguageId());
				this.OnUpdateOutput();
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
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		private void OnWriteSignalResultHandler(Exception error, int[] results)
		{
			if (this.IsAsyncError(error, results))
			{
				return;
			}
			string message = this.GetTranslatedErrorMessage(error, results);
			this.syncContextUserChangedSignalValue.Post( obj =>
			{
				this.messageBoxManager.Show(message, "HMI", MessageBoxType.Error, "OK", "Cancel");
			}, null);
		}
		private bool IsAsyncError(Exception error, int[] results)
		{
			return error == null && (results == null || results.Length <= 0 || results[0] == 0);
		}
		private string GetTranslatedErrorMessage(Exception error, int[] results)
		{
			if (error != null)
			{
				return string.Format(this.dictTranslations["HMISN_SL_WriteError"].Translation, error.Message);
			}
			if (results[0] != 0)
			{
				return this.HandleErrorCode(results[0]);
			}
			return string.Empty;
		}
		private string HandleErrorCode(int result)
		{
			switch (result)
			{
			case -4094:
			case -4093:
				return this.dictTranslations["HMISN_SL_InsufficientAuthorizations"].Translation;
			case -4091:
				return this.dictTranslations["HMISN_Ctrl_SL_InvalidPasswordDuringWriteSecureOperations"].Translation;
			}
			return string.Format(this.dictTranslations["HMISN_SL_WriteError"].Translation, result);
		}
		private void WriteSecureDlg_DialogClosed(object sender, DialogClosedEventArgs e)
		{
			if (e.DialogResult == DialogResult.OK)
			{
				this.WriteSignalInternal(this.serverNameToWrite, this.signalNameToWrite, this.signalValueToWrite, this.writeSecureDialogManager.Password);
				return;
			}
			this.IsInEditMode = false;
			this.OnUpdateOutput();
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
			this.realBackground = base.Background;
			base.OnApplyTemplate();
			base.Text=(string.Empty);
			bool isInDesignMode;
			if (!(isInDesignMode = DesignerProperties.GetIsInDesignMode(this)))
			{
				if (this.connector != null)
				{
					return;
				}
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
						break;
					case DenyAccessBehaviour.Hide:
						base.Visibility= System.Windows.Visibility.Collapsed;
						break;
					default:
						throw new ArgumentOutOfRangeException("DenyAccessBehaviour");
					}
				}
				SymbolicTextTranslation symbolicTextTranslation = new SymbolicTextTranslation("HMISN_SL_InsufficientAuthorizations");
				this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
				symbolicTextTranslation = new SymbolicTextTranslation("HMISN_Ctrl_SL_InvalidPasswordDuringWriteSecureOperations");
				this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
				symbolicTextTranslation = new SymbolicTextTranslation("HMISN_SL_WriteError");
				this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
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
				this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
				this.delegateWriteSignalResult = new OnResultDelegate(this.OnWriteSignalResultHandler);
				if (!string.IsNullOrEmpty(this.SignalName))
				{
					this.SignalName = this.SignalPrefix + this.SignalName;
					this.SignalName = SignalNameBuilder.BuildSignalName(this.SignalName, this);
					this.delegateSignalValueChanged = new OnSignalChangedDelegate(this.OnSignalValueChangedHandler);
					this.RegisterSignal(this.ServerName, this.SignalName, this.delegateSignalValueChanged);
				}
				if (!string.IsNullOrEmpty(this.EnableSignalName))
				{
					this.EnableSignalName = this.SignalPrefix + this.EnableSignalName;
					this.EnableSignalName = SignalNameBuilder.BuildSignalName(this.EnableSignalName, this);
					this.delegateEnableSignalValueChanged = new OnSignalChangedDelegate(this.OnEnableSignalValueChangedHandler);
					this.RegisterSignal(this.ServerName, this.EnableSignalName, this.delegateEnableSignalValueChanged);
				}
				if (!string.IsNullOrEmpty(this.VisibilitySignalName))
				{
					this.VisibilitySignalName = this.SignalPrefix + this.VisibilitySignalName;
					this.VisibilitySignalName = SignalNameBuilder.BuildSignalName(this.VisibilitySignalName, this);
					this.delegateVisibilitySignalValueChanged = new OnSignalChangedDelegate(this.OnVisibilitySignalValueChangedHandler);
					this.RegisterSignal(this.ServerName, this.VisibilitySignalName, this.delegateVisibilitySignalValueChanged);
				}
			}
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			this.DisposeConnector();
			this.writeSecureDialogManager.Dispose();
			this.messageBoxManager.Dispose();
			this.StopStoryboards();
		}
		private void DisposeConnector()
		{
			if (this.connector == null)
			{
				return;
			}
			this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
			if (this.delegateSignalValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.SignalName, this.delegateSignalValueChanged);
			}
			if (this.delegateEnableSignalValueChanged != null)
			{
				this.UnregisterSignal(this.ServerName, this.EnableSignalName, this.delegateEnableSignalValueChanged);
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
		private void RemoveFromUI(Control control)
		{
			string text = "LayoutRoot";
			Grid grid = base.GetTemplateChild(text) as Grid;
			if (grid != null && grid.Children.Contains(control))
			{
				grid.Children.Remove(control);
			}
		}
		protected virtual void OnUserChangedSignalValue(UserChangedSignalValueEventArgs e)
		{
			if (this.UserChangedSignalValue != null)
			{
				this.UserChangedSignalValue(this, e);
			}
		}
		protected virtual void OnUserContextChanged(string userName)
		{
		}
		private void EnableControl(bool enable)
		{
			if (enable)
			{
				if (this.isSecurityAvailable && !this.isAuthorized && this.SecurityDenyAccessBehaviour == DenyAccessBehaviour.Disable)
				{
					base.IsEnabled=(false);
				}
				else
				{
					if (string.IsNullOrEmpty(this.EnableSignalName))
					{
						base.IsEnabled=(true);
					}
					else
					{
						if (this.canEnableControlFromEnableSignal)
						{
							base.IsEnabled=(true);
						}
						else
						{
							base.IsEnabled=(false);
						}
					}
				}
			}
			else
			{
				base.IsEnabled=(false);
			}
			if (!base.IsEnabled)
			{
				this.IsInEditMode = false;
			}
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
		protected double ConvertSignalValueToDouble(object value)
		{
			double result = double.NaN;
			try
			{
				string text = Convert.ToString(value, this.CurrentCultureInfo);
				if (!string.IsNullOrEmpty(text))
				{
					result = double.Parse(text, this.CurrentCultureInfo);
				}
				else
				{
					result = double.NaN;
				}
			}
			catch
			{
				result = double.NaN;
			}
			return result;
		}
        /// <summary>

        /// </summary>
        public abstract void OnUpdateOutput();

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
		protected void WriteSignal(string serverName, string signalName, object signalValueToWrite)
		{
			signalValueToWrite = this.UpdateSignalValueToCorrectCulture(signalValueToWrite);
			if (this.WriteSecure)
			{
				this.serverNameToWrite = serverName;
				this.signalNameToWrite = signalName;
				this.signalValueToWrite = signalValueToWrite;
				this.writeSecureDialogManager.Show();
				return;
			}
			this.WriteSignalInternal(serverName, signalName, signalValueToWrite);
		}
		private void WriteSignalInternal(string serverName, string signalName, object signalValueToWrite)
		{
			try
			{
				this.syncContextUserChangedSignalValue = SynchronizationContext.Current;
				this.connector.WriteSignal(serverName, signalName, signalValueToWrite, this.delegateWriteSignalResult);
				this.hasUserChangedSignalValue = true;
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		private void WriteSignalInternal(string serverName, string signalName, object signalValueToWrite, string password)
		{
			try
			{
				this.syncContextUserChangedSignalValue = SynchronizationContext.Current;
				this.connector.WriteSignal(serverName, signalName, signalValueToWrite, password, this.delegateWriteSignalResult);
				this.hasUserChangedSignalValue = true;
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		protected void HandlePressedEnterKey()
		{
			if (this.IsAuthorized && !string.IsNullOrEmpty(this.SignalName))
			{
				try
				{
					this.WriteSignal(this.ServerName, this.SignalName, base.Text);
					this.OnUpdateOutput();
					this.IsInEditMode = false;
				}
				catch (Exception ex)
				{
					this.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		protected void PutSelectionAtEnd()
		{
			base.SelectionStart=(base.Text.Length);
		}
		protected virtual bool ShouldHandlePressedKey(KeyEventArgs e)
		{
			return true;
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (this.IsInEditMode)
			{
                if (e.Key == Key.Enter)
				{
					this.HandlePressedEnterKey();
					e.Handled=(true);
					return;
				}
				if (e.Key == Key.Escape)
				{
					this.OnUpdateOutput();
					this.IsInEditMode = false;
					e.Handled=(true);
				}
			}
		}
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			this.isFocused = true;
			base.OnGotFocus(e);
			KeyboardController.Instance.OpenKeyboardFor(this);
		}
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			this.isFocused = false;
			base.OnLostFocus(e);
			KeyboardController.Instance.DettachKeyboard();
		}
		public void SendKey(string key)
		{
			if (!this.VKActiveOnEditModeOnly || this.IsInEditMode)
			{
				if (!this.IsInEditMode)
				{
					this.IsInEditMode = true;
				}
				PressedKeyInformation pressedKeyInformation = this.CreatePressedKeyInformationFrom(key);
				if (this.ShouldHandlePressedKey(pressedKeyInformation))
				{
					this.HandlePressedKey(key, pressedKeyInformation);
				}
				this.PutSelectionAtEnd();
			}
		}
		protected virtual bool ShouldHandlePressedKey(PressedKeyInformation pressedKeyInformation)
		{
			return true;
		}
		private PressedKeyInformation CreatePressedKeyInformationFrom(string key)
		{
			return new PressedKeyInformation
			{
				Key = KeyHandler.GetKeyFromString(key),
				KeyboardModifiers = ModifierKeys.None//,
			};
		}
		protected virtual void HandlePressedKey(string key, PressedKeyInformation pressedKeyInformation)
		{
            if (pressedKeyInformation.Key != Key.LeftAlt && pressedKeyInformation.Key != Key.RightAlt)
			{
                if (pressedKeyInformation.Key != Key.LeftShift && pressedKeyInformation.Key != Key.RightShift)
				{
					if (pressedKeyInformation.Key == Key.Enter)
					{
						this.HandlePressedEnterKey();
						return;
					}
					if (pressedKeyInformation.Key != Key.Delete)
					{
						if (pressedKeyInformation.Key != Key.Back)
						{
							if (base.SelectionLength == base.Text.Length)
							{
								base.Text=(key);
								return;
							}
							base.Text=(base.Text + key);
							return;
						}
					}
					this.HandlePressedDeleteKey();
					return;
				}
			}
		}
		private void HandlePressedDeleteKey()
		{
			if (base.Text.Length > 0)
			{
				this.DeleteLastChar();
			}
		}
		private void DeleteLastChar()
		{
			base.Text=(base.Text.Remove(base.Text.Length - 1, 1));
		}
		protected virtual void OnCurrentCultureInfoChanged()
		{
		}
		protected virtual void OnIsInEditModeChanged()
		{
			if (this.IsInEditMode)
			{
				this.realBackground = base.Background;
				base.Background=(this.EditModeHintBackground);
				return;
			}
			base.Background=(this.realBackground);
		}
		private object UpdateSignalValueToCorrectCulture(object signalValueToWrite)
		{
			double num = 0.0;
            if (!double.TryParse(signalValueToWrite.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out num))
			{
				return signalValueToWrite;
			}
			return num;
		}
		protected bool IsNavigationKey(Key key)
		{
			return key == Key.Down || key == Key.End || key == Key.Home || key == Key.Left || key == Key.Right || key == Key.Tab || key == Key.Up;
		}
	}
}
