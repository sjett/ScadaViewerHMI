// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;
using System.Windows;
using System.Windows.Controls;
namespace HMI.Modules.Explorer
{
	public class ExplorerStyleSelector : StyleSelector
	{
		
		public Style ProjectStyle
		{
			get;
			set;
		}
		public Style PageStyle
		{
			get;
			set;
		}
		public Style TemplatePageStyle
		{
			get;
			set;
		}
		public Style NavigationItemStyle
		{
			get;
			set;
		}
		public Style ControlStyle
		{
			get;
			set;
		}
		public Style OrphanNavigationItemStyle
		{
			get;
			set;
		}
		public override Style SelectStyle(object item, DependencyObject container)
		{
			if (item is IProjectViewModel)
			{
				return ProjectStyle;
			}
			if (item is ITemplatePageViewModel)
			{
				return TemplatePageStyle;
			}
			if (item is IProjectPageViewModel)
			{
				return PageStyle;
			}
			if (!(item is INavigationTargetViewModel))
			{
				return ControlStyle;
			}
			if (((INavigationTargetViewModel)item).IsInvalidTarget)
			{
				return OrphanNavigationItemStyle;
			}
			return NavigationItemStyle;
		}
	}
}
