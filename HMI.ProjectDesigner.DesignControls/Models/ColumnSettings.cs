// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Infrastructure.Models
{
	public class ColumnSettings : ColumnSettingsBase
	{
		private ColumnTypes type;
		public event System.EventHandler<ValueChangedEventArgs<ColumnTypes>> TypeChanged;
		public ColumnTypes Type
		{
			get
			{
				return this.type;
			}
			set
			{
				if (this.type != value)
				{
					ValueChangedEventArgs<ColumnTypes> e = new ValueChangedEventArgs<ColumnTypes>(this.type, value);
					base.RaisePropertyChanging<ColumnTypes>(() => this.Type);
					this.type = value;
					this.TypeChanged.Raise(this, e);
					base.RaisePropertyChanged<ColumnTypes>(() => this.Type);
					base.UpdateEditorDisplayText();
				}
			}
		}
		[Browsable(false)]
		public override string TypeName
		{
			get
			{
				return System.Enum.GetName(typeof(ColumnTypes), this.Type);
			}
		}
		public override string ToString()
		{
			return this.TypeName;
		}
		public ColumnSettings(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			base.DeepCopyFields(target);
			ColumnSettings columnSettings = target as ColumnSettings;
			columnSettings.Type = this.Type;
		}
		public override ICollectionItem CreateClone()
		{
			return new ColumnSettings(base.Services, base.Parent);
		}
		protected override string GetXamlTemplate()
		{
			return string.Format("<{0}:{1} IsAutoSize=\"[IsAutoSize]\" Width=\"[Width]\" Type=\"[Type]\" IsVisible=\"[IsVisible]\"></{0}:{1}>", "HMIOperationDiary", "ColumnSettings");
		}
	}
}
