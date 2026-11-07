// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.Commands;
using HMI.Infrastructure.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 项目管理器视图模型接口
    /// </summary>
    public interface IProjectManagerViewModel : ICommandSink, INotifyPropertyChanged, ISolution
    {
        /// <summary>
        /// 添加导航页面命令执行事件
        /// </summary>
        event CancelEventHandler AddTemplatePageCommandExecuted;
        /// <summary>
        /// 重命名命令执行事件
        /// </summary>
        event CancelEventHandler RenameCommandExecuted;
        /// <summary>
        /// 项目更改事件
        /// </summary>
        event EventHandler<ValueChangedEventArgs<IProjectViewModel>> ProjectChanged;
        /// <summary>
        /// 活动页面更改事件
        /// </summary>
        event EventHandler<ValueChangedEventArgs<IProjectPageViewModel>> ActivePageChanged;
        /// <summary>
        /// 通知服务
        /// </summary>
        INotificationService NotificationService
        {
            get;
            set;
        }
        /// <summary>
        /// 选中项
        /// </summary>
        IExplorerNodeViewModel SelectedItem
        {
            get;
            set;
        }

        /// <summary>
        /// 活动页面
        /// </summary>
        IProjectPageViewModel ActivePage
        {
            get;
            set;
        }

        /// <summary>
        /// 项目
        /// </summary>
        IProjectViewModel Project
        {
            get;
            set;
        }
        /// <summary>
        /// 根节点集合
        /// </summary>
        ObservableCollection<IExplorerNodeViewModel> RootItems
        {
            get;
        }
        /// <summary>
        /// 最近使用的项目条目集合
        /// </summary>
        ObservableCollection<MruEntry> MruItems
        {
            get;
        }
        /// <summary>
        /// 是否可以重命名
        /// </summary>
        bool CanRename
        {
            get;
        }
        /// <summary>
        /// 是否可以添加导航页面
        /// </summary>
        bool CanAddNavigationPage
        {
            get;
        }
        /// <summary>
        /// 是否可以添加模板页面
        /// </summary>
        bool CanAddTemplatePage
        {
            get;
        }
        /// <summary>
        /// 是否可以全选
        /// </summary>
        bool CanSelectAll
        {
            get;
        }
        /// <summary>
        /// 添加模板页面
        /// </summary>
        void AddTemplatePage();
        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="criteria"></param>
        void Deselect(Func<IExplorerNodeViewModel, bool> criteria);
        /// <summary>
        /// 全选
        /// </summary>
        void OnSelectAll();
    }

}
