// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.ViewModels
{
	public class AlarmTypeFilter : CollectionItem
	{
		private string typeName;
		private readonly IContentReplacementService contentReplacementService;
		public event System.EventHandler<ValueChangedEventArgs<string>> TypeNameChanged;
		[Browsable(true), AlarmTypeBrowser]
		public string TypeName
		{
			get
			{
				return this.typeName;
			}
			set
			{
				if (this.typeName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.typeName, value);
					base.RaisePropertyChanging<string>(() => this.TypeName);
					this.typeName = value;
					this.TypeNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.TypeName);
				}
			}
		}
		public AlarmTypeFilter(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
			this.contentReplacementService = container.Resolve<IContentReplacementService>();
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			AlarmTypeFilter alarmTypeFilter = target as AlarmTypeFilter;
			alarmTypeFilter.TypeName = this.TypeName;
		}
		public override ICollectionItem CreateClone()
		{
			return new AlarmTypeFilter(base.Services, base.Parent);
		}
		public override string GetXaml()
		{
			string text = string.Format("<{0}:{1} TypeName=\"[TypeName]\"></{0}:{1}>", "HMIAlarmsCommon", "AlarmTypeFilter");
			return this.contentReplacementService.ReplaceStringContent(text, this);
		}
	}
}
