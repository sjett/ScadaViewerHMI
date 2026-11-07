// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using HMI.Runtime.Core;
using HMI.Runtime.Core.Helper.Extensions;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common.Converters;
using HMI.Runtime.Shared.Dialogs;
namespace HMI.RuntimeControls.Buttons
{
    /// <summary>
    /// 状态按钮基类，提供状态按钮的基本功能，包括权限控制、可见性控制、符号文本显示、内容显示、图标显示等功能，
    /// 并提供Click事件的处理逻辑。具体的状态按钮（如切换按钮、Momentary按钮等）可以继承该基类并实现抽象方法以满足特定需求。
    /// </summary>
    [ContentProperty("Content")]
    public abstract class HMIStatesButtonBase : HMIStaticStatesControlBase, IDisposable
    {
        protected const int RoundByNumberOfDigits = 5;
        protected const string UxContentPresenter = "uxContentPresenter";
        protected const string UxOutputElement = "uxOutputElement";
        protected const string DenyAccessBehaviourException = "DenyAccessBehaviour";
        protected const string SnInsufficientAuthorizations = "HMISN_SL_InsufficientAuthorizations";
        protected const string SnInvalidPasswordDuringWriteSecureOperations = "HMISN_Ctrl_SL_InvalidPasswordDuringWriteSecureOperations";
        protected const string SnWriteError = "HMISN_SL_WriteError";

        private bool isAuthorized;
        private TextBlock uxOutputElement;
        private ContentPresenter uxContentPresenter;
        private readonly MessageBoxManager messageBoxManager;
        private readonly WriteSecureDialogManager writeSecureDialogManager;
        private Dictionary<string, SymbolicTextTranslation> dictTranslations;

