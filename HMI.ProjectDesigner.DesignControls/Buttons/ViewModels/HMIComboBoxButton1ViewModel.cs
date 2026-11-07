// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using System.ComponentModel;
namespace HMI.ProjectDesigner.DesignControls.Buttons.ViewModels
{
	public class HMIComboBoxButton1ViewModel : CommonControlViewModel
	{
		private readonly ICollectionMonitor<HMIComboBoxItem> comboBoxItemsMonitor;
		private readonly ICollectionMonitor<StateCondition> stateConditionsMonitor;
		private readonly IObservableCollection<StateCondition> states = new FullyObservableCollection<StateCondition>();
		private readonly IObservableCollection<HMIComboBoxItem> writeItems = new FullyObservableCollection<HMIComboBoxItem>();
		private string stateSignalName;
		private string writeSignalName;
		private string objectID;
		private SignalPrefix signalPrefix;
		private int? enableMask;
		private string enableSignalName;
		private int? visibilityMask;
		private string visibilitySignalName;
		private string securityAuthorizationName;
		private SecurityDenyAccessBehavior securityDenyAccessBehavior;
		private bool writeSecure;
		private bool isTabStop = true;
		private System.Windows.Media.Brush borderBrush = System.Windows.Media.Brushes.Black;
		private System.Windows.Thickness borderThickness = new System.Windows.Thickness(1.0);
		private string fontFamily = "Arial";
		private double fontSize = 10.0;
		private string fontStretch = "Normal";
		private string fontStyle = "Normal";
		private string fontWeight = "Normal";
		private bool useLocalColors;

		public event System.EventHandler<ValueChangedEventArgs<string>> StateSignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> WriteSignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> ObjectIDChanged;
		public event System.EventHandler<ValueChangedEventArgs<SignalPrefix>> SignalPrefixChanged;
		public event System.EventHandler<ValueChangedEventArgs<int?>> EnableMaskChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> EnableSignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<int?>> VisibilityMaskChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> VisibilitySignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SecurityAuthorizationNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<SecurityDenyAccessBehavior>> SecurityDenyAccessBehaviorChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> WriteSecureChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> IsTabStopChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> BorderBrushChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Thickness>> BorderThicknessChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontFamilyChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> FontSizeChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontStretchChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontStyleChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontWeightChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> UseLocalColorsChanged;
		
