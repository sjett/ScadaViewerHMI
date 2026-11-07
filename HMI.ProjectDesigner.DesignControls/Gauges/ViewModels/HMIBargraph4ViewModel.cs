// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
namespace HMI.ProjectDesigner.DesignControls.Gauges.ViewModels
{
	public class HMIBargraph4ViewModel : HMIBargraphBaseViewModel
	{
		private double thresholdValue1 = 33.0;
		private double thresholdValue2 = 66.0;

		public event System.EventHandler<ValueChangedEventArgs<double>> ThresholdValue1Changed;
		public event System.EventHandler<ValueChangedEventArgs<double>> ThresholdValue2Changed;

		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue("ThresholdValue2"), MinimumValue("RangeMin")]
		public double ThresholdValue1
		{
			get
			{
				return this.thresholdValue1;
			}
			set
			{
				if (this.thresholdValue1 != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.thresholdValue1, value);
					base.RaisePropertyChanging<double>(() => this.ThresholdValue1);
					this.thresholdValue1 = value;
					this.ThresholdValue1Changed.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.ThresholdValue1);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue("RangeMax"), MinimumValue("ThresholdValue1")]
		public double ThresholdValue2
		{
			get
			{
				return this.thresholdValue2;
			}
			set
			{
				if (this.thresholdValue2 != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.thresholdValue2, value);
					base.RaisePropertyChanging<double>(() => this.ThresholdValue2);
					this.thresholdValue2 = value;
					this.ThresholdValue2Changed.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.ThresholdValue2);
				}
			}
		}
		[MaximumValue("ThresholdValue1")]
		public override double RangeMin
		{
			get
			{
				return base.RangeMin;
			}
			set
			{
				base.RangeMin = value;
			}
		}
		[MinimumValue("ThresholdValue2")]
		public override double RangeMax
		{
			get
			{
				return base.RangeMax;
			}
			set
			{
				base.RangeMax = value;
			}
		}
		public HMIBargraph4ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIBargraph4";
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("ThresholdValue1", this.ThresholdValue1);
			base.AddXamlAttribute("ThresholdValue2", this.ThresholdValue2);
		}
	}
}
