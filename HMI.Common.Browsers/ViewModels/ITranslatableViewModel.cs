// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Common.Browsers.ViewModels
{
	public interface ITranslatableViewModel : IBrowserItemViewModel
	{
		Guid Id
		{
			get;
			set;
		}
		string Name
		{
			get;
			set;
		}
		string Translation
		{
			get;
			set;
		}
	}
}
