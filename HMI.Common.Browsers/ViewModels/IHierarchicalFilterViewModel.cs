// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Common.Browsers.ViewModels
{
	public interface IHierarchicalFilterViewModel
	{
		string Name
		{
			get;
		}
		string Description
		{
			get;
		}
		bool IsMatchingFilter(ISignalDefinitionViewModel item);
	}
}
