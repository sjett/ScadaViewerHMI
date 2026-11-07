// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;

namespace HMI.Common.Browsers.ViewModels
{
	public interface IConnectorViewModel : IObservableCollection<ISignalGroupViewModel>,  IHierarchicalFilterViewModel
	{
		ISignalGroupViewModel AddSignalGroup(string name, string description);
	}
}
