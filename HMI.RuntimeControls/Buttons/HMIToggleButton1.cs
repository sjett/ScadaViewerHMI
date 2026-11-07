// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
namespace HMI.RuntimeControls.Buttons
{
    /// <summary>
    /// HMIToggleButtion1 是 HMIToggleButtonBase 的一个具体实现，
    /// 提供了 SymbolicText 属性以支持在设计时设置符号文本，并在运行时根据语言变化自动更新显示文本。
    /// 它还处理了资源清理以避免内存泄漏。
    /// </summary>
    public class HMIToggleButton1 : HMIToggleButtonBase
    {
        private OnLanguageChangedDelegate onLanguageChangedDelegate;
        private bool isInited;

        public static readonly DependencyProperty SymbolicTextProperty = DependencyProperty.Register("SymbolicText", typeof(string), typeof(HMIToggleButton1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIToggleButton1.onSymbolicTextChanged)));
        
        [Category("HMI")]
        public string SymbolicText
        {
            get
            {
                return (string)base.GetValue(HMIToggleButton1.SymbolicTextProperty);
            }
            set
            {
                base.SetValue(HMIToggleButton1.SymbolicTextProperty, value);
            }
        }
        private static void onSymbolicTextChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            HMIToggleButton1? wFToggleButton = dp as HMIToggleButton1;
            if (wFToggleButton != null && DesignerProperties.GetIsInDesignMode(wFToggleButton))
            {
                wFToggleButton.Content=(wFToggleButton.SymbolicText);
                return;
            }
        }
        public HMIToggleButton1()
        {
            base.DefaultStyleKey=(typeof(HMIToggleButton1));
        }
        ~HMIToggleButton1()
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
                base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
            }
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                base.Content=(this.SymbolicText);
                return;
            }
            if (this.isInited)
            {
                return;
            }
            this.isInited = true;
            base.Content=(string.Empty);
            if (!string.IsNullOrEmpty(this.SymbolicText))
            {
                try
                {
                    this.SymbolicText = StringPlaceholdersResolver.Resolve(this.SymbolicText, this);
                    this.onLanguageChangedDelegate = new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler);
                    base.Connector.RegisterLanguageChangedHandler(new string[]
					{
						this.SymbolicText
					}, this.onLanguageChangedDelegate);
                }
                catch (Exception ex)
                {
                    base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
                }
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && base.Connector != null && this.onLanguageChangedDelegate != null)
            {
                base.Connector.UnregisterLanguageChangedHandler(null, this.onLanguageChangedDelegate);
            }

            base.Dispose(disposing);
        }
    }
}