        public static readonly DependencyProperty ChromeFrontBrushProperty = DependencyProperty.Register("ChromeFrontBrush", typeof(Brush), typeof(HMIStatesButtonBase), null);
        public static readonly DependencyProperty MouseOverBrushProperty = DependencyProperty.Register("MouseOverBrush", typeof(Brush), typeof(HMIStatesButtonBase), null);
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(HMIStatesButtonBase), null);
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(HMIStatesButtonBase), new PropertyMetadata(null, new PropertyChangedCallback(HMIStatesButtonBase.OnContentChanged)));
        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(HMIStatesButtonBase), null);
        public static readonly DependencyProperty IconMarginProperty = DependencyProperty.Register("IconMargin", typeof(Thickness), typeof(HMIStatesButtonBase), null);
        public static readonly DependencyProperty IconMaxSizeProperty = DependencyProperty.Register("IconMaxSize", typeof(double), typeof(HMIStatesButtonBase), new PropertyMetadata(24.0, new PropertyChangedCallback(HMIStatesButtonBase.OnIconMaxSizeChanged)));
        public static readonly DependencyProperty IconOpacityProperty = DependencyProperty.Register("IconOpacity", typeof(double), typeof(HMIStatesButtonBase), new PropertyMetadata(100.0, new PropertyChangedCallback(HMIStatesButtonBase.OnIconOpacityChanged)));
        public static readonly DependencyProperty IconPlacementProperty = DependencyProperty.Register("IconPlacement", typeof(ButtonIconPlacement), typeof(HMIStatesButtonBase), new PropertyMetadata(ButtonIconPlacement.Center, new PropertyChangedCallback(HMIStatesButtonBase.OnIconPlacementChanged)));
        public static readonly DependencyProperty IconStretchProperty = DependencyProperty.Register("IconStretch", typeof(Stretch), typeof(HMIStatesButtonBase), new PropertyMetadata(Stretch.Uniform));
        public static readonly DependencyProperty EnableMaskProperty = DependencyProperty.Register("EnableMask", typeof(int?), typeof(HMIStatesButtonBase), new PropertyMetadata(null));
        public static readonly DependencyProperty EnableSignalNameProperty = DependencyProperty.Register("EnableSignalName", typeof(string), typeof(HMIStatesButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty VisibilityMaskProperty = DependencyProperty.Register("VisibilityMask", typeof(int?), typeof(HMIStatesButtonBase), new PropertyMetadata(null));
        public static readonly DependencyProperty VisibilitySignalNameProperty = DependencyProperty.Register("VisibilitySignalName", typeof(string), typeof(HMIStatesButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMIStatesButtonBase), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SymbolicTextProperty = DependencyProperty.Register("SymbolicText", typeof(string), typeof(HMIStatesButtonBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIStatesButtonBase.OnSymbolicTextChanged)));
        public static readonly DependencyProperty WriteSecureProperty = DependencyProperty.Register("WriteSecure", typeof(bool), typeof(HMIStatesButtonBase), new PropertyMetadata(false));
        
        public Brush ChromeFrontBrush
        {
            get
            {
                return (Brush)base.GetValue(HMIStatesButtonBase.ChromeFrontBrushProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.ChromeFrontBrushProperty, value);
            }
        }
        public Brush MouseOverBrush
        {
            get
            {
                return (Brush)base.GetValue(HMIStatesButtonBase.MouseOverBrushProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.MouseOverBrushProperty, value);
            }
        }
        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)base.GetValue(HMIStatesButtonBase.CornerRadiusProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.CornerRadiusProperty, value);
            }
        }
        public object Content
        {
            get
            {
                return base.GetValue(HMIStatesButtonBase.ContentProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.ContentProperty, value);
            }
        }
        public DataTemplate ContentTemplate
        {
            get
            {
                return (DataTemplate)base.GetValue(HMIStatesButtonBase.ContentTemplateProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.ContentTemplateProperty, value);
            }
        }
        public Thickness IconMargin
        {
            get
            {
                return (Thickness)base.GetValue(HMIStatesButtonBase.IconMarginProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.IconMarginProperty, value);
            }
        }
        public double IconMaxSize
        {
            get
            {
                return (double)base.GetValue(HMIStatesButtonBase.IconMaxSizeProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.IconMaxSizeProperty, value);
            }
        }
        public double IconOpacity
        {
            get
            {
                return (double)base.GetValue(HMIStatesButtonBase.IconOpacityProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.IconOpacityProperty, value);
            }
        }
        public ButtonIconPlacement IconPlacement
        {
            get
            {
                return (ButtonIconPlacement)base.GetValue(HMIStatesButtonBase.IconPlacementProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.IconPlacementProperty, value);
            }
        }
        public Stretch IconStretch
        {
            get
            {
                return (Stretch)base.GetValue(HMIStatesButtonBase.IconStretchProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.IconStretchProperty, value);
            }
        }
        [Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
        public int? EnableMask
        {
            get
            {
                return (int?)base.GetValue(HMIStatesButtonBase.EnableMaskProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.EnableMaskProperty, value);
            }
        }
        [Category("HMI")]
        public string EnableSignalName
        {
            get
            {
                return (string)base.GetValue(HMIStatesButtonBase.EnableSignalNameProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.EnableSignalNameProperty, value);
            }
        }
        [Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
        public int? VisibilityMask
        {
            get
            {
                return (int?)base.GetValue(HMIStatesButtonBase.VisibilityMaskProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.VisibilityMaskProperty, value);
            }
        }
        [Category("HMI")]
        public string VisibilitySignalName
        {
            get
            {
                return (string)base.GetValue(HMIStatesButtonBase.VisibilitySignalNameProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.VisibilitySignalNameProperty, value);
            }
        }
        public string SignalName
        {
            get
            {
                return (string)base.GetValue(HMIStatesButtonBase.SignalNameProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.SignalNameProperty, value);
            }
        }
        public string SymbolicText
        {
            get
            {
                return (string)base.GetValue(HMIStatesButtonBase.SymbolicTextProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.SymbolicTextProperty, value);
            }
        }
        public bool WriteSecure
        {
            get
            {
                return (bool)base.GetValue(HMIStatesButtonBase.WriteSecureProperty);
            }
            set
            {
                base.SetValue(HMIStatesButtonBase.WriteSecureProperty, value);
            }
        }
        private static void OnContentChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            HMIStatesButtonBase button = dependencyObject as HMIStatesButtonBase;
            if (button != null && DesignerProperties.GetIsInDesignMode(button))
            {
                button.UpdateOutput();
                return;
            }
        }
        private static void OnIconMaxSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            HMIStatesButtonBase button = dependencyObject as HMIStatesButtonBase;
            if (button == null)
            {
                return;
            }
            if ((double)eventArgs.NewValue < 0.0)
            {
                button.IconMaxSize = (double)eventArgs.OldValue;
            }
        }
        private static void OnIconOpacityChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            HMIStatesButtonBase button = dependencyObject as HMIStatesButtonBase;
            if (button == null)
            {
                return;
            }
            if ((double)eventArgs.NewValue < 0.0 || (double)eventArgs.NewValue > 100.0)
            {
                button.IconOpacity = (double)eventArgs.OldValue;
            }
        }
        private static void OnIconPlacementChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            HMIStatesButtonBase button = dependencyObject as HMIStatesButtonBase;
            if (button == null)
            {
                return;
            }
            button.UpdateIconPlacement();
        }
        private static void OnSymbolicTextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            HMIStatesButtonBase button = dependencyObject as HMIStatesButtonBase;
            if (button != null && DesignerProperties.GetIsInDesignMode(button))
            {
                button.UpdateOutput();
                return;
            }
        }
        protected HMIStatesButtonBase()
        {
            base.DefaultStyleKey=(typeof(HMIStatesButtonBase));
            this.dictTranslations = new Dictionary<string, SymbolicTextTranslation>(StringComparer.InvariantCultureIgnoreCase);
            this.messageBoxManager = new MessageBoxManager(null);
            this.writeSecureDialogManager = new WriteSecureDialogManager(new EventHandler<DialogClosedEventArgs>(this.onDialogClosedEvent));
        }
        private void OnUserContextChanged(string userName)
        {
            if (string.IsNullOrEmpty(base.SecurityAuthorizationName))
            {
                return;
            }
            this.isAuthorized = base.Connector.CheckProjectAuthorization(base.SecurityAuthorizationName);
            switch (base.SecurityDenyAccessBehaviour)
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
        private void OnSymbolicTextTranslationsChangedHandler(string[] symbolicTexts, string[] translationTexts)
        {
            try
            {
                this.UpdateTranslations(symbolicTexts, translationTexts);
                this.UpdateSymbolicText(translationTexts[symbolicTexts.Length - 1]);
                this.UpdateOutput();
            }
            catch (Exception ex)
            {
                base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
            }
        }
        private void UpdateTranslations(string[] symbolicTexts, string[] translationTexts)
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
        private void UpdateSymbolicText(string translationText)
        {
            if (string.IsNullOrEmpty(this.SymbolicText))
            {
                return;
            }
            this.SymbolicText = translationText;
        }
        private void onDialogClosedEvent(object sender, DialogClosedEventArgs e)
        {
            this.WriteSignalValue(this.writeSecureDialogManager.Password);
        }
        protected override void HandlePropertyChanged(DependencyProperty property)
        {
            base.HandlePropertyChanged(property);
            if (property == HMIStatesControlBase.ObjectIDProperty || property == HMIStatesControlBase.SignalPrefixProperty)
            {
                this.UpdateOutput();
            }
        }
        private void onEnableSignalNameChanged(string signalName, object signalValue)
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
        private void HMIStatesButtonBase_MouseEnter(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "MouseOver", true);
            base.Cursor=(Cursors.Hand);
        }
        private void HMIStatesButtonBase_MouseLeave(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
            base.Cursor=(Cursors.Arrow);
        }
        private void HMIStatesButtonBase_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "Pressed", true);
            this.OnClick();
        }
        protected virtual void HMIStatesButtonBase_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "MouseOver", true);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.uxOutputElement = null;
            this.uxContentPresenter = null;
            this.uxOutputElement = (base.GetTemplateChild("uxOutputElement") as TextBlock);
            this.uxContentPresenter = (base.GetTemplateChild("uxContentPresenter") as ContentPresenter);

            this.UpdateOutput();

            bool isInDesignTool = DesignerPropertiesExt.IsInDesignTool;
            base.Connector.UserContextChanged += new OnUserContextChangedEventHandler(this.OnUserContextChanged);
            if (isInDesignTool)
            {
                return;
            }

            this.setControlStatus();
            this.ResolvePlaceholders();
            this.UpdateOutput();
            this.setRigisters();
        }
        protected void OnClick()
        {
            if (!DesignerProperties.GetIsInDesignMode(this) && !string.IsNullOrEmpty(this.SignalName) && this.isAuthorized)
            {
                this.WriteSignal();
                return;
            }
        }
        private void ResolvePlaceholders()
        {
            this.ResolveSignalName1Placeholders();
            this.ResolveEnableSignalNamePlaceholders();
            this.ResolveVisibilitySignalNamePlaceholders();
            this.ResolveSymbolicTextPlaceholders();
        }
        private void ResolveSignalName1Placeholders()
        {
            if (string.IsNullOrEmpty(this.SignalName))
            {
                return;
            }
            this.SignalName = StringPlaceholdersResolver.Resolve(base.SignalPrefix + this.SignalName, this);
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
        private void ResolveSymbolicTextPlaceholders()
        {
            if (string.IsNullOrEmpty(this.SymbolicText))
            {
                return;
            }
            this.SymbolicText = StringPlaceholdersResolver.Resolve(this.SymbolicText, this);
        }
        protected void UpdateOutput()
        {
            this.UpdateTextBlock();
            this.UpdateContentPresenter();
        }
        protected virtual void UpdateTextBlock()
        {
            if (this.uxOutputElement != null && !string.IsNullOrEmpty(this.SymbolicText))
            {
                this.uxOutputElement.Text=(this.SymbolicText);
                return;
            }
        }
        private void UpdateContentPresenter()
        {
            if (this.uxContentPresenter == null)
            {
                return;
            }
            if (this.Content != null)
            {
                this.uxContentPresenter.Content=(this.Content);
                return;
            }
            if (this.uxOutputElement == null)
            {
                return;
            }
            this.uxContentPresenter.Content=(this.uxOutputElement);
        }
        private void setRigisters()
        {
            this.registerTranslation();
            this.registerEnableSignalName();
            this.registerVisibilitySignalName();
            this.registerMouseEvent();
        }
        private void registerTranslation()
        {
            this.fillTranslationDict();
            this.registerLanguageChangedHandler();
        }
        private void fillTranslationDict()
        {
            SymbolicTextTranslation symbolicTextTranslation = new SymbolicTextTranslation("HMISN_SL_InsufficientAuthorizations");
            this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
            symbolicTextTranslation = new SymbolicTextTranslation("HMISN_Ctrl_SL_InvalidPasswordDuringWriteSecureOperations");
            this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
            symbolicTextTranslation = new SymbolicTextTranslation("HMISN_SL_WriteError");
            this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
            if (string.IsNullOrEmpty(this.SymbolicText))
            {
                return;
            }
            symbolicTextTranslation = new SymbolicTextTranslation(this.SymbolicText);
            this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
        }
        private void registerLanguageChangedHandler()
        {
            try
            {
                string[] symbolicNames = new string[this.dictTranslations.Count];
                this.dictTranslations.Keys.CopyTo(symbolicNames, 0);
                base.Connector.RegisterLanguageChangedHandler(symbolicNames, new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler));
            }
            catch (Exception ex)
            {
                base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
            }
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
            base.MouseEnter+=(new MouseEventHandler(this.HMIStatesButtonBase_MouseEnter));
            base.MouseLeave+=(new MouseEventHandler(this.HMIStatesButtonBase_MouseLeave));
            base.MouseLeftButtonDown+=(new MouseButtonEventHandler(this.HMIStatesButtonBase_MouseLeftButtonDown));
            base.MouseLeftButtonUp+=(new MouseButtonEventHandler(this.HMIStatesButtonBase_MouseLeftButtonUp));
        }
        private void setControlStatus()
        {
            if (string.IsNullOrEmpty(base.SecurityAuthorizationName))
            {
                this.isAuthorized = true;
                return;
            }
            this.isAuthorized = false;
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
        private void EnableControl(bool enable)
        {
            base.IsEnabled=(enable && (this.isAuthorized || base.SecurityDenyAccessBehaviour != DenyAccessBehaviour.Disable));
            VisualStateManager.GoToState(this, base.IsEnabled ? "Normal" : "Disabled", true);
        }
        private void ShowControl(bool show)
        {
            Visibility visibility = System.Windows.Visibility.Collapsed;
            if (show)
            {
                if (this.isAuthorized || base.SecurityDenyAccessBehaviour != DenyAccessBehaviour.Hide)
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
        private void WriteSignal()
        {
            if (this.WriteSecure)
            {
                this.writeSecureDialogManager.Show();
                return;
            }
            this.WriteSignalValue(null);
        }
        protected virtual void WriteSignalValue(string password = null)
        {
            double writeValue = this.GetWriteValue();
            if (password == null)
            {
                base.Connector.WriteSignal(string.Empty, this.SignalName, writeValue, new OnResultDelegate(this.HandleWriteResult));
                return;
            }
            base.Connector.WriteSignal(string.Empty, this.SignalName, writeValue, password, new OnResultDelegate(this.HandleWriteResult));
        }
        /// <summary>
        /// 获取写入值
        /// </summary>
        /// <returns></returns>
        protected abstract double GetWriteValue();

        protected virtual void HandleWriteResult(Exception error, int[] results)
        {
            if (!this.IsAsyncError(error, results))
            {
                return;
            }
            this.messageBoxManager.Show(this.GetTranslatedErrorMessage(error, results), "HMI", MessageBoxType.Error, "OK", "Cancel");
        }
        protected bool IsAsyncError(Exception error, int[] results)
        {
            return error != null || (results != null && results.Length > 0 && results[0] != 0);
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
        private string HandleErrorCode(int code)
        {
            switch (code)
            {
                case -4094:
                case -4093:
                    return this.dictTranslations["HMISN_SL_InsufficientAuthorizations"].Translation;
                case -4091:
                    return this.dictTranslations["HMISN_Ctrl_SL_InvalidPasswordDuringWriteSecureOperations"].Translation;
            }
            return string.Format(this.dictTranslations["HMISN_SL_WriteError"].Translation, code);
        }
        /// <summary>
        /// 根据IconPlacement更新图标位置
        /// </summary>
        protected abstract void UpdateIconPlacement();
        /// <summary>
        /// 根据IconPlacement获取图标所在Grid的行列索引
        /// </summary>
        /// <returns></returns>
        protected int GetIconGridRow()
        {
            int result = 1;
            switch (this.IconPlacement)
            {
                case ButtonIconPlacement.Top:
                    result = 0;
                    break;
                case ButtonIconPlacement.Bottom:
                    result = 2;
                    break;
            }
            return result;
        }
        /// <summary>
        /// 根据IconPlacement获取图标所在Grid的行列索引
        /// </summary>
        /// <returns></returns>
        protected int GetIconGridColumn()
        {
            int result = 1;
            switch (this.IconPlacement)
            {
                case ButtonIconPlacement.Left:
                    result = 0;
                    break;
                case ButtonIconPlacement.Right:
                    result = 2;
                    break;
            }
            return result;
        }
        public override void Dispose()
        {

            this.DisposeConnector();
            this.DisposeDialogManager();
            this.DisposeMouseHandler();
            this.ClearTranslations();

            // / 释放其他资源
            base.Dispose();
        }
        private void DisposeConnector()
        {
            base.Connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.OnUserContextChanged);
            base.Connector.UnregisterSignalChangedHandler(string.Empty, this.EnableSignalName, new OnSignalChangedDelegate(this.onEnableSignalNameChanged));
            base.Connector.UnregisterSignalChangedHandler(string.Empty, this.VisibilitySignalName, new OnSignalChangedDelegate(this.onVisibilitySignalNameChanged));
            base.Connector.UnregisterLanguageChangedHandler(null, new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler));
        }
        private void DisposeDialogManager()
        {
            this.messageBoxManager.Dispose();
            this.writeSecureDialogManager.Dispose();
        }
        private void DisposeMouseHandler()
        {
            base.MouseEnter-=(new MouseEventHandler(this.HMIStatesButtonBase_MouseEnter));
            base.MouseLeave-=(new MouseEventHandler(this.HMIStatesButtonBase_MouseLeave));
            base.MouseLeftButtonDown-=(new MouseButtonEventHandler(this.HMIStatesButtonBase_MouseLeftButtonDown));
            base.MouseLeftButtonUp-=(new MouseButtonEventHandler(this.HMIStatesButtonBase_MouseLeftButtonUp));
        }
        private void ClearTranslations()
        {
            if (this.dictTranslations == null)
            {
                return;
            }
            this.dictTranslations.Clear();
        }
    }
}
