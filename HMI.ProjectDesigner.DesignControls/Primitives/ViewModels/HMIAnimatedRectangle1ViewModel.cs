// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using System.ComponentModel;
using System.Windows;

namespace HMI.ProjectDesigner.DesignControls.Primitives.ViewModels
{
	public class HMIAnimatedRectangle1ViewModel : HMIAnimatedPrimitiveBaseViewModel
	{
		private CornerRadius cornerRadius;
		public event EventHandler<ValueChangedEventArgs<CornerRadius>> CornerRadiusChanged;
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public CornerRadius CornerRadius
		{
			get
			{
				return cornerRadius;
			}
			set
			{
				if (cornerRadius != value)
				{
					ValueChangedEventArgs<CornerRadius> e = new ValueChangedEventArgs<CornerRadius>(cornerRadius, value);
					base.RaisePropertyChanging<CornerRadius>(() => CornerRadius);
                    cornerRadius = value;
                    CornerRadiusChanged.Raise(this, e);
					base.RaisePropertyChanged<CornerRadius>(() => CornerRadius);
				}
			}
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("CornerRadius", CornerRadius);
		}
		public HMIAnimatedRectangle1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIAnimatedRectangle1";
		}
	}
}
