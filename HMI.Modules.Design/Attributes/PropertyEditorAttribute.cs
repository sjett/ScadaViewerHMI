// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Modules.Design.Attributes.Validation;
using HMI.Modules.Design.Resources;
using HMI.Modules.Design.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HMI.Modules.Design.Attributes
{
    /// <summary>
    /// 属性编辑器属性基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class PropertyEditorAttribute : Attribute, IPropertyEditorBuilder
    {
        /// <summary>
        /// 服务定位器
        /// </summary>
        protected IServiceProvider ServiceLocator
        {
            get;
            private set;
        }
        /// <summary>
        /// 创建编辑器
        /// </summary>
        /// <param name="property"></param>
        /// <param name="bindingSource"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public FrameworkElement CreateEditor(IProperty property, IBindingSource bindingSource)
        {
            if (property == null)
            {
                throw new InvalidOperationException(ErrorTexts.InitializeMustBeCalledBeforeCreateEditor);
            }
            IEditorView editorView = BuildEditor(property, bindingSource);
            editorView.ServiceLocator = ServiceLocator;
            editorView.Context = bindingSource;
            AssignBindings(editorView, property, bindingSource);
            return (FrameworkElement)editorView;
        }
        /// <summary>
        /// 是否支持多选兼容
        /// </summary>
        /// <param name="property"></param>
        /// <param name="otherProperty"></param>
        /// <returns></returns>
        public abstract bool IsMultiSelectionCompatibleWith(IProperty property, IProperty otherProperty);
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serviceLocator"></param>
        public virtual void Initialize(IServiceProvider serviceLocator)
        {
            ServiceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));
        }
        /// <summary>
        /// 构建编辑器
        /// </summary>
        /// <param name="property"></param>
        /// <param name="editedObject"></param>
        /// <returns></returns>
        protected abstract IEditorView BuildEditor(IProperty property, IBindingSource editedObject);
        /// <summary>
        /// 分配绑定
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="property"></param>
        /// <param name="bindingSource"></param>
        protected virtual void AssignBindings(IEditorView editor, IProperty property, IBindingSource bindingSource)
        {
            editor.ValueBinding = CreateValueBinding(bindingSource, property);
        }
        /// <summary>
        /// 创建值绑定
        /// </summary>
        /// <param name="bindingSource"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        protected virtual Binding CreateValueBinding(IBindingSource bindingSource, IProperty property)
        {
            Binding binding = new Binding
            {
                Source = bindingSource.SourceInstance,
                Path = new PropertyPath(bindingSource.PropertyPath, new object[0]),
                Mode = property.IsReadOnly || property.IsCollection ? BindingMode.OneWay : BindingMode.TwoWay,
                NotifyOnValidationError = true,
                ValidatesOnExceptions = true,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnDataErrors = true
            };
            AddValidationRules(property, bindingSource.PropertyPath, binding);
            return binding;
        }
        /// <summary>
        /// 设置验证规则
        /// </summary>
        private void AddValidationRules(IProperty property, string propertyName, Binding binding)
        {
            foreach (ValidationRuleAttribute ruleAttribute in property.GetAttributes<ValidationRuleAttribute>())
            {
                ValidationRule validationRule = ruleAttribute.GetValidationRule(property.SourceInstance, propertyName);
                validationRule.ValidatesOnTargetUpdated = true;
                binding.ValidationRules.Add(validationRule);
            }
        }
    }
}
