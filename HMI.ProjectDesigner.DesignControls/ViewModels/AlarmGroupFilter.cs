// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.ViewModels
{
	public class AlarmGroupFilter : CollectionItem
	{
		private string groupName;
		private readonly IContentReplacementService contentReplacementService;
		public event System.EventHandler<ValueChangedEventArgs<string>> GroupNameChanged;
		[Browsable(true), AlarmGroupBrowser]
		public string GroupName
		{
			get
			{
				return this.groupName;
			}
			set
			{
				if (this.groupName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.groupName, value);
					base.RaisePropertyChanging<string>(() => this.GroupName);
					this.groupName = value;
					this.GroupNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.GroupName);
				}
			}
		}
		public AlarmGroupFilter(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
			this.contentReplacementService = container.Resolve<IContentReplacementService>();
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			AlarmGroupFilter alarmGroupFilter = target as AlarmGroupFilter;
			alarmGroupFilter.GroupName = this.GroupName;
		}
		public override ICollectionItem CreateClone()
		{
			return new AlarmGroupFilter(base.Services, base.Parent);
		}
		public override string GetXaml()
		{
			string text = string.Format("<{0}:{1} GroupName=\"[GroupName]\"></{0}:{1}>", "HMIAlarmsCommon", "AlarmGroupFilter");
			return this.contentReplacementService.ReplaceStringContent(text, this);
		}
	}
}
