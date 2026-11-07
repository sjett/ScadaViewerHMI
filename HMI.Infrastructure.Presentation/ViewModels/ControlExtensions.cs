// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Presentation.ViewModels
{
	public static class ControlExtensions
	{
        /// <summary>
        /// 获取节点集合中的控件视图模型
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static IEnumerable<ICommonControlViewModel> Controls(this IEnumerable<IExplorerNodeViewModel> nodes)
		{
			return nodes.OfType<ICommonControlViewModel>();
		}
        /// <summary>
        /// 获取节点集合中符合条件的控件视图模型
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="predicate">筛选</param>
        /// <returns></returns>
        public static IEnumerable<ICommonControlViewModel> Controls(this IEnumerable<IExplorerNodeViewModel> nodes, Func<ICommonControlViewModel, bool> predicate)
		{
			return nodes.OfType<ICommonControlViewModel>().Where(predicate);
		}
	}
}
