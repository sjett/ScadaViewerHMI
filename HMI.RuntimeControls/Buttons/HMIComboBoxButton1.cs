// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common.Converters;
using HMI.Runtime.Shared.Dialogs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HMI.RuntimeControls.Buttons
{
    public class HMIComboBoxButton1 : HMIStaticStatesControlBase, IDisposable
    {
        private bool isAuthorized;
        private bool isSecurityAvailable;
        private bool allowSelectSameItem = true;
        private double newValue;
        private double oldValue;
        private ComboBox uxComboBox;
        private TextBlock outputElement;
        private readonly MessageBoxManager messageBoxManager;
        private readonly WriteSecureDialogManager writeSecureDialogManager;

        public static readonly DependencyProperty EnableMaskProperty = DependencyProperty.Register("EnableMask", typeof(int?), typeof(HMIComboBoxButton1), new PropertyMetadata(null));
        public static readonly DependencyProperty EnableSignalNameProperty = DependencyProperty.Register("EnableSignalName", typeof(string), typeof(HMIComboBoxButton1), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty VisibilityMaskProperty = DependencyProperty.Register("VisibilityMask", typeof(int?), typeof(HMIComboBoxButton1), new PropertyMetadata(null));
        public static readonly DependencyProperty VisibilitySignalNameProperty = DependencyProperty.Register("VisibilitySignalName", typeof(string), typeof(HMIComboBoxButton1), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty StateSignalNameProperty = DependencyProperty.Register("StateSignalName", typeof(string), typeof(HMIComboBoxButton1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIComboBoxButton1.OnStateSignalNamePropertyChanged)));
        public static readonly DependencyProperty WriteSignalNameProperty = DependencyProperty.Register("WriteSignalName", typeof(string), typeof(HMIComboBoxButton1), null);
        public static readonly DependencyProperty WriteSecureProperty = DependencyProperty.Register("WriteSecure", typeof(bool), typeof(HMIComboBoxButton1), new PropertyMetadata(false));
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(HMIComboBoxButton1), null);
        public event HMIComboBoxSelectionChangedHandler? SelectionChanged;


        [Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
        public int? EnableMask
        {
            get
            {
                return (int?)base.GetValue(HMIComboBoxButton1.EnableMaskProperty);
            }
            set
            {
                base.SetValue(HMIComboBoxButton1.EnableMaskProperty, value);
            }
        }
        [Category("HMI")]
        public string EnableSignalName
        {
            get
            {
                return (string)base.GetValue(HMIComboBoxButton1.EnableSignalNameProperty);
            }
            set
            {
                base.SetValue(HMIComboBoxButton1.EnableSignalNameProperty, value);
            }
        }
        [Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
        public int? VisibilityMask
        {
            get
            {
                return (int?)base.GetValue(HMIComboBoxButton1.VisibilityMaskProperty);
            }
            set
            {
                base.SetValue(HMIComboBoxButton1.VisibilityMaskProperty, value);
            }
        }
        [Category("HMI")]
        public string VisibilitySignalName
        {
            get
            {
                return (string)base.GetValue(HMIComboBoxButton1.VisibilitySignalNameProperty);
            }
            set
            {
                base.SetValue(HMIComboBoxButton1.VisibilitySignalNameProperty, value);
            }
        }
        [Category("HMI")]
        public string StateSignalName
        {
            get
            {
                return (string)base.GetValue(HMIComboBoxButton1.StateSignalNameProperty);
            }
            set
            {
                base.SetValue(HMIComboBoxButton1.StateSignalNameProperty, value);
            }
        }
        [Category("HMI")]
        public string WriteSignalName
        {
            get
            {
                return (string)base.GetValue(HMIComboBoxButton1.WriteSignalNameProperty);
            }
            set
            {
                base.SetValue(HMIComboBoxButton1.WriteSignalNameProperty, value);
            }
        }
        [Category("HMI")]
        public ObservableCollection<HMIComboBoxItem> WriteItems
        {
            get;
            set;
        }
        [Category("HMI")]
        public bool WriteSecure
        {
            get
            {
                return (bool)base.GetValue(HMIComboBoxButton1.WriteSecureProperty);
            }
            set
            {
                base.SetValue(HMIComboBoxButton1.WriteSecureProperty, value);
            }
        }
        [Browsable(false), Category("HMI")]
        public int SelectedIndex
        {
            get
            {
                return this.uxComboBox.SelectedIndex;
            }
            set
            {
                this.uxComboBox.SelectedIndex=(value);
            }
        }
        public HMIComboBoxItem SelectedItem
        {
            get;
            private set;
        }
        protected TextBlock OutputElement
        {
            get
            {
                return this.outputElement;
            }
        }
        private static void OnStateSignalNamePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            HMIComboBoxButton1? comboBoxButton = dependencyObject as HMIComboBoxButton1;
            if (comboBoxButton != null && DesignerProperties.GetIsInDesignMode(comboBoxButton))
            {
                comboBoxButton.setOutputElementTextInDesignModel();
                return;
            }
        }
        public HMIComboBoxButton1()
        {
            base.DefaultStyleKey=(typeof(HMIComboBoxButton1));

            base.DataContext=(this);

            this.messageBoxManager = new MessageBoxManager(null);
            this.writeSecureDialogManager = new WriteSecureDialogManager(new EventHandler<DialogClosedEventArgs>(this.onWriteSecureDialogClosed));
            this.WriteItems = new ObservableCollection<HMIComboBoxItem>();
        }
        private void onUserContextChanged(string userName)
        {
            if (string.IsNullOrEmpty(base.SecurityAuthorizationName))
            {
                return;
            }
            this.isAuthorized = base.Connector.CheckProjectAuthorization(base.SecurityAuthorizationName);
            switch (base.SecurityDenyAccessBehaviour)
            {
                case DenyAccessBehaviour.Disable:
                    this.SetControlEnabled(this.isAuthorized);
                    return;
                case DenyAccessBehaviour.Hide:
                    this.setControlVisibility(this.isAuthorized);
                    return;
                default:
                    throw new ArgumentOutOfRangeException("DenyAccessBehaviour");
            }
        }
        private void OnSymbolicTextTranslationsChangedHandler(string[] symbolicNames, string[] translationTexts)
        {
            for (int i = 0; i < symbolicNames.Length; i++)
            {
                string symbolicName = symbolicNames[i];
                string translationText = translationTexts[i];
                this.setSymbolicTranslation(translationText, symbolicName);
            }
        }
        private void OnStateSignalChanged(string signalName, object signalValue)
        {
            if (signalValue == null)
            {
                return;
            }
            this.SetComboBoxSelectedItem(signalValue);
        }
        private void SetComboBoxSelectedItem(object signalValue)
        {
            this.unregisterSelectionChanged();
            this.setSelectedItem(this.ConvertSignalValueToDouble(signalValue));
            this.registerSelectionChanged();
        }
        private void comboBox_0_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItem = (this.uxComboBox.SelectedItem as HMIComboBoxItem);
            this.WriteSignal();
            this.RaiseSelectionChanged();
        }
        private void onWriteSecureDialogClosed(object sender, DialogClosedEventArgs e)
        {
            this.WriteSignalInternal(this.writeSecureDialogManager.Password);
        }
        private void onEnableSignalNameChanged(string signalName, object signalValue)
        {
            double num = this.convertSignalValue(signalValue);
            bool enabled;
            if (!double.IsNaN(num) && !object.Equals(num, 0.0))
            {
                if (!this.EnableMask.HasValue)
                {
                    enabled = true;
                }
                else
                {
                    long signalBitmask = Convert.ToInt64(num);
                    enabled = ((signalBitmask & (long)this.EnableMask.Value) != (long)this.EnableMask.Value);
                }
            }
            else
            {
                enabled = false;
            }
            this.SetControlEnabled(enabled);
        }
        private void onVisibilitySignalNameChanged(string signalName, object signalValue)
        {
            double val = this.convertSignalValue(signalValue);
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
            this.setControlVisibility(visible);
        }
        private void HMIComboBoxButton1_MouseEnter(object sender, MouseEventArgs e)
        {
            base.Cursor=(Cursors.Hand);
        }
        private void HMIComboBoxButton1_MouseLeave(object sender, MouseEventArgs e)
        {
            base.Cursor=(Cursors.Arrow);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.outputElement = null;
            this.outputElement = (base.GetTemplateChild("OutputElement") as TextBlock);
            this.uxComboBox = (base.GetTemplateChild("uxComboBox") as ComboBox);

            bool isInDesignTool = DesignerProperties.GetIsInDesignMode(this);

            base.Connector.UserContextChanged += new OnUserContextChangedEventHandler(this.onUserContextChanged);

            if (isInDesignTool)
            {
                this.setOutputElementTextInDesignModel();
                return;
            }

            this.setControlStatus();
            this.setOutputElement();
            this.setResolveHolder_Register();
            this.setWriteItems();
        }
        private void setOutputElement()
        {
            if (this.outputElement == null)
            {
                return;
            }
            this.outputElement.Text=(string.Empty);
            this.outputElement.Visibility= System.Windows.Visibility.Collapsed;
        }
        private void setOutputElementTextInDesignModel()
        {
            if (this.outputElement == null)
            {
                return;
            }
            string text = (!string.IsNullOrEmpty(this.StateSignalName)) ? StringPlaceholdersResolver.Resolve(base.SignalPrefix + this.StateSignalName, this) : string.Empty;
            this.outputElement.Text=(text);
        }
        private void setResolveHolder_Register()
        {
            if (this.uxComboBox == null)
            {
                return;
            }

            VisualStateManager.GoToState(this, this.HasRequiredSignalConfiguration() ? "Normal" : "Warning", true);

            this.ResolvePlaceholders();
            this.setRigisters();
        }
        private void ResolvePlaceholders()
        {
            this.ResolveStateSignalNamePlaceholders();
            this.ResolveWriteSignalNamePlaceholders();
            this.ResolveEnableSignalNamePlaceholders();
            this.ResolveVisibilitySignalNamePlaceholders();
        }
        private void ResolveStateSignalNamePlaceholders()
        {
            if (string.IsNullOrEmpty(this.StateSignalName))
            {
                return;
            }
            this.StateSignalName = StringPlaceholdersResolver.Resolve(base.SignalPrefix + this.StateSignalName, this);
        }
        private void ResolveWriteSignalNamePlaceholders()
        {
            if (string.IsNullOrEmpty(this.WriteSignalName))
            {
                return;
            }
            this.WriteSignalName = StringPlaceholdersResolver.Resolve(base.SignalPrefix + this.WriteSignalName, this);
        }
        private void ResolveEnableSignalNamePlaceholders()
        {
            if (string.IsNullOrEmpty(this.EnableSignalName))
            {
                return;
            }
            this.EnableSignalName = StringPlaceholdersResolver.Resolve(base.SignalPrefix + this.EnableSignalName, this);
        }
        private void ResolveVisibilitySignalNamePlaceholders()
        {
            if (string.IsNullOrEmpty(this.VisibilitySignalName))
            {
                return;
            }
            this.VisibilitySignalName = StringPlaceholdersResolver.Resolve(base.SignalPrefix + this.VisibilitySignalName, this);
        }
        private void setRigisters()
        {
            this.registerSelectionChanged();
            this.registerStateSignalName();
            this.registerEnableSignalName();
            this.registerVisibilitySignalName();
            this.registerMouseEvent();
        }
        private void registerSelectionChanged()
        {
            this.uxComboBox.SelectionChanged+=(new SelectionChangedEventHandler(this.comboBox_0_SelectionChanged));
        }
        private void unregisterSelectionChanged()
        {
            this.uxComboBox.SelectionChanged-=(new SelectionChangedEventHandler(this.comboBox_0_SelectionChanged));
        }
        private void registerStateSignalName()
        {
            base.Connector.RegisterSignalChangedHandler(string.Empty, this.StateSignalName, new OnSignalChangedDelegate(this.OnStateSignalChanged));
        }
        private void registerEnableSignalName()
        {
            if (string.IsNullOrEmpty(this.EnableSignalName))
            {
                return;
            }
            base.Connector.RegisterSignalChangedHandler(string.Empty, this.EnableSignalName, new OnSignalChangedDelegate(this.onEnableSignalNameChanged));
        }
        private void registerVisibilitySignalName()
        {
            if (string.IsNullOrEmpty(this.VisibilitySignalName))
            {
                return;
            }
            base.Connector.RegisterSignalChangedHandler(string.Empty, this.VisibilitySignalName, new OnSignalChangedDelegate(this.onVisibilitySignalNameChanged));
        }
        private void registerMouseEvent()
        {
            base.MouseEnter+=(new MouseEventHandler(this.HMIComboBoxButton1_MouseEnter));
            base.MouseLeave+=(new MouseEventHandler(this.HMIComboBoxButton1_MouseLeave));
        }
        private void setControlStatus()
        {
            if (string.IsNullOrEmpty(base.SecurityAuthorizationName))
            {
                this.isAuthorized = true;
                this.isSecurityAvailable = false;
                return;
            }
            this.isAuthorized = false;
            this.isSecurityAvailable = true;
            switch (base.SecurityDenyAccessBehaviour)
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
        private void SetControlEnabled(bool enabled)
        {
            base.IsEnabled=(enabled && (!this.isSecurityAvailable || this.isAuthorized || base.SecurityDenyAccessBehaviour != DenyAccessBehaviour.Disable));
            VisualStateManager.GoToState(this, base.IsEnabled ? "Normal" : "Disabled", true);
        }
        private void setControlVisibility(bool visible)
        {
            Visibility vb = System.Windows.Visibility.Collapsed;
            if (visible)
            {
                if (!this.isSecurityAvailable || this.isAuthorized || base.SecurityDenyAccessBehaviour != DenyAccessBehaviour.Hide)
                {
                    vb =  System.Windows.Visibility.Visible;
                }
            }

            base.Visibility=(vb);
        }
        private double convertSignalValue(object signalValue)
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
        private void setWriteItems()
        {
            this.setWriteItemsSymboText();
            this.setWriteItemsTranslation();
            this.setWriteItemsConnectorRegister();
        }
        private void setWriteItemsSymboText()
        {
            foreach (var writeItem in this.WriteItems)
            {
                writeItem.SymbolicText = StringPlaceholdersResolver.Resolve(writeItem.SymbolicText, this);
            }
        }
        private void setWriteItemsTranslation()
        {
            if (this.WriteItems == null)
            {
                return;
            }

            foreach (var writeItem in this.WriteItems)
            {
                    writeItem.Translation = writeItem.SymbolicText;
            }
        }
        private void setWriteItemsConnectorRegister()
        {
            if (this.WriteItems == null)
            {
                return;
            }

            string[] symbolicNames = this.WriteItems.Select(t => t.SymbolicText).ToArray();
            base.Connector.RegisterLanguageChangedHandler(symbolicNames, new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler));
        }
        protected override void HandlePropertyChanged(DependencyProperty property)
        {
            base.HandlePropertyChanged(property);
            if (property != HMIStatesControlBase.ObjectIDProperty && property != HMIStatesControlBase.SignalPrefixProperty)
            {
                return;
            }
            this.setOutputElementTextInDesignModel();
        }
        private void WriteSignal()
        {
            if (this.WriteSecure)
            {
                this.writeSecureDialogManager.Show();
                return;
            }
            this.WriteSignalInternal(null);
        }
        private void WriteSignalInternal(string password = null)
        {
            if (password == null)
            {
                base.Connector.WriteSignal(string.Empty, this.WriteSignalName, this.SelectedItem.Value, new OnResultDelegate(this.OnWriteSignalResultHandler));
                return;
            }

            base.Connector.WriteSignal(string.Empty, this.WriteSignalName, this.SelectedItem.Value, password, new OnResultDelegate(this.OnWriteSignalResultHandler));
        }
        private void OnWriteSignalResultHandler(Exception error, int[] results)
        {
            if (this.IsAsyncError(results))
            {
                this.SetComboBoxSelectedItem((object)this.oldValue);
                this.messageBoxManager.Show("Error on write operation: " + results[0], "HMI", MessageBoxType.Error, "OK", "Cancel");
                return;
            }

            this.oldValue = this.ConvertSignalValueToDouble(this.uxComboBox.SelectedValue);
            this.SetComboBoxSelectedItem((object)this.newValue);
        }
        private bool HasRequiredSignalConfiguration()
        {
            return this.WriteItems != null && Enumerable.Any<HMIComboBoxItem>(this.WriteItems) && !string.IsNullOrWhiteSpace(this.StateSignalName) && !string.IsNullOrWhiteSpace(this.WriteSignalName);
        }
        private void setSelectedItem(double signalValue)
        {
            if (!this.allowSelectSameItem && this.newValue.Equals(signalValue))
            {
                return;
            }

            this.uxComboBox.SelectedIndex=(-1);
            HMIComboBoxItem selectedItem = this.WriteItems.FirstOrDefault(t => t.Value.Equals(signalValue));
            this.uxComboBox.SelectedItem=(selectedItem);

            this.allowSelectSameItem = false;
            this.newValue = signalValue;
        }
        private double ConvertSignalValueToDouble(object signalValue)
        {
            double result;
            double.TryParse(signalValue.ToString(), System.Globalization.NumberStyles.Any, Thread.CurrentThread.CurrentCulture, out result);
            return result;
        }
        private bool IsAsyncError(int[] results)
        {
            return results == null || results[0] != 0;
        }
        private void setSymbolicTranslation(string translationText, string symbolicName)
        {
            HMIComboBoxItem? wFComboBoxItem = this.WriteItems.FirstOrDefault(t => t.SymbolicText == symbolicName);
            if (wFComboBoxItem == null)
            {
                return;
            }
            wFComboBoxItem.Translation = translationText;
        }
        private void RaiseSelectionChanged()
        {
            this.SelectionChanged?.Invoke(this, this.SelectedItem);
        }
        /// <summary>
        /// 释放，取消注册事件，避免内存泄漏
        /// </summary>
        public override void Dispose()
        {
            this.DisposeComboBoxButton1Connector();

            this.DisposeMouseHandlers();

            this.messageBoxManager.Dispose();
            this.writeSecureDialogManager.Dispose();

            // 调用基类的Dispose方法，确保基类也能正确释放资源
            base.Dispose();
        }
        private void DisposeComboBoxButton1Connector()
        {
            base.Connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.onUserContextChanged);
            base.Connector.UnregisterSignalChangedHandler(string.Empty, this.StateSignalName, new OnSignalChangedDelegate(this.OnStateSignalChanged));
            base.Connector.UnregisterSignalChangedHandler(string.Empty, this.EnableSignalName, new OnSignalChangedDelegate(this.onEnableSignalNameChanged));
            base.Connector.UnregisterSignalChangedHandler(string.Empty, this.VisibilitySignalName, new OnSignalChangedDelegate(this.onVisibilitySignalNameChanged));
            base.Connector.UnregisterLanguageChangedHandler(null, new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler));
        }
        private void DisposeMouseHandlers()
        {
            base.MouseEnter-=(new MouseEventHandler(this.HMIComboBoxButton1_MouseEnter));
            base.MouseLeave-=(new MouseEventHandler(this.HMIComboBoxButton1_MouseLeave));
        }
        
    }
}
