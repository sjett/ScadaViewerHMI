// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.Reactive;
using HMI.Infrastructure;
using HMI.Infrastructure.Presentation.Events;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Presentation.Views;
using HMI.Infrastructure.Services;
using HMI.Modules.Explorer.Resources;
using HMI.Modules.Explorer.ViewModels;
using NLog;
using System.Collections.ObjectModel;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Globalization;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.DragDrop;

namespace HMI.Modules.Explorer.Views
{
    /// <summary>
    /// ExplorerView.xaml 的交互逻辑
    /// </summary>
    public partial class ExplorerView : UserControl, IExplorerView
    {
        private readonly Subject<IExplorerNodeViewModel> bringSelectionIntoViewObserver;
        private readonly IEventAggregator eventAggregator;
        private readonly INotificationService notificationService;
        private readonly IUiScheduler uiScheduler;
        private static readonly Logger logger = LogManager.GetLogger("ExplorerView");

        protected IExplorerViewModel ViewModel
		{
			get
			{
				return DataContext as IExplorerViewModel;
			}
		}

        public ExplorerView()
        {
            InitializeComponent();
        }

        public ExplorerView(IExplorerViewModel viewModel, IControlFactory controlFactory, INotificationService notificationService, IEventAggregator eventAggregator, IUiScheduler uiScheduler)
        {
            this.notificationService = notificationService;
            this.eventAggregator = eventAggregator;
            this.uiScheduler = uiScheduler;
            DataContext = viewModel;
            this.InitializeComponent();

            ControlBuilder controlBuilder = Resources["ControlBuilder"] as ControlBuilder;
            controlBuilder.ControlFactory = controlFactory;

            ViewModel.ProjectManager.RenameCommandExecuted += new CancelEventHandler(ProjectManager_RenameCommandExecuted);

            this.uxExplorerTree.MouseDoubleClick += new MouseButtonEventHandler(uxExplorerTree_MouseDoubleClick);
            //树节点修改后触发
            this.uxExplorerTree.PreviewEdited += new RadTreeViewItemEditedEventHandler(this.uxExplorerTree_PreviewEdited);
            //树节点拖拽
            this.uxExplorerTree.AddHandler(RadDragAndDropManager.DropQueryEvent, new EventHandler<DragDropQueryEventArgs>(OnDropQuery), true);
            this.uxExplorerTree.PreviewDragEnded += new RadTreeViewDragEndedEventHandler(this.uxExplorerTree_PreviewDragEnded);
            
            SubscribeToIsNodeSelectedChangedEvent();

            //控件节点选中状态变更订阅
            bringSelectionIntoViewObserver = new Subject<IExplorerNodeViewModel>();

            //ObservableExtensions
            //    .Subscribe<IExplorerNodeViewModel>(
            //        Observable.Do<IExplorerNodeViewModel>(
            //            Observable.ObserveOn<IExplorerNodeViewModel>(
            //                Observable.Throttle<IExplorerNodeViewModel>(
            //                    Observable.Where<IExplorerNodeViewModel>(
            //                        bringSelectionIntoViewObserver, 
            //                        (IExplorerNodeViewModel model) => model != null && model.IsSelected), 
            //                    TimeSpan.FromMilliseconds(50.0)),
            //                uiScheduler),
            //            new Action<IExplorerNodeViewModel>(BringSelectionIntoView)));

            bringSelectionIntoViewObserver
                .Where(model => model?.IsSelected == true)
                .Throttle(TimeSpan.FromMilliseconds(50))
                .ObserveOn(uiScheduler)
                .Subscribe(BringSelectionIntoView);

        }

