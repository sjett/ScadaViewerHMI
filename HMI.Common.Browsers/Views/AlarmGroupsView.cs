// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Telerik.Windows.Controls;
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Presentation.Views;
namespace HMI.Common.Browsers.Views
{
    [ExcludeFromCodeCoverage, TemplatePart(Name = "uxAlarmGroups", Type = typeof(RadGridView))]
    public class AlarmGroupsView : BrowserView<IServerAlarmGroupBrowserViewModel>
    {
        private const string AlarmGroupsControlName = "uxAlarmGroups";
        private RadGridView uxAlarmGroups;
        static AlarmGroupsView()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AlarmGroupsView), new FrameworkPropertyMetadata(typeof(AlarmGroupsView)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.uxAlarmGroups = this.FindTemplateChild<RadGridView>("uxAlarmGroups");
            base.HookGridActionHandlers(this.uxAlarmGroups);
            foreach (GridViewColumn current in this.uxAlarmGroups.Columns)
            {
                current.Width = new GridViewLength(1.0, GridViewLengthUnitType.Star);
            }
            base.InitializeGridSettings(this.uxAlarmGroups, "AlarmGroupsGrid");
        }
    }
}
