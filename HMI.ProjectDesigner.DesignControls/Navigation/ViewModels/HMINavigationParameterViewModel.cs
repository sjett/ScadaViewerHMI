// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using System.ComponentModel;
namespace HMI.ProjectDesigner.DesignControls.Navigation.ViewModels
{
	public class HMINavigationParameterViewModel : CommonControlViewModel
	{
		private readonly ICollectionMonitor<NavigationParameter> navigationParametersMonitor;
		private readonly IObservableCollection<NavigationParameter> parameters = new FullyObservableCollection<NavigationParameter>();
		
		[Browsable(true), CollectionEditor(typeof(NavigationParameter), UseHorizontalEditing = true), PropertyCategory("Navigation", typeof(StandardPropertyCategories))]
		public IObservableCollection<NavigationParameter> Parameters
		{
			get
			{
				return this.parameters;
			}
		}
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(150.0, 25.0);
			}
		}
		public HMINavigationParameterViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
			this.navigationParametersMonitor = container.Resolve<ICollectionMonitor<NavigationParameter>>();
			this.navigationParametersMonitor.Start(this.Parameters);
		}
		public override string GetXamlControlName()
		{
			return "HMINavigationParameter";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMINavigation=\"clr-namespace:HMI.RuntimeControls.Navigation;assembly=HMI.RuntimeControls\"",
                "HMIShared=\"clr-namespace:HMI.Runtime.Shared;assembly=HMI.Runtime.Shared\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMINavigation;
		//}
		protected override void AddXamlCollections(System.Text.StringBuilder collectionXaml)
		{
			base.AddXamlCollections(collectionXaml);
			base.AddXamlCollection(collectionXaml, "Parameters", this.Parameters, true);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
		}
	}
}
