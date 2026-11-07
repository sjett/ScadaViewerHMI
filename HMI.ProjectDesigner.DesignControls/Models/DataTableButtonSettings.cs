// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.Infrastructure.Models
{
	public class DataTableButtonSettings : ButtonSettings
	{
		private DataTableButtons button;

		public event System.EventHandler<ValueChangedEventArgs<DataTableButtons>> ButtonChanged;

		[Browsable(false)]
		public DataTableButtons Button
		{
			get
			{
				return this.button;
			}
			set
			{
				if (this.button != value)
				{
					base.RaisePropertyChanging<DataTableButtons>(() => this.Button);
					ValueChangedEventArgs<DataTableButtons> e = new ValueChangedEventArgs<DataTableButtons>(this.button, value);
					this.button = value;
					this.ButtonChanged.Raise(this, e);
					base.RaisePropertyChanged<DataTableButtons>(() => this.Button);
					base.UpdateEditorDisplayText();
				}
			}
		}
		protected override string ItemClassName
		{
			get
			{
				return "ButtonSettings";
			}
		}
		protected override string NamespacePrefix
		{
			get
			{
				return "HMIDataTable";
			}
		}
		public DataTableButtonSettings(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
		}
		public DataTableButtonSettings(IServiceProvider container, IExplorerNodeViewModel parentNode, DataTableButtons button) : base(container, parentNode)
		{
			this.Button = button;
		}
		public override string ToString()
		{
			return this.Button.ToString();
		}
		public override ICollectionItem CreateClone()
		{
			return new DataTableButtonSettings(base.Services, base.Parent);
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			base.DeepCopyFields(target);
			DataTableButtonSettings dataTableButtonSettings = target as DataTableButtonSettings;
			dataTableButtonSettings.Button = this.Button;
		}
		public override string GetXaml()
		{
			return string.Format("<{0}:{1}.{2}>{3}</{0}:{1}.{2}>", new object[]
			{
				this.NamespacePrefix,
				"HMIDataTableButtons",
				this.Button,
				base.GetXaml()
			});
		}
	}
}
