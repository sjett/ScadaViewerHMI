// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.Events;
using HMI.Domain.Services;

namespace HMI.Domain.Models
{
    /// <summary>
    /// 属性接口
    /// </summary>
    public interface IProperty :  INotifyProperty, IBindingSource, IDisposable
    {
        /// <summary>
        /// 属性编辑器构建器
        /// </summary>
        IPropertyEditorBuilder PropertyEditorBuilder
        {
            get;
        }
        /// <summary>
        /// 是否只读
        /// </summary>
        bool IsReadOnly
        {
            get;
        }
        /// <summary>
        /// 是否集合
        /// </summary>
        bool IsCollection
        {
            get;
        }
        /// <summary>
        /// 是否可见
        /// </summary>
        bool IsVisible
        {
            get;
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        bool IsEnabled
        {
            get;
        }
        /// <summary>
        /// 显示名称
        /// </summary>
        string DisplayName
        {
            get;
        }
        /// <summary>
        /// 名称
        /// </summary>
        string Name
        {
            get;
        }
        /// <summary>
        /// 类别
        /// </summary>
        string Category
        {
            get;
        }
        /// <summary>
        /// 排序号
        /// </summary>
        double SortOrder
        {
            get;
        }
        /// <summary>
        /// 类型
        /// </summary>
        Type Type
        {
            get;
        }
        /// <summary>
        /// 是否支持模板化
        /// </summary>
        bool SupportsTemplating
        {
            get;
        }
        /// <summary>
        /// 是否未定义
        /// </summary>
        bool IsUndefined
        {
            get;
        }
        /// <summary>
        /// 获取属性特性
        /// </summary>
        /// <typeparam name="TAttribute">属性特性</typeparam>
        /// <returns></returns>
        IEnumerable<TAttribute> GetAttributes<TAttribute>();
        /// <summary>
        /// 是否与指定属性兼容
        /// </summary>
        /// <param name="property">指定属性</param>
        /// <returns></returns>
        bool IsCompatibileWith(IProperty property);
    }
}
