// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace HMI.Common.Browsers.ViewModels
{
	public class ServerViewModel : FullyObservableCollection<IConnectorViewModel>, INotifyPropertyChanged, IObservableCollection<IConnectorViewModel>, IEnumerable, IList<IConnectorViewModel>, ICollection<IConnectorViewModel>, IEnumerable<IConnectorViewModel>, INotifyCollectionChanged, IHierarchicalFilterViewModel, IServerViewModel
	{
		private readonly IHierarchicalFilterViewModel parent;
		
		public string Name
		{
			get;
			private set;
		}
		public string Description
		{
			get;
			private set;
		}
		public ServerViewModel(IHierarchicalFilterViewModel parent, string name, string description)
		{
            this.parent = parent;
            Name = name;
            Description = description;
		}
		public bool IsMatchingFilter(ISignalDefinitionViewModel item)
		{
			return parent.IsMatchingFilter(item) && item.Server == Name;
		}
		public IConnectorViewModel AddConnector(string name, string description)
		{
			IConnectorViewModel connectorViewModel = this.SingleOrDefault(t=> t.Name == name);
			if (connectorViewModel == null)
			{
				connectorViewModel = new ConnectorViewModel(this, name, description);
				base.Add(connectorViewModel);
			}
			return connectorViewModel;
		}
		public override string ToString()
		{
			return string.Format("{0}/{1}", parent, Name);
		}
		public override bool Equals(object obj)
		{
			return AreEqual(obj as ServerViewModel);
		}
		private bool AreEqual(ServerViewModel other)
		{
			return other != null && other.Name == Name && parent.Equals(other.parent);
		}
		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ parent.GetHashCode();
		}
	}
}
