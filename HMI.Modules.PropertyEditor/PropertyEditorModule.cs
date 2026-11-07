// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Domain.Services;
using HMI.Infrastructure.Presentation;
using HMI.Infrastructure.ViewModels;
using HMI.Modules.PropertyEditor.Views;
using HMI.Modules.PropertyEditor.ViewModels;
using HMI.Modules.PropertyEditor.Services;
namespace HMI.Modules.PropertyEditor
{
	public class PropertyEditorModule : PluginModule
	{
		protected override void AddViewsToRegions()
		{
			base.RegionManager.RegisterViewWithRegion("MainPropertyEditorRegion", typeof(IMainPropertyEditorView));
			base.RegionManager.RegisterViewWithRegion("HorizontalPropertyEditorRegion", typeof(HorizontalPropertyEditorView));
			base.RegionManager.RegisterViewWithRegion("PropertyEditorRegion", typeof(PropertyEditorView));
		}
		protected override void RegisterServices(IServiceCollection containerRegistry)
		{
            containerRegistry.AddTransient<IPropertyInspectorService, PropertyInspectorService>();
		}
		public override void RegisterTypes(IServiceCollection containerRegistry)
		{
			base.RegisterTypes(containerRegistry);

			containerRegistry.AddTransient<IPropertyEditorViewModel, PropertyEditorViewModel>();

			containerRegistry.AddTransient<IMainPropertyEditorView, MainPropertyEditorView>();

            containerRegistry.AddTransient<IHorizontalPropertyEditorViewModel, HorizontalPropertyEditorViewModel>();
        }
	}
}
