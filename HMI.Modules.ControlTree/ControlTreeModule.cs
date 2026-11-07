// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation;
using HMI.Modules.ControlTree.Services;
namespace HMI.Modules.ControlTree
{
	public class ControlTreeModule : PluginModule
	{
		protected override void AddViewsToRegions()
		{
			base.RegionManager.RegisterViewWithRegion("ControlTreeRegion", typeof(IControlTreeView));
			base.RegionManager.RegisterViewWithRegion("PrimitivesRegion", typeof(IPrimitivesView));
		}
		protected override void RegisterServices(IServiceCollection containerRegistry)
		{
			containerRegistry.AddTransient<IControlTreeService, ControlTreeService>();
			containerRegistry.AddTransient<IControlTreeItemMapperService, ControlTreeItemMapperService>();
		}
		public override void RegisterTypes(IServiceCollection containerRegistry)
		{
			base.RegisterTypes(containerRegistry);

            containerRegistry.AddTransient<IControlTreeViewModel, ControlTreeViewModel>();
			containerRegistry.AddTransient<IControlTreeView, ControlTreeView>();
			containerRegistry.AddTransient<IPrimitivesView, PrimitivesView>();
		}
	}
}