        private void uxExplorerTree_PreviewDragEnded(object sender, RadTreeViewDragEndedEventArgs e)
        {
            if (e.DraggedItems.OfType<IProjectPageViewModel>().Any<IProjectPageViewModel>() && e.TargetDropItem.DataContext is IProjectPageViewModel)
            {
                e.Handled = true;
            }
        }
        private void BringSelectionIntoView(IExplorerNodeViewModel model)
        {
            this.uxExplorerTree.BringPathIntoView(model.Path);
            RadTreeViewItem itemByPath = this.uxExplorerTree.GetItemByPath(model.Path);
            if (itemByPath != null)
            {
                this.uxExplorerTree.ScrollViewer.ScrollToHorizontalOffset((double)(this.uxExplorerTree.ItemsIndent * (itemByPath.Level + 1)));
            }
        }
        private void SubscribeToIsNodeSelectedChangedEvent()
        {
            IsNodeSelectedChangedEvent @event = eventAggregator.GetEvent<IsNodeSelectedChangedEvent>();
            Contract.Assert(@event != null);
            @event.Subscribe(new Action<IExplorerNodeViewModel>(OnIsNodeSelectedChanged));
        }
        public void OnIsNodeSelectedChanged(IExplorerNodeViewModel obj)
        {
            bringSelectionIntoViewObserver.OnNext(obj);
        }
        private void uxExplorerTree_PreviewEdited(object sender, RadTreeViewItemEditedEventArgs e)
        {
            RadTreeViewItem radTreeViewItem = e.OriginalSource as RadTreeViewItem;
            if (radTreeViewItem == null)
            {
                e.Handled = false;
                return;
            }
            switch (ViewModel.ValidateRename(radTreeViewItem.Item as IExplorerNodeViewModel, e.OldValue as string, e.NewValue as string))
            {
                case RenameStatus.NullNode:
                    break;
                case RenameStatus.InvalidName:
                    this.ShowWarning(ExplorerTexts.InvalidNameMessageFormat, e.NewValue);
                    break;
                case RenameStatus.NameExists:
                    this.ShowWarning(ExplorerTexts.NameExistsMessageFormat, e.NewValue);
                    break;
                default:
                    return;
            }
            e.Handled = true;
        }
        private void ShowWarning(string messageFormat, object newValue)
        {
            string text = string.Format(CultureInfo.CurrentCulture, messageFormat, new object[]
			{
				newValue
			});
            notificationService.ShowWarning(text, null);
        }
        private void uxExplorerTree_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.ProjectManager.CanRename)
            {
                bool handled = false;
                ViewModel.ProjectManager.ExecuteCommand(EditorCommands.Rename, null, out handled);
                e.Handled = handled;
            }
        }
        private void ProjectManager_RenameCommandExecuted(object sender, CancelEventArgs e)
        {
            RadTreeViewItem radTreeViewItem = this.uxExplorerTree.ContainerFromItemRecursive(ViewModel.ProjectManager.SelectedItem);
            if (radTreeViewItem != null)
            {
                radTreeViewItem.BeginEdit();
            }
        }
        private void uxExplorerTree_Selected(object sender, RadRoutedEventArgs e)
        {
        }
        private void uxExplorerTree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                object obj = e.AddedItems[0];
                try
                {
                    this.uxExplorerTree.BringItemIntoView(obj);
                }
                catch (InvalidOperationException ex)
                {
                    logger.WarnException(ex.Message, ex);
                }
            }
        }
        private static void OnDropQuery(object sender, DragDropQueryEventArgs e)
        {
            RadTreeViewItem radTreeViewItem = e.Options.Destination as RadTreeViewItem;
            object dataContext = e.Options.Source.DataContext;
            if (radTreeViewItem != null && dataContext != null)
            {
                if (radTreeViewItem.DropPosition == DropPosition.Inside)
                {
                    e.QueryResult = new bool?(((IExplorerNodeViewModel)radTreeViewItem.DataContext).IsValidDropTarget(new ObservableCollection<object>
					{
						dataContext
					}));
                    if (e.QueryResult.Value)
                    {
                        e.Handled = true;
                    }
                    return;
                }
            }
            e.QueryResult = new bool?(false);
        }
    }
}
