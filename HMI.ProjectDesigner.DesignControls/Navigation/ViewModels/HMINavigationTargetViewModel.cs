// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using System.ComponentModel;

namespace HMI.ProjectDesigner.DesignControls.Navigation.ViewModels
{
	public class HMINavigationTargetViewModel : CommonControlViewModel,  INavigationTargetViewModel     
	{
		private IExplorerNodeViewModel defaultPage;
		private System.Windows.Media.Stretch stretchContent;
		private System.Windows.Controls.ScrollBarVisibility horizontalScrollBarVisibility;
		private System.Windows.Controls.ScrollBarVisibility verticalScrollBarVisibility;
		
		public event System.EventHandler<ValueChangedEventArgs<IExplorerNodeViewModel>> DefaultPageChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Stretch>> StretchContentChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Controls.ScrollBarVisibility>> HorizontalScrollBarVisibilityChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Controls.ScrollBarVisibility>> VerticalScrollBarVisibilityChanged;
		
		[Browsable(true), OptionListEditor("Children", IsUnselectable = true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public IExplorerNodeViewModel DefaultPage
		{
			get
			{
				return this.defaultPage;
			}
			set
			{
				if (this.defaultPage != value)
				{
					ValueChangedEventArgs<IExplorerNodeViewModel> e = new ValueChangedEventArgs<IExplorerNodeViewModel>(this.defaultPage, value);
					base.RaisePropertyChanging<IExplorerNodeViewModel>(() => this.DefaultPage);
					this.defaultPage = value;
					this.DefaultPageChanged.Raise(this, e);
					base.RaisePropertyChanged<IExplorerNodeViewModel>(() => this.DefaultPage);
				}
			}
		}
		[Browsable(false)]
		public bool IsStretchContentEnabled
		{
			get
			{
				return this.Project != null && !this.Project.StretchPages;
			}
		}
		[EnabledProperty("IsStretchContentEnabled"), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Stretch StretchContent
		{
			get
			{
				return this.stretchContent;
			}
			set
			{
				if (this.stretchContent != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Stretch>(() => this.StretchContent);
					ValueChangedEventArgs<System.Windows.Media.Stretch> e = new ValueChangedEventArgs<System.Windows.Media.Stretch>(this.stretchContent, value);
					this.stretchContent = value;
					this.StretchContentChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Stretch>(() => this.StretchContent);
					base.RaisePropertyChanged<bool>(() => this.AreScrollBarsEnabled);
				}
			}
		}
		[Browsable(false)]
		public bool AreScrollBarsEnabled
		{
			get
			{
				return this.Project != null && !this.Project.StretchPages && this.StretchContent == System.Windows.Media.Stretch.None;
			}
		}
		[Browsable(true), EditorOrder(-233.0), EnabledProperty("AreScrollBarsEnabled"), PropertyCategory("Layout", typeof(StandardPropertyCategories))]
		public System.Windows.Controls.ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get
			{
				return this.horizontalScrollBarVisibility;
			}
			set
			{
				if (this.horizontalScrollBarVisibility != value)
				{
					base.RaisePropertyChanging<System.Windows.Controls.ScrollBarVisibility>(() => this.HorizontalScrollBarVisibility);
					ValueChangedEventArgs<System.Windows.Controls.ScrollBarVisibility> e = new ValueChangedEventArgs<System.Windows.Controls.ScrollBarVisibility>(this.horizontalScrollBarVisibility, value);
					this.horizontalScrollBarVisibility = value;
					this.HorizontalScrollBarVisibilityChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Controls.ScrollBarVisibility>(() => this.HorizontalScrollBarVisibility);
				}
			}
		}
		[Browsable(true), EditorOrder(-232.0), EnabledProperty("AreScrollBarsEnabled"), PropertyCategory("Layout", typeof(StandardPropertyCategories))]
		public System.Windows.Controls.ScrollBarVisibility VerticalScrollBarVisibility
		{
			get
			{
				return this.verticalScrollBarVisibility;
			}
			set
			{
				if (this.verticalScrollBarVisibility != value)
				{
					base.RaisePropertyChanging<System.Windows.Controls.ScrollBarVisibility>(() => this.VerticalScrollBarVisibility);
					ValueChangedEventArgs<System.Windows.Controls.ScrollBarVisibility> e = new ValueChangedEventArgs<System.Windows.Controls.ScrollBarVisibility>(this.verticalScrollBarVisibility, value);
					this.verticalScrollBarVisibility = value;
					this.VerticalScrollBarVisibilityChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Controls.ScrollBarVisibility>(() => this.VerticalScrollBarVisibility);
				}
			}
		}
		[Browsable(false)]
		public bool IsInvalidTarget
		{
			get
			{
				return false;
			}
		}
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(300.0, 100.0);
			}
		}
		public HMINavigationTargetViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) 
			: base(container, parentValue, controlType)
		{
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMINavigation=\"clr-namespace:HMI.RuntimeControls.Navigation;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMINavigation;
		//}
		public override bool IsValidDropTarget(System.Collections.Generic.IEnumerable<object> payload)
		{
			return payload.OfType<IProjectPageViewModel>().Any((IProjectPageViewModel p) => p.Parent is INavigationTargetViewModel);
		}
		public override string GetXamlControlName()
		{
			return "HMINavigationTarget";
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			string propertyValue = string.Format("{0}.{1}", this.Project.ProjectName, this.DefaultPage);
			base.AddXamlAttribute("DefaultPage", propertyValue);
			if (!this.IsStretchContentEnabled)
			{
				this.SetStretchContentToFill();
			}
			base.AddXamlAttribute("StretchContent", this.StretchContent);
			if (!this.AreScrollBarsEnabled)
			{
				this.SetScrollBarsToDisabled();
			}
			base.AddXamlAttribute("HorizontalScrollBarVisibility", this.HorizontalScrollBarVisibility);
			base.AddXamlAttribute("VerticalScrollBarVisibility", this.VerticalScrollBarVisibility);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
		}
		public override string ToString()
		{
			return base.ID ?? string.Empty;
		}
		private void SetStretchContentToFill()
		{
			this.StretchContent = System.Windows.Media.Stretch.Fill;
		}
		private void SetScrollBarsToDisabled()
		{
			this.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Disabled;
			this.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Disabled;
		}
	}
}
