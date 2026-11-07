// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Infrastructure.Presentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 导航目标服务实现类
    /// </summary>
    public class NavigationTargetService : INavigationTargetService
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly SynchronizedObservableCollection<INavigationTargetViewModel> _navigationTargets = new SynchronizedObservableCollection<INavigationTargetViewModel>(null, null);
        public IObservableCollection<INavigationTargetViewModel> NavigationTargets
        {
            get
            {
                return _navigationTargets;
            }
        }
        public void UpdateNavigationTargets(IEnumerable<IExplorerNodeViewModel> addedNodes, IEnumerable<IExplorerNodeViewModel> removedNodes)
        {
            if (addedNodes != null)
            {
                AddNavigationTargets(addedNodes);
            }
            if (removedNodes != null)
            {
                RemoveNavigationTargets(removedNodes);
            }
        }
        public NavigationTargetService(IServiceProvider container)
        {
            _eventAggregator = container.Resolve<IEventAggregator>();

            var @event = _eventAggregator.GetEvent<NewProjectEvent>();
            if (@event != null)
            {
                @event.Subscribe(new Action<object>(NewProjectChangedHandler), ThreadOption.PublisherThread);
            }
        }

        public void NewProjectChangedHandler(object obj)
        {
            _navigationTargets.Clear();
        }
        private void AddNavigationTargets(IEnumerable<IExplorerNodeViewModel> nodes)
        {
            IEnumerable<INavigationTargetViewModel> items = nodes.OfType<INavigationTargetViewModel>().Where(IsNotRegistered);
            NavigationTargets.AddRange(items);
        }
        private void RemoveNavigationTargets(IEnumerable<IExplorerNodeViewModel> nodes)
        {
            IEnumerable<INavigationTargetViewModel> items = nodes.OfType<INavigationTargetViewModel>().Where(IsRegistered);
            NavigationTargets.RemoveRange(items);
        }
        
        private bool IsNotRegistered(INavigationTargetViewModel navigationTarget)
        {
            return !NavigationTargets.Contains(navigationTarget);
        }
        
        private bool IsRegistered(INavigationTargetViewModel navigationTarget)
        {
            return NavigationTargets.Contains(navigationTarget);
        }
    }
}
