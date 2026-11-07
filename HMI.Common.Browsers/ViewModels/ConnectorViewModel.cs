// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;

namespace HMI.Common.Browsers.ViewModels
{
	public class ConnectorViewModel : FullyObservableCollection<ISignalGroupViewModel>,  IConnectorViewModel
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
		public ConnectorViewModel(IHierarchicalFilterViewModel parent, string name, string description)
		{
            this.parent = parent;
            Name = name;
            Description = description;
		}
		public bool IsMatchingFilter(ISignalDefinitionViewModel item)
		{
			return parent.IsMatchingFilter(item) && item.Connector == Name;
		}
		public ISignalGroupViewModel AddSignalGroup(string name, string description)
		{
			var signalGroupViewModel = this.SingleOrDefault(t=> t.Name == name);
			if (signalGroupViewModel == null)
			{
				signalGroupViewModel = new SignalGroupViewModel(this, name, description);
				base.Add(signalGroupViewModel);
			}
			return signalGroupViewModel;
		}
		public override string ToString()
		{
			return string.Format("{0}/{1}", parent, Name);
		}
		public override bool Equals(object? obj)
		{
			return AreEqual(obj as ConnectorViewModel);
		}
		private bool AreEqual(ConnectorViewModel? other)
		{
			return other != null && other.Name == Name && parent.Equals(other.parent);
		}
		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ parent.GetHashCode();
		}
	}
}
