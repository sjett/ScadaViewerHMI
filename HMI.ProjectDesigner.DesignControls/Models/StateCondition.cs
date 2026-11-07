// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using System.ComponentModel;
namespace HMI.Infrastructure.Models
{
	public class StateCondition : CollectionItem
	{
		private string stateName;
		private string signalName;
		private StatesConditionComparator comparator = StatesConditionComparator.Equal;
		private string signalValue = "0";
		private System.Windows.Media.Brush background = (System.Windows.Media.Brush)System.Windows.Media.Brushes.White.GetAsFrozen();
		private System.Windows.Media.Brush foreground = (System.Windows.Media.Brush)System.Windows.Media.Brushes.Black.GetAsFrozen();
		private bool isVisible = true;
		private bool isEnabled = true;
		private bool isBlinking;
		private StateBlinkingFrequency blinkingFrequency;
		private System.Windows.Media.Brush alternateBackground = (System.Windows.Media.Brush)System.Windows.Media.Brushes.White.GetAsFrozen();
		private System.Windows.Media.Brush alternateForeground = (System.Windows.Media.Brush)System.Windows.Media.Brushes.Black.GetAsFrozen();
		private string signalPrefix;
		private readonly IContentReplacementService contentReplacementService;

		public event System.EventHandler<ValueChangedEventArgs<string>> StateNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<StatesConditionComparator>> ComparatorChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SignalValueChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> BackgroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> ForegroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> IsVisibleChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> IsEnabledChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> IsBlinkingChanged;
		public event System.EventHandler<ValueChangedEventArgs<StateBlinkingFrequency>> BlinkingFrequencyChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> AlternateBackgroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> AlternateForegroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SignalPrefixChanged;

