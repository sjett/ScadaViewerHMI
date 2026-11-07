// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Telerik.Windows.Controls;
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Presentation.Views;
namespace HMI.Common.Browsers.Views
{
    [ExcludeFromCodeCoverage, TemplatePart(Name = "uxAlarmTypes", Type = typeof(RadGridView))]
    public class AlarmTypesView : BrowserView<IServerAlarmTypeBrowserViewModel>
    {
        private const string AlarmTypesControlName = "uxAlarmTypes";
        private RadGridView uxAlarmTypes;
        static AlarmTypesView()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AlarmTypesView), new FrameworkPropertyMetadata(typeof(AlarmTypesView)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.uxAlarmTypes = this.FindTemplateChild<RadGridView>("uxAlarmTypes");
            base.HookGridActionHandlers(this.uxAlarmTypes);
            foreach (GridViewColumn current in this.uxAlarmTypes.Columns)
            {
                current.Width = new GridViewLength(1.0, GridViewLengthUnitType.Star);
            }
            base.InitializeGridSettings(this.uxAlarmTypes, "AlarmTypesGrid");
        }
    }
}
