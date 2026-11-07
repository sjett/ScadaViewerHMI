// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Linq;
using System.Windows.Input;
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.Commands;
using HMI.Infrastructure.Presentation.Events;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using HMI.Modules.Application.Events;
//using NLog;

namespace HMI.Editor.ViewModels
{
    public class ProjectManagerViewModel : CommandSink, IProjectManagerViewModel, ISolution
    {
        //Logger logger = NLog.LogManager.GetLogger("ProjectManagerViewModel");

        private const string DefaultTemplatePageName = "TemplatePage";
        private const string DefaultSubPageName = "SubPage";

        private readonly IServiceProvider container;
        private readonly IEventAggregator eventAggregator;

        private readonly IMruService mruService;
        private readonly IProjectManagerService projectManagerService;

        private readonly ObservableCollection<MruEntry> mruItems = new ObservableCollection<MruEntry>();
        private readonly ObservableCollection<IExplorerNodeViewModel> rootItems = new ObservableCollection<IExplorerNodeViewModel>();

        private IProjectViewModel project;
        private IProjectPageViewModel activePage;
        private IClipboardManagerService clipboardService;
        private IExplorerNodeViewModel selectedItem;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event CancelEventHandler? AddTemplatePageCommandExecuted;
        public event CancelEventHandler? RenameCommandExecuted;
        public event EventHandler<ValueChangedEventArgs<IProjectViewModel>>? ProjectChanged;
        public event EventHandler<ValueChangedEventArgs<IProjectPageViewModel>>? ActivePageChanged;
        public event EventHandler<ValueChangedEventArgs<IExplorerNodeViewModel>>? SelectedItemChanged;

