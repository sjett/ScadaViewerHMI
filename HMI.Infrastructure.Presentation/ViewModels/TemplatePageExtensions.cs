// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Presentation.ViewModels
{
	public static class TemplatePageExtensions
	{
		
		public static IEnumerable<ITemplatePageViewModel> ChildTemplatePages(this IEnumerable<IExplorerNodeViewModel> nodes)
		{
			return nodes.Where(t=> t is ITemplatePageViewModel).Cast<ITemplatePageViewModel>();
		}
		public static IEnumerable<ITemplatePageViewModel> ChildTemplatePages(this IEnumerable<IExplorerNodeViewModel> nodes, Func<ITemplatePageViewModel, bool> predicate)
		{
			return nodes.Where(t=> t is ITemplatePageViewModel).Cast<ITemplatePageViewModel>().Where(predicate);
		}

	}
}
