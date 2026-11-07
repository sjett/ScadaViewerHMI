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
using HMI.Modules.Design.ViewModels.Browsers;
using HMI.Common.Browsers.Views;

namespace HMI.Modules.Design.Views.Browsers
{
    /// <summary>
    /// XamlBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class XamlBrowser : SelectorBrowserWindow
    {
        public XamlBrowser()
        {
            InitializeComponent();
        }

        public XamlBrowser(ISelectorBrowserViewModel<IMediaBrowserPageViewModel<IXamlSourceViewModel>> pageViewModel)
            : base(pageViewModel)
        {
            this.InitializeComponent();
        }
    }
}