		[Browsable(true), CollectionEditor(typeof(StateCondition), UseHorizontalEditing = true), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public IObservableCollection<StateCondition> States
		{
			get
			{
				return this.states;
			}
		}
		[Browsable(true), CollectionEditor(typeof(HMIComboBoxItem)), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public IObservableCollection<HMIComboBoxItem> WriteItems
		{
			get
			{
				return this.writeItems;
			}
		}
		[Browsable(true), EditorOrder(-250.0), PropertyCategory("Signals", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName
		{
			get
			{
				return this.stateSignalName;
			}
			set
			{
				if (this.stateSignalName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateSignalName, value);
					base.RaisePropertyChanging<string>(() => this.StateSignalName);
					this.stateSignalName = value;
					this.StateSignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateSignalName);
				}
			}
		}
		[Browsable(true), EditorOrder(-250.0), PropertyCategory("Signals", typeof(StandardPropertyCategories)), SignalBrowser]
		public string WriteSignalName
		{
			get
			{
				return this.writeSignalName;
			}
			set
			{
				if (this.writeSignalName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.writeSignalName, value);
					base.RaisePropertyChanging<string>(() => this.WriteSignalName);
					this.writeSignalName = value;
					this.WriteSignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.WriteSignalName);
				}
			}
		}
		[Browsable(true), DisplayName("ObjectName"), EditorOrder(-248.0), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public string ObjectID
		{
			get
			{
				return this.objectID;
			}
			set
			{
				if (this.objectID != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.objectID, value);
					base.RaisePropertyChanging<string>(() => this.ObjectID);
					this.objectID = value;
					this.ObjectIDChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ObjectID);
				}
			}
		}
		[Browsable(true), EditorOrder(-249.0), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public SignalPrefix SignalPrefix
		{
			get
			{
				return this.signalPrefix;
			}
			set
			{
				if (this.signalPrefix != value)
				{
					ValueChangedEventArgs<SignalPrefix> e = new ValueChangedEventArgs<SignalPrefix>(this.signalPrefix, value);
					base.RaisePropertyChanging<SignalPrefix>(() => this.SignalPrefix);
					this.signalPrefix = value;
					this.SignalPrefixChanged.Raise(this, e);
					base.RaisePropertyChanged<SignalPrefix>(() => this.SignalPrefix);
				}
			}
		}
		[PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public int? EnableMask
		{
			get
			{
				return this.enableMask;
			}
			set
			{
				if (this.enableMask != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.enableMask, value);
					base.RaisePropertyChanging<int?>(() => this.EnableMask);
					this.enableMask = value;
					this.EnableMaskChanged.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.EnableMask);
				}
			}
		}
		[Browsable(true), PropertyCategory("Security", typeof(StandardPropertyCategories)), SignalBrowser]
		public string EnableSignalName
		{
			get
			{
				return this.enableSignalName;
			}
			set
			{
				if (this.enableSignalName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.enableSignalName, value);
					base.RaisePropertyChanging<string>(() => this.EnableSignalName);
					this.enableSignalName = value;
					this.EnableSignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.EnableSignalName);
				}
			}
		}
		[PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public int? VisibilityMask
		{
			get
			{
				return this.visibilityMask;
			}
			set
			{
				if (this.visibilityMask != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.visibilityMask, value);
					base.RaisePropertyChanging<int?>(() => this.VisibilityMask);
					this.visibilityMask = value;
					this.VisibilityMaskChanged.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.VisibilityMask);
				}
			}
		}
		[Browsable(true), PropertyCategory("Security", typeof(StandardPropertyCategories)), SignalBrowser]
		public string VisibilitySignalName
		{
			get
			{
				return this.visibilitySignalName;
			}
			set
			{
				if (this.visibilitySignalName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.visibilitySignalName, value);
					base.RaisePropertyChanging<string>(() => this.VisibilitySignalName);
					this.visibilitySignalName = value;
					this.VisibilitySignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.VisibilitySignalName);
				}
			}
		}
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
		[Browsable(true), PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public bool WriteSecure
		{
			get
			{
				return this.writeSecure;
			}
			set
			{
				if (this.writeSecure != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.writeSecure, value);
					base.RaisePropertyChanging<bool>(() => this.WriteSecure);
					this.writeSecure = value;
					this.WriteSecureChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.WriteSecure);
				}
			}
		}
		[Browsable(true)]
		public bool IsTabStop
		{
			get
			{
				return this.isTabStop;
			}
			set
			{
				if (this.isTabStop != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.isTabStop, value);
					base.RaisePropertyChanging<bool>(() => this.IsTabStop);
					this.isTabStop = value;
					this.IsTabStopChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.IsTabStop);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush BorderBrush
		{
			get
			{
				return this.borderBrush;
			}
			set
			{
				if (this.borderBrush != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.BorderBrush);
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.borderBrush, value);
					this.borderBrush = value;
					this.BorderBrushChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.BorderBrush);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Thickness BorderThickness
		{
			get
			{
				return this.borderThickness;
			}
			set
			{
				if (this.borderThickness != value)
				{
					base.RaisePropertyChanging<System.Windows.Thickness>(() => this.BorderThickness);
					ValueChangedEventArgs<System.Windows.Thickness> e = new ValueChangedEventArgs<System.Windows.Thickness>(this.borderThickness, value);
					this.borderThickness = value;
					this.BorderThicknessChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Thickness>(() => this.BorderThickness);
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
		[EditorOrder(-253.0), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool UseLocalColors
		{
			get
			{
				return this.useLocalColors;
			}
			set
			{
				if (this.useLocalColors != value)
				{
					base.RaisePropertyChanging<bool>(() => this.UseLocalColors);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.useLocalColors, value);
					this.useLocalColors = value;
					this.UseLocalColorsChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.UseLocalColors);
				}
			}
		}
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(100.0, 25.0);
			}
		}
		public HMIComboBoxButton1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
			this.comboBoxItemsMonitor = container.Resolve<ICollectionMonitor<HMIComboBoxItem>>();
			this.comboBoxItemsMonitor.Start(this.WriteItems);
			this.stateConditionsMonitor = container.Resolve<ICollectionMonitor<StateCondition>>();
			this.stateConditionsMonitor.Start(this.States);
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIButtons=\"clr-namespace:HMI.RuntimeControls.Buttons;assembly=HMI.RuntimeControls\"",
                "HMIShared=\"clr-namespace:HMI.Runtime.Shared;assembly=HMI.Runtime.Shared\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIButtons;
		//}
		public override string GetXamlControlName()
		{
			return "HMIComboBoxButton1";
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("StateSignalName", this.StateSignalName);
			base.AddXamlAttribute("WriteSignalName", this.WriteSignalName);
			base.AddXamlAttribute("ObjectID", this.ObjectID);
			base.AddXamlAttribute("SignalPrefix", base.GetSignalPrefix());
			base.AddXamlAttribute("EnableMask", this.EnableMask);
			base.AddXamlAttribute("EnableSignalName", this.EnableSignalName);
			base.AddXamlAttribute("VisibilityMask", this.VisibilityMask);
			base.AddXamlAttribute("VisibilitySignalName", this.VisibilitySignalName);
			base.AddXamlAttribute("SecurityAuthorizationName", this.SecurityAuthorizationName);
			base.AddXamlAttribute("SecurityDenyAccessBehaviour", this.SecurityDenyAccessBehaviour);
			base.AddXamlAttribute("WriteSecure", this.WriteSecure);
			base.AddXamlAttribute("IsTabStop", this.IsTabStop);
			base.AddXamlAttribute("BorderBrush", this.BorderBrush);
			base.AddXamlAttribute("BorderThickness", this.BorderThickness);
			base.AddXamlAttribute("FontFamily", this.FontFamily);
			base.AddXamlAttribute("FontSize", this.FontSize);
			base.AddXamlAttribute("FontStretch", this.FontStretch);
			base.AddXamlAttribute("FontStyle", this.FontStyle);
			base.AddXamlAttribute("FontWeight", this.FontWeight);
			base.AddXamlAttribute("Background", this.UseLocalColors ? base.Background : this.Project.StateDefaultBackground);
			base.AddXamlAttribute("Foreground", this.UseLocalColors ? base.Foreground : this.Project.StateDefaultForeground);
		}
		protected override void AddXamlCollections(System.Text.StringBuilder collectionXaml)
		{
			base.AddXamlCollections(collectionXaml);
			base.AddXamlCollection(collectionXaml, "WriteItems", this.WriteItems, true);
			base.AddXamlCollection(collectionXaml, "States", this.States, true);
		}
	}
}
