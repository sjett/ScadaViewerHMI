// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    ///  可观察的集合组
    /// </summary>
    public class ObservableCollectionGroup : FullyObservableCollection<IExplorerNodeViewModel>, IEnumerable<IExplorerNodeViewModel>, INotifyPropertyChanged, IObservableCollection<IExplorerNodeViewModel>, IList<IExplorerNodeViewModel>, ICollection<IExplorerNodeViewModel>, IEnumerable, IDisposable, IObservableCollectionGroup, INotifyCollectionChanged
    {
        private readonly Func<IExplorerNodeViewModel, bool> _filter;
        private readonly Dictionary<IExplorerNodeViewModel, bool> _groupItems = new Dictionary<IExplorerNodeViewModel, bool>();
        private bool _isDisposed;

        public ObservableCollectionGroup(IExplorerNodeViewModel root, Func<IExplorerNodeViewModel, bool> filter)
        {
            Contract.Requires<ArgumentNullException>(root != null, "root");
            Contract.Requires<ArgumentNullException>(filter != null, "filter");

            this._filter = filter;

            AddGroupItem(root, false);
        }
        public void AddNonGroupItem(IExplorerNodeViewModel root)
        {
            AddGroupItem(root, true);
        }

        private void AddGroupItem(IExplorerNodeViewModel root, bool isStandaloneItem)
        {
            _groupItems[root] = isStandaloneItem;

            if (!isStandaloneItem)
            {
                root.Children.CollectionChanged += OnCollectionChanged;

                AddItems(root.Children);// 包括子节点

                return;
            }

            AddItems(
            [
                root
            ]);
        }
        private void AddItems(IEnumerable<IExplorerNodeViewModel> children)
        {
            foreach (IExplorerNodeViewModel child in children)
            {
                AddItem(child);
            }
        }

        public void RemoveNonGroupItem(IExplorerNodeViewModel item)
        {
            bool isStandaloneItem;
            _groupItems.TryGetValue(item, out isStandaloneItem);

            RemoveGroupItem(item, isStandaloneItem);
        }
        private void RemoveGroupItem(IExplorerNodeViewModel item, bool isStandaloneItem)
        {
            if (isStandaloneItem)
            {
                RemoveOldItems(
                [
                    item
                ]);
            }
            else
            {
                RemoveOldItems(item.Children);
                item.Children.CollectionChanged -= OnCollectionChanged;
            }

            _groupItems.Remove(item);
        }

        private IEnumerable<IExplorerNodeViewModel> GetItemsByFilter(IEnumerable<IExplorerNodeViewModel> items)
        {
            foreach(var item in items.Where(_filter))
            {
                yield return item;

                foreach (var child in GetItemsByFilter(item.Children))
                {
                    yield return child;
                }

            }
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            while (_groupItems.Count > 0)
            {
                RemoveNonGroupItem(_groupItems.Keys.First());
            }

            foreach (IExplorerNodeViewModel current in this)
            {
                current.Children.CollectionChanged -= OnCollectionChanged;
            }

            base.Clear();
        }
        private void AddItem(IExplorerNodeViewModel item)
        {
            base.Add(item);
            item.Children.CollectionChanged += OnCollectionChanged;
        }
        private void RemoveItem(IExplorerNodeViewModel item)
        {
            item.Children.CollectionChanged -= OnCollectionChanged;
            base.Remove(item);
        }
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IList newItems = e.NewItems;
            IList oldItems = e.OldItems;
            if (newItems != null)
            {
                AddNewItems(newItems.OfType<IExplorerNodeViewModel>());
            }
            if (oldItems != null)
            {
                RemoveOldItems(oldItems.OfType<IExplorerNodeViewModel>());
            }
        }
        private void AddNewItems(IEnumerable<IExplorerNodeViewModel> newItems)
        {
            foreach (IExplorerNodeViewModel item in GetItemsByFilter(newItems))
            {
                AddItem(item);
            }
        }
        private void RemoveOldItems(IEnumerable<IExplorerNodeViewModel> oldItems)
        {
            foreach (IExplorerNodeViewModel item in GetItemsByFilter(oldItems))
            {
                RemoveItem(item);
            }

        }

    }
}
