// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Telerik.Windows.Controls;
using HMI.Common.Browsers.ViewModels;
namespace HMI.Common.Browsers.Views
{
    [ExcludeFromCodeCoverage, TemplatePart(Name = "uxProjectAuthorizations", Type = typeof(RadGridView))]
    public class ProjectAuthorizationsView : BrowserView<IServerProjectAuthorizationBrowserViewModel>
    {
        private const string ProjectAuthorizationsControlName = "uxProjectAuthorizations";
        private RadGridView uxProjectAuthorizations;
        static ProjectAuthorizationsView()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ProjectAuthorizationsView), new FrameworkPropertyMetadata(typeof(ProjectAuthorizationsView)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.uxProjectAuthorizations = (base.Template.FindName(ProjectAuthorizationsControlName, this) as RadGridView);
            base.HookGridActionHandlers(this.uxProjectAuthorizations);
            foreach (GridViewColumn current in this.uxProjectAuthorizations.Columns)
            {
                current.Width = new GridViewLength(1.0, GridViewLengthUnitType.Star);
            }
            base.InitializeGridSettings(this.uxProjectAuthorizations, "ProjectAuthorizationsGrid");
        }
    }
}
