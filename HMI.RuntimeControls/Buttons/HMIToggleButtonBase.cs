// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
using HMI.Runtime.Shared.Common.Converters;
using HMI.Runtime.Shared.Dialogs;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using DialogResult = HMI.Runtime.Shared.Dialogs.DialogResult;

namespace HMI.RuntimeControls.Buttons
{
    /// <summary>
    /// HMIToggleButtonBase 是一个抽象类，继承自 WPF 的 ToggleButton，并实现了 IDisposable 接口。
    /// 它提供了与 HMI 系统交互的基础功能，允许用户通过 ToggleButton 控件来控制和监视 HMI 信号的状态。
    /// </summary>
    [
        TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates"), 
        TemplateVisualState(Name = "Normal", GroupName = "CommonStates"), 
        TemplateVisualState(Name = "Unchecked", GroupName = "CheckStates"), 
        TemplateVisualState(Name = "Indeterminate", GroupName = "CheckStates"), 
        TemplateVisualState(Name = "Focused", GroupName = "FocusStates"), 
        TemplateVisualState(Name = "Disabled", GroupName = "CommonStates"), 
        TemplateVisualState(Name = "Checked", GroupName = "CheckStates"), 
        TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates"), 
        TemplateVisualState(Name = "Pressed", GroupName = "CommonStates")]
    public abstract class HMIToggleButtonBase : ToggleButton, IDisposable
    {
        protected const string DenyAccessBehaviourException = "DenyAccessBehaviour";
        protected const string SNInsufficientAuthorizations = "HMISN_SL_InsufficientAuthorizations";
        protected const string SNInvalidPasswordDuringWriteSecureOperations = "HMISN_Ctrl_SL_InvalidPasswordDuringWriteSecureOperations";
        protected const string SNWriteError = "HMISN_SL_WriteError";

        private WriteSecureDialogManager writeSecureDialogManager;
        private MessageBoxManager messageBoxManager;
        private IHMIConnector connector;

        private bool enabled_SignalNameChanged;
        private bool isToggled;
        private bool hasUserChangedSignalValue;
        private bool enable_EnableSignalNameChanged;
        private bool visible_inner;
        private bool isAuthorized;

        private OnLanguageChangedDelegate onLanguageChangedDelegate;
        private OnResultDelegate onResultDelegate;
        private OnSignalChangedDelegate onSignalNameChangedDelegate;
        private OnSignalChangedDelegate onEnableSignalNameChangedDelegate;
        private OnSignalChangedDelegate onVisibilitySignalNameChangedDelegate;
        private Dictionary<string, SymbolicTextTranslation> dictTranslations;
        private SynchronizationContext synchronizationContext;

        private string serverNameToWrite;
        private string signalNameToWrite;
        private object signalValueToWrite;

