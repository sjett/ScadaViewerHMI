// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace HMI.Common.Browsers.ViewModels
{
	public interface IServerViewModel : INotifyPropertyChanged, IObservableCollection<IConnectorViewModel>, IEnumerable, IList<IConnectorViewModel>, ICollection<IConnectorViewModel>, IEnumerable<IConnectorViewModel>, INotifyCollectionChanged, IHierarchicalFilterViewModel
	{
		IConnectorViewModel AddConnector(string name, string description);
	}
}
