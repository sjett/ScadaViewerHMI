// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 公共控件视图模型接口
    /// </summary>
    public interface ICommonControlViewModel : IExplorerNodeViewModel, ISignalPlaceholderResolver
    {
        /// <summary>
        /// 模板源更改事件
        /// </summary>
        event EventHandler<ValueChangedEventArgs<ITemplatePageViewModel>> TemplateSourceChanged;
        /// <summary>
        /// 焦点请求事件
        /// </summary>
        event EventHandler FocusRequested;
        /// <summary>
        /// 操作监视器
        /// </summary>
        IActionMonitor ActionMonitor
        {
            get;
        }
        /// <summary>
        /// 模板标题
        /// </summary>
        string TemplateTitle
        {
            get;
            set;
        }
        /// <summary>
        /// 模板模态性
        /// </summary>
        DialogModality TemplateModality
        {
            get;
            set;
        }
        /// <summary>
        /// 是否显示模板
        /// </summary>
        bool ShowTemplate
        {
            get;
            set;
        }
        /// <summary>
        /// 模板源
        /// </summary>
        ITemplatePageViewModel TemplateSource
        {
            get;
            set;
        }
        /// <summary>
        /// 参数值集合
        /// </summary>
        ObservableCollection<IParameterValue> ParameterValues
        {
            get;
        }
        /// <summary>
        /// 模板是否启用
        /// </summary>
        bool IsTemplateEnabled
        {
            get;
            set;
        }
        /// <summary>
        /// 设计器所需大小
        /// </summary>
        Size DesignerDesiredSize
        {
            get;
        }
        /// <summary>
        /// 控件唯一标识符
        /// </summary>
        Guid ControlID
        {
            get;
            set;
        }
        /// <summary>
        /// 工具提示
        /// </summary>
        string ToolTip
        {
            get;
            set;
        }
        /// <summary>
        /// Z索引
        /// </summary>
        int ZIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 旋转角度
        /// </summary>
        double Angle
        {
            get;
            set;
        }
        /// <summary>
        /// X平移值
        /// </summary>
        double XTranslation
        {
            get;
            set;
        }
        /// <summary>
        /// Y平移值
        /// </summary>
        double YTranslation
        {
            get;
            set;
        }
        /// <summary>
        /// 控件宽度
        /// </summary>
        double ControlWidth
        {
            get;
            set;
        }
        /// <summary>
        /// 控件高度
        /// </summary>
        double ControlHeight
        {
            get;
            set;
        }
        /// <summary>
        /// 实际宽度
        /// </summary>
        double ActualWidth
        {
            get;
            set;
        }
        /// <summary>
        /// 实际高度
        /// </summary>
        double ActualHeight
        {
            get;
            set;
        }
        /// <summary>
        /// 宽度
        /// </summary>
        double Width
        {
            get;
            set;
        }
        /// <summary>
        /// 高度
        /// </summary>
        double Height
        {
            get;
            set;
        }
        /// <summary>
        /// 不透明度
        /// </summary>
        double Opacity
        {
            get;
            set;
        }
        /// <summary>
        /// 控件类型
        /// </summary>
        Type ControlType
        {
            get;
        }
        /// <summary>
        /// 显示选择装饰器
        /// </summary>
        bool ShowSelectionDecorator
        {
            get;
            set;
        }
        /// <summary>
        /// 显示选择预览
        /// </summary>
        bool ShowSelectionPreview
        {
            get;
            set;
        }
        /// <summary>
        /// 显示旋转装饰器
        /// </summary>
        bool ShowRotationDecorator
        {
            get;
            set;
        }
        /// <summary>
        /// 显示拖动装饰器
        /// </summary>
        bool ShowDragDecorator
        {
            get;
            set;
        }
        /// <summary>
        /// 显示调整大小装饰器
        /// </summary>
        bool ShowResizeDecorator
        {
            get;
            set;
        }
        /// <summary>
        /// 缩放比例
        /// </summary>
        double Scale
        {
            get;
        }
        /// <summary>
        /// 反向缩放比例
        /// </summary>
        double InverseScale
        {
            get;
        }
        /// <summary>
        /// 是否强制等比例调整大小
        /// </summary>
        bool ForceProportionalResize
        {
            get;
        }
        /// <summary>
        /// 模板页面集合
        /// </summary>
        ObservableCollection<ITemplatePageViewModel> TemplatePages
        {
            get;
        }
        /// <summary>
        /// 选中控件
        /// </summary>
        void Select();
        /// <summary>
        /// 切换选择控件
        /// </summary>
        void ToggleSelection();
        /// <summary>
        /// 更新选择状态
        /// </summary>
        /// <param name="isShiftKeyPressed">Shift 是否按下</param>
        /// <param name="isControlKeyPressed">Ctrl 是否按下</param>
        void UpdateSelection(bool isShiftKeyPressed, bool isControlKeyPressed);
        /// <summary>
        /// 旋转控件
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="transformMode"></param>
        /// <param name="snapAngle"></param>
        void Rotate(double angle, TransformMode transformMode, bool snapAngle = false);
        /// <summary>
        /// 平移控件
        /// </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <param name="mode"></param>
        void Translate(double horizontal, double vertical, TransformMode mode);
        /// <summary>
        /// 调整控件大小
        /// </summary>
        /// <param name="horizontalHandlePosition">水平手柄位置</param>
        /// <param name="verticalHandlePosition">垂直手柄位置</param>
        /// <param name="dragX"></param>
        /// <param name="dragY"></param>
        void Resize(HorizontalAlignment horizontalHandlePosition, VerticalAlignment verticalHandlePosition, double dragX, double dragY);
        /// <summary>
        /// 获取XAML
        /// </summary>
        /// <returns></returns>
        string GetXaml();
        /// <summary>
        /// 获取命名空间集合
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetNamespaces();
        /// <summary>
        /// 获取所需程序集集合
        /// </summary>
        /// <returns></returns>
        IEnumerable<AssemblyReference> GetRequiredAssemblies();
        /// <summary>
        /// 获取Silverlight对象参数集合
        /// </summary>
        /// <returns></returns>
        IEnumerable<KeyValuePair<string, string>> GetSilverlightObjectParameters();

        /// <summary>
        /// 请求焦点
        /// </summary>
        void RequestFocus();
        /// <summary>
        /// 更新装饰器
        /// </summary>
        void UpdateDecorators();
        /// <summary>
        /// 变换开始
        /// </summary>
        void TransformationStarted();
    }
}
