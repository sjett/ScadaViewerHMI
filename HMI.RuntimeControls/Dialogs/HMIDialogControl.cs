// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Xceed.Wpf.Toolkit;

namespace HMI.RuntimeControls.Dialogs
{
    public class HMIDialogControl : ChildWindow
    {
        public static readonly DependencyProperty TitleBackgroundProperty = DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(HMIDialogControl), new PropertyMetadata(null));
        public static readonly DependencyProperty TitleForegroundProperty = DependencyProperty.Register("TitleForeground", typeof(Brush), typeof(HMIDialogControl), new PropertyMetadata(null));
        public Brush TitleBackground
        {
            get
            {
                return (Brush)base.GetValue(HMIDialogControl.TitleBackgroundProperty);
            }
            set
            {
                base.SetValue(HMIDialogControl.TitleBackgroundProperty, value);
            }
        }
        public Brush TitleForeground
        {
            get
            {
                return (Brush)base.GetValue(HMIDialogControl.TitleForegroundProperty);
            }
            set
            {
                base.SetValue(HMIDialogControl.TitleForegroundProperty, value);
            }
        }

        public HMIDialogControl()
        {
            this.DefaultStyleKey = typeof(HMIDialogControl);
        }
    }
}
