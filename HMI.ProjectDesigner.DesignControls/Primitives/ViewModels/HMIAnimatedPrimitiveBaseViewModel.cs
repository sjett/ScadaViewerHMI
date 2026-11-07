// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Primitives.ViewModels
{
	public class HMIAnimatedPrimitiveBaseViewModel : HMIAnimatedStatesControlBaseViewModel
	{
		private Thickness lineThickness = new Thickness(1.0);
		private Brush animatedFill = (Brush)Brushes.Red.GetAsFrozen();
		private double rangeMin;
		private double rangeMax = 100.0;
		private string signalName;

		public event EventHandler<ValueChangedEventArgs<Thickness>> LineThicknessChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> AnimatedFillChanged;
		public event EventHandler<ValueChangedEventArgs<double>> RangeMinChanged;
		public event EventHandler<ValueChangedEventArgs<double>> RangeMaxChanged;
		public event EventHandler<ValueChangedEventArgs<string>> SignalNameChanged;

		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Thickness LineThickness
		{
			get
			{
				return lineThickness;
			}
			set
			{
				if (lineThickness != value)
				{
					ValueChangedEventArgs<Thickness> e = new ValueChangedEventArgs<Thickness>(lineThickness, value);
					base.RaisePropertyChanging<Thickness>(() => LineThickness);
                    lineThickness = value;
                    LineThicknessChanged.Raise(this, e);
					base.RaisePropertyChanged<Thickness>(() => LineThickness);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush AnimatedFill
		{
			get
			{
				return animatedFill;
			}
			set
			{
				if (animatedFill != value)
				{
					ValueChangedEventArgs<Brush> e = new ValueChangedEventArgs<Brush>(animatedFill, value);
					base.RaisePropertyChanging<Brush>(() => AnimatedFill);
                    animatedFill = value;
                    AnimatedFillChanged.Raise(this, e);
					base.RaisePropertyChanged<Brush>(() => AnimatedFill);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public double RangeMin
		{
			get
			{
				return rangeMin;
			}
			set
			{
				if (rangeMin != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(rangeMin, value);
					base.RaisePropertyChanging<double>(() => RangeMin);
                    rangeMin = value;
                    RangeMinChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => RangeMin);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public double RangeMax
		{
			get
			{
				return rangeMax;
			}
			set
			{
				if (rangeMax != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(rangeMax, value);
					base.RaisePropertyChanging<double>(() => RangeMax);
                    rangeMax = value;
                    RangeMaxChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => RangeMax);
				}
			}
		}
		[Browsable(true), EditorOrder(-250.0), PropertyCategory("Signals", typeof(StandardPropertyCategories)), SignalBrowser]
		public string SignalName
		{
			get
			{
				return signalName;
			}
			set
			{
				if (signalName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(signalName, value);
					base.RaisePropertyChanging<string>(() => SignalName);
                    signalName = value;
                    SignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => SignalName);
				}
			}
		}
		public HMIAnimatedPrimitiveBaseViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIPrimitives=\"clr-namespace:HMI.RuntimeControls.Primitives;assembly=HMI.RuntimeControls\"",
                "HMIShared=\"clr-namespace:HMI.Runtime.Shared;assembly=HMI.Runtime.Shared\""
            };
		}
		//public override IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIPrimitives;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("SignalName", SignalName);
			base.AddXamlAttribute("LineThickness", LineThickness);
			base.AddXamlAttribute("AnimatedFill", AnimatedFill);
			base.AddXamlAttribute("RangeMin", RangeMin);
			base.AddXamlAttribute("RangeMax", RangeMax);
			base.AddXamlAttribute("BorderBrush", base.UseLocalColors ? base.Foreground : this.Project.StateDefaultForeground);
		}
	}
}