		[Browsable(true), EditorOrder(0.0)]
		public string StateName
		{
			get
			{
				return this.stateName;
			}
			set
			{
				if (this.stateName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateName, value);
					base.RaisePropertyChanging<string>(() => this.StateName);
					this.stateName = value;
					this.StateNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateName);
				}
			}
		}
		[Browsable(true), EditorOrder(1.0), SignalBrowser]
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
		[Browsable(true), EditorOrder(2.0)]
		public StatesConditionComparator Comparator
		{
			get
			{
				return this.comparator;
			}
			set
			{
				if (this.comparator != value)
				{
					ValueChangedEventArgs<StatesConditionComparator> e = new ValueChangedEventArgs<StatesConditionComparator>(this.comparator, value);
					base.RaisePropertyChanging<StatesConditionComparator>(() => this.Comparator);
					this.comparator = value;
					this.ComparatorChanged.Raise(this, e);
					base.RaisePropertyChanged<StatesConditionComparator>(() => this.Comparator);
					this.SetConditionProperty();
				}
			}
		}
		[Browsable(true), EditorOrder(3.0)]
		public string SignalValue
		{
			get
			{
				return this.signalValue;
			}
			set
			{
				if (this.signalValue != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.signalValue, value);
					base.RaisePropertyChanging<string>(() => this.SignalValue);
					this.signalValue = value;
					this.SignalValueChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.SignalValue);
					this.SetConditionProperty();
				}
			}
		}
		[Browsable(true), EditorOrder(4.0)]
		public System.Windows.Media.Brush Background
		{
			get
			{
				return this.background;
			}
			set
			{
				if (this.background != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.background, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.Background);
					this.background = value;
					this.BackgroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.Background);
				}
			}
		}
		[Browsable(true), EditorOrder(5.0)]
		public System.Windows.Media.Brush Foreground
		{
			get
			{
				return this.foreground;
			}
			set
			{
				if (this.foreground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.foreground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.Foreground);
					this.foreground = value;
					this.ForegroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.Foreground);
				}
			}
		}
		[EditorOrder(10.0)]
		public bool IsVisible
		{
			get
			{
				return this.isVisible;
			}
			set
			{
				if (this.isVisible != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.isVisible, value);
					base.RaisePropertyChanging<bool>(() => this.IsVisible);
					this.isVisible = value;
					this.IsVisibleChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.IsVisible);
					base.RaisePropertyChanged<bool>(() => this.IsBlinkingEnabled);
				}
			}
		}
		[EditorOrder(11.0)]
		public bool IsEnabled
		{
			get
			{
				return this.isEnabled;
			}
			set
			{
				if (this.isEnabled != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.isEnabled, value);
					base.RaisePropertyChanging<bool>(() => this.IsEnabled);
					this.isEnabled = value;
					this.IsEnabledChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.IsEnabled);
				}
			}
		}
		[Browsable(true), EditorOrder(12.0), EnabledProperty("IsVisible")]
		public bool IsBlinking
		{
			get
			{
				return this.isBlinking;
			}
			set
			{
				if (this.isBlinking != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.isBlinking, value);
					base.RaisePropertyChanging<bool>(() => this.IsBlinking);
					this.isBlinking = value;
					this.IsBlinkingChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.IsBlinking);
					base.RaisePropertyChanged<bool>(() => this.IsBlinkingEnabled);
				}
			}
		}
		[Browsable(true), EditorOrder(13.0), EnabledProperty("IsBlinkingEnabled")]
		public StateBlinkingFrequency BlinkingFrequency
		{
			get
			{
				return this.blinkingFrequency;
			}
			set
			{
				if (this.blinkingFrequency != value)
				{
					ValueChangedEventArgs<StateBlinkingFrequency> e = new ValueChangedEventArgs<StateBlinkingFrequency>(this.blinkingFrequency, value);
					base.RaisePropertyChanging<StateBlinkingFrequency>(() => this.BlinkingFrequency);
					this.blinkingFrequency = value;
					this.BlinkingFrequencyChanged.Raise(this, e);
					base.RaisePropertyChanged<StateBlinkingFrequency>(() => this.BlinkingFrequency);
				}
			}
		}
		[Browsable(true), EditorOrder(14.0), EnabledProperty("IsBlinkingEnabled")]
		public System.Windows.Media.Brush AlternateBackground
		{
			get
			{
				return this.alternateBackground;
			}
			set
			{
				if (this.alternateBackground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.alternateBackground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.AlternateBackground);
					this.alternateBackground = value;
					this.AlternateBackgroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.AlternateBackground);
				}
			}
		}
		[Browsable(true), EditorOrder(15.0), EnabledProperty("IsBlinkingEnabled")]
		public System.Windows.Media.Brush AlternateForeground
		{
			get
			{
				return this.alternateForeground;
			}
			set
			{
				if (this.alternateForeground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.alternateForeground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.AlternateForeground);
					this.alternateForeground = value;
					this.AlternateForegroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.AlternateForeground);
				}
			}
		}
		[Browsable(false)]
		public string SignalPrefix
		{
			get
			{
				return this.signalPrefix;
			}
			set
			{
				if (this.signalPrefix != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.signalPrefix, value);
					base.RaisePropertyChanging<string>(() => this.SignalPrefix);
					this.signalPrefix = value;
					this.SignalPrefixChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.SignalPrefix);
				}
			}
		}
		[Browsable(false)]
		public string Condition
		{
			get;
			set;
		}
		[Browsable(false)]
		public string RealBlinkingFrequency
		{
			get
			{
				IProjectManagerViewModel projectManagerViewModel = base.Services.Resolve<IProjectManagerViewModel>();
				IProjectViewModel project = projectManagerViewModel.Project;
				switch (this.BlinkingFrequency)
				{
				case StateBlinkingFrequency.Fast:
					return project.FastBlinkSpeed.ToString(System.Globalization.CultureInfo.InvariantCulture);
				case StateBlinkingFrequency.Slow:
					return project.SlowBlinkSpeed.ToString(System.Globalization.CultureInfo.InvariantCulture);
				}
				return project.MediumBlinkSpeed.ToString(System.Globalization.CultureInfo.InvariantCulture);
			}
		}
		[Browsable(false)]
		public bool IsBlinkingEnabled
		{
			get
			{
				return this.IsVisible && this.IsBlinking;
			}
		}
		protected virtual string Definition
		{
			get
			{
				return "HMIStateCondition";
			}
		}
		public StateCondition(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
			this.contentReplacementService = container.Resolve<IContentReplacementService>();
			this.SetConditionProperty();
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			StateCondition stateCondition = target as StateCondition;
			stateCondition.AlternateBackground = this.AlternateBackground;
			stateCondition.AlternateForeground = this.AlternateForeground;
			stateCondition.Background = this.Background;
			stateCondition.BlinkingFrequency = this.BlinkingFrequency;
			stateCondition.Comparator = this.Comparator;
			stateCondition.Foreground = this.Foreground;
			stateCondition.IsBlinking = this.IsBlinking;
			stateCondition.IsVisible = this.IsVisible;
			stateCondition.IsEnabled = this.IsEnabled;
			if (this.SignalName != null)
			{
				stateCondition.SignalName = (this.SignalName.Clone() as string);
			}
			stateCondition.SignalValue = this.SignalValue;
			stateCondition.StateName = this.StateName;
			stateCondition.SignalPrefix = this.SignalPrefix;
		}
		public override ICollectionItem CreateClone()
		{
			return new StateCondition(base.Services, base.Parent);
		}

		public override string GetXaml()
		{
			string conditionParameterXaml = this.GetConditionParameterXaml("HMIShared");
			string templateFormat = this.GetTemplateFormat("HMIShared", this.Definition, conditionParameterXaml);
			return this.contentReplacementService.ReplaceStringContent(templateFormat, this);
		}
		private string GetConditionParameterXaml(string prefix)
		{
			string arg = "HMIStateConditionParameter";
			string text = string.Format("<{0}:{1} ParameterName=\"$Input1\" SignalName=\"[SignalName]\"></{0}:{1}>", prefix, arg);
			return this.contentReplacementService.ReplaceStringContent(text, this);
		}

		private void SetConditionProperty()
		{
			switch (this.Comparator)
			{
			case StatesConditionComparator.Different:
				this.Condition = string.Format("($Input1 {0} {1})", "!=", this.SignalValue);
				break;
			case StatesConditionComparator.Equal:
				this.Condition = string.Format("($Input1 {0} {1})", "==", this.SignalValue);
				break;
			case StatesConditionComparator.GreaterThan:
				this.Condition = string.Format("($Input1 {0} {1})", ">", this.SignalValue);
				break;
			case StatesConditionComparator.GreaterThanOrEqual:
				this.Condition = string.Format("($Input1 {0} {1})", ">=", this.SignalValue);
				break;
			case StatesConditionComparator.LessThan:
				this.Condition = string.Format("($Input1 {0} {1})", "<", this.SignalValue);
				break;
			case StatesConditionComparator.LessThanOrEqual:
				this.Condition = string.Format("($Input1 {0} {1})", "<=", this.SignalValue);
				break;
			case StatesConditionComparator.AndWord:
			case StatesConditionComparator.AndDWord:
				this.Condition = string.Format("(($Input1 {0} {1}) == {1})", "&", this.SignalValue);
				break;
			}
			this.Condition = System.Security.SecurityElement.Escape(this.Condition);
		}
		protected virtual string GetTemplateFormat(string prefix, string definition, string conditionParameter)
		{
			return string.Format("<{0}:{1} Background=\"[Background]\" AlternateBackground=\"[AlternateBackground]\" Condition=\"[Condition]\" ConditionType=\"Equation\" Foreground=\"[Foreground]\" AlternateForeground=\"[AlternateForeground]\" IsBlinking=\"[IsBlinking]\" BlinkingFrequency=\"[RealBlinkingFrequency]\" StateName=\"[StateName]\" IsVisible=\"[IsVisible]\" IsEnabled=\"[IsEnabled]\">{3}" + "<{0}:{1}.ConditionParameters>{3}{2}{3}</{0}:{1}.ConditionParameters>{3}</{0}:{1}>", new object[]
			{
				prefix,
				definition,
				conditionParameter,
				System.Environment.NewLine
			});
		}
	}
}
