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
	public class HMIAnimatedStatesControlBaseViewModel : HMIStatesControlBaseViewModel
	{
		private readonly ICollectionMonitor<AnimatedStateCondition> animatedStatesMonitor;
		private readonly IObservableCollection<AnimatedStateCondition> animatedStates = new FullyObservableCollection<AnimatedStateCondition>();
		
		[Browsable(true), 
			CollectionEditor(typeof(AnimatedStateCondition), UseHorizontalEditing = true), 
			PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public IObservableCollection<AnimatedStateCondition> AnimatedStates
		{
			get
			{
				return animatedStates;
			}
		}
		public HMIAnimatedStatesControlBaseViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
            animatedStatesMonitor = container.Resolve<ICollectionMonitor<AnimatedStateCondition>>();
            animatedStatesMonitor.Start(AnimatedStates);
		}
		protected override void AddXamlCollections(StringBuilder collectionXaml)
		{
			base.AddXamlCollections(collectionXaml);
			base.AddXamlCollection(collectionXaml, "States", AnimatedStates, true);
		}
	}
}
