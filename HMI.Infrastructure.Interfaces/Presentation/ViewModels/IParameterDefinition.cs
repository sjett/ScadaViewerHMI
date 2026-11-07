// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 参数定义接口
    /// </summary>
    public interface IParameterDefinition : ICollectionItem
    {
        /// <summary>
        /// 名称更改事件
        /// </summary>
        event EventHandler<ValueChangedEventArgs<string>> NameChanged;
        /// <summary>
        /// 默认值更改事件
        /// </summary>
        event EventHandler<ValueChangedEventArgs<string>> DefaultValueChanged;
        /// <summary>
        /// 名称
        /// </summary>
        string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 默认值
        /// </summary>
        string DefaultValue
        {
            get;
            set;
        }
        /// <summary>
        /// 获取参数定义的XAML引用
        /// </summary>
        /// <returns></returns>
        string GetParameterDefinitionXamlReference();

        /// <summary>
        /// 添加命名空间定义
        /// </summary>
        /// <param name="list"></param>
        void AddNamespaceDefinitions(List<string> list);

        /// <summary>
        /// 添加程序集引用
        /// </summary>
        /// <param name="references"></param>
        void AddReferences(List<AssemblyReference> references);
    }
}
