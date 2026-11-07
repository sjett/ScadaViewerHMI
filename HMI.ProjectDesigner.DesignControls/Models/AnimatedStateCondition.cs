// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows.Media;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Modules.Design.Attributes;
namespace HMI.Infrastructure.Models
{
	public class AnimatedStateCondition : StateCondition
	{
		private System.Windows.Media.Brush animatedFill = (System.Windows.Media.Brush)System.Windows.Media.Brushes.White.GetAsFrozen();
		private System.Windows.Media.Brush alternateAnimatedFill = (System.Windows.Media.Brush)System.Windows.Media.Brushes.White.GetAsFrozen();
		
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> AnimatedFillChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> AlternateAnimatedFillChanged;

		[Browsable(true), EditorOrder(6.0)]
		public System.Windows.Media.Brush AnimatedFill
		{
			get
			{
				return this.animatedFill;
			}
			set
			{
				if (this.animatedFill != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.animatedFill, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.AnimatedFill);
					this.animatedFill = value;
					this.AnimatedFillChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.AnimatedFill);
				}
			}
		}
		[Browsable(true), EditorOrder(15.0), EnabledProperty("IsBlinking")]
		public System.Windows.Media.Brush AlternateAnimatedFill
		{
			get
			{
				return this.alternateAnimatedFill;
			}
			set
			{
				if (this.alternateAnimatedFill != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.alternateAnimatedFill, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.AlternateAnimatedFill);
					this.alternateAnimatedFill = value;
					this.AlternateAnimatedFillChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.AlternateAnimatedFill);
				}
			}
		}
		protected override string Definition
		{
			get
			{
				return "HMIAnimatedStateCondition";
			}
		}
		public AnimatedStateCondition(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			base.DeepCopyFields(target);
			AnimatedStateCondition animatedStateCondition = target as AnimatedStateCondition;
			animatedStateCondition.AlternateAnimatedFill = this.AlternateAnimatedFill;
			animatedStateCondition.AnimatedFill = this.AnimatedFill;
		}
		public override ICollectionItem CreateClone()
		{
			return new AnimatedStateCondition(base.Services, base.Parent);
		}
		protected override string GetTemplateFormat(string prefix, string definition, string conditionParameter)
		{
			return string.Format("<{0}:{1} Background=\"[Background]\" AlternateBackground=\"[AlternateBackground]\" Condition=\"[Condition]\" ConditionType=\"Equation\" Foreground=\"[Foreground]\" AlternateForeground=\"[AlternateForeground]\" IsBlinking=\"[IsBlinking]\" BlinkingFrequency=\"[RealBlinkingFrequency]\" StateName=\"[StateName]\" IsVisible=\"[IsVisible]\" AnimatedFill=\"[AnimatedFill]\" AlternateAnimatedFill=\"[AlternateAnimatedFill]\">{3}" + "<{0}:{1}.ConditionParameters>{3}{2}{3}</{0}:{1}.ConditionParameters>{3}</{0}:{1}>", new object[]
			{
				prefix,
				definition,
				conditionParameter,
				System.Environment.NewLine
			});
		}
	}
}
