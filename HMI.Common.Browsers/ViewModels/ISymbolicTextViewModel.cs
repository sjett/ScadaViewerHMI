// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
namespace HMI.Common.Browsers.ViewModels
{
	public interface ISymbolicTextViewModel : IBrowserItemViewModel
	{
		Guid ID
		{
			get;
			set;
		}
		string SymbolicTextName
		{
			get;
			set;
		}
		IDictionary<int, string> Translations
		{
			get;
		}
	}
}
