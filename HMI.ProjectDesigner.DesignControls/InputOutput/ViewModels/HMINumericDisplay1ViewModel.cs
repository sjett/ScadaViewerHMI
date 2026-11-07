// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
namespace HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels
{
	public class HMINumericDisplay1ViewModel : CommonControlViewModel
	{
		private int precision = 2;
		private string unit;
		private string fontFamily = "Arial";
		private double fontSize = 10.0;
		private string fontStretch = "Normal";
		private string fontStyle = "Normal";
		private string fontWeight = "Normal";
		private System.Windows.HorizontalAlignment horizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
		private System.Windows.VerticalAlignment verticalContentAlignment = System.Windows.VerticalAlignment.Center;
		private string securityAuthorizationName;
		private SecurityDenyAccessBehavior securityDenyAccessBehavior;
		private string visibilitySignalName;
		private int? visibilityMask;
		private bool binaryMask;
		private string conditionRule1;
		private string conditionRule2;
		private string conditionRule3;
		private string conditionRule4;
		private readonly System.Collections.ObjectModel.ObservableCollection<ConditionSignal> conditionSignalsContents = new System.Collections.ObjectModel.ObservableCollection<ConditionSignal>();
		private int? maskSignal1;
		private int? maskSignal2;
		private int? maskSignal3;
		private int? maskSignal4;
		private string limitSignalName1;
		private string limitSignalName2;
		private string limitSignalName3;
		private string limitSignalName4;
		private string objectID;
		private SignalPrefix signalPrefix;
		private string signalName;
		public event System.EventHandler<ValueChangedEventArgs<int>> PrecisionChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> UnitChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontFamilyChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> FontSizeChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontStretchChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontStyleChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontWeightChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.HorizontalAlignment>> TextHorizontalAlignmentChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.VerticalAlignment>> TextVerticalAlignmentChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SecurityAuthorizationNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<SecurityDenyAccessBehavior>> SecurityDenyAccessBehaviorChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> VisibilitySignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<int?>> VisibilityMaskChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> BinaryMaskChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRule1Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRule2Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRule3Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRule4Changed;
		public event System.EventHandler<ValueChangedEventArgs<int?>> MaskSignal1Changed;
		public event System.EventHandler<ValueChangedEventArgs<int?>> MaskSignal2Changed;
		public event System.EventHandler<ValueChangedEventArgs<int?>> MaskSignal3Changed;
		public event System.EventHandler<ValueChangedEventArgs<int?>> MarkSignal4Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> LimitSignalName1Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> LimitSignalName2Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> LimitSignalName3Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> LimitSignalName4Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> ObjectIDChanged;
		public event System.EventHandler<ValueChangedEventArgs<SignalPrefix>> SignalPrefixChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SignalNameChanged;
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue(30.0), MinimumValue(0.0)]
		public int Precision
		{
			get
			{
				return this.precision;
			}
			set
			{
				if (this.precision != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.precision, value);
					base.RaisePropertyChanging<int>(() => this.Precision);
					this.precision = value;
					this.PrecisionChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.Precision);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string Unit
		{
			get
			{
				return this.unit;
			}
			set
			{
				if (this.unit != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.unit, value);
					base.RaisePropertyChanging<string>(() => this.Unit);
					this.unit = value;
					this.UnitChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.Unit);
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
		[Browsable(true), EditorOrder(-235.0), PropertyCategory("Layout", typeof(StandardPropertyCategories))]
		public System.Windows.HorizontalAlignment HorizontalContentAlignment
		{
			get
			{
				return this.horizontalContentAlignment;
			}
			set
			{
				if (this.horizontalContentAlignment != value)
				{
					ValueChangedEventArgs<System.Windows.HorizontalAlignment> e = new ValueChangedEventArgs<System.Windows.HorizontalAlignment>(this.horizontalContentAlignment, value);
					base.RaisePropertyChanging<System.Windows.HorizontalAlignment>(() => this.HorizontalContentAlignment);
					this.horizontalContentAlignment = value;
					this.TextHorizontalAlignmentChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.HorizontalAlignment>(() => this.HorizontalContentAlignment);
				}
			}
		}
		[Browsable(true), EditorOrder(-234.0), PropertyCategory("Layout", typeof(StandardPropertyCategories))]
		public System.Windows.VerticalAlignment VerticalContentAlignment
		{
			get
			{
				return this.verticalContentAlignment;
			}
			set
			{
				if (this.verticalContentAlignment != value)
				{
					ValueChangedEventArgs<System.Windows.VerticalAlignment> e = new ValueChangedEventArgs<System.Windows.VerticalAlignment>(this.verticalContentAlignment, value);
					base.RaisePropertyChanging<System.Windows.VerticalAlignment>(() => this.VerticalContentAlignment);
					this.verticalContentAlignment = value;
					this.TextVerticalAlignmentChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.VerticalAlignment>(() => this.VerticalContentAlignment);
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
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public bool BinaryMask
		{
			get
			{
				return this.binaryMask;
			}
			set
			{
				if (this.binaryMask != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.binaryMask, value);
					base.RaisePropertyChanging<bool>(() => this.BinaryMask);
					this.binaryMask = value;
					this.BinaryMaskChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.BinaryMask);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule1
		{
			get
			{
				return this.conditionRule1;
			}
			set
			{
				if (this.conditionRule1 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.conditionRule1, value);
					base.RaisePropertyChanging<string>(() => this.ConditionRule1);
					this.conditionRule1 = value;
					this.ConditionRule1Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ConditionRule1);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule2
		{
			get
			{
				return this.conditionRule2;
			}
			set
			{
				if (this.conditionRule2 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.conditionRule2, value);
					base.RaisePropertyChanging<string>(() => this.ConditionRule2);
					this.conditionRule2 = value;
					this.ConditionRule2Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ConditionRule2);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule3
		{
			get
			{
				return this.conditionRule3;
			}
			set
			{
				if (this.conditionRule3 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.conditionRule3, value);
					base.RaisePropertyChanging<string>(() => this.ConditionRule3);
					this.conditionRule3 = value;
					this.ConditionRule3Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ConditionRule3);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule4
		{
			get
			{
				return this.conditionRule4;
			}
			set
			{
				if (this.conditionRule4 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.conditionRule4, value);
					base.RaisePropertyChanging<string>(() => this.ConditionRule4);
					this.conditionRule4 = value;
					this.ConditionRule4Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ConditionRule4);
				}
			}
		}
		[Browsable(true), CollectionEditor(typeof(ConditionSignal)), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public System.Collections.ObjectModel.ObservableCollection<ConditionSignal> ConditionSignalsContents
		{
			get
			{
				return this.conditionSignalsContents;
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal1
		{
			get
			{
				return this.maskSignal1;
			}
			set
			{
				if (this.maskSignal1 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.maskSignal1, value);
					base.RaisePropertyChanging<int?>(() => this.MaskSignal1);
					this.maskSignal1 = value;
					this.MaskSignal1Changed.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.MaskSignal1);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal2
		{
			get
			{
				return this.maskSignal2;
			}
			set
			{
				if (this.maskSignal2 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.maskSignal2, value);
					base.RaisePropertyChanging<int?>(() => this.MaskSignal2);
					this.maskSignal2 = value;
					this.MaskSignal2Changed.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.MaskSignal2);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal3
		{
			get
			{
				return this.maskSignal3;
			}
			set
			{
				if (this.maskSignal3 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.maskSignal3, value);
					base.RaisePropertyChanging<int?>(() => this.MaskSignal3);
					this.maskSignal3 = value;
					this.MaskSignal3Changed.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.MaskSignal3);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal4
		{
			get
			{
				return this.maskSignal4;
			}
			set
			{
				if (this.maskSignal4 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.maskSignal4, value);
					base.RaisePropertyChanging<int?>(() => this.MaskSignal4);
					this.maskSignal4 = value;
					this.MarkSignal4Changed.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.MaskSignal4);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string LimitSignalName1
		{
			get
			{
				return this.limitSignalName1;
			}
			set
			{
				if (this.limitSignalName1 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.limitSignalName1, value);
					base.RaisePropertyChanging<string>(() => this.LimitSignalName1);
					this.limitSignalName1 = value;
					this.LimitSignalName1Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.LimitSignalName1);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string LimitSignalName2
		{
			get
			{
				return this.limitSignalName2;
			}
			set
			{
				if (this.limitSignalName2 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.limitSignalName2, value);
					base.RaisePropertyChanging<string>(() => this.LimitSignalName2);
					this.limitSignalName2 = value;
					this.LimitSignalName2Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.LimitSignalName2);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string LimitSignalName3
		{
			get
			{
				return this.limitSignalName3;
			}
			set
			{
				if (this.limitSignalName3 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.limitSignalName3, value);
					base.RaisePropertyChanging<string>(() => this.LimitSignalName3);
					this.limitSignalName3 = value;
					this.LimitSignalName3Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.LimitSignalName3);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string LimitSignalName4
		{
			get
			{
				return this.limitSignalName4;
			}
			set
			{
				if (this.limitSignalName4 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.limitSignalName4, value);
					base.RaisePropertyChanging<string>(() => this.LimitSignalName4);
					this.limitSignalName4 = value;
					this.LimitSignalName4Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.LimitSignalName4);
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
		[Browsable(true), EditorOrder(-250.0), PropertyCategory("Signals", typeof(StandardPropertyCategories)), SignalBrowser]
		public string SignalName
		{
			get
			{
				return this.signalName;
			}
			set
			{
				if (this.signalName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.signalName, value);
					base.RaisePropertyChanging<string>(() => this.SignalName);
					this.signalName = value;
					this.SignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.SignalName);
				}
			}
		}
		[Browsable(false)]
		public string ResolvedSignalName
		{
			get
			{
				return base.GetFullSignalName(this.SignalName);
			}
		}
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(110.0, 25.0);
			}
		}
		public HMINumericDisplay1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
			base.RegisterResolvedSignalNameProperty(() => this.SignalName, () => this.ResolvedSignalName);
		}
		public override string GetXamlControlName()
		{
			return "HMINumericDisplay1";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIInputOutput=\"clr-namespace:HMI.RuntimeControls.InputOutput;assembly=HMI.RuntimeControls\"",
                "HMIShared=\"clr-namespace:HMI.Runtime.Shared;assembly=HMI.Runtime.Shared\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIInputOutput;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("Precision", this.Precision);
			base.AddXamlAttribute("Unit", this.Unit);
			base.AddXamlAttribute("FontFamily", this.FontFamily);
			base.AddXamlAttribute("FontSize", this.FontSize);
			base.AddXamlAttribute("FontStretch", this.FontStretch);
			base.AddXamlAttribute("FontStyle", this.FontStyle);
			base.AddXamlAttribute("FontWeight", this.FontWeight);
			base.AddXamlAttribute("HorizontalContentAlignment", this.HorizontalContentAlignment);
			base.AddXamlAttribute("VerticalContentAlignment", this.VerticalContentAlignment);
			base.AddXamlAttribute("SecurityAuthorizationName", this.SecurityAuthorizationName);
			base.AddXamlAttribute("SecurityDenyAccessBehaviour", this.SecurityDenyAccessBehaviour);
			base.AddXamlAttribute("VisibilityMask", this.VisibilityMask);
			base.AddXamlAttribute("VisibilitySignalName", this.VisibilitySignalName);
			base.AddXamlAttribute("BinaryMask", this.BinaryMask);
			base.AddXamlAttribute("ConditionRule1", this.ConditionRule1);
			base.AddXamlAttribute("ConditionRule2", this.ConditionRule2);
			base.AddXamlAttribute("ConditionRule3", this.ConditionRule3);
			base.AddXamlAttribute("ConditionRule4", this.ConditionRule4);
			base.AddXamlAttribute("MaskSignal1", this.MaskSignal1);
			base.AddXamlAttribute("MaskSignal2", this.MaskSignal2);
			base.AddXamlAttribute("MaskSignal3", this.MaskSignal3);
			base.AddXamlAttribute("MaskSignal4", this.MaskSignal4);
			base.AddXamlAttribute("LimitSignalName1", this.LimitSignalName1);
			base.AddXamlAttribute("LimitSignalName2", this.LimitSignalName2);
			base.AddXamlAttribute("LimitSignalName3", this.LimitSignalName3);
			base.AddXamlAttribute("LimitSignalName4", this.LimitSignalName4);
			base.AddXamlAttribute("Foreground", base.Foreground);
			base.AddXamlAttribute("ObjectID", this.ObjectID);
			base.AddXamlAttribute("SignalPrefix", base.GetSignalPrefix());
			base.AddXamlAttribute("SignalName", this.SignalName);
		}
		protected override void AddXamlCollections(System.Text.StringBuilder collectionXaml)
		{
			base.AddXamlCollections(collectionXaml);
			base.AddXamlCollection(collectionXaml, "ConditionSignals", this.ConditionSignalsContents, true);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsTemplateEnabled = true;
		}
	}
}
