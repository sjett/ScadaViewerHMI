// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using System.ComponentModel;
using System.Text;
namespace HMI.ProjectDesigner.DesignControls.ViewModels
{
	public class HMIStaticStatesControlBaseViewModel : HMIStatesControlBaseViewModel
	{
		private readonly ICollectionMonitor<StateCondition> stateConditionsMonitor;
		private readonly IObservableCollection<StateCondition> states = new FullyObservableCollection<StateCondition>();
		
		[Browsable(true), CollectionEditor(typeof(StateCondition), UseHorizontalEditing = true), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public IObservableCollection<StateCondition> States
		{
			get
			{
				return states;
			}
		}
		public HMIStaticStatesControlBaseViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) 
			: base(container, parentValue, controlType)
		{
            stateConditionsMonitor = container.Resolve<ICollectionMonitor<StateCondition>>();
            stateConditionsMonitor.Start(States);
		}
		protected override void AddXamlCollections(StringBuilder collectionXaml)
		{
			base.AddXamlCollections(collectionXaml);
			base.AddXamlCollection(collectionXaml, "States", States, true);
		}
	}
}
