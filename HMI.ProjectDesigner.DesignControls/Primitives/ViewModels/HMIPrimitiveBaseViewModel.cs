// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using System.ComponentModel;

namespace HMI.ProjectDesigner.DesignControls.Primitives.ViewModels
{
	public class HMIPrimitiveBaseViewModel : HMIStaticStatesControlBaseViewModel
	{
		private double lineThickness = 1.0;

		public event EventHandler<ValueChangedEventArgs<double>> LineThicknessChanged;

		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories)), MinimumValue(0.0)]
		public double LineThickness
		{
			get
			{
				return lineThickness;
			}
			set
			{
				if (lineThickness != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(lineThickness, value);
					base.RaisePropertyChanging<double>(() => LineThickness);
                    lineThickness = value;
                    LineThicknessChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => LineThickness);
				}
			}
		}
		public HMIPrimitiveBaseViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
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
			base.AddXamlAttribute("LineThickness", LineThickness);
		}
	}
}
