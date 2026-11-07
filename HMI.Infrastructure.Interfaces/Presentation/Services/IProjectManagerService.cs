// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 项目管理服务接口
    /// </summary>
    public interface IProjectManagerService
    {
        /// <summary>
        /// 保存项目
        /// </summary>
        /// <param name="project">项目视图模型</param>
        void Save(IProjectViewModel project);
        /// <summary>
        /// 项目另存为
        /// </summary>
        /// <param name="project">项目视图模型</param>
        /// <param name="projectFolder">项目存储目录</param>
        /// <param name="projectName">项目名称</param>
        void SaveAs(IProjectViewModel project, string projectFolder, string projectName);
        /// <summary>
        /// 打开项目
        /// </summary>
        /// <param name="path">项目路径</param>
        /// <returns>项目视图模型</returns>
        IProjectViewModel OpenProject(string path);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder">项目存储目录</param>
        /// <param name="name">项目名称</param>
        /// <returns>项目视图模型</returns>
        IProjectViewModel NewProject(string folder, string name);
        /// <summary>
        /// 添加模板页面
        /// </summary>
        /// <param name="parent">上级节点</param>
        /// <param name="name">模板页名称</param>
        /// <returns>模板页面视图模型</returns>
        ITemplatePageViewModel AddTemplatePage(IExplorerNodeViewModel parent, string name);
        /// <summary>
        /// 添加导航页面
        /// </summary>
        /// <param name="parent">上级节点</param>
        /// <param name="name">导航页面名称</param>
        /// <returns>页面视图模型</returns>
        IProjectPageViewModel AddNavigationPage(IExplorerNodeViewModel parent, string name);
        /// <summary>
        /// 添加普通页面
        /// </summary>
        /// <param name="parent">上级节点</param>
        /// <param name="name">页面名称</param>
        /// <returns>页面视图模型</returns>
        IProjectPageViewModel AddPage(IExplorerNodeViewModel parent, string name);
        /// <summary>
        /// 移除页面
        /// </summary>
        /// <param name="page">页面视图模型</param>
        void RemovePage(IProjectPageViewModel page);
        /// <summary>
        /// 移除多个节点(图元组件等)
        /// </summary>
        /// <param name="parent">上级节点</param>
        /// <param name="items">要移除的节点</param>
        void RemoveItems(IExplorerNodeViewModel parent, IEnumerable<IExplorerNodeViewModel> items);
    }

}
