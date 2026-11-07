// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
namespace HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels
{
	public abstract class HMINumericBoxBaseViewModel : HMITextBoxBaseViewModel
	{
		private int precision;

		public event System.EventHandler<ValueChangedEventArgs<int>> PrecisionChanged;

		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue(30.0), MinimumValue(0.0)]
		public int Precision
		{
			get
			{
				return this.precision;
			}
			set
			{
				if (this.precision != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.precision, value);
					base.RaisePropertyChanging<int>(() => this.Precision);
					this.precision = value;
					this.PrecisionChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.Precision);
				}
			}
		}
		public HMINumericBoxBaseViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("Precision", this.Precision);
		}
	}
}
