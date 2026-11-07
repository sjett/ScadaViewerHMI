// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Telerik.Windows.Controls;
using HMI.Common.Browsers.ViewModels;
namespace HMI.Common.Browsers.Views
{
    [ExcludeFromCodeCoverage, TemplatePart(Name = "uxAlarmGroupsTree", Type = typeof(RadTreeView))]
    public class AlarmGroupTreeView : BrowserView<IServerAlarmGroupTreeBrowserViewModel>
    {
        private const string AlarmGroupsTreeControlName = "uxAlarmGroupsTree";
        private RadTreeView uxAlarmGroups;
        static AlarmGroupTreeView()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AlarmGroupTreeView), new FrameworkPropertyMetadata(typeof(AlarmGroupTreeView)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.uxAlarmGroups = (base.Template.FindName("uxAlarmGroupsTree", this) as RadTreeView);
            base.HookDoubleClickRowHandlers(this.uxAlarmGroups);
        }
    }
}
