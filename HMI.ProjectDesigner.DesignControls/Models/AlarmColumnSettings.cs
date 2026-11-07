// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Infrastructure.Models
{
	public class AlarmColumnSettings : ColumnSettingsBase
	{
		private AlarmColumnTypes type;
		private bool isFilterable = true;

		public event System.EventHandler<ValueChangedEventArgs<AlarmColumnTypes>> TypeChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> IsFilterableChanged;

		[Browsable(true)]
		public AlarmColumnTypes Type
		{
			get
			{
				return this.type;
			}
			set
			{
				if (this.type != value)
				{
					ValueChangedEventArgs<AlarmColumnTypes> e = new ValueChangedEventArgs<AlarmColumnTypes>(this.type, value);
					base.RaisePropertyChanging<AlarmColumnTypes>(() => this.Type);
					this.type = value;
					this.TypeChanged.Raise(this, e);
					base.RaisePropertyChanged<AlarmColumnTypes>(() => this.Type);
					base.UpdateEditorDisplayText();
				}
			}
		}
		public override string TypeName
		{
			get
			{
				return System.Enum.GetName(typeof(AlarmColumnTypes), this.Type);
			}
		}
		[Browsable(true)]
		public virtual bool IsFilterable
		{
			get
			{
				return this.isFilterable;
			}
			set
			{
				if (this.isFilterable == value)
				{
					return;
				}
				base.RaisePropertyChanging<bool>(() => this.IsFilterable);
				ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.isFilterable, value);
				this.isFilterable = value;
				this.IsFilterableChanged.Raise(this, e);
				base.RaisePropertyChanged<bool>(() => this.IsFilterable);
			}
		}
		public AlarmColumnSettings(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
		}
		public AlarmColumnSettings(AlarmColumnTypes type, IServiceProvider services, IExplorerNodeViewModel parent) : base(services, parent)
		{
			this.Type = type;
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			base.DeepCopyFields(target);
			AlarmColumnSettings alarmColumnSettings = target as AlarmColumnSettings;
			alarmColumnSettings.Type = this.Type;
			alarmColumnSettings.IsFilterable = this.IsFilterable;
		}
		public override ICollectionItem CreateClone()
		{
			return new AlarmColumnSettings(base.Services, base.Parent);
		}
		protected override string GetXamlTemplate()
		{
			return string.Format("<{0}:{1} IsAutoSize=\"[IsAutoSize]\" Width=\"[Width]\" Type=\"[Type]\" IsVisible=\"[IsVisible]\" IsFilterable=\"[IsFilterable]\"></{0}:{1}>", "HMIAlarms", "ColumnSettings");
		}
	}
}
