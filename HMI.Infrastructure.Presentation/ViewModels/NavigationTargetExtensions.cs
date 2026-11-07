// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Presentation.ViewModels
{
	public static class NavigationTargetExtensions
	{
		public static IEnumerable<INavigationTargetViewModel> NavigationTargets(this IEnumerable<IExplorerNodeViewModel> nodes)
		{
			return nodes
				.Where(t=> t is INavigationTargetViewModel)
				.Cast<INavigationTargetViewModel>();
		}
		public static IEnumerable<INavigationTargetViewModel> NavigationTargets(this IEnumerable<IExplorerNodeViewModel> nodes, Func<INavigationTargetViewModel, bool> predicate)
		{
			return nodes
				.Where(t=> t is INavigationTargetViewModel)
				.Cast<INavigationTargetViewModel>()
				.Where(predicate);
		}

	}
}
