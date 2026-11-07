// SPDX-License-Identifier: GPL-3.0-only
using System.Windows.Input;

namespace HMI.Common.Browsers.ViewModels
{
	public interface ISelectorBrowserViewModel : IBrowserViewModel
	{
		ICommand RefreshCommand
		{
			get;
		}
		string SelectedValue
		{
			get;
			set;
		}
		IList<ISelectorBrowserPageViewModel> Pages
		{
			get;
		}
		ISelectorBrowserPageViewModel ActivePage
		{
			get;
			set;
		}
		bool AreMultiplePagesVisible
		{
			get;
		}
	}
}
