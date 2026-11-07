// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure.Services;
using System.Windows.Media;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 项目视图模型接口
    /// </summary>
    public interface IProjectViewModel : IExplorerNodeViewModel, IProject
    {
        /// <summary>
        /// 服务器设置更改事件
        /// </summary>
        event EventHandler ServerSettingsChanged;
        /// <summary>
        /// StretchPages属性更改事件
        /// </summary>
        event EventHandler<ValueChangedEventArgs<bool>> StretchPagesChanged;
        /// <summary>
        /// 项目标题
        /// </summary>
        string ProjectTitle
        {
            get;
            set;
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        string ProjectName
        {
            get;
        }
        /// <summary>
        /// 旧项目名称
        /// </summary>
        string OldProjectName
        {
            get;
        }
        /// <summary>
        /// 项目完整路径
        /// </summary>
        string ProjectFullPath
        {
            get;
        }
        /// <summary>
        /// 项目命名空间
        /// </summary>
        string ProjectNamespace
        {
            get;
        }
        /// <summary>
        /// Bin文件夹路径
        /// </summary>
        string BinFolder
        {
            get;
        }
        /// <summary>
        /// 构建文件夹路径
        /// </summary>
        string BuildFolder
        {
            get;
        }
        /// <summary>
        /// 客户端Bin文件夹路径
        /// </summary>
        [Obsolete]
        string ClientBinFolder
        {
            get;
        }
        /// <summary>
        /// 调试文件夹路径
        /// </summary>
        string DebugFolder
        {
            get;
        }
        /// <summary>
        /// App项目文件夹路径
        /// </summary>
        string AppProjectFolder
        {
            get;
        }
        /// <summary>
        /// 项目模板文件夹路径
        /// </summary>
        string ProjectTemplatesFolder
        {
            get;
        }
        /// <summary>
        /// HMI Silverlight控件压缩文件路径
        /// </summary>
        string HMISilverlightControlsZipFile
        {
            get;
        }
        /// <summary>
        /// App项目名称
        /// </summary>
        string AppProjectName
        {
            get;
        }
        /// <summary>
        /// 服务器名称
        /// </summary>
        string ServerName
        {
            get;
            set;
        }
        /// <summary>
        /// 物理地址
        /// </summary>
        string Host
        {
            get;
            set;
        }
        /// <summary>
        /// 协议
        /// </summary>
        //HttpProtocol Protocol
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 端口号
        /// </summary>
        int Port
        {
            get;
            set;
        }
        int Timeout
        {
            get;
            set;
        }
        string Username
        {
            get;
            set;
        }
        string Password
        {
            get;
            set;
        }
        string SecretKey
        {
            get;
            set;
        }
        /// <summary>
        /// 是否拉伸页面以适应屏幕
        /// </summary>
        bool StretchPages
        {
            get;
            set;
        }
        /// <summary>
        /// 页面默认背景色
        /// </summary>
        Brush PageDefaultBackground
        {
            get;
            set;
        }
        /// <summary>
        /// HTML页面背景色, wpf用于窗体背景色
        /// </summary>
        Color AppPageBackground
        {
            get;
            set;
        }
        /// <summary>
        /// 项目是否已修改
        /// </summary>
        bool IsModified
        {
            get;
            set;
        }
        /// <summary>
        /// 页面集合
        /// </summary>
        IObservableCollection<IProjectPageViewModel> Pages
        {
            get;
        }
        /// <summary>
        /// 模板页面集合
        /// </summary>
        IEnumerable<ITemplatePageViewModel> TemplatePages
        {
            get;
        }
        /// <summary>
        /// 动作监视器
        /// </summary>
        IActionMonitor ActionMonitor
        {
            get;
        }
        /// <summary>
        /// 状态默认背景色
        /// </summary>
        Brush StateDefaultBackground
        {
            get;
            set;
        }
        /// <summary>
        /// 状态默认前景色
        /// </summary>
        Brush StateDefaultForeground
        {
            get;
            set;
        }
        /// <summary>
        /// 快速闪烁速度
        /// </summary>
        double FastBlinkSpeed
        {
            get;
            set;
        }
        /// <summary>
        /// 中等闪烁速度
        /// </summary>
        double MediumBlinkSpeed
        {
            get;
            set;
        }
        /// <summary>
        /// 缓慢闪烁速度
        /// </summary>
        double SlowBlinkSpeed
        {
            get;
            set;
        }
        /// <summary>
        /// 设置项目属性
        /// </summary>
        /// <param name="name"></param>
        /// <param name="folder"></param>
        void SetProjectProperties(string name, string folder);
    }

    
}
