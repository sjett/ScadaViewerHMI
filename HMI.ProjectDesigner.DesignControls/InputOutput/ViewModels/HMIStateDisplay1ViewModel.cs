// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
namespace HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels
{
	public class HMIStateDisplay1ViewModel : CommonControlViewModel
	{
		private readonly ICollectionMonitor<ConditionSignal> conditionSignalsMonitor;
		private bool useLocalColors;
		private string objectID;
		private SignalPrefix signalPrefix;
		private string securityAuthorizationName;
		private SecurityDenyAccessBehavior securityDenyAccessBehavior;
		private string visibilitySignalName;
		private int? visibilityMask;
		private bool binaryMask;
		private string conditionRule1;
		private string conditionRule2;
		private string conditionRule3;
		private string conditionRule4;
		private string conditionRule5;
		private string conditionRule6;
		private string conditionRule7;
		private string conditionRule8;
		private readonly IObservableCollection<ConditionSignal> conditionSignals = new FullyObservableCollection<ConditionSignal>();
		private int? maskSignal1;
		private int? maskSignal2;
		private int? maskSignal3;
		private int? maskSignal4;
		private int? maskSignal5;
		private int? maskSignal6;
		private int? maskSignal7;
		private int? maskSignal8;
		private string stateSignalName1;
		private string stateSignalName2;
		private string stateSignalName3;
		private string stateSignalName4;
		private string stateSignalName5;
		private string stateSignalName6;
		private string stateSignalName7;
		private string stateSignalName8;
		private string symbolicTextNormalState;
		private bool registerSymbolicTextState = true;
		private string symbolicTextState1;
		private string symbolicTextState2;
		private string symbolicTextState3;
		private string symbolicTextState4;
		private string symbolicTextState5;
		private string symbolicTextState6;
		private string symbolicTextState7;
		private string symbolicTextState8;
		private string fontFamily = "Arial";
		private double fontSize = 10.0;
		private string fontStretch = "Normal";
		private string fontStyle = "Normal";
		private string fontWeight = "Normal";

