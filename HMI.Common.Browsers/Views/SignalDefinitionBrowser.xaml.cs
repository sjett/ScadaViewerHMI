// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using HMI.Common.Instrumentation.ErrorHandling;
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

using HMI.Common.Browsers.Resources;
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Presentation.Events.Interactions;

namespace HMI.Common.Browsers.Views
{
    /// <summary>
    /// SignalDefinitionBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class SignalDefinitionBrowser : SelectorBrowserWindow
    {
        private readonly IEventAggregator eventAggregator;

        public SignalDefinitionBrowser()
        {
            InitializeComponent();
        }
        public SignalDefinitionBrowser(ISignalDefinitionBrowserViewModel viewModel, IEventAggregator eventAggregator)
            : base(viewModel)
        {
            Contract.Requires<ArgumentNullException>(eventAggregator != null, "eventAggregator");
            this.eventAggregator = eventAggregator;
            this.InitializeComponent();
            viewModel.Error += new EventHandler<HMI.Common.Browsers.ViewModels.ExceptionEventArgs>(this.ViewModel_Error);
        }
        private void ViewModel_Error(object sender, HMI.Common.Browsers.ViewModels.ExceptionEventArgs e)
        {
            this.eventAggregator.PublishNotification(NotificationSeverity.Error, ErrorTexts.ErrorGettingSymbolicTexts, e.Exception.Message, e.Exception);
        }
    }
}
