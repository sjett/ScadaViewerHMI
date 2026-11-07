// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.Events;
using HMI.Common.Presentation.ViewModels;
using HMI.Infrastructure.Presentation.Views;
using System.Collections.Specialized;
using System.ComponentModel;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    public class SelectionControlViewModel : ControlGroupContainerViewModel, ICommonControlViewModel, IExplorerNodeViewModel, IHierarchicalNode, INotifyPropertyChanged, IDisposable, INotifyProperty, INotifyPropertyChanging, IViewModel, ISignalPlaceholderResolver, ISelectionControlViewModel
	{
        public override bool ShowDragDecorator
		{
			get
			{
				return true;
			}
		}
        public override bool ShowResizeDecorator
		{
			get
			{
				return true;
			}
		}
        public override bool ShowRotationDecorator
		{
			get
			{
				return true;
			}
		}
        public override bool ShowSelectionDecorator
		{
			get
			{
				return true;
			}
		}
        /// <param name="e"></param>
        protected override void UpdateChildNames(NotifyCollectionChangedEventArgs e)
		{
		}
        /// <param name="container"></param>
        /// <param name="parentValue"></param>
        public SelectionControlViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue) 
            : base(container, parentValue, typeof(SelectionControl))
		{
            ChildrenMonitor.Stop();
            PropertyMonitor.Stop();
		}
        /// <returns></returns>
        public ICommonControlViewModel CreateGroupFromSelection()
		{
			return new ControlGroupContainerViewModel(this);
		}
	}
}
