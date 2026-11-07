// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Presentation.ViewModels
{
	public static class PageExtensions
	{
		public static IEnumerable<IProjectPageViewModel> Pages(this IEnumerable<IExplorerNodeViewModel> nodes)
		{
			return nodes
				.Where(t=> t is IProjectPageViewModel && !(t is ITemplatePageViewModel))
				.Cast<IProjectPageViewModel>();
		}
		public static IEnumerable<IProjectPageViewModel> Pages(this IEnumerable<IExplorerNodeViewModel> nodes, Func<IProjectPageViewModel, bool> predicate)
		{
			return nodes
				.Where(t=> t is IProjectPageViewModel && !(t is ITemplatePageViewModel))
				.Cast<IProjectPageViewModel>()
				.Where(predicate);
		}

	}
}
