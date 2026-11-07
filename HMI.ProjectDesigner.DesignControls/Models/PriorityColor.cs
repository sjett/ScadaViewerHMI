// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows.Media;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
namespace HMI.Infrastructure.Models
{
	public class PriorityColor : CollectionItem
	{
		private System.Windows.Media.Brush acknowledgedAlarmColor = (System.Windows.Media.Brush)new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 153)).GetAsFrozen();
		private System.Windows.Media.Brush acknowledgedAndGoneAlarmColor = (System.Windows.Media.Brush)new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(154, 205, 50)).GetAsFrozen();
		private System.Windows.Media.Brush activeAlarmColor = (System.Windows.Media.Brush)System.Windows.Media.Brushes.Tomato.GetAsFrozen();
		private System.Windows.Media.Brush inactiveAlarmColor = (System.Windows.Media.Brush)new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 204, 102)).GetAsFrozen();
		private int priority;
		private readonly IContentReplacementService contentReplacementService;

		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> AcknowledgedAlarmColorChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> AcknowledgedAndGoneAlarmColorChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> ActiveAlarmColorChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> InactiveAlarmColorChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> PriorityChanged;

		[Browsable(true)]
		public System.Windows.Media.Brush AcknowledgedAlarmColor
		{
			get
			{
				return this.acknowledgedAlarmColor;
			}
			set
			{
				if (this.acknowledgedAlarmColor != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.acknowledgedAlarmColor, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.AcknowledgedAlarmColor);
					this.acknowledgedAlarmColor = value;
					this.AcknowledgedAlarmColorChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.AcknowledgedAlarmColor);
				}
			}
		}
		[Browsable(true)]
		public System.Windows.Media.Brush AcknowledgedAndGoneAlarmColor
		{
			get
			{
				return this.acknowledgedAndGoneAlarmColor;
			}
			set
			{
				if (this.acknowledgedAndGoneAlarmColor != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.acknowledgedAndGoneAlarmColor, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.AcknowledgedAndGoneAlarmColor);
					this.acknowledgedAndGoneAlarmColor = value;
					this.AcknowledgedAndGoneAlarmColorChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.AcknowledgedAndGoneAlarmColor);
				}
			}
		}
		[Browsable(true)]
		public System.Windows.Media.Brush ActiveAlarmColor
		{
			get
			{
				return this.activeAlarmColor;
			}
			set
			{
				if (this.activeAlarmColor != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.activeAlarmColor, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.ActiveAlarmColor);
					this.activeAlarmColor = value;
					this.ActiveAlarmColorChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.ActiveAlarmColor);
				}
			}
		}
		[Browsable(true)]
		public System.Windows.Media.Brush InactiveAlarmColor
		{
			get
			{
				return this.inactiveAlarmColor;
			}
			set
			{
				if (this.inactiveAlarmColor != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.inactiveAlarmColor, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.InactiveAlarmColor);
					this.inactiveAlarmColor = value;
					this.InactiveAlarmColorChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.InactiveAlarmColor);
				}
			}
		}
		[Browsable(true)]
		public int Priority
		{
			get
			{
				return this.priority;
			}
			set
			{
				if (this.priority != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.priority, value);
					base.RaisePropertyChanging<int>(() => this.Priority);
					this.priority = value;
					this.PriorityChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.Priority);
				}
			}
		}
		public PriorityColor(IServiceProvider services, IExplorerNodeViewModel parentNode) : base(services, parentNode)
		{
			this.contentReplacementService = services.Resolve<IContentReplacementService>();
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			PriorityColor priorityColor = target as PriorityColor;
			priorityColor.AcknowledgedAlarmColor = this.AcknowledgedAlarmColor;
			priorityColor.AcknowledgedAndGoneAlarmColor = this.AcknowledgedAndGoneAlarmColor;
			priorityColor.ActiveAlarmColor = this.ActiveAlarmColor;
			priorityColor.InactiveAlarmColor = this.InactiveAlarmColor;
			priorityColor.Priority = this.Priority;
		}
		public override ICollectionItem CreateClone()
		{
			return new PriorityColor(base.Services, base.Parent);
		}
		public override string GetXaml()
		{
			string text = string.Format("<{0}:{1} AcknowledgedAlarmColor=\"[AcknowledgedAlarmColor]\" AcknowledgedAndGoneAlarmColor=\"[AcknowledgedAndGoneAlarmColor]\" ActiveAlarmColor=\"[ActiveAlarmColor]\" InactiveAlarmColor=\"[InactiveAlarmColor]\" Priority=\"[Priority]\"></{0}:{1}>", "HMIAlarms", "PriorityColor");
			return this.contentReplacementService.ReplaceStringContent(text, this);
		}
	}
}
