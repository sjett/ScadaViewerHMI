// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using System.Windows;

namespace HMI.Domain.Services
{
    /// <summary>
    /// 属性编辑器构建器接口
    /// </summary>
    public interface IPropertyEditorBuilder
    {
        /// <summary>
        /// 创建编辑器
        /// </summary>
        /// <param name="property">属性</param>
        /// <param name="bindingSource">绑定源</param>
        /// <returns></returns>
        FrameworkElement CreateEditor(IProperty property, IBindingSource bindingSource);
        /// <summary>
        /// 是否支持多选兼容
        /// </summary>
        /// <param name="thisProperty">当前属性</param>
        /// <param name="otherProperty">其它属性</param>
        /// <returns></returns>
        bool IsMultiSelectionCompatibleWith(IProperty thisProperty, IProperty otherProperty);
    }
}
