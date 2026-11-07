// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Services;
using HMI.Modules.ControlTree.ViewModels;
using System.IO;
using System.Runtime.Remoting;
using System.Windows;

namespace HMI.Modules.ControlTree.Services
{
	public class ControlTreeItemMapperService : IControlTreeItemMapperService
	{
		private readonly IResourceProviderService _resourceProviderService;
		public ControlTreeItemMapperService(IResourceProviderService resourceProviderService)
		{
			if (resourceProviderService == null)
			{
				throw new ArgumentNullException("resourceProviderService", "resourceProviderService is null.");
			}
            _resourceProviderService = resourceProviderService;
		}
		public ControlTreeItem ConvertFromControlCategory(ControlCategory category)
		{
			if (category == null)
			{
				throw new ArgumentNullException("category", "category is null.");
			}
			ControlTreeItem treeItem = new ControlTreeItem();
			treeItem.Name = category.Name;
			foreach (ControlCategory childCategory in category.Children)
			{
				treeItem.Children.Add(ConvertFromControlCategory(childCategory));
			}
			return treeItem;
		}
		public ControlTreeControlItem ConvertFromControlCategoryMapping(ControlCategoryMapping mapping)
		{
			if (mapping == null)
			{
				throw new ArgumentNullException("mapping", "mapping is null.");
			}
			ControlTreeControlItem treeControlItem = new ControlTreeControlItem(mapping.ControlName);
			string assemblyName = mapping.AssemblyName;
			string className = mapping.ClassName;
			treeControlItem.ControlInstance = CreateControlInstance(className, assemblyName)!;
			if (!string.IsNullOrEmpty(mapping.XamlPath))
			{
				BindControlContent(treeControlItem, mapping.XamlPath);
			}
			return treeControlItem;
		}
        private void BindControlContent(ControlTreeControlItem treeControlItem, string xamlPath)
        {
            StaticSymbolDefinitionViewModel symbolDefinition = new StaticSymbolDefinitionViewModel();
            using (TextReader resourceReader = this._resourceProviderService.GetResourceReader(xamlPath))
            {
                symbolDefinition.ContentString = resourceReader.ReadToEnd();
            }
            FrameworkElement? frameworkElement = treeControlItem.ControlInstance as FrameworkElement;
            frameworkElement!.DataContext = symbolDefinition;
        }
        private object? CreateControlInstance(string typeName, string assemblyName)
        {
            object? result;
            try
            {
                ObjectHandle? objectHandle = Activator.CreateInstance(assemblyName, typeName);
                result = objectHandle!.Unwrap();
            }
            catch (ArgumentNullException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            return result;
        }
    }
}
