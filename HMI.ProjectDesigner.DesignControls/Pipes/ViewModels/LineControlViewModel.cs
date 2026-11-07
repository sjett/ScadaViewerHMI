// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
namespace HMI.ProjectDesigner.DesignControls.Pipes.ViewModels
{
	public class LineControlViewModel : PipeControlViewModel
	{
		private ICollectionMonitor<ConditionSignal> conditionSignalsMonitor;
		private string conditionRule;
		private IObservableCollection<ConditionSignal> conditionSignals = new FullyObservableCollection<ConditionSignal>();
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRuleChanged;
		[Browsable(true)]
		public string ConditionRule
		{
			get
			{
				return this.conditionRule;
			}
			set
			{
				if (this.conditionRule != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.conditionRule, value);
					base.RaisePropertyChanging<string>(() => this.ConditionRule);
					this.conditionRule = value;
					this.ConditionRuleChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ConditionRule);
				}
			}
		}
		[Browsable(true)]
		public IObservableCollection<ConditionSignal> ConditionSignals
		{
			get
			{
				return this.conditionSignals;
			}
		}
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(50.0, 50.0);
			}
		}
		public LineControlViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
			this.conditionSignalsMonitor = container.Resolve<ICollectionMonitor<ConditionSignal>>();
			this.conditionSignalsMonitor.Start(this.ConditionSignals);
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("ConditionRule", this.ConditionRule);
		}
		protected override void AddXamlCollections(System.Text.StringBuilder collectionXaml)
		{
			base.AddXamlCollections(collectionXaml);
			base.AddXamlCollection(collectionXaml, "ConditionSignals", this.ConditionSignals, true);
		}
	}
}
