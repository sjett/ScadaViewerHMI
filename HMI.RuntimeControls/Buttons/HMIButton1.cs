// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
namespace HMI.RuntimeControls.Buttons
{
    public class HMIButton1 : HMIButtonBase
    {
        private bool inited;
        private OnLanguageChangedDelegate _languageChangedHandler;

        public static readonly DependencyProperty SymbolicTextProperty = DependencyProperty.Register("SymbolicText", typeof(string), typeof(HMIButton1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIButton1.OnSymbolicTextChanged)));
        
        [Category("HMI")]
        public string SymbolicText
        {
            get
            {
                return (string)base.GetValue(HMIButton1.SymbolicTextProperty);
            }
            set
            {
                base.SetValue(HMIButton1.SymbolicTextProperty, value);
            }
        }
        private static void OnSymbolicTextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            HMIButton1? button = dependencyObject as HMIButton1;
            if (button != null && DesignerProperties.GetIsInDesignMode(button))
            {
                button.Content = e.NewValue;
                return;
            }
        }
        public HMIButton1()
        {
            base.DefaultStyleKey=(typeof(HMIButton1));
        }
        ~HMIButton1()
        {
            this.Dispose(false);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                if (!string.IsNullOrEmpty(this.SymbolicText))
                {
                    base.Content=(this.SymbolicText);
                    return;
                }
            }
            else
            {
                if (this.inited)
                {
                    return;
                }
                this.inited = true;
                if (!string.IsNullOrEmpty(this.SymbolicText))
                {
                    this.SymbolicText = StringPlaceholdersResolver.Resolve(this.SymbolicText, this);
                    base.Content=(string.Empty);
                }
                if (!string.IsNullOrEmpty(this.SymbolicText))
                {
                    try
                    {
                        this._languageChangedHandler = new OnLanguageChangedDelegate(this.OnLanguageChanged);
                        base.Connector.RegisterLanguageChangedHandler(new string[]
						{
							this.SymbolicText
						}, this._languageChangedHandler);
                    }
                    catch (Exception ex)
                    {
                        base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
                    }
                }
            }
        }
        private void OnLanguageChanged(string[] symbolicNames, string[] translationTexts)
        {
            try
            {
                if (translationTexts[0] != null && !translationTexts[0].Equals("!null!", StringComparison.InvariantCultureIgnoreCase))
                {
                    base.Content = (translationTexts[0]);
                }
                else
                {
                    base.Content = (symbolicNames[0]);
                }
            }
            catch (Exception ex)
            {
                base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && base.Connector != null && this._languageChangedHandler != null)
            {
                base.Connector.UnregisterLanguageChangedHandler(null, this._languageChangedHandler);
            }
            base.Dispose(disposing);
        }
    }
}
