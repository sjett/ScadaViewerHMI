// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.ObjectModel;
namespace HMI.Runtime.Shared
{
	public interface IHMINavigationParameter
	{
		ObservableCollection<ParameterItem> Parameters
		{
			get;
		}
	}
}