        /// <param name="container"></param>
        public ProjectManagerViewModel(
            IServiceProvider container,
            IProjectManagerService projectManagerService,
            IEventAggregator eventAggregator,
            IMruService mruService,
            IClipboardManagerService clipboardService,
            INotificationService notificationService)
        {
            Contract.Requires<ArgumentNullException>(container != null);

            this.container = container;
            this.projectManagerService = projectManagerService;
            this.eventAggregator = eventAggregator;
            this.mruService = mruService;
            this.clipboardService = clipboardService;
            NotificationService = notificationService;

            RegisterEventHandlers();
        }
        [Browsable(false)]
		public IActionMonitor ActionMonitor
		{
			get
			{
				if (Project != null)
				{
					return Project.ActionMonitor;
				}
				return null;
			}
		}
        public bool CanUndo
		{
			get
			{
				return ActionMonitor != null && ActionMonitor.CanUndo;
			}
		}
        public bool CanRedo
		{
			get
			{
				return ActionMonitor != null && ActionMonitor.CanRedo;
			}
		}
        public bool CanOpenMruItem
		{
			get
			{
				return MruItems.Any();
			}
		}
        public bool CanSave
		{
			get
			{
				return Project != null;
			}
		}
		/// <summary>
         /// </summary>
        public ObservableCollection<MruEntry> MruItems
		{
			get
			{
				return mruItems;
			}
		}
        public ObservableCollection<IExplorerNodeViewModel> RootItems
		{
			get
			{
				return rootItems;
			}
		}
        [Browsable(false)]
		public INotificationService NotificationService
		{
			get;
			set;
		}
        public bool CanSelectAll
		{
			get
			{
				return ActivePage.Children != null && ActivePage.Children.Count > 0;
			}
		}
        public bool CanRename
		{
			get
			{
				return SelectedItem != null && SelectedItem.IsEditable;
			}
		}
        public bool CanAddNavigationPage
		{
			get
			{
				return SelectedItem != null && (SelectedItem is INavigationTargetViewModel || SelectedItem.Parent is INavigationTargetViewModel);
			}
		}
        public bool CanAddTemplatePage
		{
			get
			{
				return Project != null;
			}
		}
        public bool CanRemovePage
		{
			get
			{
				return ActivePage != null && (ActivePage is ITemplatePageViewModel || !(ActivePage.Parent is IProjectViewModel));
			}
		}
        public IProjectViewModel Project
        {
			get
			{
				return project;
			}
			set
			{
				if (project != value)
				{
                    ActivePage = null;
					ValueChangedEventArgs<IProjectViewModel> e = new ValueChangedEventArgs<IProjectViewModel>(project, value);
                    project = value;
                    ProjectChanged?.Raise(this, e);
                    PropertyChanged?.Raise(this, () => Project);
                    UpdateRootItems();
                    UpdateProjectSelectedItem();
				}
			}
		}
        IPage ISolution.ActivePage
		{
			get
			{
				return this.ActivePage;
			}
		}
        IProject ISolution.Project
		{
			get
			{
				return this.Project;
			}
		}
        public IProjectPageViewModel? ActivePage
		{
			get
			{
				return activePage;
			}
			set
			{
				if (activePage != value)
				{
					ValueChangedEventArgs<IProjectPageViewModel> e = new ValueChangedEventArgs<IProjectPageViewModel>(activePage, value);
                    Deselect((n) => n != SelectedItem);
                    activePage = value;
                    ActivePageChanged.Raise(this, e);
                    PropertyChanged.Raise(this, () => ActivePage);
				}
			}
		}
        public IExplorerNodeViewModel SelectedItem
		{
			get
			{
				return selectedItem;
			}
			set
			{
				if (selectedItem != value)
				{
					ValueChangedEventArgs<IExplorerNodeViewModel> e = new ValueChangedEventArgs<IExplorerNodeViewModel>(selectedItem, value);
                    selectedItem = value;
                    SelectedItemChanged?.Raise(this, e);
                    PropertyChanged?.Raise(this, () => SelectedItem);
					if (selectedItem != null && selectedItem.IsSelected != true)
					{
                        selectedItem.IsSelected = true;
					}
				}
			}
		}
        /// <summary>
        /// ȫѡ
        /// </summary>
        public void OnSelectAll()
		{
			foreach (IExplorerNodeViewModel current in ActivePage.Children)
			{
				current.IsSelected=true;
			}
		}
        public void AddTemplatePage()
		{
			Contract.Requires<InvalidOperationException>(Project != null);
			ITemplatePageViewModel templatePageViewModel = projectManagerService.AddTemplatePage(project, DefaultTemplatePageName);
            SelectedItem = templatePageViewModel;
		}
        /// <param name="criteria"></param>
        public void Deselect(Func<IExplorerNodeViewModel, bool> criteria)
		{
			this.Deselect(Project, criteria);
		}
        /// <param name="node"></param>
        /// <param name="criteria"></param>
        private void Deselect(IExplorerNodeViewModel node, Func<IExplorerNodeViewModel, bool> criteria)
		{
			if (node == null)
			{
				return;
			}
			if (criteria(node))
			{
                if (node.IsSelected)
                    node.IsSelected = false;
			}
			IObservableCollection<IExplorerNodeViewModel> observableCollection = node.Children ?? new FullyObservableCollection<IExplorerNodeViewModel>();
			foreach (IExplorerNodeViewModel current in observableCollection)
			{
                Deselect(current, criteria);
			}
		}
        public void OnRemovePage()
		{
			if (!CanRemovePage)
			{
				return;
			}
            projectManagerService.RemovePage(ActivePage);
		}
        private void UpdateProjectSelectedItem()
		{
			if (Project != null)
			{
                Project.IsSelected=true;
				return;
			}
            SelectedItem = null;
		}
        private void RegisterToSelectedNodeChangedEvent()
		{
			IsNodeSelectedChangedEvent @event = eventAggregator.GetEvent<IsNodeSelectedChangedEvent>();
			if (@event != null)
			{
				@event.Subscribe(new Action<IExplorerNodeViewModel>(OnSelectedNodeChanged), ThreadOption.UIThread);
			}
		}
        private void RegisterEventHandlers()
		{
            RegisterToSelectedNodeChangedEvent();
            RegisterToApplicationStartedEvent();
		}
        private void RegisterToApplicationStartedEvent()
		{
			ApplicationStartedEvent @event = eventAggregator.GetEvent<ApplicationStartedEvent>();
			if (@event != null)
			{
				@event.Subscribe(new Action<object>(OnApplicationStarted),ThreadOption.UIThread);
			}
		}
        /// <param name="obj"></param>
        private void OnApplicationStarted(object obj)
		{
            RegisterCommandBindings();
            UpdateMostRecentlyUsedItems(false);
		}
        /// <param name="node"></param>
        private void UpdateActivePage(IExplorerNodeViewModel node)
		{
			IProjectViewModel? projectViewModel = node as IProjectViewModel;
			if (projectViewModel != null)
			{
                ActivePage = projectViewModel.Children?.OfType<IProjectPageViewModel>().FirstOrDefault<IProjectPageViewModel>();
				return;
			}
			IProjectPageViewModel? projectPageViewModel = node as IProjectPageViewModel;
			if (projectPageViewModel != null)
			{
                ActivePage = projectPageViewModel;
				return;
			}
			ICommonControlViewModel? commonControlViewModel = node as ICommonControlViewModel;
			if (commonControlViewModel != null)
			{
                ActivePage = commonControlViewModel.ParentPage;
				return;
			}
            ActivePage = null;
		}
        /// <param name="obj"></param>
        private void OnSelectedNodeChanged(IExplorerNodeViewModel obj)
		{
            //logger.Info("{0},{1}", obj.ID, obj.IsSelected);

            if (obj.IsSelected)
			{
				if (obj is ICommonControlViewModel)
				{
                    this.SetSelectionRecursively(project, false, (node) => !(node is ICommonControlViewModel));
				}
                UpdateActivePage(obj);
            }
			if (ActivePage != null)
			{
                ActivePage.UpdateContainerSelectionStatus();
            }
			if (SelectedItem == null && obj.IsSelected)
            {
                SelectedItem = obj;
			}
		}
        /// <param name="item"></param>
        /// <param name="isSelected"></param>
        /// <param name="filter"></param>
        private void SetSelectionRecursively(IExplorerNodeViewModel item, bool isSelected, Func<IExplorerNodeViewModel, bool> filter)
		{
			if (item == null)
			{
				return;
			}
			if (filter(item))
			{
                if (item.IsSelected != isSelected)
                    item.IsSelected = isSelected;
			}
			foreach (IExplorerNodeViewModel current in item.Children)
			{
                SetSelectionRecursively(current, isSelected, filter);
			}
		}
        private void UpdateMostRecentlyUsedItems(bool prependCurrentProjectToMRU)
		{
			if (prependCurrentProjectToMRU)
			{
                mruService.PrependProjectToMRU(Project.ProjectFullPath);
			}
            MruItems.Clear();
			foreach (MruEntry current in mruService.GetMruItems())
			{
                MruItems.Add(current);
			}
		}
        private void RegisterCommandBindings()
		{
            IEnumerable<ICommandHandler> enumerable = container.ResolveAll<ICommandHandler>();
            foreach (ICommandHandler current in enumerable)
			{
				current.RegisterCommand(this);
			}
            // ȫѡ
            base.RegisterCommand(ApplicationCommands.SelectAll, (object param) => CanSelectAll, delegate(object param)
			{
                OnSelectAll();
			});
            base.RegisterCommand(EditorCommands.AddNavigationPage, (object param) => CanAddNavigationPage, delegate(object param)
			{
                OnAddNavigationPage();
			});
            base.RegisterCommand(EditorCommands.AddTemplatePage, (object param) => CanAddTemplatePage, delegate(object param)
			{
                OnAddTemplatePage();
			});
            base.RegisterCommand(EditorCommands.RemovePage, (object param) => CanRemovePage, delegate(object param)
			{
                OnRemovePage();
			});
            base.RegisterCommand(EditorCommands.Rename, (object param) => CanRename, delegate(object param)
			{
                OnRenameItem();
			});
            base.RegisterCommand(ApplicationCommands.Undo, (object param) => CanUndo, delegate(object param)
			{
                OnUndo();
			});
            base.RegisterCommand(ApplicationCommands.Redo, (object param) => CanRedo, delegate(object param)
			{
                OnRedo();
			});
		}
        public void OnUndo()
		{
            ActionMonitor.Undo();
		}
        public void OnRedo()
		{
            ActionMonitor.Redo();
		}
        private void OnRenameItem()
		{
            RenameCommandExecuted.Raise(this, new CancelEventArgs());
		}
        private void OnAddNavigationPage()
		{
            ActionMonitor.StartGrouping();
			IExplorerNodeViewModel explorerNodeViewModel = SelectedItem is INavigationTargetViewModel ? SelectedItem : SelectedItem.Parent;
            projectManagerService.AddNavigationPage(explorerNodeViewModel, DefaultSubPageName);
            ActionMonitor.EndGrouping();
		}
		private void OnAddTemplatePage()
		{
            ActionMonitor.StartGrouping();
            AddTemplatePageCommandExecuted.Raise(this, new CancelEventArgs());
            AddTemplatePage();
            ActionMonitor.EndGrouping();
		}
		private void UpdateRootItems()
		{
            SelectedItem = null;
            RootItems.Clear();
			if (Project != null)
			{
                RootItems.Add(Project);
			}
		}
	}
}
