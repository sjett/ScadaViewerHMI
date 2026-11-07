// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Common.Browsers.ViewModels
{
	public interface ICultureViewModel
	{
		int ID
		{
			get;
			set;
		}
		string Name
		{
			get;
			set;
		}
		bool IsVisible
		{
			get;
			set;
		}
	}
}
