// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.Events;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace HMI.Common.Browsers.ViewModels
{
	public class AlarmTreeItem : PropertyChangedNotifier
	{
		private Guid _id;
		private bool? _isSelected = new bool?(false);
        private AlarmTreeItem _parent;
		private string _shortName;
		private string _symbolicTextName;
		private string _translatedShortName;

		public event EventHandler? SelectionChanged;

		public ObservableCollection<AlarmTreeItem> Children
		{
			get;
			private set;
		}
		public string FullName
		{
			get
			{
				if (this.Parent != null)
				{
					string fullName = this.Parent.FullName;
					string shortName = this.ShortName;
					return CombineNameSegments(fullName, shortName);
				}
				return this.ShortName;
			}
		}
		public string TranslatedFullName
		{
			get
			{
				if (this.Parent != null)
				{
					string translatedFullName = this._parent.TranslatedFullName;
					string translatedShortName = this.TranslatedShortName;
					return CombineNameSegments(translatedFullName, translatedShortName);
				}
				return this.TranslatedShortName;
			}
		}
		public Guid Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
				base.RaisePropertyChanged<Guid>(() => this.Id);
			}
		}
		public bool HasChildren
		{
			get
			{
				return this.Children.Any<AlarmTreeItem>();
			}
		}
		public bool? IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				UpdateSelectionState(this, value, updateChildren: true, updateParent: true);
				this.RootItem.OnSelectionChanged();
			}
		}
		private AlarmTreeItem RootItem
		{
			get
			{
				if (this.Parent != null)
				{
					return this.Parent.RootItem;
				}
				return this;
			}
		}
		public AlarmTreeItem Parent
		{
			get
			{
				return this._parent;
			}
			set
			{
				this._parent = value;
				base.RaisePropertyChanged<AlarmTreeItem>(() => this.Parent);
				base.RaisePropertyChanged<string>(() => this.FullName);
			}
		}
		public string ShortName
		{
			get
			{
				return this._shortName;
			}
			set
			{
				this._shortName = value;
				base.RaisePropertyChanged<string>(() => this.ShortName);
				base.RaisePropertyChanged<string>(() => this.FullName);
			}
		}
		public string TranslatedShortName
		{
			get
			{
				return this._translatedShortName;
			}
			set
			{
				this._translatedShortName = value;
				base.RaisePropertyChanged<string>(() => this.TranslatedShortName);
			}
		}
		public string SymbolicTextName
		{
			get
			{
				return this._symbolicTextName;
			}
			set
			{
				this._symbolicTextName = value;
				base.RaisePropertyChanged<string>(() => this.SymbolicTextName);
			}
		}
		public bool AreAllChildrenSelected
		{
			get
			{
				return this.HasChildren && this.IsSelected.HasValue && this.IsSelected.Value;
			}
		}
		public bool AreAllChildrenUnselected
		{
			get
			{
				return this.HasChildren && this.IsSelected.HasValue && !this.IsSelected.Value;
			}
		}
		public AlarmTreeItem(Guid id, string shortName, string translatedShortName, string symbolicTextName)
		{
			this._id = id;
			this._shortName = shortName;
			this._translatedShortName = translatedShortName;
			this._symbolicTextName = symbolicTextName;

			this.Children = new ObservableCollection<AlarmTreeItem>();
			this.Children.CollectionChanged += new NotifyCollectionChangedEventHandler(this.onCollectionChanged);
		}
		public override bool Equals(object obj)
		{
			var alarmTreeItem = obj as AlarmTreeItem;
			return alarmTreeItem != null && alarmTreeItem._id == this._id;
		}
		public override int GetHashCode()
		{
			return this._id.GetHashCode();
		}
		public IEnumerable<string> GetSelectedNames()
		{
            if (this.HasChildren)
            {
                return this.Children.Where(w => w.IsSelected == true).Select(p => p.ShortName);
            }
            else
            {
                return Enumerable.Empty<string>();
            }
		}
		public virtual void SelectNames(string groupNamesText)
		{
			string[] array = groupNamesText.Split('.');
			if (array.Length > 0)
			{
				if (array[0] == "*")
				{
					this.IsSelected = new bool?(true);//所有子节点全选
                    return;
				}

				SelectChildNames(array, groupNamesText);
			}
		}
        private void SelectChildNames(string[] nameSegments, string remainingNames)
        {
            var child = this.Children.FirstOrDefault(item => item.ShortName == nameSegments[0]);
            if (child != null)
            {
                if (nameSegments.Length != 1)
                {
                    string childNames = remainingNames.Substring(nameSegments[0].Length + 1);
                    child.SelectNames(childNames);
                    return;
                }
                if (!child.HasChildren)
                {
                    child.IsSelected = new bool?(true);
                }
            }
        }
        private void onCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.RaisePropertyChanged<bool>(() => this.HasChildren);
		}
		protected virtual void OnSelectionChanged()
		{
			SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
		public void AddItem(Guid itemId, string name, string translatedName)
		{
			if (string.IsNullOrEmpty(name))
			{
				return;
			}

			string[] nameSegments = name.Split('.');
			string[] translatedNameSegments = translatedName.Split('.');

			if (nameSegments.Length != translatedNameSegments.Length)
			{
				return;
			}
			AlarmTreeItem alarmTreeItem = this;
			for (int i = 0; i < nameSegments.Length; i++)
			{
				alarmTreeItem = AddAlarmTreeItem(alarmTreeItem, itemId, name, nameSegments[i], translatedNameSegments[i]);
			}
		}
        private AlarmTreeItem AddAlarmTreeItem(AlarmTreeItem parentItem, Guid itemId, string symbolicTextName, string shortName, string translatedName)
        {
            return parentItem.Children.FirstOrDefault(t => t.ShortName == shortName) ?? CreateAlarmTreeItem(itemId, translatedName, shortName, symbolicTextName, parentItem);
        }
        private AlarmTreeItem CreateAlarmTreeItem(Guid itemId, string translatedName, string shortName, string symbolicTextName, AlarmTreeItem parent)
        {
            AlarmTreeItem alarmTreeItem = new AlarmTreeItem(itemId, shortName, translatedName, symbolicTextName)
            {
                Parent = parent
            };
            parent.Children.Add(alarmTreeItem);
            return alarmTreeItem;
        }

        internal static IEnumerable<string> GetSelectedNames(AlarmTreeItem alarmTreeItem)
		{
			return alarmTreeItem.GetSelectedNames();
		}
        private static string CombineNameSegments(string parentName, string childName)
        {
            string combinedName = string.Format("{0}.{1}", parentName, childName);
            if (combinedName.StartsWith("*.", StringComparison.Ordinal))
            {
                combinedName = combinedName.Substring(2);
            }
            return combinedName;
        }
        private static void UpdateSelectionState(AlarmTreeItem item, bool? isSelected, bool updateChildren, bool updateParent)
        {
            if (isSelected == item._isSelected)
            {
                return;
            }
            item._isSelected = isSelected;

            if (updateChildren && item._isSelected.HasValue)
            {
                foreach (AlarmTreeItem child in item.Children)
                {
                    UpdateSelectionState(child, item._isSelected, updateChildren: true, updateParent: false);
                }
            }

            if (updateParent && item._parent != null)
            {
                UpdateParentSelectionState(item._parent);
            }

            item.RaisePropertyChanged<bool?>(() => item.IsSelected);
        }
        private static void UpdateParentSelectionState(AlarmTreeItem parent)
        {
            bool? flag = null;
            for (int i = 0; i < parent.Children.Count; i++)
            {
                bool? childSelectionState = parent.Children[i].IsSelected;
                if (i == 0)
                {
                    flag = childSelectionState;
                }
                else
                {
                    if (flag != childSelectionState)
                    {
                        flag = null;
                        break;
                    }
                }
            }
            UpdateSelectionState(parent, flag, updateChildren: false, updateParent: true);
        }
    }
}
