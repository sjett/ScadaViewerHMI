// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HMI.Modules.Explorer
{
	public class ExplorerDataTemplateSelector : DataTemplateSelector
	{
		
		public HierarchicalDataTemplate ProjectTemplate
		{
			get;
			set;
		}
		public HierarchicalDataTemplate PageTemplate
		{
			get;
			set;
		}
		public HierarchicalDataTemplate TemplatePageTemplate
		{
			get;
			set;
		}
		public HierarchicalDataTemplate NavigationItemTemplate
		{
			get;
			set;
		}
		//孤立导航项模板
		public HierarchicalDataTemplate OrphanNavigationItemTemplate
		{
			get;
			set;
		}
		public DataTemplate ControlTemplate
		{
			get;
			set;
		}
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item is IProjectViewModel)
			{
				return ProjectTemplate;
			}
			if (item is ITemplatePageViewModel)
			{
				return TemplatePageTemplate;
			}
			if (item is IProjectPageViewModel)
			{
				return PageTemplate;
			}
			if (!(item is INavigationTargetViewModel))
			{
				return ControlTemplate;
			}
			if (((INavigationTargetViewModel)item).IsInvalidTarget)
			{
				return OrphanNavigationItemTemplate;
			}
			return NavigationItemTemplate;
		}
	}
}
