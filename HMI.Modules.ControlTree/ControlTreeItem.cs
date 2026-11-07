// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.ObjectModel;
using HMI.Common.Presentation.Events;
using HMI.Modules.ControlTree.Resources;

namespace HMI.Modules.ControlTree
{
	public class ControlTreeItem : PropertyChangedNotifier
	{
		private string _name;
		private readonly ObservableCollection<ControlTreeItem> _children = new ObservableCollection<ControlTreeItem>();
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
                _name = value;
				base.RaisePropertyChanged<string>(() => Name);
			}
		}
		public string DisplayName
		{
			get
			{
				return CategoryResource.ResourceManager.GetString(this.Name) ?? this.Name;
			}
		}

		public ObservableCollection<ControlTreeItem> Children
		{
			get
			{
				return _children;
			}
		}
		public ControlTreeItem()
		{
		}
		public ControlTreeItem(string name)
		{
            Name = name;
		}
		public void AddChild(ControlTreeItem child)
		{
			if (child == null)
			{
				throw new ArgumentNullException("category", "category is null.");
			}
            Children.Add(child);
		}
		public bool RemoveChild(ControlTreeItem child)
		{
			if (child == null)
			{
				throw new ArgumentNullException("category", "category is null.");
			}
			return Children.Remove(child);
		}
		public ControlCategory ToControlCategory()
		{
			throw new NotImplementedException();
		}
	}
}
