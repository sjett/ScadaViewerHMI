// SPDX-License-Identifier: GPL-3.0-only
using System.Windows.Controls;

namespace HMI.Common.Presentation.Views
{
    public static class ControlExtensions
    {
        public static T FindTemplateChild<T>(this Control control, string name) where T : class
        {
            return control.Template.FindName(name, control) as T;
        }
    }
}
