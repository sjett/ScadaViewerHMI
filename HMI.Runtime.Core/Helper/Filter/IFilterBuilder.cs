// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Core.Helper.Filter
{
	public interface IFilterBuilder
	{
		Predicate<object> CreateFilter(string text);
	}
}
