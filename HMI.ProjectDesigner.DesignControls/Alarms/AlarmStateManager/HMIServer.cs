// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.ObjectModel;
using HMI.Common.Presentation.Events;
namespace HMI.ProjectDesigner.DesignControls.Alarms.AlarmStateManager
{
	public class HMIServer : PropertyChangedNotifier
	{
		private readonly System.Collections.ObjectModel.ObservableCollection<HMIAlarmGroup> alarmGroups;
		private readonly System.Guid id;
		private bool isExpanded;
		private bool isSelected;
		private string name;
		public System.Collections.ObjectModel.ObservableCollection<HMIAlarmGroup> AlarmGroups
		{
			get
			{
				return this.alarmGroups;
			}
		}
		public System.Guid ID
		{
			get
			{
				return this.id;
			}
		}
		public bool IsExpanded
		{
			get
			{
				return this.isExpanded;
			}
			set
			{
				this.isExpanded = value;
				base.RaisePropertyChanged<bool>(() => this.IsExpanded);
			}
		}
		public bool IsSelected
		{
			get
			{
				return this.isSelected;
			}
			set
			{
				this.isSelected = value;
				base.RaisePropertyChanged<bool>(() => this.IsSelected);
			}
		}
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				base.RaisePropertyChanged<string>(() => this.Name);
			}
		}
		public HMIServer(System.Guid id, string serverName)
		{
			this.id = id;
			this.name = serverName;
			this.alarmGroups = new System.Collections.ObjectModel.ObservableCollection<HMIAlarmGroup>();
		}
	}
}
