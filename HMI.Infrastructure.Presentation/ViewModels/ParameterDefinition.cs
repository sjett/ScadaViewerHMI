// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Infrastructure.Presentation.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    

    /// <summary>
    /// 参数定义抽象类
    /// </summary>
    public abstract class ParameterDefinition : CollectionItem,  IExplicitPropertyMetadataProvider, IParameterDefinition
    {
        private string name;
        private string defaultValue;

        /// <summary>
        /// 名称更改事件
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<string>>? NameChanged;
        /// <summary>
        /// 默认值更改事件
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<string>>? DefaultValueChanged;
        /// <summary>
        /// 内容替换服务
        /// </summary>
        [Browsable(false)]
        public IContentReplacementService ContentReplacementService
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Browsable(true), EditorOrder(1.0), CustomMethodValidation("ValidateUniqueName"), NotNull]
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (this.name != value)
                {
                    ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.name, value);
                    base.RaisePropertyChanging<string>(() => this.Name);
                    this.name = value;
                    this.NameChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<string>(() => this.Name);
                }
            }
        }
        /// <summary>
        /// 默认值
        /// </summary>
        [EditorOrder(2.0)]
        public string DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
            set
            {
                if (this.defaultValue != value)
                {
                    base.RaisePropertyChanging<string>(() => this.DefaultValue);
                    ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.defaultValue, value);
                    this.defaultValue = value;
                    this.DefaultValueChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<string>(() => this.DefaultValue);
                }
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container"></param>
        /// <param name="parentNode"></param>
        public ParameterDefinition(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
        {
            this.NameChanged += this.onNameChanged;
            this.ContentReplacementService = container.Resolve<IContentReplacementService>();
        }
        /// <summary>
        /// 获取属性的特性集合
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public abstract ICollection<Attribute> GetAttributes(string propertyName);
        /// <summary>
        /// 深度复制字段
        /// </summary>
        /// <param name="target"></param>
        public override void DeepCopyFields(ICollectionItem target)
        {
            ParameterDefinition parameterDefinition = target as ParameterDefinition;
            parameterDefinition.Name = this.Name;
            parameterDefinition.DefaultValue = this.DefaultValue;
        }

        /// <summary>
        ///  获取参数定义的XAML引用
        /// </summary>
        /// <returns></returns>
        public string GetParameterDefinitionXamlReference()
        {
            return string.Format("[PC:{0}]", this.Name);
        }
        /// <summary>
        /// 获取XAML表示形式
        /// </summary>
        /// <returns></returns>
        public override string GetXaml()
        {
            string parameterItemTemplate = ParameterDefinition.GetParameterItemTemplate();
            return string.Format(parameterItemTemplate, this.Name, string.Empty);
        }
        /// <summary>
        /// 添加命名空间定义
        /// </summary>
        /// <param name="list"></param>
        public virtual void AddNamespaceDefinitions(List<string> list)
        {
        }
        /// <summary>
        /// 添加程序集引用
        /// </summary>
        /// <param name="references"></param>
        public virtual void AddReferences(List<AssemblyReference> references)
        {
        }
        /// <summary>
        /// 名称更改处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onNameChanged(object sender, ValueChangedEventArgs<string> e)
        {
            base.UpdateEditorDisplayText();
        }
        /// <summary>
        /// 字符串表示形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name ?? GeneralTexts.NoName;
        }
        /// <summary>
        /// 获取参数项模板
        /// </summary>
        /// <returns></returns>
        protected static string GetParameterItemTemplate()
        {
            return string.Format("<{0}:{1} ParameterName=\"{2}\" ParameterValue=\"{3}\"></{0}:{1}>", new object[]
            {
                "HMIShared",
                "ParameterItem",
                "{0}",
                "{1}"
            });
        }
        /// <summary>
        /// 验证唯一名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ValidationResult ValidateUniqueName(string name, CultureInfo culture)
        {
            if (base.ParentCollection == null)
            {
                return ValidationResult.ValidResult;
            }
            IEnumerable<ParameterDefinition> source = base.ParentCollection.OfType<ParameterDefinition>();
            if (source.Any((pdef) => pdef != this && pdef.Name == name))
            {
                return new ValidationResult(false, ErrorTexts.ParameterDefinitionNameMustBeUnique);
            }
            return ValidationResult.ValidResult;
        }
    }
}
