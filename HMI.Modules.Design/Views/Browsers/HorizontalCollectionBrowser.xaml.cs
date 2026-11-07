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
using HMI.Common.Browsers.Views;
using HMI.Modules.Design.ViewModels.Browsers;

namespace HMI.Modules.Design.Views.Browsers
{
    /// <summary>
    /// HorizontalCollectionBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class HorizontalCollectionBrowser : BrowserWindowInstance
    {
        public HorizontalCollectionBrowser()
        {
            InitializeComponent();
        }
        public HorizontalCollectionBrowser(ICollectionBrowserViewModel viewModel)
            : base(viewModel)
		{
			this.InitializeComponent();
		}
    }
}
