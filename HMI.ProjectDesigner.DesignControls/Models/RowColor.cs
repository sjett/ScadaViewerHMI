// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using System.ComponentModel;
namespace HMI.Infrastructure.Models
{
	public class RowColor : CollectionItem, INotifyPropertyChanged
	{
		private System.Windows.Media.Brush activeAlarmBackground = (System.Windows.Media.Brush)System.Windows.Media.Brushes.Tomato.GetAsFrozen();
		private System.Windows.Media.Brush activeAlarmForeground = (System.Windows.Media.Brush)System.Windows.Media.Brushes.Black.GetAsFrozen();
		private System.Windows.Media.Brush inactiveAlarmBackground = (System.Windows.Media.Brush)new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 204, 102)).GetAsFrozen();
		private System.Windows.Media.Brush inactiveAlarmForeground = (System.Windows.Media.Brush)System.Windows.Media.Brushes.Black.GetAsFrozen();
		private System.Windows.Media.Brush acknowledgedAlarmBackground = (System.Windows.Media.Brush)new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 153)).GetAsFrozen();
		private System.Windows.Media.Brush acknowledgedAlarmForeground = (System.Windows.Media.Brush)System.Windows.Media.Brushes.Black.GetAsFrozen();
		private System.Windows.Media.Brush acknowledgedAndGoneAlarmBackground = (System.Windows.Media.Brush)new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 153)).GetAsFrozen();
		private System.Windows.Media.Brush acknowledgedAndGoneAlarmForeground = (System.Windows.Media.Brush)System.Windows.Media.Brushes.Black.GetAsFrozen();
		private string alarmGroup;
		private string alarmType;
		private int? priority;
		private string hornPath;
		private bool loop;
		private readonly IContentReplacementService contentReplacementService;

		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> ActiveAlarmBackgroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> ActiveAlarmForegroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> InactiveAlarmBackgroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> InactiveAlarmForegroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> AcknowledgedAlarmBackgroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> AcknowledgedAlarmForegroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> AcknowledgedAndGoneAlarmBackgroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> AcknowledgedAndGoneAlarmForegroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> AlarmGroupChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> AlarmTypeChanged;
		public event System.EventHandler<ValueChangedEventArgs<int?>> PriorityChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> HornPathChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> LoopChanged;

		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush ActiveAlarmBackground
		{
			get
			{
				return this.activeAlarmBackground;
			}
			set
			{
				if (this.activeAlarmBackground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.activeAlarmBackground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.ActiveAlarmBackground);
					this.activeAlarmBackground = value;
					this.ActiveAlarmBackgroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.ActiveAlarmBackground);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush ActiveAlarmForeground
		{
			get
			{
				return this.activeAlarmForeground;
			}
			set
			{
				if (this.activeAlarmForeground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.activeAlarmForeground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.ActiveAlarmForeground);
					this.activeAlarmForeground = value;
					this.ActiveAlarmForegroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.ActiveAlarmForeground);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush InactiveAlarmBackground
		{
			get
			{
				return this.inactiveAlarmBackground;
			}
			set
			{
				if (this.inactiveAlarmBackground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.inactiveAlarmBackground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.InactiveAlarmBackground);
					this.inactiveAlarmBackground = value;
					this.InactiveAlarmBackgroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.InactiveAlarmBackground);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush InactiveAlarmForeground
		{
			get
			{
				return this.inactiveAlarmForeground;
			}
			set
			{
				if (this.inactiveAlarmForeground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.inactiveAlarmForeground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.InactiveAlarmForeground);
					this.inactiveAlarmForeground = value;
					this.InactiveAlarmForegroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.InactiveAlarmForeground);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush AcknowledgedAlarmBackground
		{
			get
			{
				return this.acknowledgedAlarmBackground;
			}
			set
			{
				if (this.acknowledgedAlarmBackground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.acknowledgedAlarmBackground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.AcknowledgedAlarmBackground);
					this.acknowledgedAlarmBackground = value;
					this.AcknowledgedAlarmBackgroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.AcknowledgedAlarmBackground);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush AcknowledgedAlarmForeground
		{
			get
			{
				return this.acknowledgedAlarmForeground;
			}
			set
			{
				if (this.acknowledgedAlarmForeground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.acknowledgedAlarmForeground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.AcknowledgedAlarmForeground);
					this.acknowledgedAlarmForeground = value;
					this.AcknowledgedAlarmForegroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.AcknowledgedAlarmForeground);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush AcknowledgedAndGoneAlarmBackground
		{
			get
			{
				return this.acknowledgedAndGoneAlarmBackground;
			}
			set
			{
				if (this.acknowledgedAndGoneAlarmBackground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.acknowledgedAndGoneAlarmBackground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.AcknowledgedAndGoneAlarmBackground);
					this.acknowledgedAndGoneAlarmBackground = value;
					this.AcknowledgedAndGoneAlarmBackgroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.AcknowledgedAndGoneAlarmBackground);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush AcknowledgedAndGoneAlarmForeground
		{
			get
			{
				return this.acknowledgedAndGoneAlarmForeground;
			}
			set
			{
				if (this.acknowledgedAndGoneAlarmForeground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.acknowledgedAndGoneAlarmForeground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.AcknowledgedAndGoneAlarmForeground);
					this.acknowledgedAndGoneAlarmForeground = value;
					this.AcknowledgedAndGoneAlarmForegroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.AcknowledgedAndGoneAlarmForeground);
				}
			}
		}
		[Browsable(true), AlarmGroupBrowser, PropertyCategory("AlarmConfiguration", typeof(StandardPropertyCategories))]
		public string AlarmGroup
		{
			get
			{
				return this.alarmGroup;
			}
			set
			{
				if (this.alarmGroup != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.alarmGroup, value);
					base.RaisePropertyChanging<string>(() => this.AlarmGroup);
					this.alarmGroup = value;
					this.AlarmGroupChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.AlarmGroup);
				}
			}
		}
		[Browsable(true), AlarmTypeBrowser, PropertyCategory("AlarmConfiguration", typeof(StandardPropertyCategories))]
		public string AlarmType
		{
			get
			{
				return this.alarmType;
			}
			set
			{
				if (this.alarmType != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.alarmType, value);
					base.RaisePropertyChanging<string>(() => this.AlarmType);
					this.alarmType = value;
					this.AlarmTypeChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.AlarmType);
				}
			}
		}
		[Browsable(true), PropertyCategory("AlarmConfiguration", typeof(StandardPropertyCategories))]
		public int? Priority
		{
			get
			{
				return this.priority;
			}
			set
			{
				if (this.priority != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.priority, value);
					base.RaisePropertyChanging<int?>(() => this.Priority);
					this.priority = value;
					this.PriorityChanged.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.Priority);
				}
			}
		}
		[Browsable(true), MediaBrowser(MediaType.Audio), PropertyCategory("AlarmConfiguration", typeof(StandardPropertyCategories))]
		public string HornPath
		{
			get
			{
				return this.hornPath;
			}
			set
			{
				if (this.hornPath != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.hornPath, value);
					base.RaisePropertyChanging<string>(() => this.HornPath);
					this.hornPath = value;
					this.HornPathChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.HornPath);
				}
			}
		}
		[Browsable(true), PropertyCategory("AlarmConfiguration", typeof(StandardPropertyCategories))]
		public bool Loop
		{
			get
			{
				return this.loop;
			}
			set
			{
				if (this.loop != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.loop, value);
					base.RaisePropertyChanging<bool>(() => this.Loop);
					this.loop = value;
					this.LoopChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.Loop);
				}
			}
		}
		public RowColor(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
			this.contentReplacementService = container.Resolve<IContentReplacementService>();
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			RowColor rowColor = target as RowColor;
			rowColor.ActiveAlarmBackground = this.ActiveAlarmBackground;
			rowColor.ActiveAlarmForeground = this.ActiveAlarmForeground;
			rowColor.InactiveAlarmForeground = this.InactiveAlarmForeground;
			rowColor.InactiveAlarmBackground = this.InactiveAlarmBackground;
			rowColor.AcknowledgedAlarmBackground = this.AcknowledgedAlarmBackground;
			rowColor.AcknowledgedAlarmForeground = this.AcknowledgedAlarmForeground;
			rowColor.AcknowledgedAndGoneAlarmBackground = this.AcknowledgedAndGoneAlarmBackground;
			rowColor.AcknowledgedAndGoneAlarmForeground = this.AcknowledgedAndGoneAlarmForeground;
			rowColor.AlarmGroup = this.AlarmGroup;
			rowColor.AlarmType = this.AlarmType;
			rowColor.Priority = this.Priority;
			rowColor.HornPath = this.HornPath;
			rowColor.Loop = this.Loop;
		}
		public override ICollectionItem CreateClone()
		{
			return new RowColor(base.Services, base.Parent);
		}
		public override string GetXaml()
		{
			string arg = ProjectFilesResolver.Resolve(this.HornPath, MediaType.Audio);
			string text = string.Format("<{0}:{1} ActiveAlarmBackground=\"[ActiveAlarmBackground]\" ActiveAlarmForeground=\"[ActiveAlarmForeground]\" InactiveAlarmBackground=\"[InactiveAlarmBackground]\" InactiveAlarmForeground=\"[InactiveAlarmForeground]\" AcknowledgedAlarmBackground=\"[AcknowledgedAlarmBackground]\" AcknowledgedAlarmForeground=\"[AcknowledgedAlarmForeground]\" AlarmGroup=\"[AlarmGroup]\" AlarmType=\"[AlarmType]\" Priority=\"[Priority]\" HornPath=\"{2}\" Loop=\"[Loop]\"></{0}:{1}>", "HMIAlarms", "RowColor", arg);
			return this.contentReplacementService.ReplaceStringContent(text, this);
		}
	}
	}
