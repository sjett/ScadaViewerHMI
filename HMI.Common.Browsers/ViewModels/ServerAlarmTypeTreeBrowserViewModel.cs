// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.Services;
using HMI.Common.Presentation.Reactive;
using HMI.Common.Presentation.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace HMI.Common.Browsers.ViewModels
{
	public class ServerAlarmTypeTreeBrowserViewModel : ServerAlarmTypeBrowserViewModel, INotifyPropertyChanged, ISelectorBrowserPageViewModel<ITranslatableViewModel>, IViewModel, ISelectorBrowserPageViewModel, IServerAlarmTypeBrowserViewModel, IServerAlarmTypeTreeBrowserViewModel
	{
        private readonly DelegateCommand selectAllCommand;
        private readonly DelegateCommand unselectAllCommand;
		private readonly AlarmTreeItem rootAlarmTreeItem = new AlarmTreeItem(Guid.Empty, "*", "*", "*");
		public ICommand SelectAllCommand
		{
			get
			{
				return selectAllCommand;
			}
		}
		public ICommand UnselectAllCommand
		{
			get
			{
				return unselectAllCommand;
			}
		}
		[Browsable(false)]
		public ObservableCollection<AlarmTreeItem> AlarmTypeItems
		{
			get
			{
				return rootAlarmTreeItem.Children;
			}
		}
		public ServerAlarmTypeTreeBrowserViewModel(IAlarmService alarmService, IUiScheduler uiScheduler, IAsyncScheduler asyncScheduler) : base(alarmService, uiScheduler, asyncScheduler)
		{
            selectAllCommand = new DelegateCommand(SelectAll, CanSelectAll);
            unselectAllCommand = new DelegateCommand(UnselectAll, CanUnselectAll);
		}
		private bool CanSelectAll()
		{
			return SelectedValue != "*";
		}
		private void SelectAll()
		{
			if (!CanSelectAll())
			{
				return;
			}
            rootAlarmTreeItem.IsSelected = new bool?(true);
			RefreshSelectionCommands();
		}
        void RefreshSelectionCommands()
        {
            this.selectAllCommand.RaiseCanExecuteChanged();
            this.unselectAllCommand.RaiseCanExecuteChanged();
        }
        private bool CanUnselectAll()
		{
			return rootAlarmTreeItem.IsSelected != false;
		}
		private void UnselectAll()
		{
			if (!CanUnselectAll())
			{
				return;
			}
            rootAlarmTreeItem.IsSelected = new bool?(false);
            selectAllCommand.RaiseCanExecuteChanged();
		}
		protected override void EndRefresh()
		{
			foreach (ITranslatableViewModel current in Values)
			{
                rootAlarmTreeItem.AddItem(current.Id, current.Name, current.Translation);
			}
			SubscribeToTreeSelectionChanges(rootAlarmTreeItem);
            RestoreSelectedAlarmTypes();
		}
        void SubscribeToTreeSelectionChanges(AlarmTreeItem treeItem)
        {
            foreach (AlarmTreeItem child in treeItem.Children)
            {
                SubscribeToTreeSelectionChanges(child);
            }
            treeItem.SelectionChanged += OnTreeSelectionChanged;
        }
        private void RestoreSelectedAlarmTypes()
		{
			if (SelectedValue == null)
			{
                SelectedValue = "*";
			}
			string[] selectedAlarmTypeNames = SelectedValue.Split(';');
			for (int i = 0; i < selectedAlarmTypeNames.Length; i++)
			{
				string selectedTypeNames = selectedAlarmTypeNames[i];
                rootAlarmTreeItem.SelectNames(selectedTypeNames);
			}
		}
		void OnTreeSelectionChanged(object? sender, EventArgs e)
		{
            UpdateSelectedAlarmTypes();
		}
		private void UpdateSelectedAlarmTypes()
		{
            SelectedValue = string.Join(";", rootAlarmTreeItem.GetSelectedNames());
			RefreshSelectionCommands();
		}
		protected override void FillValues(bool invalidateCache)
		{
			UnsubscribeFromTreeSelectionChanges(rootAlarmTreeItem);
            rootAlarmTreeItem.IsSelected = null;
            AlarmTypeItems.Clear();
			base.FillValues(invalidateCache);
		}
        void UnsubscribeFromTreeSelectionChanges(AlarmTreeItem treeItem)
        {
            foreach (AlarmTreeItem child in treeItem.Children)
            {
                UnsubscribeFromTreeSelectionChanges(child);
            }
            treeItem.SelectionChanged -= OnTreeSelectionChanged;
        }
    }
}
