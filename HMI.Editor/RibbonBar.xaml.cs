// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HMI.Editor
{
    /// <summary>
    /// RibbonBar.xaml 的交互逻辑
    /// </summary>
    public partial class RibbonBar : UserControl
    {
        public bool IsMinimized
        {
            get
            {
                return this.uxRibbonBar.IsMinimized;
            }
            set
            {
                this.uxRibbonBar.IsMinimized=(value);
            }
        }
        public bool IsMinimizable
        {
            get
            {
                return this.uxRibbonBar.IsMinimizable;
            }
            set
            {
                this.uxRibbonBar.IsMinimizable=(value);
            }
        }

        public RibbonBar()
        {
            InitializeComponent();
        }
    }
}
