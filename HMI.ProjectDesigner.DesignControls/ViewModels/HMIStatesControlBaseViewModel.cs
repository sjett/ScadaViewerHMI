// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using System.ComponentModel;
namespace HMI.ProjectDesigner.DesignControls.ViewModels
{
	public class HMIStatesControlBaseViewModel : CommonControlViewModel
	{
		private bool useLocalColors;
		private string objectID;
		private SignalPrefix signalPrefix;
		private string securityAuthorizationName;
		private SecurityDenyAccessBehavior securityDenyAccessBehavior;

		public event EventHandler<ValueChangedEventArgs<bool>> UseLocalColorsChanged;
		public event EventHandler<ValueChangedEventArgs<string>> ObjectIDChanged;
		public event EventHandler<ValueChangedEventArgs<SignalPrefix>> SignalPrefixChanged;
		public event EventHandler<ValueChangedEventArgs<string>> SecurityAuthorizationNameChanged;
		public event EventHandler<ValueChangedEventArgs<SecurityDenyAccessBehavior>> SecurityDenyAccessBehaviorChanged;

		[Browsable(true), EditorOrder(-253.0), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool UseLocalColors
		{
			get
			{
				return useLocalColors;
			}
			set
			{
				if (useLocalColors != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(useLocalColors, value);
					base.RaisePropertyChanging<bool>(() => UseLocalColors);
                    useLocalColors = value;
                    UseLocalColorsChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => UseLocalColors);
				}
			}
		}
		[Browsable(true), DisplayName("ObjectName"), EditorOrder(-248.0), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public string ObjectID
		{
			get
			{
				return objectID;
			}
			set
			{
				if (objectID != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(objectID, value);
					base.RaisePropertyChanging<string>(() => ObjectID);
                    objectID = value;
                    ObjectIDChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => ObjectID);
				}
			}
		}
		[Browsable(true), EditorOrder(-249.0), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public SignalPrefix SignalPrefix
		{
			get
			{
				return signalPrefix;
			}
			set
			{
				if (signalPrefix != value)
				{
					ValueChangedEventArgs<SignalPrefix> e = new ValueChangedEventArgs<SignalPrefix>(signalPrefix, value);
					base.RaisePropertyChanging<SignalPrefix>(() => SignalPrefix);
                    signalPrefix = value;
                    SignalPrefixChanged.Raise(this, e);
					base.RaisePropertyChanged<SignalPrefix>(() => SignalPrefix);
				}
			}
		}
		[Browsable(true), ProjectAuthorizationsBrowser, PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public string SecurityAuthorizationName
		{
			get
			{
				return securityAuthorizationName;
			}
			set
			{
				if (securityAuthorizationName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(securityAuthorizationName, value);
					base.RaisePropertyChanging<string>(() => SecurityAuthorizationName);
                    securityAuthorizationName = value;
                    SecurityAuthorizationNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => SecurityAuthorizationName);
				}
			}
		}
		[Browsable(true), PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public SecurityDenyAccessBehavior SecurityDenyAccessBehaviour
		{
			get
			{
				return securityDenyAccessBehavior;
			}
			set
			{
				if (securityDenyAccessBehavior != value)
				{
					ValueChangedEventArgs<SecurityDenyAccessBehavior> e = new ValueChangedEventArgs<SecurityDenyAccessBehavior>(securityDenyAccessBehavior, value);
					base.RaisePropertyChanging<SecurityDenyAccessBehavior>(() => SecurityDenyAccessBehaviour);
                    securityDenyAccessBehavior = value;
                    SecurityDenyAccessBehaviorChanged.Raise(this, e);
					base.RaisePropertyChanged<SecurityDenyAccessBehavior>(() => SecurityDenyAccessBehaviour);
				}
			}
		}
		public HMIStatesControlBaseViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIInputOutput=\"clr-namespace:HMI.RuntimeControls.InputOutput;assembly=HMI.RuntimeControls\"",
				"HMIShared=\"clr-namespace:HMI.Runtime.Shared;assembly=HMI.Runtime.Shared\""
			};
		}
		//public override IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIInputOutput;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("Background", UseLocalColors ? base.Background : this.Project.StateDefaultBackground);
			base.AddXamlAttribute("Foreground", UseLocalColors ? base.Foreground : this.Project.StateDefaultForeground);
			base.AddXamlAttribute("ObjectID", ObjectID);
			base.AddXamlAttribute("SignalPrefix", base.GetSignalPrefix());
			base.AddXamlAttribute("SecurityAuthorizationName", SecurityAuthorizationName);
			base.AddXamlAttribute("SecurityDenyAccessBehaviour", SecurityDenyAccessBehaviour);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsTemplateEnabled = true;
		}
	}
}
