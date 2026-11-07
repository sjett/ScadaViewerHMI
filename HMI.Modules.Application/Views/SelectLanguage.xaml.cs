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

using Telerik.Windows.Controls;
using HMI.Modules.Application.Viewmodels;

namespace HMI.Modules.Application.Views
{
    /// <summary>
    /// SelectLanguage.xaml 的交互逻辑
    /// </summary>
    public partial class SelectLanguage : RadWindow
    {
        public SelectLanguage()
        {
            InitializeComponent();

            ViewModel = new SelectLanguageViewModel();
        }

        public ISelectLanguageViewModel ViewModel
        {
            get
            {
                return base.DataContext as ISelectLanguageViewModel;
            }
            set
            {
                base.DataContext = value;
            }
        }

        private void radButton_0_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
        private void radButton_1_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
