// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Primitives.ViewModels
{
	public class HMIBorder1ViewModel : CommonControlViewModel
	{
		private CornerRadius cornerRadius;
		private Thickness borderThickness = new Thickness(1.0);
		private Color topBackgroundColor = Colors.White;
		private Color bottomBackgroundColor = Colors.LightBlue;
		private Brush borderBrush = (Brush)Brushes.DarkBlue.GetAsFrozen();

		public event EventHandler<ValueChangedEventArgs<CornerRadius>> CornerRadiusChanged;
		public event EventHandler<ValueChangedEventArgs<Thickness>> BorderThicknessChanged;
		public event EventHandler<ValueChangedEventArgs<Color>> TopBackgroundColorChanged;
		public event EventHandler<ValueChangedEventArgs<Color>> BottomBackgroundColorChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> BorderBrushChanged;

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
		[EditorOrder(-1.0), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Thickness BorderThickness
		{
			get
			{
				return borderThickness;
			}
			set
			{
				if (borderThickness != value)
				{
					ValueChangedEventArgs<Thickness> e = new ValueChangedEventArgs<Thickness>(borderThickness, value);
					base.RaisePropertyChanging<Thickness>(() => BorderThickness);
                    borderThickness = value;
                    BorderThicknessChanged.Raise(this, e);
					base.RaisePropertyChanged<Thickness>(() => BorderThickness);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Color TopBackgroundColor
		{
			get
			{
				return topBackgroundColor;
			}
			set
			{
				if (topBackgroundColor != value)
				{
					ValueChangedEventArgs<Color> e = new ValueChangedEventArgs<Color>(topBackgroundColor, value);
					base.RaisePropertyChanging<Color>(() => TopBackgroundColor);
                    topBackgroundColor = value;
                    TopBackgroundColorChanged.Raise(this, e);
					base.RaisePropertyChanged<Color>(() => TopBackgroundColor);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Color BottomBackgroundColor
		{
			get
			{
				return bottomBackgroundColor;
			}
			set
			{
				if (bottomBackgroundColor != value)
				{
					ValueChangedEventArgs<Color> e = new ValueChangedEventArgs<Color>(bottomBackgroundColor, value);
					base.RaisePropertyChanging<Color>(() => BottomBackgroundColor);
                    bottomBackgroundColor = value;
                    BottomBackgroundColorChanged.Raise(this, e);
					base.RaisePropertyChanged<Color>(() => BottomBackgroundColor);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush BorderBrush
		{
			get
			{
				return borderBrush;
			}
			set
			{
				if (borderBrush != value)
				{
					ValueChangedEventArgs<Brush> e = new ValueChangedEventArgs<Brush>(borderBrush, value);
					base.RaisePropertyChanging<Brush>(() => BorderBrush);
                    borderBrush = value;
                    BorderBrushChanged.Raise(this, e);
					base.RaisePropertyChanged<Brush>(() => BorderBrush);
				}
			}
		}
		public HMIBorder1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
            TopBackgroundColorChanged += new EventHandler<ValueChangedEventArgs<Color>>(HMIBorder1ViewModel_TopBackgroundColorChanged);
            BottomBackgroundColorChanged += new EventHandler<ValueChangedEventArgs<Color>>(HMIBorder1ViewModel_BottomBackgroundColorChanged);
            UpdateBackground();
		}
		private void HMIBorder1ViewModel_BottomBackgroundColorChanged(object sender, ValueChangedEventArgs<Color> e)
		{
            UpdateBackground();
		}
		private void HMIBorder1ViewModel_TopBackgroundColorChanged(object sender, ValueChangedEventArgs<Color> e)
		{
            UpdateBackground();
		}
		private void UpdateBackground()
		{
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush(TopBackgroundColor, BottomBackgroundColor, 90.0);
			base.Background = (Brush)linearGradientBrush.GetAsFrozen();
		}
		public override string GetXamlControlName()
		{
			return "HMIBorder1";
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("CornerRadius", CornerRadius);
			base.AddXamlAttribute("BorderThickness", BorderThickness);
			base.AddXamlAttribute("TopColor", TopBackgroundColor);
			base.AddXamlAttribute("BottomColor", BottomBackgroundColor);
			base.AddXamlAttribute("BorderBrush", BorderBrush);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();

			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
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
	}
}
