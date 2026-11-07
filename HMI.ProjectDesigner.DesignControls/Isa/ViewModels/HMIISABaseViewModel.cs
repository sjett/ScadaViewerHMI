// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.ComponentModel;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.Isa.ViewModels
{
	public class HMIISABaseViewModel : HMIStaticStatesControlBaseViewModel
	{
		private double lineThickness = 1.0;
		public event System.EventHandler<ValueChangedEventArgs<double>> LineThicknessChanged;
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public double LineThickness
		{
			get
			{
				return this.lineThickness;
			}
			set
			{
				if (this.lineThickness != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.lineThickness, value);
					base.RaisePropertyChanging<double>(() => this.LineThickness);
					this.lineThickness = value;
					this.LineThicknessChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.LineThickness);
				}
			}
		}
		public HMIISABaseViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIIsa=\"clr-namespace:HMI.RuntimeControls.Isa;assembly=HMI.RuntimeControls\"",
                "HMIShared=\"clr-namespace:HMI.Runtime.Shared;assembly=HMI.Runtime.Shared\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIIsa;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("LineThickness", this.LineThickness);
		}
	}
}
