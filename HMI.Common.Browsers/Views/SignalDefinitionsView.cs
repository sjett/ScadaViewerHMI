// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Telerik.Windows.Controls;
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Presentation.Views;
namespace HMI.Common.Browsers.Views
{
    [ExcludeFromCodeCoverage, TemplatePart(Name = "uxSignals", Type = typeof(RadGridView))]
    public class SignalDefinitionsView : BrowserView<IServerSignalDefinitionBrowserViewModel>
    {
        private const string SignalsGridName = "uxSignals";
        private RadGridView uxSignals;
        static SignalDefinitionsView()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(SignalDefinitionsView), new FrameworkPropertyMetadata(typeof(SignalDefinitionsView)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.uxSignals = this.FindTemplateChild<RadGridView>("uxSignals");
            base.HookGridActionHandlers(this.uxSignals);
            base.InitializeGridSettings(this.uxSignals, "SignalsGrid");
        }
    }
}
