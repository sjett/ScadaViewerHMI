// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.ViewModels;

namespace HMI.Common.Browsers.Views
{
    public class BrowserWindowInstance : BrowserWindow
    {
        public BrowserWindowInstance() { }

        public BrowserWindowInstance(IBrowserViewModel viewModel) : base(viewModel) { }
    }
}
