// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Infrastructure.Services;
using System.Collections.Specialized;
using HMI.Common.Instrumentation.ErrorHandling;

namespace HMI.Infrastructure.Presentation.Services.UndoRedo
{

    /// <summary>
    /// 集合监视器实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionMonitor<T> : ICollectionMonitor<T>
    {
        private readonly IActionMonitor actionMonitor;
        private IObservableCollection<T> observableCollection;
        /// <summary>
        /// 集合监视器构造函数
        /// </summary>
        /// <param name="actionMonitor"></param>
        public CollectionMonitor(IActionMonitor actionMonitor)
        {
            Contract.Requires<ArgumentNullException>(actionMonitor != null, "actionMonitor");
            this.actionMonitor = actionMonitor!;
        }
        /// <summary>
        /// 开始监视
        /// </summary>
        /// <param name="target"></param>
        public void Start(IObservableCollection<T> target)
        {
            Contract.Requires<ArgumentNullException>(target != null, "target");
            Contract.Requires<InvalidOperationException>(this.observableCollection == null, "target");
            this.observableCollection = target!;
            target.CollectionChanged += new NotifyCollectionChangedEventHandler(this.onCollectionChanged);
        }
        /// <summary>
        /// 停止监视
        /// </summary>
        public void Stop()
        {
            Contract.Requires<InvalidOperationException>(this.observableCollection != null, "target");
            this.observableCollection!.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.onCollectionChanged);
            this.observableCollection = null;
        }

        private void onCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            IAction action = this.getAction(e);
            this.actionMonitor.RecordAction(action);
        }
        private IAction getAction(NotifyCollectionChangedEventArgs e)
        {
            IAction result = null;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    result = new CollectionItemsAddedAction<T>(this.observableCollection, e.OldStartingIndex, e.NewStartingIndex, e.NewItems!.Cast<T>().ToArray<T>());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    result = new CollectionItemsRemovedAction<T>(this.observableCollection, e.OldStartingIndex, e.NewStartingIndex, e.OldItems!.Cast<T>().ToArray<T>());
                    break;
                case NotifyCollectionChangedAction.Replace:
                    result = new CollectionItemsReplacedAction<T>(this.observableCollection, e.OldStartingIndex, e.NewStartingIndex, e.NewItems!.Cast<T>().ToArray<T>(), e.OldItems!.Cast<T>().ToArray<T>());
                    break;
                case NotifyCollectionChangedAction.Move:
                    result = new CollectionItemsMovedAction<T>(this.observableCollection, e.OldStartingIndex, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    result = new CollectionItemsResetAction<T>(this.observableCollection, e.OldStartingIndex, e.NewStartingIndex, (e.OldItems != null) ? e.OldItems.Cast<T>().ToArray<T>() : null);
                    break;
            }
            return result;
        }
    }
}
