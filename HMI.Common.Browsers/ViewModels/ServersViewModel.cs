// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.Resources;
using HMI.Common.Collections;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
namespace HMI.Common.Browsers.ViewModels
{
	internal class ServersViewModel : FullyObservableCollection<IServerViewModel>, INotifyPropertyChanged, IObservableCollection<IServerViewModel>, IList<IServerViewModel>, ICollection<IServerViewModel>, IEnumerable<IServerViewModel>, IEnumerable, INotifyCollectionChanged, IHierarchicalFilterViewModel, IServersViewModel
	{
		
		public string Name
		{
			get
			{
				return BrowserTexts.Servers;
			}
		}
		public string Description
		{
			get
			{
				return string.Empty;
			}
		}
		public IServerViewModel AddServer(string name, string description)
		{
			IServerViewModel serverViewModel = this.SingleOrDefault(server => server.Name == name);
			if (serverViewModel == null)
			{
				serverViewModel = new ServerViewModel(this, name, description);
				base.Add(serverViewModel);
			}
			return serverViewModel;
		}
		public bool IsMatchingFilter(ISignalDefinitionViewModel item)
		{
			return true;
		}
		public override string ToString()
		{
			return Name;
		}
		public override bool Equals(object obj)
		{
			return AreEqual(obj as ServersViewModel);
		}
		private bool AreEqual(ServersViewModel other)
		{
			return other != null && other.Name == Name;
		}
		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}
