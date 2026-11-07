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
using System.Windows.Threading;
using DialogResult = HMI.Runtime.Shared.Dialogs.DialogResult;

namespace HMI.RuntimeControls.Buttons
{
    [
        TemplateVisualState(Name = "Focused", GroupName = "FocusStates"),
        TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates"), 
        TemplateVisualState(Name = "Disabled", GroupName = "CommonStates"), 
        TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates"), 
        TemplateVisualState(Name = "Normal", GroupName = "CommonStates"), 
        TemplateVisualState(Name = "Pressed", GroupName = "CommonStates")]
    public class HMIExtendedButton : Button, IDisposable
    {
        private MessageBoxManager messageBoxManager;
        private WriteSecureDialogManager writeSecureDialogManager;
        private IHMIConnector connector;
        private Dictionary<string, SymbolicTextTranslation> dictTranslations;

        private bool isAuthorized;
        private bool locked;
        private TimeSpan waitIntervalTS;

        private OnLanguageChangedDelegate onLanguageChangedDelegate;
        private OnResultDelegate delegateWriteSignalResult;
        private SynchronizationContext synchronizationContext;

        private string serverNameToWrite;
        private string signalNameToWrite;
        private string signalName2ToWrite;
        private object signalValueToWrite;

        public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMIExtendedButton), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMIExtendedButton), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMIExtendedButton), new PropertyMetadata(DenyAccessBehaviour.Hide));
        public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(HMIExtendedButton), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty EnableMaskProperty = DependencyProperty.Register("EnableMask", typeof(int?), typeof(HMIExtendedButton), new PropertyMetadata(null));
        public static readonly DependencyProperty EnableSignalNameProperty = DependencyProperty.Register("EnableSignalName", typeof(string), typeof(HMIExtendedButton), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty VisibilityMaskProperty = DependencyProperty.Register("VisibilityMask", typeof(int?), typeof(HMIExtendedButton), new PropertyMetadata(null));
        public static readonly DependencyProperty VisibilitySignalNameProperty = DependencyProperty.Register("VisibilitySignalName", typeof(string), typeof(HMIExtendedButton), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SignalName1Property = DependencyProperty.Register("SignalName1", typeof(string), typeof(HMIExtendedButton), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SignalName2Property = DependencyProperty.Register("SignalName2", typeof(string), typeof(HMIExtendedButton), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMIExtendedButton), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SymbolicTextProperty = DependencyProperty.Register("SymbolicText", typeof(string), typeof(HMIExtendedButton), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIExtendedButton.OnSymbolicTextChanged)));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(HMIExtendedButton), new PropertyMetadata(0.0));
        public static readonly DependencyProperty WaitIntervalProperty = DependencyProperty.Register("WaitInterval", typeof(int), typeof(HMIExtendedButton), new PropertyMetadata(500));
        public static readonly DependencyProperty WriteSecureProperty = DependencyProperty.Register("WriteSecure", typeof(bool), typeof(HMIExtendedButton), new PropertyMetadata(false));

        public event UserChangedSignalValueEventHandler? UserChangedSignalValue;

        [Category("HMI")]
        public string ObjectID
        {
            get
            {
                return (string)base.GetValue(HMIExtendedButton.ObjectIDProperty);
            }
            set
            {
                base.SetValue(HMIExtendedButton.ObjectIDProperty, value);
            }
        }
        [Category("HMI")]
        public string SecurityAuthorizationName
        {
            get
            {
                return (string)base.GetValue(HMIExtendedButton.SecurityAuthorizationNameProperty);
            }
            set
            {
                base.SetValue(HMIExtendedButton.SecurityAuthorizationNameProperty, value);
            }
        }
        [Category("HMI")]
        public DenyAccessBehaviour SecurityDenyAccessBehaviour
        {
            get
            {
                return (DenyAccessBehaviour)base.GetValue(HMIExtendedButton.SecurityDenyAccessBehaviourProperty);
            }
            set
            {
                base.SetValue(HMIExtendedButton.SecurityDenyAccessBehaviourProperty, value);
            }
        }
        [Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
        public int? EnableMask
        {
            get
            {
                return (int?)base.GetValue(HMIExtendedButton.EnableMaskProperty);
            }
            set
            {
                base.SetValue(HMIExtendedButton.EnableMaskProperty, value);
            }
        }
        [Category("HMI")]
        public string EnableSignalName
        {
            get
            {
                return (string)base.GetValue(HMIExtendedButton.EnableSignalNameProperty);
            }
            set
            {
                base.SetValue(HMIExtendedButton.EnableSignalNameProperty, value);
            }
        }
        [Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
        public int? VisibilityMask
        {
            get
            {
                return (int?)base.GetValue(HMIExtendedButton.VisibilityMaskProperty);
            }
            set
            {
                base.SetValue(HMIExtendedButton.VisibilityMaskProperty, value);
            }
        }
        [Category("HMI")]
        public string VisibilitySignalName
        {
            get
            {
                return (string)base.GetValue(HMIExtendedButton.VisibilitySignalNameProperty);
            }
            set
            {
                base.SetValue(HMIExtendedButton.VisibilitySignalNameProperty, value);
            }
        }
        [Category("HMI")]
        public string SignalName1
        {
            get
            {
                return (string)base.GetValue(HMIExtendedButton.SignalName1Property);
            }
            set
            {
                base.SetValue(HMIExtendedButton.SignalName1Property, value);
            }
        }
        [Category("HMI")]
        public string SignalName2
        {
            get
            {
                return (string)base.GetValue(HMIExtendedButton.SignalName2Property);
            }
            set
            {
                base.SetValue(HMIExtendedButton.SignalName2Property, value);
            }
        }
        [Category("HMI")]
        public string SignalPrefix
        {
            get
            {
                return (string)base.GetValue(HMIExtendedButton.SignalPrefixProperty);
            }
            set
            {
                base.SetValue(HMIExtendedButton.SignalPrefixProperty, value);
            }
        }
        [Category("HMI")]
        public string SymbolicText
        {
            get
            {
                return (string)base.GetValue(HMIExtendedButton.SymbolicTextProperty);
            }
            set
            {
                base.SetValue(HMIExtendedButton.SymbolicTextProperty, value);
            }
        }
        [Category("HMI")]
        public double Value
        {
            get
            {
                return (double)base.GetValue(HMIExtendedButton.ValueProperty);
            }
            set
            {
                base.SetValue(HMIExtendedButton.ValueProperty, value);
            }
        }
        [Category("HMI")]
        public int WaitInterval
        {
            get
            {
                return (int)base.GetValue(HMIExtendedButton.WaitIntervalProperty);
            }
            set
            {
                base.SetValue(HMIExtendedButton.WaitIntervalProperty, value);
            }
        }
        [Category("HMI")]
        public bool WriteSecure
        {
            get
            {
                return (bool)base.GetValue(HMIExtendedButton.WriteSecureProperty);
            }
            set
            {
                base.SetValue(HMIExtendedButton.WriteSecureProperty, value);
            }
        }

        private string GetServerName()
        {
            return (string)base.GetValue(HMIExtendedButton.ServerNameProperty);
        }
        private static void OnSymbolicTextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            HMIExtendedButton? button = dependencyObject as HMIExtendedButton;
            if (button != null && DesignerProperties.GetIsInDesignMode(button))
            {
                button.Content = eventArgs.NewValue;
                return;
            }
        }
        public HMIExtendedButton()
        {
            base.DefaultStyleKey=(typeof(HMIExtendedButton));

            this.dictTranslations = new Dictionary<string, SymbolicTextTranslation>(StringComparer.InvariantCultureIgnoreCase);
            this.messageBoxManager = new MessageBoxManager(null);
            this.writeSecureDialogManager = new WriteSecureDialogManager(new EventHandler<DialogClosedEventArgs>(this.onDialogClosedEvent));
        }
        ~HMIExtendedButton()
        {
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
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
                if (this.dictTranslations.ContainsKey(this.SymbolicText))
                {
                    SymbolicTextTranslation symbolicTextTranslation = this.dictTranslations[this.SymbolicText];

                    base.Content=(symbolicTextTranslation.Translation);
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
            this.synchronizationContext.Post(new SendOrPostCallback(obj => {
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
        private void OnEnableSignalNameChanged(string signalName, object signalValue)
        {
            double val = this.ConvertSignalValueToDouble(signalValue);
            bool enable;
            if (!double.IsNaN(val) && !object.Equals(val, 0.0))
            {
                if (!this.EnableMask.HasValue)
                {
                    enable = true;
                }
                else
                {
                    long signalBitmask = Convert.ToInt64(val);
                    enable = ((signalBitmask & (long)this.EnableMask.Value) != (long)this.EnableMask.Value);
                }
            }
            else
            {
                enable = false;
            }
            this.EnableControl(enable);
        }
        private void onVisibilitySignalNameChanged(string signalName, object signalValue)
        {
            double val = this.ConvertSignalValueToDouble(signalValue);
            bool visible;
            if (!double.IsNaN(val) && !object.Equals(val, 0.0))
            {
                if (!this.VisibilityMask.HasValue)
                {
                    visible = true;
                }
                else
                {
                    long signalBitmask = Convert.ToInt64(val);
                    visible = ((signalBitmask & (long)this.VisibilityMask.Value) != (long)this.VisibilityMask.Value);
                }
            }
            else
            {
                visible = false;
            }
            this.ShowControl(visible);
        }
        private void HMIExtendedButton_MouseEnter(object sender, MouseEventArgs e)
        {
            base.Cursor=(Cursors.Hand);
        }
        private void HMIExtendedButton_MouseLeave(object sender, MouseEventArgs e)
        {
            base.Cursor=(Cursors.Arrow);
        }
        private void onDialogClosedEvent(object sender, DialogClosedEventArgs e)
        {
            if (e.DialogResult == DialogResult.OK)
            {
                this.WriteSignalInternal(this.serverNameToWrite, this.signalNameToWrite, this.signalName2ToWrite, this.signalValueToWrite, this.writeSecureDialogManager.Password);
                return;
            }
            this.locked = false;
            this.EnableControl(true);
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
            if (!string.IsNullOrEmpty(this.SymbolicText))
            {
                base.Content=(this.SymbolicText);
            }
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            this.Value = Math.Round(this.Value, 5);
            if (this.connector != null)
            {
                return;
            }

            this.setControlStatus();
            this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
            if (!string.IsNullOrEmpty(this.SymbolicText))
            {
                base.Content=(string.Empty);
            }
            this.waitIntervalTS = TimeSpan.FromMilliseconds((double)this.WaitInterval);
            this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.connector_UserContextChanged);

            this.ResolvePlaceholders();
            this.setRigisters();
        }
        protected override void OnClick()
        {
            base.OnClick();
            if (!DesignerProperties.GetIsInDesignMode(this) && !string.IsNullOrEmpty(this.SignalName1) && !string.IsNullOrEmpty(this.SignalName2) && this.isAuthorized)
            {
                this.locked = true;
                this.EnableControl(false);
                this.WriteSignal(this.GetServerName(), this.SignalName1, this.SignalName2, this.Value);
                return;
            }
        }
        private void ResolvePlaceholders()
        {
            this.ResolveSignalName1Placeholders();
            this.ResolveSignalName2Placeholders();
            this.ResolveEnableSignalNamePlaceholders();
            this.ResolveVisibilitySignalNamePlaceholders();
        }
        private void ResolveSignalName1Placeholders()
        {
            if (string.IsNullOrEmpty(this.SignalName1))
            {
                return;
            }
            this.SignalName1 = StringPlaceholdersResolver.Resolve(this.SignalPrefix + this.SignalName1, this);
        }
        private void ResolveSignalName2Placeholders()
        {
            if (string.IsNullOrEmpty(this.SignalName2))
            {
                return;
            }
            this.SignalName2 = StringPlaceholdersResolver.Resolve(this.SignalPrefix + this.SignalName2, this);
        }
        private void ResolveEnableSignalNamePlaceholders()
        {
            if (string.IsNullOrEmpty(this.EnableSignalName))
            {
                return;
            }
            this.EnableSignalName = StringPlaceholdersResolver.Resolve(this.SignalPrefix + this.EnableSignalName, this);
        }
        private void ResolveVisibilitySignalNamePlaceholders()
        {
            if (string.IsNullOrEmpty(this.VisibilitySignalName))
            {
                return;
            }
            this.VisibilitySignalName = StringPlaceholdersResolver.Resolve(this.SignalPrefix + this.VisibilitySignalName, this);
        }
        private void setRigisters()
        {
            this.delegateWriteSignalResult = new OnResultDelegate(this.OnWriteSignalResultHandler);
            this.registerTranslation();
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

            if (!string.IsNullOrEmpty(this.SymbolicText))
            {
                symbolicTextTranslation = new SymbolicTextTranslation(this.SymbolicText);
                this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
            }
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
        private void registerEnableSignalName()
        {
            if (string.IsNullOrEmpty(this.EnableSignalName))
            {
                return;
            }
            this.connector.RegisterSignalChangedHandler(string.Empty, this.EnableSignalName, new OnSignalChangedDelegate(this.OnEnableSignalNameChanged));
        }
        private void registerVisibilitySignalName()
        {
            if (string.IsNullOrEmpty(this.VisibilitySignalName))
            {
                return;
            }
            this.connector.RegisterSignalChangedHandler(string.Empty, this.VisibilitySignalName, new OnSignalChangedDelegate(this.onVisibilitySignalNameChanged));
        }
        private void registerMouseEvent()
        {
            base.MouseEnter+=(new MouseEventHandler(this.HMIExtendedButton_MouseEnter));
            base.MouseLeave+=(new MouseEventHandler(this.HMIExtendedButton_MouseLeave));
        }
        private void Dispose(bool dispose)
        {
            if (!dispose)
            {
                return;
            }
            this.DisposeConnector();
            this.DisposeMouseHandler();
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
            this.connector.UnregisterSignalChangedHandler(string.Empty, this.EnableSignalName, new OnSignalChangedDelegate(this.OnEnableSignalNameChanged));
            this.connector.UnregisterSignalChangedHandler(string.Empty, this.VisibilitySignalName, new OnSignalChangedDelegate(this.onVisibilitySignalNameChanged));
            this.connector.UnregisterLanguageChangedHandler(null, this.onLanguageChangedDelegate);
            this.connector.Dispose();
        }
        private void DisposeMouseHandler()
        {
            base.MouseEnter-=(new MouseEventHandler(this.HMIExtendedButton_MouseEnter));
            base.MouseLeave-=(new MouseEventHandler(this.HMIExtendedButton_MouseLeave));
        }
        private void OnUserChangedSignalValue(UserChangedSignalValueEventArgs e)
        {
            this.UserChangedSignalValue?.Invoke(this, e);
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
            base.IsEnabled=(enable && !this.locked && (this.isAuthorized || this.SecurityDenyAccessBehaviour != DenyAccessBehaviour.Disable));
        }
        private void ShowControl(bool show)
        {
            Visibility visibility = System.Windows.Visibility.Collapsed;
            if (show)
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
        private void WriteSignal(string serverName, string signalName, string signalName2, object signalValue)
        {
            if (this.WriteSecure)
            {
                this.serverNameToWrite = serverName;
                this.signalNameToWrite = signalName;
                this.signalName2ToWrite = signalName2;
                this.signalValueToWrite = signalValue;
                this.writeSecureDialogManager.Show();
                return;
            }
            this.WriteSignalInternal(serverName, signalName, signalName2, signalValue);
        }
        private void WriteSignalInternal(string serverName, string signalName, string signalName2, object signalValue)
        {
            SendOrPostCallback sendOrPostCallback = null;
            this.synchronizationContext = SynchronizationContext.Current;
            try
            {
                this.connector.WriteSignal(serverName, signalName, 0.0, this.delegateWriteSignalResult);
                if (sendOrPostCallback == null)
                {
                    sendOrPostCallback = new SendOrPostCallback(obj =>
                    {
                        this.OnUserChangedSignalValue(new UserChangedSignalValueEventArgs(signalName, 0.0));
                    });
                }
                this.synchronizationContext.Post(sendOrPostCallback, null);
            }
            catch (Exception ex)
            {
                this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
            }
            
            var delayBeforeSecondWriteTimer = new DispatcherTimer();
            delayBeforeSecondWriteTimer.Interval=(this.waitIntervalTS);
            delayBeforeSecondWriteTimer.Tick += (o, e) =>
            {
                delayBeforeSecondWriteTimer.Stop();
                this.synchronizationContext = SynchronizationContext.Current;
                try
                {
                    this.connector.WriteSignal(serverName, signalName2, signalValue, this.delegateWriteSignalResult);
                    if (sendOrPostCallback == null)
                    {
                        sendOrPostCallback = new SendOrPostCallback(obj =>
                        {
                            this.OnUserChangedSignalValue(new UserChangedSignalValueEventArgs(signalName2, signalValue));
                        });
                    }
                    this.synchronizationContext.Post(sendOrPostCallback, null);
                }
                catch (Exception ex)
                {
                    this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", this.GetType().Name, ex.Message), new object[0]);
                }

                var delayBeforeFinalWriteTimer = new DispatcherTimer();
                delayBeforeFinalWriteTimer.Interval = (this.waitIntervalTS);
                delayBeforeFinalWriteTimer.Tick += (o, e) =>
                {
                    SendOrPostCallback sendOrPostCallback = null;
                    delayBeforeFinalWriteTimer.Stop();
                    this.synchronizationContext = SynchronizationContext.Current;
                    try
                    {
                        this.connector.WriteSignal(serverName, signalName, 1.0, this.delegateWriteSignalResult);
                        if (sendOrPostCallback == null)
                        {
                            sendOrPostCallback = new SendOrPostCallback(state =>
                            {
                                this.OnUserChangedSignalValue(new UserChangedSignalValueEventArgs(signalName, 1.0));
                            });
                        }
                        this.synchronizationContext.Post(sendOrPostCallback, null);
                    }
                    catch (Exception ex)
                    {
                        this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", this.GetType().Name, ex.Message), new object[0]);
                    }
                    this.locked = false;
                    this.EnableControl(true);
                };
                delayBeforeFinalWriteTimer.Start();
            };
            delayBeforeSecondWriteTimer.Start();
        }
        private void WriteSignalInternal(string serverName, string signalName, string signalName2, object signalValue, string password)
        {
            SendOrPostCallback sendOrPostCallback = null;

            this.synchronizationContext = SynchronizationContext.Current;
            try
            {
                this.connector.WriteSignal(serverName, signalName, 0.0, password, this.delegateWriteSignalResult);
                if (sendOrPostCallback == null)
                {
                    sendOrPostCallback = new SendOrPostCallback(obj =>
                    {
                        this.OnUserChangedSignalValue(new UserChangedSignalValueEventArgs(signalName, 0.0));
                    });
                }
                this.synchronizationContext.Post(sendOrPostCallback, null);
            }
            catch (Exception ex)
            {
                this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
            }

            var delayBeforeSecondWriteTimer = new DispatcherTimer();
            delayBeforeSecondWriteTimer.Interval=(this.waitIntervalTS);
            delayBeforeSecondWriteTimer.Tick+=(o, e)=>
            {
                SendOrPostCallback sendOrPostCallback = null;
                delayBeforeSecondWriteTimer.Stop();
                this.synchronizationContext = SynchronizationContext.Current;
                try
                {
                    this.connector.WriteSignal(serverName, signalName2, signalValue, password, this.delegateWriteSignalResult);
                    if (sendOrPostCallback == null)
                    {
                        sendOrPostCallback = new SendOrPostCallback(obj =>
                        {
                            this.OnUserChangedSignalValue(new UserChangedSignalValueEventArgs(signalName2, signalValue));
                        });
                    }
                    this.synchronizationContext.Post(sendOrPostCallback, null);
                }
                catch (Exception ex)
                {
                    this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", this.GetType().Name, ex.Message), new object[0]);
                }

                var delayBeforeFinalWriteTimer = new DispatcherTimer();
                delayBeforeFinalWriteTimer.Interval = (this.waitIntervalTS);
                delayBeforeFinalWriteTimer.Tick += (o, e) =>
                {
                    SendOrPostCallback sendOrPostCallback = null;
                    delayBeforeFinalWriteTimer.Stop();
                    this.synchronizationContext = SynchronizationContext.Current;
                    try
                    {
                        this.connector.WriteSignal(serverName, signalName, 1.0, password, this.delegateWriteSignalResult);
                        if (sendOrPostCallback == null)
                        {
                            sendOrPostCallback = new SendOrPostCallback(obj =>
                            {
                                this.OnUserChangedSignalValue(new UserChangedSignalValueEventArgs(signalName, 1.0));
                            });
                        }
                        this.synchronizationContext.Post(sendOrPostCallback, null);
                    }
                    catch (Exception ex)
                    {
                        this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", this.GetType().Name, ex.Message), new object[0]);
                    }
                    this.locked = false;
                    this.EnableControl(true);
                };
                delayBeforeFinalWriteTimer.Start();
            };
            delayBeforeSecondWriteTimer.Start();
        }
    }
}
