// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
using HMI.Runtime.Shared.Common.Converters;
using HMI.Runtime.Shared.Dialogs;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DialogResult = HMI.Runtime.Shared.Dialogs.DialogResult;

namespace HMI.RuntimeControls.Buttons
{
    [
        TemplateVisualState(Name = "Focused", GroupName = "FocusStates"), 
        TemplateVisualState(Name = "Normal", GroupName = "CommonStates"), 
        TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates"), 
        TemplateVisualState(Name = "Pressed", GroupName = "CommonStates"), 
        TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates"), 
        TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
    public abstract class HMIButtonBase : Button, IDisposable
    {
        private const string DenyAccessBehaviourException = "DenyAccessBehaviour";
        private const string SNInsufficientAuthorizations = "HMISN_SL_InsufficientAuthorizations";
        private const string SNInvalidPasswordDuringWriteSecureOperations = "HMISN_Ctrl_SL_InvalidPasswordDuringWriteSecureOperations";
        private const string SNWriteError = "HMISN_SL_WriteError";

        private WriteSecureDialogManager writeSecureDialogManager;
        private MessageBoxManager messageBoxManager;
        private IHMIConnector connector;

        private bool hasUserChangedSignalValue;
        private bool canEnableControlFromEnableSignal;
        private bool canShowControlFromVisibilitySignal;
        private bool isAuthorized;

        private OnLanguageChangedDelegate onLanguageChangedDelegate;
        private OnResultDelegate delegateWriteSignalResult;
        private OnSignalChangedDelegate onSignalNameChangedDelegate;
        private OnSignalChangedDelegate onEnableSignalNameChangedDelegate;
        private OnSignalChangedDelegate onVisibilitySignalNameChangedDelegate;

        private Dictionary<string, SymbolicTextTranslation> dictTranslations;
        private SynchronizationContext syncContextUserChangedSignalValue;

        private string serverNameToWrite;
        private string signalNameToWrite;
        private object signalValueToWrite;

        public static readonly DependencyProperty EnableMaskProperty = DependencyProperty.Register("EnableMask", typeof(int?), typeof(HMIButtonBase), new PropertyMetadata(null));
        public static readonly DependencyProperty EnableSignalNameProperty = DependencyProperty.Register("EnableSignalName", typeof(string), typeof(HMIButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMIButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMIButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMIButtonBase), new PropertyMetadata(DenyAccessBehaviour.Hide));
        public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(HMIButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMIButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMIButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(HMIButtonBase), new PropertyMetadata(1.0));
        public static readonly DependencyProperty VisibilityMaskProperty = DependencyProperty.Register("VisibilityMask", typeof(int?), typeof(HMIButtonBase), new PropertyMetadata(null));
        public static readonly DependencyProperty VisibilitySignalNameProperty = DependencyProperty.Register("VisibilitySignalName", typeof(string), typeof(HMIButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty WriteSecureProperty = DependencyProperty.Register("WriteSecure", typeof(bool), typeof(HMIButtonBase), new PropertyMetadata(false));

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
                return (int?)base.GetValue(HMIButtonBase.EnableMaskProperty);
            }
            set
            {
                base.SetValue(HMIButtonBase.EnableMaskProperty, value);
            }
        }
        [Category("HMI")]
        public string EnableSignalName
        {
            get
            {
                return (string)base.GetValue(HMIButtonBase.EnableSignalNameProperty);
            }
            set
            {
                base.SetValue(HMIButtonBase.EnableSignalNameProperty, value);
            }
        }
        [Category("HMI")]
        public string ObjectID
        {
            get
            {
                return (string)base.GetValue(HMIButtonBase.ObjectIDProperty);
            }
            set
            {
                base.SetValue(HMIButtonBase.ObjectIDProperty, value);
            }
        }
        [Category("HMI")]
        public string SecurityAuthorizationName
        {
            get
            {
                return (string)base.GetValue(HMIButtonBase.SecurityAuthorizationNameProperty);
            }
            set
            {
                base.SetValue(HMIButtonBase.SecurityAuthorizationNameProperty, value);
            }
        }
        [Category("HMI")]
        public DenyAccessBehaviour SecurityDenyAccessBehaviour
        {
            get
            {
                return (DenyAccessBehaviour)base.GetValue(HMIButtonBase.SecurityDenyAccessBehaviourProperty);
            }
            set
            {
                base.SetValue(HMIButtonBase.SecurityDenyAccessBehaviourProperty, value);
            }
        }
        [Category("HMI")]
        protected string ServerName
        {
            get
            {
                return (string)base.GetValue(HMIButtonBase.ServerNameProperty);
            }
            set
            {
                base.SetValue(HMIButtonBase.ServerNameProperty, value);
            }
        }
        [Category("HMI")]
        public string SignalName
        {
            get
            {
                return (string)base.GetValue(HMIButtonBase.SignalNameProperty);
            }
            set
            {
                base.SetValue(HMIButtonBase.SignalNameProperty, value);
            }
        }
        [Category("HMI")]
        public string SignalPrefix
        {
            get
            {
                return (string)base.GetValue(HMIButtonBase.SignalPrefixProperty);
            }
            set
            {
                base.SetValue(HMIButtonBase.SignalPrefixProperty, value);
            }
        }
        [Category("HMI")]
        public double Value
        {
            get
            {
                return (double)base.GetValue(HMIButtonBase.ValueProperty);
            }
            set
            {
                base.SetValue(HMIButtonBase.ValueProperty, value);
            }
        }
        [Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
        public int? VisibilityMask
        {
            get
            {
                return (int?)base.GetValue(HMIButtonBase.VisibilityMaskProperty);
            }
            set
            {
                base.SetValue(HMIButtonBase.VisibilityMaskProperty, value);
            }
        }
        [Category("HMI")]
        public string VisibilitySignalName
        {
            get
            {
                return (string)base.GetValue(HMIButtonBase.VisibilitySignalNameProperty);
            }
            set
            {
                base.SetValue(HMIButtonBase.VisibilitySignalNameProperty, value);
            }
        }
        [Category("HMI")]
        public bool WriteSecure
        {
            get
            {
                return (bool)base.GetValue(HMIButtonBase.WriteSecureProperty);
            }
            set
            {
                base.SetValue(HMIButtonBase.WriteSecureProperty, value);
            }
        }
        public HMIButtonBase()
        {
            base.DefaultStyleKey=(typeof(HMIButtonBase));

            this.dictTranslations = new Dictionary<string, SymbolicTextTranslation>(StringComparer.InvariantCultureIgnoreCase);
            this.messageBoxManager = new MessageBoxManager(null);
            this.writeSecureDialogManager = new WriteSecureDialogManager(new EventHandler<DialogClosedEventArgs>(this.onDialogClosedEvent));
        }
        ~HMIButtonBase()
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
            if (this.hasUserChangedSignalValue)
            {
                var args = new UserChangedSignalValueEventArgs(this.SignalName, signalValue);
                this.syncContextUserChangedSignalValue.Post(new SendOrPostCallback(obj =>
                {
                    this.OnUserChangedSignalValue(args);
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
                    this.canEnableControlFromEnableSignal = true;
                }
                else
                {
                    long signalBitmask = Convert.ToInt64(num);
                    this.canEnableControlFromEnableSignal = ((signalBitmask & (long)this.EnableMask.Value) != (long)this.EnableMask.Value);
                }
            }
            else
            {
                this.canEnableControlFromEnableSignal = false;
            }
            this.EnableControl(this.canEnableControlFromEnableSignal);
        }
        private void onVisibilitySignalNameChanged(string signalName, object signalValue)
        {
            double num = this.ConvertSignalValueToDouble(signalValue);
            if (!double.IsNaN(num) && !object.Equals(num, 0.0))
            {
                if (!this.VisibilityMask.HasValue)
                {
                    this.canShowControlFromVisibilitySignal = true;
                }
                else
                {
                    long signalBitmask = Convert.ToInt64(num);
                    this.canShowControlFromVisibilitySignal = ((signalBitmask & (long)this.VisibilityMask.Value) != (long)this.VisibilityMask.Value);
                }
            }
            else
            {
                this.canShowControlFromVisibilitySignal = false;
            }
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

            var message = this.GetTranslatedErrorMessage(error, results);

            this.syncContextUserChangedSignalValue.Post(new SendOrPostCallback(obj =>
            {
                this.messageBoxManager.Show(this.serverNameToWrite, "HMI", MessageBoxType.Error, "OK", "Cancel");
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
        private void HMIButtonBase_MouseEnter(object sender, MouseEventArgs e)
        {
            base.Cursor=(Cursors.Hand);
        }
        private void HMIButtonBase_MouseLeave(object sender, MouseEventArgs e)
        {
            base.Cursor=(Cursors.Arrow);
        }
        private void onDialogClosedEvent(object sender, DialogClosedEventArgs e)
        {
            if (e.DialogResult == DialogResult.OK)
            {
                this.WriteSignalInternal(this.serverNameToWrite, this.signalNameToWrite, this.signalValueToWrite, this.writeSecureDialogManager.Password);
            }
        }
        private void connector_UserContextChanged(string userName)
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
            if (!DesignerProperties.GetIsInDesignMode(this) && this.connector == null)
            {
                this.Value = Math.Round(this.Value, 5);
                this.setControlStatus();

                this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
                this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.connector_UserContextChanged);

                this.setRigisters();
                return;
            }
        }
        protected override void OnClick()
        {
            base.OnClick();

            if (!DesignerProperties.GetIsInDesignMode(this) && !string.IsNullOrEmpty(this.SignalName) && this.isAuthorized)
            {
                this.WriteSignal(this.ServerName, this.SignalName, this.Value);
                return;
            }
        }
        protected virtual void OnUserChangedSignalValue(UserChangedSignalValueEventArgs e)
        {
            this.UserChangedSignalValue?.Invoke(this, e);
        }
        private void setRigisters()
        {
            this.delegateWriteSignalResult = new OnResultDelegate(this.OnWriteSignalResultHandler);
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
            base.MouseEnter += (new MouseEventHandler(this.HMIButtonBase_MouseEnter));
            base.MouseLeave += (new MouseEventHandler(this.HMIButtonBase_MouseLeave));
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
            base.IsEnabled=(enable && (string.IsNullOrEmpty(this.EnableSignalName) || this.canEnableControlFromEnableSignal) && (this.isAuthorized || this.SecurityDenyAccessBehaviour != DenyAccessBehaviour.Disable));
        }
        private void ShowControl(bool show)
        {
            Visibility visibility = System.Windows.Visibility.Collapsed;
            if (show && (string.IsNullOrEmpty(this.VisibilitySignalName) || this.canShowControlFromVisibilitySignal))
            {
                if (this.isAuthorized || this.SecurityDenyAccessBehaviour != DenyAccessBehaviour.Hide)
                {
                    visibility =  System.Windows.Visibility.Visible;
                }
            }

            base.Visibility=(visibility);
        }
        private double ConvertSignalValueToDouble(object value)
        {
            double result;
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
            this.WriteSignalInternal(serverName, signalName, signalValue);
        }
        private void WriteSignalInternal(string serverName, string signalName, object signalValue)
        {
            try
            {
                this.syncContextUserChangedSignalValue = SynchronizationContext.Current;
                this.connector.WriteSignal(serverName, signalName, signalValue, this.delegateWriteSignalResult);
                this.hasUserChangedSignalValue = true;
            }
            catch (Exception ex)
            {
                this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
            }
        }
        private void WriteSignalInternal(string serverName, string signalName, object signalValue, string password)
        {
            try
            {
                this.syncContextUserChangedSignalValue = SynchronizationContext.Current;
                this.connector.WriteSignal(serverName, signalName, signalValue, password, this.delegateWriteSignalResult);
                this.hasUserChangedSignalValue = true;
            }
            catch (Exception ex)
            {
                this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
            }
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
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            this.DisposeConnector();
            this.unregisterMouseEvent();
            this.writeSecureDialogManager.Dispose();
            this.messageBoxManager.Dispose();
            if (this.dictTranslations != null)
            {
                this.dictTranslations.Clear();
            }
            this.StopStoryboards();
        }
        private void DisposeConnector()
        {
            if (this.connector == null)
            {
                return;
            }
            this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
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
        private void unregisterMouseEvent()
        {
            base.MouseEnter-=(new MouseEventHandler(this.HMIButtonBase_MouseEnter));
            base.MouseLeave-=(new MouseEventHandler(this.HMIButtonBase_MouseLeave));
        }
    }
}
