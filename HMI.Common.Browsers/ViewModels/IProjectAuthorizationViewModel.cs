// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Common.Browsers.ViewModels
{
	public interface IProjectAuthorizationViewModel : IBrowserItemViewModel
	{
		Guid ID
		{
			get;
			set;
		}
		string Name
		{
			get;
			set;
		}
	}
}
