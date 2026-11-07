// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.ObjectModel;
using HMI.Common.Presentation.Events;
namespace HMI.ProjectDesigner.DesignControls.Alarms.AlarmStateManager
{
	public class HMIAlarmGroup : PropertyChangedNotifier
	{
		private readonly System.Collections.ObjectModel.ObservableCollection<HMIAlarm> listAlarms;
		private readonly System.Guid id;
		private bool isExpanded;
		private bool isSelected;
		private string name;
		private readonly HMIServer server;
		public System.Collections.ObjectModel.ObservableCollection<HMIAlarm> Alarms
		{
			get
			{
				return this.listAlarms;
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
		public HMIServer Server
		{
			get
			{
				return this.server;
			}
		}
		public HMIAlarmGroup(HMIServer server, System.Guid id, string groupName)
		{
			this.server = server;
			this.id = id;
			this.name = groupName;
			this.listAlarms = new System.Collections.ObjectModel.ObservableCollection<HMIAlarm>();
		}
	}
}