        public static readonly DependencyProperty EnableMaskProperty = DependencyProperty.Register("EnableMask", typeof(int?), typeof(HMIToggleButtonBase), new PropertyMetadata(null));
        public static readonly DependencyProperty EnableSignalNameProperty = DependencyProperty.Register("EnableSignalName", typeof(string), typeof(HMIToggleButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty IsTipModeEnabledProperty = DependencyProperty.Register("IsTipModeEnabled", typeof(bool), typeof(HMIToggleButtonBase), new PropertyMetadata(false));
        public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMIToggleButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMIToggleButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMIToggleButtonBase), new PropertyMetadata(DenyAccessBehaviour.Hide));
        public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(HMIToggleButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMIToggleButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMIToggleButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty UseSynchronizedTogglingProperty = DependencyProperty.Register("UseSynchronizedToggling", typeof(bool), typeof(HMIToggleButtonBase), new PropertyMetadata(false));
        public static readonly DependencyProperty Value1Property = DependencyProperty.Register("Value1", typeof(double), typeof(HMIToggleButtonBase), new PropertyMetadata(0.0));
        public static readonly DependencyProperty Value2Property = DependencyProperty.Register("Value2", typeof(double), typeof(HMIToggleButtonBase), new PropertyMetadata(1.0));
        public static readonly DependencyProperty VisibilityMaskProperty = DependencyProperty.Register("VisibilityMask", typeof(int?), typeof(HMIToggleButtonBase), new PropertyMetadata(null));
        public static readonly DependencyProperty VisibilitySignalNameProperty = DependencyProperty.Register("VisibilitySignalName", typeof(string), typeof(HMIToggleButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty WriteSecureProperty = DependencyProperty.Register("WriteSecure", typeof(bool), typeof(HMIToggleButtonBase), new PropertyMetadata(false));

        public event UserChangedSignalValueEventHandler? UserChangedSignalValue;

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
                return (int?)base.GetValue(HMIToggleButtonBase.EnableMaskProperty);
            }
            set
            {
                base.SetValue(HMIToggleButtonBase.EnableMaskProperty, value);
            }
        }
        [Category("HMI")]
        public string EnableSignalName
        {
            get
            {
                return (string)base.GetValue(HMIToggleButtonBase.EnableSignalNameProperty);
            }
            set
            {
                base.SetValue(HMIToggleButtonBase.EnableSignalNameProperty, value);
            }
        }
        protected bool IsAuthorized
        {
            get
            {
                return this.isAuthorized;
            }
        }
        protected bool IsInDesignMode
        {
            get
            {
                return DesignerProperties.GetIsInDesignMode(this);
            }
        }
        [Category("HMI")]
        public bool IsTipModeEnabled
        {
            get
            {
                return (bool)base.GetValue(HMIToggleButtonBase.IsTipModeEnabledProperty);
            }
            set
            {
                base.SetValue(HMIToggleButtonBase.IsTipModeEnabledProperty, value);
            }
        }
        [Category("HMI")]
        public string ObjectID
        {
            get
            {
                return (string)base.GetValue(HMIToggleButtonBase.ObjectIDProperty);
            }
            set
            {
                base.SetValue(HMIToggleButtonBase.ObjectIDProperty, value);
            }
        }
        [Category("HMI")]
        public string SecurityAuthorizationName
        {
            get
            {
                return (string)base.GetValue(HMIToggleButtonBase.SecurityAuthorizationNameProperty);
            }
            set
            {
                base.SetValue(HMIToggleButtonBase.SecurityAuthorizationNameProperty, value);
            }
        }
        [Category("HMI")]
        public DenyAccessBehaviour SecurityDenyAccessBehaviour
        {
            get
            {
                return (DenyAccessBehaviour)base.GetValue(HMIToggleButtonBase.SecurityDenyAccessBehaviourProperty);
            }
            set
            {
                base.SetValue(HMIToggleButtonBase.SecurityDenyAccessBehaviourProperty, value);
            }
        }
        [Category("HMI")]
        protected string ServerName
        {
            get
            {
                return (string)base.GetValue(HMIToggleButtonBase.ServerNameProperty);
            }
            set
            {
                base.SetValue(HMIToggleButtonBase.ServerNameProperty, value);
            }
        }
        [Category("HMI")]
        public string SignalName
        {
            get
            {
                return (string)base.GetValue(HMIToggleButtonBase.SignalNameProperty);
            }
            set
            {
                base.SetValue(HMIToggleButtonBase.SignalNameProperty, value);
            }
        }
        [Category("HMI")]
        public string SignalPrefix
        {
            get
            {
                return (string)base.GetValue(HMIToggleButtonBase.SignalPrefixProperty);
            }
            set
            {
                base.SetValue(HMIToggleButtonBase.SignalPrefixProperty, value);
            }
        }
        [Category("HMI")]
        public bool UseSynchronizedToggling
        {
            get
            {
                return (bool)base.GetValue(HMIToggleButtonBase.UseSynchronizedTogglingProperty);
            }
            set
            {
                base.SetValue(HMIToggleButtonBase.UseSynchronizedTogglingProperty, value);
            }
        }
        /// <summary>
        ///  Value1 是 ToggleButton 选中时写入的值，
        ///  Value2 是 ToggleButton 没有选中时写入的值。
        ///  通过设置这两个属性，可以支持用户自定义在选中和未选中状态下写入不同的值到信号中。
        ///  默认为 Value1=1.0，Value2=0.0。
        /// </summary>
        [Category("HMI")]
        public double Value1
        {
            get
            {
                return (double)base.GetValue(HMIToggleButtonBase.Value1Property);
            }
            set
            {
                base.SetValue(HMIToggleButtonBase.Value1Property, value);
            }
        }
        /// <summary>
        ///  Value1 是 ToggleButton 选中时写入的值，
        ///  Value2 是 ToggleButton 没有选中时写入的值。
        ///  通过设置这两个属性，可以支持用户自定义在选中和未选中状态下写入不同的值到信号中。
        ///  默认为 Value1=1.0，Value2=0.0。
        /// </summary>
        [Category("HMI")]
        public double Value2
        {
            get
            {
                return (double)base.GetValue(HMIToggleButtonBase.Value2Property);
            }
            set
            {
                base.SetValue(HMIToggleButtonBase.Value2Property, value);
            }
        }
        [Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
        public int? VisibilityMask
        {
            get
            {
                return (int?)base.GetValue(HMIToggleButtonBase.VisibilityMaskProperty);
            }
            set
            {
                base.SetValue(HMIToggleButtonBase.VisibilityMaskProperty, value);
            }
        }
        [Category("HMI")]
        public string VisibilitySignalName
        {
            get
            {
                return (string)base.GetValue(HMIToggleButtonBase.VisibilitySignalNameProperty);
            }
            set
            {
                base.SetValue(HMIToggleButtonBase.VisibilitySignalNameProperty, value);
            }
        }
        [Category("HMI")]
        public bool WriteSecure
        {
            get
            {
                return (bool)base.GetValue(HMIToggleButtonBase.WriteSecureProperty);
            }
            set
            {
                base.SetValue(HMIToggleButtonBase.WriteSecureProperty, value);
            }
        }
        public HMIToggleButtonBase()
        {
            base.DefaultStyleKey=(typeof(HMIToggleButtonBase));
            this.dictTranslations = new Dictionary<string, SymbolicTextTranslation>(StringComparer.InvariantCultureIgnoreCase);
            this.messageBoxManager = new MessageBoxManager(null);
            this.writeSecureDialogManager = new WriteSecureDialogManager(new EventHandler<DialogClosedEventArgs>(this.onDialogClosedEvent));
        }
        ~HMIToggleButtonBase()
        {
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void onSignalNameChanged(string signalName, object signalValue)
        {
            double num = this.ConvertSignalValueToDouble(signalValue);
            if (!this.IsTipModeEnabled)
            {
                base.IsChecked=(new bool?(num.Equals(this.Value1)));
                this.isToggled = true;
            }

            if (!this.enabled_SignalNameChanged)// 为避免在注册事件时，事件被触发而导致EnableControl方法被调用两次，第一次是enabled_SignalNameChanged为false，第二次才是正确的值
            {
                this.enabled_SignalNameChanged = true;
                this.hasUserChangedSignalValue = false;
                this.EnableControl(this.enabled_SignalNameChanged);
            }

            if (this.hasUserChangedSignalValue)// 避免在用户改变值后，SignalName改变引起的值改变再次触发UserChangedSignalValue事件
            {
                this.synchronizationContext.Post(new SendOrPostCallback(obj =>
                {
                    this.OnUserChangedSignalValue(new UserChangedSignalValueEventArgs(this.SignalName, num));
                }), null);
            }
            this.hasUserChangedSignalValue = false;
        }
        private void onEnableSignalNameChanged(string signalName, object signalValue)
        {
            double num = this.ConvertSignalValueToDouble(signalValue);
            if (!double.IsNaN(num) && !object.Equals(num, 0.0))
            {
                if (!this.EnableMask.HasValue)
                {
                    this.enable_EnableSignalNameChanged = true;
                }
                else
                {
                    long signalBitmask = Convert.ToInt64(num);
                    this.enable_EnableSignalNameChanged = ((signalBitmask & (long)this.EnableMask.Value) != (long)this.EnableMask.Value);
                }
            }
            else
            {
                this.enable_EnableSignalNameChanged = false;
            }
            this.EnableControl(this.enable_EnableSignalNameChanged);
        }
        private void onVisibilitySignalNameChanged(string signalName, object signalValue)
        {
            double num = this.ConvertSignalValueToDouble(signalValue);
            if (!double.IsNaN(num) && !object.Equals(num, 0.0))
            {
                if (!this.VisibilityMask.HasValue)
                {
                    this.visible_inner = true;
                }
                else
                {
                    long signalBitmask = Convert.ToInt64(num);
                    this.visible_inner = ((signalBitmask & (long)this.VisibilityMask.Value) != (long)this.VisibilityMask.Value);
                }
            }
            else
            {
                this.visible_inner = false;
            }
            this.ShowControl(this.visible_inner);
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
            }
            catch (Exception ex)
            {
                this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
            }
        }
        private void HandleWriteResult(Exception error, int[] results)
        {
            if (this.IsAsyncError(error, results))
            {
                return;
            }
            if (this.IsTipModeEnabled)
            {
                base.IsChecked=(new bool?(true));
                base.OnToggle();
            }
            else
            {
                base.OnToggle();
            }
            var message = this.GetTranslatedErrorMessage(error, results);
            this.synchronizationContext.Post(new SendOrPostCallback(obj =>
            {
                this.messageBoxManager.Show(message, "HMI", MessageBoxType.Error, "OK", "Cancel");
            }), null);
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
        private string HandleErrorCode(int errorCode)
        {
            switch (errorCode)
            {
                case -4094:
                case -4093:
                    return this.dictTranslations["HMISN_SL_InsufficientAuthorizations"].Translation;
                case -4091:
                    return this.dictTranslations["HMISN_Ctrl_SL_InvalidPasswordDuringWriteSecureOperations"].Translation;
            }
            return string.Format(this.dictTranslations["HMISN_SL_WriteError"].Translation, errorCode);
        }
        private void HMIToggleButtonBase_MouseEnter(object sender, MouseEventArgs e)
        {
            base.Cursor=(Cursors.Hand);
        }
        private void HMIToggleButtonBase_MouseLeave(object sender, MouseEventArgs e)
        {
            base.Cursor=(Cursors.Arrow);
        }
        private void onDialogClosedEvent(object? sender, DialogClosedEventArgs e)
        {
            if (e.DialogResult == DialogResult.OK)
            {
                this.WriteSignalValue(this.serverNameToWrite, this.signalNameToWrite, this.signalValueToWrite, this.writeSecureDialogManager.Password);
                return;
            }
            base.OnToggle();
        }
        private void iwfconnector_0_UserContextChanged(string userName)
        {
            if (string.IsNullOrEmpty(this.SecurityAuthorizationName))
            {
                return;
            }
            this.isAuthorized = this.connector.CheckProjectAuthorization(this.SecurityAuthorizationName);
            switch (this.SecurityDenyAccessBehaviour)
            {
                case DenyAccessBehaviour.Disable:
                    this.EnableControl(this.isAuthorized);
                    return;
                case DenyAccessBehaviour.Hide:
                    this.ShowControl(this.isAuthorized);
                    return;
                default:
                    throw new ArgumentOutOfRangeException("DenyAccessBehaviour");
            }
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            base.IsChecked=(new bool?(false));
            base.IsThreeState=(false);
            this.isToggled = false;
            if (!this.IsInDesignMode && this.connector == null)
            {
                this.setControlStatus();
                this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
                base.IsEnabled=(false);
                this.Value1 = Math.Round(this.Value1, 5);
                this.Value2 = Math.Round(this.Value2, 5);
                this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.iwfconnector_0_UserContextChanged);
                this.setRigisters();
                return;
            }
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (this.IsTipModeEnabled)
            {
                bool? isChecked = base.IsChecked;
                if (isChecked.GetValueOrDefault() && isChecked.HasValue)
                {
                    this.OnToggle();
                }
                base.OnMouseLeave(e);
                return;
            }
            base.OnMouseLeave(e);
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (this.IsTipModeEnabled)
            {
                base.IsChecked=(new bool?(false));
                this.OnToggle();
                return;
            }
            base.OnMouseLeftButtonDown(e);
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (this.IsTipModeEnabled)
            {
                base.IsChecked=(new bool?(true));
                this.OnToggle();
                return;
            }
            base.OnMouseLeftButtonUp(e);
        }
        protected override void OnToggle()
        {
            base.OnToggle();
            if (!this.IsInDesignMode && !string.IsNullOrEmpty(this.SignalName) && this.isAuthorized && (!this.UseSynchronizedToggling || this.isToggled))
            {
                this.isToggled = false;
                try
                {
                    this.synchronizationContext = SynchronizationContext.Current;
                    if (base.IsChecked.HasValue && base.IsChecked.Value)
                    {
                        if (this.IsTipModeEnabled)
                        {
                            this.WriteSignalValue(this.ServerName, this.SignalName, this.Value1);
                        }
                        else
                        {
                            this.WriteSignal(this.ServerName, this.SignalName, this.Value1);
                        }
                    }
                    else
                    {
                        if (this.IsTipModeEnabled)
                        {
                            this.WriteSignalValue(this.ServerName, this.SignalName, this.Value2);
                        }
                        else
                        {
                            this.WriteSignal(this.ServerName, this.SignalName, this.Value2);
                        }
                    }
                    this.hasUserChangedSignalValue = true;
                }
                catch (Exception ex)
                {
                    this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
                }
                return;
            }
        }
        protected virtual void OnUserChangedSignalValue(UserChangedSignalValueEventArgs e)
        {
            this.UserChangedSignalValue?.Invoke(this, e);
        }
        private void setRigisters()
        {
            this.onResultDelegate = new OnResultDelegate(this.HandleWriteResult);
            this.registerTranslation();
            this.registerSignalName();
            this.registerEnableSignalName();
            this.registerVisibilitySignalName();
            this.registerMouseEvent();
        }
        private void registerTranslation()
        {
            SymbolicTextTranslation symbolicTextTranslation = new SymbolicTextTranslation("HMISN_SL_InsufficientAuthorizations");
            this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
            symbolicTextTranslation = new SymbolicTextTranslation("HMISN_Ctrl_SL_InvalidPasswordDuringWriteSecureOperations");
            this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
            symbolicTextTranslation = new SymbolicTextTranslation("HMISN_SL_WriteError");
            this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
            try
            {
                string[] symbolicNames = new string[this.dictTranslations.Count];
                this.dictTranslations.Keys.CopyTo(symbolicNames, 0);
                this.onLanguageChangedDelegate = new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler);
                this.connector.RegisterLanguageChangedHandler(symbolicNames, this.onLanguageChangedDelegate);
            }
            catch (Exception ex)
            {
                this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
            }
        }
        private void registerSignalName()
        {
            if (string.IsNullOrEmpty(this.SignalName))
            {
                return;
            }
            this.SignalName = this.SignalPrefix + this.SignalName;
            this.SignalName = SignalNameBuilder.BuildSignalName(this.SignalName, this);
            this.onSignalNameChangedDelegate = new OnSignalChangedDelegate(this.onSignalNameChanged);
            this.RegisterSignal(this.ServerName, this.SignalName, this.onSignalNameChangedDelegate);
        }
        private void registerEnableSignalName()
        {
            if (string.IsNullOrEmpty(this.EnableSignalName))
            {
                return;
            }
            this.EnableSignalName = this.SignalPrefix + this.EnableSignalName;
            this.EnableSignalName = SignalNameBuilder.BuildSignalName(this.EnableSignalName, this);
            this.onEnableSignalNameChangedDelegate = new OnSignalChangedDelegate(this.onEnableSignalNameChanged);
            this.RegisterSignal(this.ServerName, this.EnableSignalName, this.onEnableSignalNameChangedDelegate);
        }
        private void registerVisibilitySignalName()
        {
            if (string.IsNullOrEmpty(this.VisibilitySignalName))
            {
                return;
            }
            this.VisibilitySignalName = this.SignalPrefix + this.VisibilitySignalName;
            this.VisibilitySignalName = SignalNameBuilder.BuildSignalName(this.VisibilitySignalName, this);
            this.onVisibilitySignalNameChangedDelegate = new OnSignalChangedDelegate(this.onVisibilitySignalNameChanged);
            this.RegisterSignal(this.ServerName, this.VisibilitySignalName, this.onVisibilitySignalNameChangedDelegate);
        }
        private void registerMouseEvent()
        {
            base.MouseEnter+=(new MouseEventHandler(this.HMIToggleButtonBase_MouseEnter));
            base.MouseLeave+=(new MouseEventHandler(this.HMIToggleButtonBase_MouseLeave));
        }
        private void setControlStatus()
        {
            if (string.IsNullOrEmpty(this.SecurityAuthorizationName))
            {
                this.isAuthorized = true;
                return;
            }
            this.isAuthorized = false;
            switch (this.SecurityDenyAccessBehaviour)
            {
                case DenyAccessBehaviour.Disable:
                    base.IsEnabled=(false);
                    return;
                case DenyAccessBehaviour.Hide:
                    base.Visibility= System.Windows.Visibility.Collapsed;
                    return;
                default:
                    throw new ArgumentOutOfRangeException("DenyAccessBehaviour");
            }
        }
        private void EnableControl(bool enable)
        {
            base.IsEnabled=(enable && this.enabled_SignalNameChanged && (string.IsNullOrEmpty(this.EnableSignalName) || this.enable_EnableSignalNameChanged) && (this.isAuthorized || this.SecurityDenyAccessBehaviour != DenyAccessBehaviour.Disable));
        }
        private void ShowControl(bool visible)
        {
            Visibility visibility = System.Windows.Visibility.Collapsed;
            if (visible && (string.IsNullOrEmpty(this.VisibilitySignalName) || this.visible_inner))
            {
                if (this.isAuthorized || this.SecurityDenyAccessBehaviour != DenyAccessBehaviour.Hide)
                {
                    visibility =  System.Windows.Visibility.Visible;
                }
            }

            base.Visibility=(visibility);
        }
        private double ConvertSignalValueToDouble(object signalValue)
        {
            double result;
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
        protected void WriteSignal(string serverName, string signalName, object signalValue)
        {
            if (this.WriteSecure)
            {
                this.serverNameToWrite = serverName;
                this.signalNameToWrite = signalName;
                this.signalValueToWrite = signalValue;
                this.writeSecureDialogManager.Show();
                return;
            }
            this.WriteSignalValue(serverName, signalName, signalValue);
        }
        private void WriteSignalValue(string serverName, string signalName, object signalValue)
        {
            try
            {
                this.synchronizationContext = SynchronizationContext.Current;
                this.connector.WriteSignal(serverName, signalName, signalValue, this.onResultDelegate);
                this.hasUserChangedSignalValue = true;
            }
            catch (Exception ex)
            {
                this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
            }
        }
        private void WriteSignalValue(string serverName, string signalName, object signalValue, string password)
        {
            try
            {
                this.synchronizationContext = SynchronizationContext.Current;
                this.connector.WriteSignal(serverName, signalName, signalValue, password, this.onResultDelegate);
                this.hasUserChangedSignalValue = true;
            }
            catch (Exception ex)
            {
                this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            this.DisposeConnector();
            this.disposeMouseEventHandler();
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
            this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.iwfconnector_0_UserContextChanged);
            if (this.onSignalNameChangedDelegate != null)
            {
                this.UnregisterSignal(this.ServerName, this.SignalName, this.onSignalNameChangedDelegate);
            }
            if (this.onEnableSignalNameChangedDelegate != null)
            {
                this.UnregisterSignal(this.ServerName, this.EnableSignalName, this.onEnableSignalNameChangedDelegate);
            }
            if (this.onVisibilitySignalNameChangedDelegate != null)
            {
                this.UnregisterSignal(this.ServerName, this.VisibilitySignalName, this.onVisibilitySignalNameChangedDelegate);
            }
            if (this.onLanguageChangedDelegate != null)
            {
                this.connector.UnregisterLanguageChangedHandler(null, this.onLanguageChangedDelegate);
            }
            this.connector.Dispose();
        }
        private void disposeMouseEventHandler()
        {
            base.MouseEnter-=(new MouseEventHandler(this.HMIToggleButtonBase_MouseEnter));
            base.MouseLeave-=(new MouseEventHandler(this.HMIToggleButtonBase_MouseLeave));
        }
    }
}
