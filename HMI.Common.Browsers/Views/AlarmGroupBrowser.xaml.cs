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
using System.Windows.Shapes;

using HMI.Common.Browsers.ViewModels;

namespace HMI.Common.Browsers.Views
{
    /// <summary>
    /// AlarmGroupBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmGroupBrowser : SelectorBrowserWindow
    {
        public AlarmGroupBrowser()
        {
            InitializeComponent();
        }
        public AlarmGroupBrowser(IAlarmGroupBrowserViewModel viewModel)
            : base(viewModel)
        {
            this.InitializeComponent();
        }
    }
}
