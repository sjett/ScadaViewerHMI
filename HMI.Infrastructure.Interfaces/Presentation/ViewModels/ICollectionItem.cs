// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Services;
using System.ComponentModel;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 集合项接口
    /// </summary>
    public interface ICollectionItem : IHierarchicalNode, INotifyPropertyChanged
    {
        /// <summary>
        /// 编辑器显示文本更改事件
        /// </summary>
        event EventHandler<EventArgs> EditorDisplayTextChanged;
        /// <summary>
        /// 属性监视器
        /// </summary>
        IPropertyMonitor PropertyMonitor
        {
            get;
            set;
        }
        /// <summary>
        /// 父集合
        /// </summary>
        IEnumerable<ICollectionItem> ParentCollection
        {
            get;
            set;
        }
        /// <summary>
        /// 唯一标识符
        /// </summary>
        Guid Id
        {
            get;
            set;
        }
        /// <summary>
        /// 显示文本
        /// </summary>
        string DisplayText
        {
            get;
        }
        /// <summary>
        /// 深度复制字段
        /// </summary>
        /// <param name="target"></param>
        void DeepCopyFields(ICollectionItem target);
        /// <summary>
        /// 克隆集合项
        /// </summary>
        /// <returns></returns>
        ICollectionItem Clone();
        /// <summary>
        /// 创建克隆
        /// </summary>
        /// <returns></returns>
        ICollectionItem CreateClone();
        /// <summary>
        /// 更新编辑器显示文本
        /// </summary>
        void UpdateEditorDisplayText();
        /// <summary>
        /// 获取XAML
        /// </summary>
        /// <returns></returns>
        string GetXaml();
    }
}
