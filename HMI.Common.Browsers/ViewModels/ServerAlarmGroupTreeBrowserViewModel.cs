// SPDX-License-Identifier: GPL-3.0-only
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using HMI.Common.Browsers.Services;
using HMI.Common.Presentation.Reactive;
using HMI.Common.Presentation.ViewModels;
namespace HMI.Common.Browsers.ViewModels
{
	public class ServerAlarmGroupTreeBrowserViewModel : ServerAlarmGroupBrowserViewModel, INotifyPropertyChanged, ISelectorBrowserPageViewModel<ITranslatableViewModel>, IViewModel, ISelectorBrowserPageViewModel, IServerAlarmGroupBrowserViewModel, IServerAlarmGroupTreeBrowserViewModel
	{
		internal readonly DelegateCommand selectAllCommand;
		internal readonly DelegateCommand unselectAllCommand;
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
		public ObservableCollection<AlarmTreeItem> AlarmGroupItems
		{
			get
			{
				return rootAlarmTreeItem.Children;
			}
		}
		public ServerAlarmGroupTreeBrowserViewModel(IAlarmService alarmService, IUiScheduler uiScheduler, IAsyncScheduler asyncScheduler) : base(alarmService, uiScheduler, asyncScheduler)
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
            selectAllCommand.RaiseCanExecuteChanged();
            unselectAllCommand.RaiseCanExecuteChanged();
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
            RestoreSelectedAlarmGroups();
		}
        void SubscribeToTreeSelectionChanges(AlarmTreeItem treeItem)
        {
            foreach (AlarmTreeItem child in treeItem.Children)
            {
                SubscribeToTreeSelectionChanges(child);
            }
            treeItem.SelectionChanged += OnTreeSelectionChanged;
        }
        private void RestoreSelectedAlarmGroups()
		{
			if (SelectedValue == null)
			{
                SelectedValue = "*";
			}
			string[] selectedAlarmGroupNames = SelectedValue.Split(';');
			for (int i = 0; i < selectedAlarmGroupNames.Length; i++)
			{
				string selectedGroupNames = selectedAlarmGroupNames[i];
                rootAlarmTreeItem.SelectNames(selectedGroupNames);
			}
		}
		void OnTreeSelectionChanged(object sender, EventArgs e)
		{
            UpdateSelectedAlarmGroups();
		}
		private void UpdateSelectedAlarmGroups()
		{
            SelectedValue = string.Join(";", rootAlarmTreeItem.GetSelectedNames());
			RefreshSelectionCommands();
		}
		protected override void FillValues(bool invalidateCache)
		{
			UnsubscribeFromTreeSelectionChanges(rootAlarmTreeItem);
            rootAlarmTreeItem.IsSelected = null;
            AlarmGroupItems.Clear();
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
