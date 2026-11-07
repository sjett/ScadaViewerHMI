// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HMI.Runtime.Core;
using HMI.Runtime.Core.Helper.Extensions;
using HMI.Runtime.Shared;
namespace HMI.RuntimeControls.Buttons
{
    /// <summary>
    ///  状态切换按钮：根据绑定的状态信号值在两种状态之间切换，
    ///  点击按钮时向信号写入对应状态的值。按钮的显示内容也会根据状态信号值的变化而变化。
    /// </summary>
    public class HMIStatesToggleButton1 : HMIStatesButtonBase, IDisposable
    {
        private bool checkedStatus;
        private bool lockWrite = true;
        private string text;
        private TextBlock uxOutputElement;
        private Image uxButtonImageChecked;
        private Image uxButtonImageUnchecked;

        public static readonly DependencyProperty CheckedBackgroundProperty = DependencyProperty.Register("CheckedBackground", typeof(Brush), typeof(HMIStatesToggleButton1), null);
        public static readonly DependencyProperty IconImageCheckedProperty = DependencyProperty.Register("IconImageChecked", typeof(ImageSource), typeof(HMIStatesToggleButton1), null);
        public static readonly DependencyProperty IconImageUncheckedProperty = DependencyProperty.Register("IconImageUnchecked", typeof(ImageSource), typeof(HMIStatesToggleButton1), null);
        public static readonly DependencyProperty IsTipModeEnabledProperty = DependencyProperty.Register("IsTipModeEnabled", typeof(bool), typeof(HMIStatesToggleButton1), new PropertyMetadata(false));
        public static readonly DependencyProperty StateSignalNameProperty = DependencyProperty.Register("StateSignalName", typeof(string), typeof(HMIStatesToggleButton1), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty CheckedSymbolicTextProperty = DependencyProperty.Register("CheckedSymbolicText", typeof(string), typeof(HMIStatesToggleButton1), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty UncheckedSymbolicTextProperty = DependencyProperty.Register("UncheckedSymbolicText", typeof(string), typeof(HMIStatesToggleButton1), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Value1Property = DependencyProperty.Register("Value1", typeof(double), typeof(HMIStatesToggleButton1), null);
        public static readonly DependencyProperty Value2Property = DependencyProperty.Register("Value2", typeof(double), typeof(HMIStatesToggleButton1), null);
        
        public Brush CheckedBackground
        {
            get
            {
                return (Brush)base.GetValue(HMIStatesToggleButton1.CheckedBackgroundProperty);
            }
            set
            {
                base.SetValue(HMIStatesToggleButton1.CheckedBackgroundProperty, value);
            }
        }
        public ImageSource IconImageChecked
        {
            get
            {
                return (ImageSource)base.GetValue(HMIStatesToggleButton1.IconImageCheckedProperty);
            }
            set
            {
                base.SetValue(HMIStatesToggleButton1.IconImageCheckedProperty, value);
            }
        }
        public ImageSource IconImageUnchecked
        {
            get
            {
                return (ImageSource)base.GetValue(HMIStatesToggleButton1.IconImageUncheckedProperty);
            }
            set
            {
                base.SetValue(HMIStatesToggleButton1.IconImageUncheckedProperty, value);
            }
        }
        /// <summary>
        ///  提示模式：当启用时，按钮的状态在鼠标按下时切换，松开时写入信号；当禁用时，按钮的状态在鼠标松开时切换，并写入信号
        /// </summary>
        public bool IsTipModeEnabled
        {
            get
            {
                return (bool)base.GetValue(HMIStatesToggleButton1.IsTipModeEnabledProperty);
            }
            set
            {
                base.SetValue(HMIStatesToggleButton1.IsTipModeEnabledProperty, value);
            }
        }
        public string StateSignalName
        {
            get
            {
                return (string)base.GetValue(HMIStatesToggleButton1.StateSignalNameProperty);
            }
            set
            {
                base.SetValue(HMIStatesToggleButton1.StateSignalNameProperty, value);
            }
        }
        public string CheckedSymbolicText
        {
            get
            {
                return (string)base.GetValue(HMIStatesToggleButton1.CheckedSymbolicTextProperty);
            }
            set
            {
                base.SetValue(HMIStatesToggleButton1.CheckedSymbolicTextProperty, value);
            }
        }
        public string UncheckedSymbolicText
        {
            get
            {
                return (string)base.GetValue(HMIStatesToggleButton1.UncheckedSymbolicTextProperty);
            }
            set
            {
                base.SetValue(HMIStatesToggleButton1.UncheckedSymbolicTextProperty, value);
            }
        }
        /// <summary>
        /// checked value
        /// </summary>
        public double Value1
        {
            get
            {
                return (double)base.GetValue(HMIStatesToggleButton1.Value1Property);
            }
            set
            {
                base.SetValue(HMIStatesToggleButton1.Value1Property, value);
            }
        }
        /// <summary>
        /// unchecked value
        /// </summary>
        public double Value2
        {
            get
            {
                return (double)base.GetValue(HMIStatesToggleButton1.Value2Property);
            }
            set
            {
                base.SetValue(HMIStatesToggleButton1.Value2Property, value);
            }
        }
        public HMIStatesToggleButton1()
        {
            base.DefaultStyleKey=(typeof(HMIStatesToggleButton1));
        }
        private void onStateSignalNameChanged(string signalName, object signalValue)
        {
            if (signalValue == null)
            {
                return;
            }
            this.setStatus((double)signalValue);
        }
        private void OnCheckedSymbolicTextTranslationsChangedHandler(string[] symbolicTexts, string[] translationTexts)
        {
            if (translationTexts.Length <= 0)
            {
                return;
            }
            this.CheckedSymbolicText = translationTexts[0];
            base.UpdateOutput();
        }
        private void OnUncheckedSymbolicTextTranslationsChangedHandler(string[] symbolicTexts, string[] translationTexts)
        {
            if (translationTexts.Length <= 0)
            {
                return;
            }
            this.UncheckedSymbolicText = translationTexts[0];
            base.UpdateOutput();
        }
        /// <summary>
        /// 在提示模式下，鼠标按下时切换状态但不写入信号，松开时写入信号，
        /// 因此在点击事件处理程序中先将lockWrite设置为false以允许写入信号，
        /// 然后调用WriteSignalValue方法，在WriteSignalValue方法中根据lockWrite的值来决定写入哪个值，
        /// 最后在HandleWriteResult方法中如果写入失败且处于提示模式，则将状态切换回去。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void HMIStatesButtonBase_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            base.HMIStatesButtonBase_MouseLeftButtonUp(sender, e);
            if (!this.IsTipModeEnabled)
            {
                return;
            }
            this.handlerClickEvent();
        }
        private void handlerClickEvent()
        {
            this.lockWrite = false;
            base.OnClick();
            this.lockWrite = true;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.uxOutputElement = null;
            this.uxOutputElement = (base.GetTemplateChild("uxOutputElement") as TextBlock);
            this.uxButtonImageChecked = (base.GetTemplateChild("uxButtonImageChecked") as Image);
            this.uxButtonImageUnchecked = (base.GetTemplateChild("uxButtonImageUnchecked") as Image);

            this.UpdateText();
            this.UpdateIcon();

            if (DesignerPropertiesExt.IsInDesignTool)
            {
                return;
            }

            this.Value1 = Math.Round(this.Value1, 5);
            this.Value2 = Math.Round(this.Value2, 5);

            this.ResolvePlaceholders();
            this.setRigisters();
        }
        private void UpdateText()
        {
            this.text = base.SymbolicText;
            base.UpdateOutput();
        }
        private void ResolvePlaceholders()
        {
            this.ResolveStateSignalNamePlaceholders();
            this.ResolveCheckedSymbolicTextPlaceholders();
            this.ResolveUncheckedSymbolicTextPlaceholders();
        }
        private void ResolveStateSignalNamePlaceholders()
        {
            if (!string.IsNullOrEmpty(this.StateSignalName))
            {
                this.StateSignalName = StringPlaceholdersResolver.Resolve(base.SignalPrefix + this.StateSignalName, this);
                return;
            }
            if (string.IsNullOrEmpty(base.SignalName))
            {
                return;
            }
            this.StateSignalName = StringPlaceholdersResolver.Resolve(base.SignalPrefix + base.SignalName, this);
        }
        private void ResolveCheckedSymbolicTextPlaceholders()
        {
            if (!string.IsNullOrEmpty(this.CheckedSymbolicText))
            {
                this.CheckedSymbolicText = StringPlaceholdersResolver.Resolve(this.CheckedSymbolicText, this);
                return;
            }
            if (string.IsNullOrEmpty(base.SymbolicText))
            {
                return;
            }
            this.CheckedSymbolicText = StringPlaceholdersResolver.Resolve(base.SymbolicText, this);
        }
        private void ResolveUncheckedSymbolicTextPlaceholders()
        {
            if (!string.IsNullOrEmpty(this.UncheckedSymbolicText))
            {
                this.UncheckedSymbolicText = StringPlaceholdersResolver.Resolve(this.UncheckedSymbolicText, this);
                return;
            }
            if (string.IsNullOrEmpty(base.SymbolicText))
            {
                return;
            }
            this.UncheckedSymbolicText = StringPlaceholdersResolver.Resolve(base.SymbolicText, this);
        }
        private void setRigisters()
        {
            this.registerStateSignalNameHandler();
            this.registerCheckedSymbolicTextHandler();
            this.registerUncheckedSymbolicTextHandler();
        }
        private void registerStateSignalNameHandler()
        {
            if (string.IsNullOrEmpty(this.StateSignalName))
            {
                return;
            }
            base.Connector.RegisterSignalChangedHandler(string.Empty, this.StateSignalName, new OnSignalChangedDelegate(this.onStateSignalNameChanged));
        }
        private void registerCheckedSymbolicTextHandler()
        {
            if (string.IsNullOrEmpty(this.CheckedSymbolicText))
            {
                return;
            }
            base.Connector.RegisterLanguageChangedHandler(new string[]
			{
				this.CheckedSymbolicText
			}, new OnLanguageChangedDelegate(this.OnCheckedSymbolicTextTranslationsChangedHandler));
        }
        private void registerUncheckedSymbolicTextHandler()
        {
            if (string.IsNullOrEmpty(this.UncheckedSymbolicText))
            {
                return;
            }
            base.Connector.RegisterLanguageChangedHandler(new string[]
			{
				this.UncheckedSymbolicText
			}, new OnLanguageChangedDelegate(this.OnUncheckedSymbolicTextTranslationsChangedHandler));
        }
        protected override void WriteSignalValue(string password = null)
        {
            this.toggleCheckValue(default(bool?));
            base.WriteSignalValue(password);
        }
        protected override double GetWriteValue()
        {
            if (!this.IsTipModeEnabled)
            {
                if (!this.checkedStatus)
                {
                    return this.Value2;// unchecked value
                }
                return this.Value1;// checked value
            }
            else
            {
                if (!this.lockWrite)
                {
                    return this.Value2;// unchecked value
                }
                return this.Value1;// checked value
            }
        }
        protected override void HandleWriteResult(Exception error, int[] results)
        {
            if (this.IsAsyncError(error, results) && !this.IsTipModeEnabled)
            {
                this.toggleCheckValue(default(bool?));// 切换回去，因为在非提示模式下状态已经切换了，所以写入失败时需要切换回去以保持状态与信号值一致
            }
            base.HandleWriteResult(error, results);
        }
        private void UpdateIcon()
        {
            this.UpdateIconPlacement();
            VisualStateManager.GoToState(this, "Unchecked", true);
        }
        protected override void UpdateIconPlacement()
        {
            if (this.uxButtonImageChecked != null)
            {
                Grid.SetRow(this.uxButtonImageChecked, base.GetIconGridRow());
                Grid.SetColumn(this.uxButtonImageChecked, base.GetIconGridColumn());
            }
            if (this.uxButtonImageUnchecked != null)
            {
                Grid.SetRow(this.uxButtonImageUnchecked, base.GetIconGridRow());
                Grid.SetColumn(this.uxButtonImageUnchecked, base.GetIconGridColumn());
            }
        }
        private void setStatus(double signalValue)
        {
            if (object.Equals(signalValue, this.Value1))// checked value
            {
                this.toggleCheckValue(new bool?(true));
                this.setCheckedStatus();
                return;
            }
            if (object.Equals(signalValue, this.Value2))// unchecked value
            {
                this.toggleCheckValue(new bool?(false));
                this.setUncheckedStatus();
                return;
            }
            this.setNomalStatus();
        }
        private void setCheckedStatus()
        {
            VisualStateManager.GoToState(this, "Checked", true);
            this.text = this.CheckedSymbolicText;
            base.UpdateOutput();
        }
        private void setUncheckedStatus()
        {
            VisualStateManager.GoToState(this, "Unchecked", true);
            this.text = this.UncheckedSymbolicText;
            base.UpdateOutput();
        }
        private void setNomalStatus()
        {
            VisualStateManager.GoToState(this, "NoCheck", true);
            VisualStateManager.GoToState(this, "Normal", true);
            this.text = base.SymbolicText;
            base.UpdateOutput();
        }
        protected override void UpdateTextBlock()
        {
            if (this.uxOutputElement != null && !string.IsNullOrEmpty(this.text))
            {
                this.uxOutputElement.Text=(this.text);
                return;
            }
        }
        private void toggleCheckValue(bool? check)
        {
            if (this.IsTipModeEnabled)
            {
                return;
            }
            this.checkedStatus = ((!check.HasValue) ? (!this.checkedStatus) : check.Value);
        }
        public override void Dispose()
        {
            this.DisposeConnector2();

            base.Dispose();
        }
        private void DisposeConnector2()
        {
            base.Connector.UnregisterSignalChangedHandler(string.Empty, this.StateSignalName, new OnSignalChangedDelegate(this.onStateSignalNameChanged));
            base.Connector.UnregisterLanguageChangedHandler(null, new OnLanguageChangedDelegate(this.OnCheckedSymbolicTextTranslationsChangedHandler));
            base.Connector.UnregisterLanguageChangedHandler(null, new OnLanguageChangedDelegate(this.OnUncheckedSymbolicTextTranslationsChangedHandler));
        }
    }
}
