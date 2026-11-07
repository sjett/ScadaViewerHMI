// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation;
using HMI.Modules.Explorer.ViewModels;
using HMI.Modules.Explorer.Views;
namespace HMI.Modules.Explorer
{
	public class ExplorerModule : PluginModule
	{
		protected override void RegisterServices(IServiceCollection containerRegistry)
		{
		}
		public override void RegisterTypes(IServiceCollection containerRegistry)
		{
			base.RegisterTypes(containerRegistry);

			containerRegistry.AddTransient<IExplorerView, ExplorerView>();
			containerRegistry.AddTransient<IExplorerViewModel, ExplorerViewModel>();
		}
		protected override void AddViewsToRegions()
		{
			//RegionManagerExtensions.RegisterViewWithRegion(base.RegionManager, "ExplorerRegion", typeof(IExplorerView));
			base.RegionManager.RegisterViewWithRegion("ExplorerRegion", typeof(IExplorerView));

        }
	}
}
