// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common.Converters;
using HMI.Runtime.Shared.Dialogs;
using HMI.Runtime.Shared.NavigationService;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HMI.RuntimeControls.Buttons
{
    public class HMILinkButton1 : Button, IDisposable
    {
        private bool isAuthorized;
        private IHMIConnector connector;
        private INavigationService navigationService;
        private OnLanguageChangedDelegate onLanguageChangedDelegate;

        public static readonly DependencyProperty PopupHeightProperty = DependencyProperty.Register("PopupHeight", typeof(int), typeof(HMILinkButton1), new PropertyMetadata(768));
        public static readonly DependencyProperty PopupModalityProperty = DependencyProperty.Register("PopupModality", typeof(DialogModality), typeof(HMILinkButton1), new PropertyMetadata(DialogModality.Modeless));
        public static readonly DependencyProperty PopupWidthProperty = DependencyProperty.Register("PopupWidth", typeof(int), typeof(HMILinkButton1), new PropertyMetadata(1024));
        public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMILinkButton1), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMILinkButton1), new PropertyMetadata(DenyAccessBehaviour.Hide));
        public static readonly DependencyProperty EnableMaskProperty = DependencyProperty.Register("EnableMask", typeof(int?), typeof(HMILinkButton1), new PropertyMetadata(null));
        public static readonly DependencyProperty EnableSignalNameProperty = DependencyProperty.Register("EnableSignalName", typeof(string), typeof(HMILinkButton1), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty VisibilityMaskProperty = DependencyProperty.Register("VisibilityMask", typeof(int?), typeof(HMILinkButton1), new PropertyMetadata(null));
        public static readonly DependencyProperty VisibilitySignalNameProperty = DependencyProperty.Register("VisibilitySignalName", typeof(string), typeof(HMILinkButton1), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMIExtendedButton), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMIExtendedButton), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SymbolicTextProperty = DependencyProperty.Register("SymbolicText", typeof(string), typeof(HMILinkButton1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMILinkButton1.onSymbolicTextChanged)));
        public static readonly DependencyProperty UrlProperty = DependencyProperty.Register("Url", typeof(string), typeof(HMILinkButton1), new PropertyMetadata(string.Empty));
        

        [Category("HMI")]
        public int PopupHeight
        {
            get
            {
                return (int)base.GetValue(HMILinkButton1.PopupHeightProperty);
            }
            set
            {
                base.SetValue(HMILinkButton1.PopupHeightProperty, value);
            }
        }
        [Category("HMI")]
        public DialogModality PopupModality
        {
            get
            {
                return (DialogModality)base.GetValue(HMILinkButton1.PopupModalityProperty);
            }
            set
            {
                base.SetValue(HMILinkButton1.PopupModalityProperty, value);
            }
        }
        [Category("HMI")]
        public int PopupWidth
        {
            get
            {
                return (int)base.GetValue(HMILinkButton1.PopupWidthProperty);
            }
            set
            {
                base.SetValue(HMILinkButton1.PopupWidthProperty, value);
            }
        }
        [Category("HMI")]
        public string SecurityAuthorizationName
        {
            get
            {
                return (string)base.GetValue(HMILinkButton1.SecurityAuthorizationNameProperty);
            }
            set
            {
                base.SetValue(HMILinkButton1.SecurityAuthorizationNameProperty, value);
            }
        }
        [Category("HMI")]
        public DenyAccessBehaviour SecurityDenyAccessBehaviour
        {
            get
            {
                return (DenyAccessBehaviour)base.GetValue(HMILinkButton1.SecurityDenyAccessBehaviourProperty);
            }
            set
            {
                base.SetValue(HMILinkButton1.SecurityDenyAccessBehaviourProperty, value);
            }
        }
        [Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
        public int? EnableMask
        {
            get
            {
                return (int?)base.GetValue(HMILinkButton1.EnableMaskProperty);
            }
            set
            {
                base.SetValue(HMILinkButton1.EnableMaskProperty, value);
            }
        }
        [Category("HMI")]
        public string EnableSignalName
        {
            get
            {
                return (string)base.GetValue(HMILinkButton1.EnableSignalNameProperty);
            }
            set
            {
                base.SetValue(HMILinkButton1.EnableSignalNameProperty, value);
            }
        }
        [Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
        public int? VisibilityMask
        {
            get
            {
                return (int?)base.GetValue(HMILinkButton1.VisibilityMaskProperty);
            }
            set
            {
                base.SetValue(HMILinkButton1.VisibilityMaskProperty, value);
            }
        }
        [Category("HMI")]
        public string VisibilitySignalName
        {
            get
            {
                return (string)base.GetValue(HMILinkButton1.VisibilitySignalNameProperty);
            }
            set
            {
                base.SetValue(HMILinkButton1.VisibilitySignalNameProperty, value);
            }
        }
        [Category("HMI")]
        public string ObjectID
        {
            get
            {
                return (string)base.GetValue(HMILinkButton1.ObjectIDProperty);
            }
            set
            {
                base.SetValue(HMILinkButton1.ObjectIDProperty, value);
            }
        }
        [Category("HMI")]
        public string SignalPrefix
        {
            get
            {
                return (string)base.GetValue(HMILinkButton1.SignalPrefixProperty);
            }
            set
            {
                base.SetValue(HMILinkButton1.SignalPrefixProperty, value);
            }
        }
        [Category("HMI")]
        public string SymbolicText
        {
            get
            {
                return (string)base.GetValue(HMILinkButton1.SymbolicTextProperty);
            }
            set
            {
                base.SetValue(HMILinkButton1.SymbolicTextProperty, value);
            }
        }
        [Category("HMI")]
        public string Url
        {
            get
            {
                return (string)base.GetValue(HMILinkButton1.UrlProperty);
            }
            set
            {
                base.SetValue(HMILinkButton1.UrlProperty, value);
            }
        }
        private bool IsInDesign()
        {
            return DesignerProperties.GetIsInDesignMode(this);
        }
        private static void onSymbolicTextChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            HMILinkButton1? wFLinkButton = dp as HMILinkButton1;
            if (wFLinkButton != null && wFLinkButton.IsInDesign())
            {
                wFLinkButton.Content=(e.NewValue);
                return;
            }
        }
        public HMILinkButton1()
        {
            base.DefaultStyleKey=(typeof(HMILinkButton1));
            this.SetNavigationService(new NavigationService());
        }
        ~HMILinkButton1()
        {
            this.Dispose(false);
        }
        private void OnSymbolicTextTranslationsChangedHandler(string[] symbolicTexts, string[] translationTexts)
        {
            try
            {
                if (translationTexts[0] != null && !translationTexts[0].Equals("!null!", StringComparison.InvariantCultureIgnoreCase))
                {
                    base.Content=(translationTexts[0]);
                }
                else
                {
                    base.Content=(symbolicTexts[0]);
                }
            }
            catch (Exception ex)
            {
                this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
            }
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
        private void HMILinkButton1_MouseEnter(object sender, MouseEventArgs e)
        {
            base.Cursor=(Cursors.Hand);
        }
        private void HMILinkButton1_MouseLeave(object sender, MouseEventArgs e)
        {
            base.Cursor=(Cursors.Arrow);
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
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.IsInDesign())
            {
                if (!string.IsNullOrEmpty(this.SymbolicText))
                {
                    base.Content=(this.SymbolicText);
                }
                return;
            }
            if (this.connector != null)
            {
                return;
            }
            this.setControlStatus();
            this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
            if (this.SymbolicText != null)
            {
                this.SymbolicText = StringPlaceholdersResolver.Resolve(this.SymbolicText, this);
                if (!string.IsNullOrEmpty(this.SymbolicText))
                {
                    try
                    {
                        this.onLanguageChangedDelegate = new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler);
                        this.connector.RegisterLanguageChangedHandler(new string[]
						{
							this.SymbolicText
						}, this.onLanguageChangedDelegate);
                    }
                    catch (Exception ex)
                    {
                        this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
                    }
                }
            }
            this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.connector_UserContextChanged);

            this.ResolvePlaceholders();
            this.setRigisters();
        }
        private void setRigisters()
        {
            this.registerEnableSignalName();
            this.registerVisibilitySignalName();
            this.registerMouseEvent();
        }
        protected override void OnClick()
        {
            base.OnClick();
            if (string.IsNullOrEmpty(this.Url))
            {
                return;
            }
            this.Url = StringPlaceholdersResolver.Resolve(this.Url, this);
            try
            {
                this.GetNavigationService().Navigate(this.Url, this.PopupModality, this.PopupWidth, this.PopupHeight);
            }
            catch (Exception ex)
            {
                this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
            }
        }
        private void ResolvePlaceholders()
        {
            this.ResolveEnableSignalNamePlaceholders();
            this.ResolveVisibilitySignalNamePlaceholders();
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
            base.MouseEnter+=(new MouseEventHandler(this.HMILinkButton1_MouseEnter));
            base.MouseLeave+=(new MouseEventHandler(this.HMILinkButton1_MouseLeave));
        }
        private void Dispose(bool dispose)
        {
            if (!dispose)
            {
                return;
            }
            this.DisposeConnector();
            this.DisposeMouseHandler();
        }
        private void DisposeConnector()
        {
            if (this.connector == null)
            {
                return;
            }
            if (this.onLanguageChangedDelegate != null)
            {
                this.connector.UnregisterLanguageChangedHandler(null, this.onLanguageChangedDelegate);
                this.onLanguageChangedDelegate = null;
            }
            this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
        }
        private void DisposeMouseHandler()
        {
            base.MouseEnter-=(new MouseEventHandler(this.HMILinkButton1_MouseEnter));
            base.MouseLeave-=(new MouseEventHandler(this.HMILinkButton1_MouseLeave));
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
            base.IsEnabled=(enable && (this.isAuthorized || this.SecurityDenyAccessBehaviour != DenyAccessBehaviour.Disable));
            VisualStateManager.GoToState(this, base.IsEnabled ? "Normal" : "Disabled", true);
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
        
        private INavigationService GetNavigationService()
        {
            return this.navigationService;
        }
        
        private void SetNavigationService(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }
    }
}
