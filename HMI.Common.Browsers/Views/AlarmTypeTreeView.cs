// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Telerik.Windows.Controls;
using HMI.Common.Browsers.ViewModels;
namespace HMI.Common.Browsers.Views
{
    [ExcludeFromCodeCoverage, TemplatePart(Name = "uxAlarmTypesTree", Type = typeof(RadTreeView))]
    public class AlarmTypeTreeView : BrowserView<IServerAlarmTypeTreeBrowserViewModel>
    {
        public const string AlarmTypesTreeControlName = "uxAlarmTypesTree";
        private RadTreeView uxAlarmTypes;
        static AlarmTypeTreeView()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AlarmTypeTreeView), new FrameworkPropertyMetadata(typeof(AlarmTypeTreeView)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.uxAlarmTypes = (base.Template.FindName("uxAlarmTypesTree", this) as RadTreeView);
            base.HookDoubleClickRowHandlers(this.uxAlarmTypes);
        }
    }
}
