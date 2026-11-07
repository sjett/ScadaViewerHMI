// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Presentation.ViewModels;
using System.ComponentModel;

namespace HMI.Modules.Design.ViewModels.Browsers
{
    public class SelectorBrowserViewModel<T> : SelectorBrowserViewModel, ISelectorBrowserViewModel<T>, ISelectorBrowserViewModel, IBrowserViewModel, IDisposable, IViewModel, INotifyPropertyChanged where T : ISelectorBrowserPageViewModel
    {
        public SelectorBrowserViewModel(T page)
        {
            base.Pages.Add(page);
        }
    }
}
