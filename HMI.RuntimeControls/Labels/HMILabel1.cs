// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using HMI.Runtime.Core;
using HMI.Runtime.Core.Helper.Extensions;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;

namespace HMI.RuntimeControls.Labels
{
    [TemplatePart(Name = "OutputElement", Type = typeof(TextBlock))]
    public class HMILabel1 : HMIStaticStatesControlBase
    {
        private const string ElementOutput = "OutputElement";
        private bool isInitialized;
        private TextBlock tbOutputElement;

        public static readonly DependencyProperty SymbolicTextProperty = DependencyProperty.Register("SymbolicText", typeof(string), typeof(HMILabel1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnSymbolicTextPropertyChanged)));
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignments), typeof(HMILabel1), new PropertyMetadata(TextAlignments.Center));
        
        /// <summary>
        /// 编码
        /// </summary>
        [Category("HMI")]
        public string SymbolicText
        {
            get
            {
                return (string)base.GetValue(SymbolicTextProperty);
            }
            set
            {
                base.SetValue(SymbolicTextProperty, value);
            }
        }
        [Category("HMI")]
        public TextAlignments TextAlignment
        {
            get
            {
                return (TextAlignments)base.GetValue(TextAlignmentProperty);
            }
            set
            {
                base.SetValue(TextAlignmentProperty, value);
            }
        }
        private static void OnSymbolicTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HMILabel1 wFLabel = d as HMILabel1;
            if (wFLabel != null)
            {
                if (e.NewValue != e.OldValue)
                {
                    wFLabel.UpdateOutputElementText();
                    wFLabel.RegisterToLanguageChanged();
                    return;
                }
            }
        }
        public HMILabel1()
        {
            base.DefaultStyleKey=typeof(HMILabel1);
        }
        private void OnSymbolicTextTranslationsChangedHandler(string[] symbolicTexts, string[] translationTexts)
        {
            if (tbOutputElement == null)
            {
                return;
            }
            try
            {
                if (translationTexts[0] != null && !translationTexts[0].Equals("!null!", StringComparison.InvariantCultureIgnoreCase))
                {
                    tbOutputElement.Text=translationTexts[0];
                }
                else
                {
                    tbOutputElement.Text=symbolicTexts[0];
                }
            }
            catch (Exception ex)
            {
                base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
            }
        }
        public override void OnApplyTemplate()
        {
            tbOutputElement = null;
            base.OnApplyTemplate();
            base.IsTabStop=false;
            tbOutputElement = base.GetTemplateChild("OutputElement") as TextBlock;
            UpdateOutputElementText();
            if (!base.IsInDesignMode && !isInitialized)
            {
                isInitialized = true;
                ResolveSymbolicTextPlaceholders();
                UpdateOutputElementText();
                RegisterToLanguageChanged();
                return;
            }
        }
        private void ResolveSymbolicTextPlaceholders()
        {
            if (string.IsNullOrEmpty(SymbolicText))
            {
                return;
            }
            SymbolicText = StringPlaceholdersResolver.Resolve(SymbolicText, this);
        }
        private void UpdateOutputElementText()
        {
            if (tbOutputElement == null)
            {
                return;
            }
            tbOutputElement.Text = SymbolicText ?? string.Empty;
        }
        private void RegisterToLanguageChanged()
        {
            if (!DesignerPropertiesExt.IsInDesignTool && base.Connector != null && tbOutputElement != null && !string.IsNullOrEmpty(SymbolicText))
            {
                base.Connector.RegisterLanguageChangedHandler(new string[]
				{
                    SymbolicText
                }, new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler));
                return;
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeConnectorLanguageChangedHandler();
            }
            base.Dispose(disposing);
        }
        private void DisposeConnectorLanguageChangedHandler()
        {
            if (base.Connector == null)
            {
                return;
            }
            base.Connector.UnregisterLanguageChangedHandler(null, new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler));
        }
    }
}
