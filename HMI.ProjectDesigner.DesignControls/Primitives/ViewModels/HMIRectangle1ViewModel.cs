// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using System.ComponentModel;

namespace HMI.ProjectDesigner.DesignControls.Primitives.ViewModels
{
	public class HMIRectangle1ViewModel : HMIPrimitiveBaseViewModel
	{
		private double radiusX;
		private double radiusY;

		public event EventHandler<ValueChangedEventArgs<double>> RadiusXChanged;
		public event EventHandler<ValueChangedEventArgs<double>> RadiusYChanged;

		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public double RadiusX
		{
			get
			{
				return radiusX;
			}
			set
			{
				double num = radiusX;
				if (Math.Abs(num - value) < 1E-05)
				{
					return;
				}
				base.RaisePropertyChanging<double>(() => RadiusX);
                radiusX = value;
                RadiusXChanged.Raise(this, new ValueChangedEventArgs<double>(num, value));
				base.RaisePropertyChanged<double>(() => RadiusX);
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public double RadiusY
		{
			get
			{
				return radiusY;
			}
			set
			{
				double num = radiusY;
				if (Math.Abs(num - value) < 1E-05)
				{
					return;
				}
				base.RaisePropertyChanging<double>(() => RadiusY);
                radiusY = value;
                RadiusYChanged.Raise(this, new ValueChangedEventArgs<double>(num, value));
				base.RaisePropertyChanged<double>(() => RadiusY);
			}
		}
		public HMIRectangle1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIRectangle1";
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("RadiusX", RadiusX);
			base.AddXamlAttribute("RadiusY", RadiusY);
		}
	}
}
