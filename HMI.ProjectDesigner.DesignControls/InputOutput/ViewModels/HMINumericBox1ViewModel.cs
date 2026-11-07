// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels
{
	public class HMINumericBox1ViewModel : HMINumericBoxBaseViewModel
	{
		private string unit;

		public event System.EventHandler<ValueChangedEventArgs<string>> UnitChanged;

		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string Unit
		{
			get
			{
				return this.unit;
			}
			set
			{
				if (this.unit != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.unit, value);
					base.RaisePropertyChanging<string>(() => this.Unit);
					this.unit = value;
					this.UnitChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.Unit);
				}
			}
		}
		public HMINumericBox1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMINumericBox1";
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("Unit", this.Unit);
		}
	}
}
