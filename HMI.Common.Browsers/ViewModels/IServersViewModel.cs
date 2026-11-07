// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
namespace HMI.Common.Browsers.ViewModels
{
	public interface IServersViewModel : INotifyPropertyChanged, IObservableCollection<IServerViewModel>, IList<IServerViewModel>, ICollection<IServerViewModel>, IEnumerable<IServerViewModel>, IEnumerable, INotifyCollectionChanged, IHierarchicalFilterViewModel
	{
		IServerViewModel AddServer(string name, string description);
	}
}
