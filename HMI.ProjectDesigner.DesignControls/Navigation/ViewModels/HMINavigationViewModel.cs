// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using System.ComponentModel;
namespace HMI.ProjectDesigner.DesignControls.Navigation.ViewModels
{
	public class HMINavigationViewModel : CommonControlViewModel, INavigationViewModel
	{
		private readonly ICollectionMonitor<NavigationItem> navigationParametersMonitor;
		private readonly INavigationTargetService navigationTargetService;
		private string securityAuthorizationName;
		private SecurityDenyAccessBehavior securityDenyAccessBehavior;
		private string symbolicText;
		private string fontFamily = "Arial";
		private double fontSize = 10.0;
		private string fontStretch = "Normal";
		private string fontStyle = "Normal";
		private string fontWeight = "Normal";
		private readonly IObservableCollection<NavigationItem> navigations = new FullyObservableCollection<NavigationItem>();

		public event System.EventHandler<ValueChangedEventArgs<string>> SecurityAuthorizationNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<SecurityDenyAccessBehavior>> SecurityDenyAccessBehaviorChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SymbolicTextChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontFamilyChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> FontSizeChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontStretchChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontStyleChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontWeightChanged;

		[Browsable(true), ProjectAuthorizationsBrowser, PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public string SecurityAuthorizationName
		{
			get
			{
				return this.securityAuthorizationName;
			}
			set
			{
				if (this.securityAuthorizationName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.securityAuthorizationName, value);
					base.RaisePropertyChanging<string>(() => this.SecurityAuthorizationName);
					this.securityAuthorizationName = value;
					this.SecurityAuthorizationNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.SecurityAuthorizationName);
				}
			}
		}
		[Browsable(true), PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public SecurityDenyAccessBehavior SecurityDenyAccessBehaviour
		{
			get
			{
				return this.securityDenyAccessBehavior;
			}
			set
			{
				if (this.securityDenyAccessBehavior != value)
				{
					ValueChangedEventArgs<SecurityDenyAccessBehavior> e = new ValueChangedEventArgs<SecurityDenyAccessBehavior>(this.securityDenyAccessBehavior, value);
					base.RaisePropertyChanging<SecurityDenyAccessBehavior>(() => this.SecurityDenyAccessBehaviour);
					this.securityDenyAccessBehavior = value;
					this.SecurityDenyAccessBehaviorChanged.Raise(this, e);
					base.RaisePropertyChanged<SecurityDenyAccessBehavior>(() => this.SecurityDenyAccessBehaviour);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string SymbolicText
		{
			get
			{
				return this.symbolicText;
			}
			set
			{
				if (this.symbolicText != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.symbolicText, value);
					base.RaisePropertyChanging<string>(() => this.SymbolicText);
					this.symbolicText = value;
					this.SymbolicTextChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.SymbolicText);
				}
			}
		}
		[Browsable(true), FontFamilyEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string FontFamily
		{
			get
			{
				return this.fontFamily;
			}
			set
			{
				if (this.fontFamily != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.fontFamily, value);
					base.RaisePropertyChanging<string>(() => this.FontFamily);
					this.fontFamily = value;
					this.FontFamilyChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.FontFamily);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories)), MinimumValue(4.0)]
		public double FontSize
		{
			get
			{
				return this.fontSize;
			}
			set
			{
				if (this.fontSize != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.fontSize, value);
					base.RaisePropertyChanging<double>(() => this.FontSize);
					this.fontSize = value;
					this.FontSizeChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.FontSize);
				}
			}
		}
		[Browsable(true), FontStretchEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string FontStretch
		{
			get
			{
				return this.fontStretch;
			}
			set
			{
				if (this.fontStretch != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.fontStretch, value);
					base.RaisePropertyChanging<string>(() => this.FontStretch);
					this.fontStretch = value;
					this.FontStretchChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.FontStretch);
				}
			}
		}
		[Browsable(true), FontStyleEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string FontStyle
		{
			get
			{
				return this.fontStyle;
			}
			set
			{
				if (this.fontStyle != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.fontStyle, value);
					base.RaisePropertyChanging<string>(() => this.FontStyle);
					this.fontStyle = value;
					this.FontStyleChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.FontStyle);
				}
			}
		}
		[Browsable(true), FontWeightEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string FontWeight
		{
			get
			{
				return this.fontWeight;
			}
			set
			{
				if (this.fontWeight != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.fontWeight, value);
					base.RaisePropertyChanging<string>(() => this.FontWeight);
					this.fontWeight = value;
					this.FontWeightChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.FontWeight);
				}
			}
		}
		[Browsable(true), CollectionEditor(typeof(NavigationItem), UseHorizontalEditing = true), PropertyCategory("Navigation", typeof(StandardPropertyCategories))]
		public IObservableCollection<NavigationItem> Navigations
		{
			get
			{
				return this.navigations;
			}
		}
		[Browsable(false)]
		public IObservableCollection<INavigationTargetViewModel> NavigationTargets
		{
			get
			{
				return this.navigationTargetService.NavigationTargets;
			}
		}
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(100.0, 25.0);
			}
		}
		public HMINavigationViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
			this.navigationParametersMonitor = container.Resolve<ICollectionMonitor<NavigationItem>>();
			this.navigationParametersMonitor.Start(this.Navigations);

			this.navigationTargetService = container.Resolve<INavigationTargetService>();
		}
		public override string GetXamlControlName()
		{
			return "HMINavigation";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMINavigation=\"clr-namespace:HMI.RuntimeControls.Navigation;assembly=HMI.RuntimeControls\"",
                "HMIShared=\"clr-namespace:HMI.Runtime.Shared;assembly=HMI.Runtime.Shared\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMINavigation;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("Foreground", base.Foreground);
			base.AddXamlAttribute("SecurityAuthorizationName", this.SecurityAuthorizationName);
			base.AddXamlAttribute("SecurityDenyAccessBehaviour", this.SecurityDenyAccessBehaviour);
			base.AddXamlAttribute("FontFamily", this.FontFamily);
			base.AddXamlAttribute("FontSize", this.FontSize);
			base.AddXamlAttribute("FontStretch", this.FontStretch);
			base.AddXamlAttribute("FontStyle", this.FontStyle);
			base.AddXamlAttribute("FontWeight", this.FontWeight);
			base.AddXamlAttribute("SymbolicText", this.SymbolicText);
		}
		protected override void AddXamlCollections(System.Text.StringBuilder collectionXaml)
		{
			base.AddXamlCollections(collectionXaml);
			base.AddXamlCollection(collectionXaml, "Navigations", this.Navigations, true);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
		}
	}
}