		public event EventHandler<ValueChangedEventArgs<bool>> UseLocalColorsChanged;
		public event EventHandler<ValueChangedEventArgs<string>> ObjectIDChanged;
		public event EventHandler<ValueChangedEventArgs<SignalPrefix>> SignalPrefixChanged;
		public event EventHandler<ValueChangedEventArgs<string>> SecurityAuthorizationNameChanged;
		public event EventHandler<ValueChangedEventArgs<SecurityDenyAccessBehavior>> SecurityDenyAccessBehaviorChanged;
		public event EventHandler<ValueChangedEventArgs<string>> VisibilitySignalNameChanged;
		public event EventHandler<ValueChangedEventArgs<int?>> VisibilityMaskChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> BinaryMaskChanged;
		public event EventHandler<ValueChangedEventArgs<string>> ConditionRule1Changed;
		public event EventHandler<ValueChangedEventArgs<string>> ConditionRule2Changed;
		public event EventHandler<ValueChangedEventArgs<string>> ConditionRule3Changed;
		public event EventHandler<ValueChangedEventArgs<string>> ConditionRule4Changed;
		public event EventHandler<ValueChangedEventArgs<string>> ConditionRule5Changed;
		public event EventHandler<ValueChangedEventArgs<string>> ConditionRule6Changed;
		public event EventHandler<ValueChangedEventArgs<string>> ConditionRule7Changed;
		public event EventHandler<ValueChangedEventArgs<string>> ConditionRule8Changed;
		public event EventHandler<ValueChangedEventArgs<int?>> MaskSignal1Changed;
		public event EventHandler<ValueChangedEventArgs<int?>> MaskSignal2Changed;
		public event EventHandler<ValueChangedEventArgs<int?>> MaskSignal3Changed;
		public event EventHandler<ValueChangedEventArgs<int?>> MaskSignal4Changed;
		public event EventHandler<ValueChangedEventArgs<int?>> MaskSignal5Changed;
		public event EventHandler<ValueChangedEventArgs<int?>> MaskSignal6Changed;
		public event EventHandler<ValueChangedEventArgs<int?>> MaskSignal7Changed;
		public event EventHandler<ValueChangedEventArgs<int?>> MaskSignal8Changed;
		public event EventHandler<ValueChangedEventArgs<string>> StateSignalName1Changed;
		public event EventHandler<ValueChangedEventArgs<string>> StateSignalName2Changed;
		public event EventHandler<ValueChangedEventArgs<string>> StateSignalName3Changed;
		public event EventHandler<ValueChangedEventArgs<string>> StateSignalName4Changed;
		public event EventHandler<ValueChangedEventArgs<string>> StateSignalName5Changed;
		public event EventHandler<ValueChangedEventArgs<string>> StateSignalName6Changed;
		public event EventHandler<ValueChangedEventArgs<string>> StateSignalName7Changed;
		public event EventHandler<ValueChangedEventArgs<string>> StateSignalName8Changed;
		public event EventHandler<ValueChangedEventArgs<string>> SymbolicTextNormalStateChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> RegisterSymbolicTextStateChanged;
		public event EventHandler<ValueChangedEventArgs<string>> SymbolicTextState1Changed;
		public event EventHandler<ValueChangedEventArgs<string>> SymbolicTextState2Changed;
		public event EventHandler<ValueChangedEventArgs<string>> SymbolicTextState3Changed;
		public event EventHandler<ValueChangedEventArgs<string>> SymbolicTextState4Changed;
		public event EventHandler<ValueChangedEventArgs<string>> SymbolicTextState5Changed;
		public event EventHandler<ValueChangedEventArgs<string>> SymbolicTextState6Changed;
		public event EventHandler<ValueChangedEventArgs<string>> SymbolicTextState7Changed;
		public event EventHandler<ValueChangedEventArgs<string>> SymbolicTextState8Changed;
		public event EventHandler<ValueChangedEventArgs<string>> FontFamilyChanged;
		public event EventHandler<ValueChangedEventArgs<double>> FontSizeChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontStretchChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontStyleChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontWeightChanged;

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
					RaisePropertyChanging(() => UseLocalColors);
					useLocalColors = value;
					UseLocalColorsChanged.Raise(this, e);
					RaisePropertyChanged(() => UseLocalColors);
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
					RaisePropertyChanging(() => ObjectID);
					objectID = value;
					ObjectIDChanged.Raise(this, e);
					RaisePropertyChanged(() => ObjectID);
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
					RaisePropertyChanging(() => SignalPrefix);
					signalPrefix = value;
					SignalPrefixChanged.Raise(this, e);
					RaisePropertyChanged(() => SignalPrefix);
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
					RaisePropertyChanging(() => SecurityAuthorizationName);
					securityAuthorizationName = value;
					SecurityAuthorizationNameChanged.Raise(this, e);
					RaisePropertyChanged(() => SecurityAuthorizationName);
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
					RaisePropertyChanging(() => SecurityDenyAccessBehaviour);
					securityDenyAccessBehavior = value;
					SecurityDenyAccessBehaviorChanged.Raise(this, e);
					RaisePropertyChanged(() => SecurityDenyAccessBehaviour);
				}
			}
		}
		[Browsable(true), PropertyCategory("Security", typeof(StandardPropertyCategories)), SignalBrowser]
		public string VisibilitySignalName
		{
			get
			{
				return visibilitySignalName;
			}
			set
			{
				if (visibilitySignalName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(visibilitySignalName, value);
					RaisePropertyChanging(() => VisibilitySignalName);
					visibilitySignalName = value;
					VisibilitySignalNameChanged.Raise(this, e);
					RaisePropertyChanged(() => VisibilitySignalName);
				}
			}
		}
		[PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public int? VisibilityMask
		{
			get
			{
				return visibilityMask;
			}
			set
			{
				if (visibilityMask != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(visibilityMask, value);
					RaisePropertyChanging(() => VisibilityMask);
					visibilityMask = value;
					VisibilityMaskChanged.Raise(this, e);
					RaisePropertyChanged(() => VisibilityMask);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public bool BinaryMask
		{
			get
			{
				return binaryMask;
			}
			set
			{
				if (binaryMask != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(binaryMask, value);
					RaisePropertyChanging(() => BinaryMask);
					binaryMask = value;
					BinaryMaskChanged.Raise(this, e);
					RaisePropertyChanged(() => BinaryMask);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule1
		{
			get
			{
				return conditionRule1;
			}
			set
			{
				if (conditionRule1 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(conditionRule1, value);
					RaisePropertyChanging(() => ConditionRule1);
					conditionRule1 = value;
					ConditionRule1Changed.Raise(this, e);
					RaisePropertyChanged(() => ConditionRule1);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule2
		{
			get
			{
				return conditionRule2;
			}
			set
			{
				if (conditionRule2 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(conditionRule2, value);
					RaisePropertyChanging(() => ConditionRule2);
					conditionRule2 = value;
					ConditionRule2Changed.Raise(this, e);
					RaisePropertyChanged(() => ConditionRule2);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule3
		{
			get
			{
				return conditionRule3;
			}
			set
			{
				if (conditionRule3 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(conditionRule3, value);
					RaisePropertyChanging(() => ConditionRule3);
					conditionRule3 = value;
					ConditionRule3Changed.Raise(this, e);
					RaisePropertyChanged(() => ConditionRule3);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule4
		{
			get
			{
				return conditionRule4;
			}
			set
			{
				if (conditionRule4 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(conditionRule4, value);
					RaisePropertyChanging(() => ConditionRule4);
					conditionRule4 = value;
					ConditionRule4Changed.Raise(this, e);
					RaisePropertyChanged(() => ConditionRule4);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule5
		{
			get
			{
				return conditionRule5;
			}
			set
			{
				if (conditionRule5 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(conditionRule5, value);
					RaisePropertyChanging(() => ConditionRule5);
					conditionRule5 = value;
					ConditionRule5Changed.Raise(this, e);
					RaisePropertyChanged(() => ConditionRule5);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule6
		{
			get
			{
				return conditionRule6;
			}
			set
			{
				if (conditionRule6 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(conditionRule6, value);
					RaisePropertyChanging(() => ConditionRule6);
					conditionRule6 = value;
					ConditionRule6Changed.Raise(this, e);
					RaisePropertyChanged(() => ConditionRule6);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule7
		{
			get
			{
				return conditionRule7;
			}
			set
			{
				if (conditionRule7 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(conditionRule7, value);
					RaisePropertyChanging(() => ConditionRule7);
					conditionRule7 = value;
					ConditionRule7Changed.Raise(this, e);
					RaisePropertyChanged(() => ConditionRule7);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule8
		{
			get
			{
				return conditionRule8;
			}
			set
			{
				if (conditionRule8 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(conditionRule8, value);
					RaisePropertyChanging(() => ConditionRule8);
					conditionRule8 = value;
					ConditionRule8Changed.Raise(this, e);
					RaisePropertyChanged(() => ConditionRule8);
				}
			}
		}
		[Browsable(true), CollectionEditor(typeof(ConditionSignal)), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public IObservableCollection<ConditionSignal> ConditionSignals
		{
			get
			{
				return conditionSignals;
			}
		}
		[PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal1
		{
			get
			{
				return maskSignal1;
			}
			set
			{
				if (maskSignal1 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(maskSignal1, value);
					RaisePropertyChanging(() => MaskSignal1);
					maskSignal1 = value;
					MaskSignal1Changed.Raise(this, e);
					RaisePropertyChanged(() => MaskSignal1);
				}
			}
		}
		[PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal2
		{
			get
			{
				return maskSignal2;
			}
			set
			{
				if (maskSignal2 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(maskSignal2, value);
					RaisePropertyChanging(() => MaskSignal2);
					maskSignal2 = value;
					MaskSignal2Changed.Raise(this, e);
					RaisePropertyChanged(() => MaskSignal2);
				}
			}
		}
		[PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal3
		{
			get
			{
				return maskSignal3;
			}
			set
			{
				if (maskSignal3 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(maskSignal3, value);
					RaisePropertyChanging(() => MaskSignal3);
					maskSignal3 = value;
					MaskSignal3Changed.Raise(this, e);
					RaisePropertyChanged(() => MaskSignal3);
				}
			}
		}
		[PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal4
		{
			get
			{
				return maskSignal4;
			}
			set
			{
				if (maskSignal4 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(maskSignal4, value);
					RaisePropertyChanging(() => MaskSignal4);
					maskSignal4 = value;
					MaskSignal4Changed.Raise(this, e);
					RaisePropertyChanged(() => MaskSignal4);
				}
			}
		}
		[PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal5
		{
			get
			{
				return maskSignal5;
			}
			set
			{
				if (maskSignal5 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(maskSignal5, value);
					RaisePropertyChanging(() => MaskSignal5);
					maskSignal5 = value;
					MaskSignal5Changed.Raise(this, e);
					RaisePropertyChanged(() => MaskSignal5);
				}
			}
		}
		[PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal6
		{
			get
			{
				return maskSignal6;
			}
			set
			{
				if (maskSignal6 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(maskSignal6, value);
					RaisePropertyChanging(() => MaskSignal6);
					maskSignal6 = value;
					MaskSignal6Changed.Raise(this, e);
					RaisePropertyChanged(() => MaskSignal6);
				}
			}
		}
		[PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal7
		{
			get
			{
				return maskSignal7;
			}
			set
			{
				if (maskSignal7 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(maskSignal7, value);
					RaisePropertyChanging(() => MaskSignal7);
					maskSignal7 = value;
					MaskSignal7Changed.Raise(this, e);
					RaisePropertyChanged(() => MaskSignal7);
				}
			}
		}
		[PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal8
		{
			get
			{
				return maskSignal8;
			}
			set
			{
				if (maskSignal8 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(maskSignal8, value);
					RaisePropertyChanging(() => MaskSignal8);
					maskSignal8 = value;
					MaskSignal8Changed.Raise(this, e);
					RaisePropertyChanged(() => MaskSignal8);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName1
		{
			get
			{
				return stateSignalName1;
			}
			set
			{
				if (stateSignalName1 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(stateSignalName1, value);
					RaisePropertyChanging(() => StateSignalName1);
					stateSignalName1 = value;
					StateSignalName1Changed.Raise(this, e);
					RaisePropertyChanged(() => StateSignalName1);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName2
		{
			get
			{
				return stateSignalName2;
			}
			set
			{
				if (stateSignalName2 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(stateSignalName2, value);
					RaisePropertyChanging(() => StateSignalName2);
					stateSignalName2 = value;
					StateSignalName2Changed.Raise(this, e);
					RaisePropertyChanged(() => StateSignalName2);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName3
		{
			get
			{
				return stateSignalName3;
			}
			set
			{
				if (stateSignalName3 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(stateSignalName3, value);
					RaisePropertyChanging(() => StateSignalName3);
					stateSignalName3 = value;
					StateSignalName3Changed.Raise(this, e);
					RaisePropertyChanged(() => StateSignalName3);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName4
		{
			get
			{
				return stateSignalName4;
			}
			set
			{
				if (stateSignalName4 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(stateSignalName4, value);
					RaisePropertyChanging(() => StateSignalName4);
					stateSignalName4 = value;
					StateSignalName4Changed.Raise(this, e);
					RaisePropertyChanged(() => StateSignalName4);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName5
		{
			get
			{
				return stateSignalName5;
			}
			set
			{
				if (stateSignalName5 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(stateSignalName5, value);
					RaisePropertyChanging(() => StateSignalName5);
					stateSignalName5 = value;
					StateSignalName5Changed.Raise(this, e);
					RaisePropertyChanged(() => StateSignalName5);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName6
		{
			get
			{
				return stateSignalName6;
			}
			set
			{
				if (stateSignalName6 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(stateSignalName6, value);
					RaisePropertyChanging(() => StateSignalName6);
					stateSignalName6 = value;
					StateSignalName6Changed.Raise(this, e);
					RaisePropertyChanged(() => StateSignalName6);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName7
		{
			get
			{
				return stateSignalName7;
			}
			set
			{
				if (stateSignalName7 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(stateSignalName7, value);
					RaisePropertyChanging(() => StateSignalName7);
					stateSignalName7 = value;
					StateSignalName7Changed.Raise(this, e);
					RaisePropertyChanged(() => StateSignalName7);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName8
		{
			get
			{
				return stateSignalName8;
			}
			set
			{
				if (stateSignalName8 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(stateSignalName8, value);
					RaisePropertyChanging(() => StateSignalName8);
					stateSignalName8 = value;
					StateSignalName8Changed.Raise(this, e);
					RaisePropertyChanged(() => StateSignalName8);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string SymbolicTextNormalState
		{
			get
			{
				return symbolicTextNormalState;
			}
			set
			{
				if (symbolicTextNormalState != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(symbolicTextNormalState, value);
					RaisePropertyChanging(() => SymbolicTextNormalState);
					symbolicTextNormalState = value;
					SymbolicTextNormalStateChanged.Raise(this, e);
					RaisePropertyChanged(() => SymbolicTextNormalState);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories))]
		public bool RegisterSymbolicTextState
		{
			get
			{
				return registerSymbolicTextState;
			}
			set
			{
				if (registerSymbolicTextState != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(registerSymbolicTextState, value);
					RaisePropertyChanging(() => RegisterSymbolicTextState);
					registerSymbolicTextState = value;
					RegisterSymbolicTextStateChanged.Raise(this, e);
					RaisePropertyChanged(() => RegisterSymbolicTextState);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string SymbolicTextState1
		{
			get
			{
				return symbolicTextState1;
			}
			set
			{
				if (symbolicTextState1 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(symbolicTextState1, value);
					RaisePropertyChanging(() => SymbolicTextState1);
					symbolicTextState1 = value;
					SymbolicTextState1Changed.Raise(this, e);
					RaisePropertyChanged(() => SymbolicTextState1);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string SymbolicTextState2
		{
			get
			{
				return symbolicTextState2;
			}
			set
			{
				if (symbolicTextState2 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(symbolicTextState2, value);
					RaisePropertyChanging(() => SymbolicTextState2);
					symbolicTextState2 = value;
					SymbolicTextState2Changed.Raise(this, e);
					RaisePropertyChanged(() => SymbolicTextState2);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string SymbolicTextState3
		{
			get
			{
				return symbolicTextState3;
			}
			set
			{
				if (symbolicTextState3 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(symbolicTextState3, value);
					RaisePropertyChanging(() => SymbolicTextState3);
					symbolicTextState3 = value;
					SymbolicTextState3Changed.Raise(this, e);
					RaisePropertyChanged(() => SymbolicTextState3);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string SymbolicTextState4
		{
			get
			{
				return symbolicTextState4;
			}
			set
			{
				if (symbolicTextState4 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(symbolicTextState4, value);
					RaisePropertyChanging(() => SymbolicTextState4);
					symbolicTextState4 = value;
					SymbolicTextState4Changed.Raise(this, e);
					RaisePropertyChanged(() => SymbolicTextState4);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string SymbolicTextState5
		{
			get
			{
				return symbolicTextState5;
			}
			set
			{
				if (symbolicTextState5 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(symbolicTextState5, value);
					RaisePropertyChanging(() => SymbolicTextState5);
					symbolicTextState5 = value;
					SymbolicTextState5Changed.Raise(this, e);
					RaisePropertyChanged(() => SymbolicTextState5);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string SymbolicTextState6
		{
			get
			{
				return symbolicTextState6;
			}
			set
			{
				if (symbolicTextState6 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(symbolicTextState6, value);
					RaisePropertyChanging(() => SymbolicTextState6);
					symbolicTextState6 = value;
					SymbolicTextState6Changed.Raise(this, e);
					RaisePropertyChanged(() => SymbolicTextState6);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string SymbolicTextState7
		{
			get
			{
				return symbolicTextState7;
			}
			set
			{
				if (symbolicTextState7 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(symbolicTextState7, value);
					RaisePropertyChanging(() => SymbolicTextState7);
					symbolicTextState7 = value;
					SymbolicTextState7Changed.Raise(this, e);
					RaisePropertyChanged(() => SymbolicTextState7);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string SymbolicTextState8
		{
			get
			{
				return symbolicTextState8;
			}
			set
			{
				if (symbolicTextState8 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(symbolicTextState8, value);
					RaisePropertyChanging(() => SymbolicTextState8);
					symbolicTextState8 = value;
					SymbolicTextState8Changed.Raise(this, e);
					RaisePropertyChanged(() => SymbolicTextState8);
				}
			}
		}
		[Browsable(true), FontFamilyEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string FontFamily
		{
			get
			{
				return fontFamily;
			}
			set
			{
				if (fontFamily != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(fontFamily, value);
					RaisePropertyChanging(() => FontFamily);
					fontFamily = value;
					FontFamilyChanged.Raise(this, e);
					RaisePropertyChanged(() => FontFamily);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories)), MinimumValue(4.0)]
		public double FontSize
		{
			get
			{
				return fontSize;
			}
			set
			{
				if (fontSize != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(fontSize, value);
					RaisePropertyChanging(() => FontSize);
					fontSize = value;
					FontSizeChanged.Raise(this, e);
					RaisePropertyChanged(() => FontSize);
				}
			}
		}
		[Browsable(true), FontStretchEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string FontStretch
		{
			get
			{
				return fontStretch;
			}
			set
			{
				if (fontStretch != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(fontStretch, value);
					RaisePropertyChanging(() => FontStretch);
					fontStretch = value;
					FontStretchChanged.Raise(this, e);
					RaisePropertyChanged(() => FontStretch);
				}
			}
		}
		[Browsable(true), FontStyleEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string FontStyle
		{
			get
			{
				return fontStyle;
			}
			set
			{
				if (fontStyle != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(fontStyle, value);
					RaisePropertyChanging(() => FontStyle);
					fontStyle = value;
					FontStyleChanged.Raise(this, e);
					RaisePropertyChanged(() => FontStyle);
				}
			}
		}
		[Browsable(true), FontWeightEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string FontWeight
		{
			get
			{
				return fontWeight;
			}
			set
			{
				if (fontWeight != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(fontWeight, value);
					RaisePropertyChanging(() => FontWeight);
					fontWeight = value;
					FontWeightChanged.Raise(this, e);
					RaisePropertyChanged(() => FontWeight);
				}
			}
		}
		public override Size DesignerDesiredSize
		{
			get
			{
				return new Size(120.0, 50.0);
			}
		}
		public HMIStateDisplay1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
			conditionSignalsMonitor = container.Resolve<ICollectionMonitor<ConditionSignal>>();
			conditionSignalsMonitor.Start(ConditionSignals);
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("Background", this.UseLocalColors ? base.Background : this.Project.StateDefaultBackground);
			base.AddXamlAttribute("Foreground", this.UseLocalColors ? base.Foreground : this.Project.StateDefaultForeground);
			base.AddXamlAttribute("ObjectID", this.ObjectID);
			base.AddXamlAttribute("SignalPrefix", base.GetSignalPrefix());
			base.AddXamlAttribute("SecurityAuthorizationName", this.SecurityAuthorizationName);
			base.AddXamlAttribute("SecurityDenyAccessBehaviour", this.SecurityDenyAccessBehaviour);
			base.AddXamlAttribute("VisibilityMask", this.VisibilityMask);
			base.AddXamlAttribute("VisibilitySignalName", this.VisibilitySignalName);
			base.AddXamlAttribute("BinaryMask", this.BinaryMask);
			base.AddXamlAttribute("ConditionRule1", this.ConditionRule1);
			base.AddXamlAttribute("ConditionRule2", this.ConditionRule2);
			base.AddXamlAttribute("ConditionRule3", this.ConditionRule3);
			base.AddXamlAttribute("ConditionRule4", this.ConditionRule4);
			base.AddXamlAttribute("ConditionRule5", this.ConditionRule5);
			base.AddXamlAttribute("ConditionRule6", this.ConditionRule6);
			base.AddXamlAttribute("ConditionRule7", this.ConditionRule7);
			base.AddXamlAttribute("ConditionRule8", this.ConditionRule8);
			base.AddXamlAttribute("MaskSignal1", this.MaskSignal1);
			base.AddXamlAttribute("MaskSignal2", this.MaskSignal2);
			base.AddXamlAttribute("MaskSignal3", this.MaskSignal3);
			base.AddXamlAttribute("MaskSignal4", this.MaskSignal4);
			base.AddXamlAttribute("MaskSignal5", this.MaskSignal5);
			base.AddXamlAttribute("MaskSignal6", this.MaskSignal6);
			base.AddXamlAttribute("MaskSignal7", this.MaskSignal7);
			base.AddXamlAttribute("MaskSignal8", this.MaskSignal8);
			base.AddXamlAttribute("StateSignalName1", this.StateSignalName1);
			base.AddXamlAttribute("StateSignalName2", this.StateSignalName2);
			base.AddXamlAttribute("StateSignalName3", this.StateSignalName3);
			base.AddXamlAttribute("StateSignalName4", this.StateSignalName4);
			base.AddXamlAttribute("StateSignalName5", this.StateSignalName5);
			base.AddXamlAttribute("StateSignalName6", this.StateSignalName6);
			base.AddXamlAttribute("StateSignalName7", this.StateSignalName7);
			base.AddXamlAttribute("StateSignalName8", this.StateSignalName8);
			base.AddXamlAttribute("SymbolicTextState1", this.SymbolicTextState1);
			base.AddXamlAttribute("SymbolicTextState2", this.SymbolicTextState2);
			base.AddXamlAttribute("SymbolicTextState3", this.SymbolicTextState3);
			base.AddXamlAttribute("SymbolicTextState4", this.SymbolicTextState4);
			base.AddXamlAttribute("SymbolicTextState5", this.SymbolicTextState5);
			base.AddXamlAttribute("SymbolicTextState6", this.SymbolicTextState6);
			base.AddXamlAttribute("SymbolicTextState7", this.SymbolicTextState7);
			base.AddXamlAttribute("SymbolicTextState8", this.SymbolicTextState8);
			base.AddXamlAttribute("RegisterSymbolicTextState", this.RegisterSymbolicTextState);
			base.AddXamlAttribute("SymbolicTextNormalState", this.SymbolicTextNormalState);
			base.AddXamlAttribute("FontFamily", this.FontFamily);
			base.AddXamlAttribute("FontSize", this.FontSize);
			base.AddXamlAttribute("FontStretch", this.FontStretch);
			base.AddXamlAttribute("FontStyle", this.FontStyle);
			base.AddXamlAttribute("FontWeight", this.FontWeight);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			IsTemplateEnabled = true;
		}
		protected override void AddXamlCollections(System.Text.StringBuilder collectionXaml)
		{
			base.AddXamlCollections(collectionXaml);
			base.AddXamlCollection(collectionXaml, "ConditionSignals", this.ConditionSignals, true);
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
		public override string GetXamlControlName()
		{
			return "HMIStateDisplay1";
		}
	}
}
