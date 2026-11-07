// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.Html.ViewModels
{
	public class HtmlBaseViewModel : CommonControlViewModel
	{
		private System.Windows.Media.Brush borderBrush = (System.Windows.Media.Brush)System.Windows.Media.Brushes.Transparent.GetAsFrozen();
		private System.Windows.Thickness borderThickness;
		public event EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> BorderBrushChanged;
		public event EventHandler<ValueChangedEventArgs<System.Windows.Thickness>> BorderThicknessChanged;
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush BorderBrush
		{
			get
			{
				return borderBrush;
			}
			set
			{
				if (borderBrush != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(borderBrush, value);
					RaisePropertyChanging(() => BorderBrush);
					borderBrush = value;
					BorderBrushChanged.Raise(this, e);
					RaisePropertyChanged(() => BorderBrush);
				}
			}
		}
		[EditorOrder(-1.0), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Thickness BorderThickness
		{
			get
			{
				return borderThickness;
			}
			set
			{
				if (borderThickness != value)
				{
					ValueChangedEventArgs<System.Windows.Thickness> e = new ValueChangedEventArgs<System.Windows.Thickness>(borderThickness, value);
					RaisePropertyChanging(() => BorderThickness);
					borderThickness = value;
					BorderThicknessChanged.Raise(this, e);
					RaisePropertyChanged(() => BorderThickness);
				}
			}
		}
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(500.0, 367.0);
			}
		}
		public HtmlBaseViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("Background", base.Background);
			base.AddXamlAttribute("BorderBrush", this.BorderBrush);
			base.AddXamlAttribute("BorderThickness", this.BorderThickness);
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIHtml=\"clr-namespace:HMI.RuntimeControls.Html;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIHtml;
		//}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			IsForegroundVisible = false;
		}
		//public override System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> GetSilverlightObjectParameters()
		//{
		//	return new System.Collections.Generic.KeyValuePair<string, string>[]
		//	{
		//		new System.Collections.Generic.KeyValuePair<string, string>("windowless", "true")
		//	};
		//}
	}
}
