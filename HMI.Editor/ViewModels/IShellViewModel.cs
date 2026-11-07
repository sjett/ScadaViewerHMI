// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using HMI.Infrastructure.Controls;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services.Logging;

namespace HMI.Editor.ViewModels
{
    /// <summary>
    /// 主视图模型接口
    /// </summary>
    public interface IShellViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 打开的文档集合
        /// </summary>
        ObservableCollection<UserControl> Documents
        {
            get;
        }
        /// <summary>
        /// 所有文档集合
        /// </summary>
        /// </summary>
        ObservableCollection<UserControl> AllDocuments
        {
            get;
        }
        /// <summary>
        /// 静态符号集合
        /// </summary>
        ObservableCollection<IStaticSymbol> StaticSymbols
        {
            get;
        }
        ILoggingService LoggingService
        {
            get;
        }

        /// <summary>
        /// 命令接收器（项目管理器视图模型）
        /// </summary>
        IProjectManagerViewModel CommandSink
        {
            get;
        }
        /// <summary>
        /// 参考线设置视图模型
        /// </summary>
        IGuideSettingsViewModel GuideSettings
        {
            get;
        }
        /// <summary>
        /// 控件工具箱是否可见
        /// </summary>
        bool IsControlToolboxVisible
        {
            get;
            set;
        }
        /// <summary>
        /// 项目资源管理器是否可见
        /// </summary>
        bool IsProjectExplorerVisible
        {
            get;
            set;
        }
        /// <summary>
        /// 属性编辑器是否可见
        /// </summary>
		bool IsPropertyEditorVisible
        {
            get;
            set;
        }
        /// <summary>
        /// 日志窗口是否可见
        /// </summary>
		bool IsLogVisible
        {
            get;
            set;
        }
        bool IsProjectEnginingVisible
        {
            get;
            set;
        }
        bool IsActionEffectEditorVisible
        {
            get;
            set;
        }

        /// <summary>
        /// AI 智能体
        /// </summary>
        AIPromptViewModel AIPrompt { get; }
    }
}
