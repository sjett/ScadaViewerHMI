// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Services;
using System.ComponentModel;
using System.Net;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    

    /// <summary>
    /// 参数值类
    /// </summary>
    public sealed class ParameterValue : CollectionItem, IExplicitPropertyMetadataProvider, IParameterValue
    {
        private IParameterDefinition parameterDefinition;
        private bool _hasExplicitValue;
        private string name;
        private string val;
        /// <summary>
        /// 参数值更改事件
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<string>>? ParameterValueChanged;
        /// <summary>
        /// 是否已链接到参数定义
        /// </summary>
        public bool IsLinked
        {
            get
            {
                return this.parameterDefinition != null;
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        [ReadOnly(true)]
        public string Name
        {
            get
            {
                if (this.parameterDefinition == null)
                {
                    return this.name;
                }
                return this.parameterDefinition.Name;
            }
            set
            {
                this.name = value;
            }
        }
        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get
            {
                if (this._hasExplicitValue)
                {
                    return this.val;
                }
                if (this.parameterDefinition == null)
                {
                    return null;
                }
                return this.parameterDefinition.DefaultValue;
            }
            set
            {
                if (this.val != value)
                {
                    base.RaisePropertyChanging<string>(() => this.Value);
                    ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.val, value);
                    this.val = value;
                    this._hasExplicitValue = true;
                    this.ParameterValueChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<string>(() => this.Value);
                }
            }
        }
        /// <summary>
        /// 参数定义
        /// </summary>
        [Browsable(false)]
        public IParameterDefinition Definition
        {
            get
            {
                return this.parameterDefinition;
            }
        }
        /// <summary>
        /// 参数定义元数据提供者
        /// </summary>
        public IExplicitPropertyMetadataProvider DefinitionMetadata
        {
            get
            {
                return this.parameterDefinition as IExplicitPropertyMetadataProvider;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="container"></param>
        /// <param name="parentNode"></param>
        public ParameterValue(IParameterDefinition definition, IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
        {
            this.parameterDefinition = definition;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container"></param>
        /// <param name="parentNode"></param>
        public ParameterValue(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
        {
        }
        /// <summary>
        /// 深度复制字段
        /// </summary>
        /// <param name="target"></param>
        public override void DeepCopyFields(ICollectionItem target)
        {
            ParameterValue parameterValue = (ParameterValue)target;
            parameterValue.Value = this.Value;
            parameterValue._hasExplicitValue = this._hasExplicitValue;
            parameterValue.parameterDefinition = this.Definition;
        }
        /// <summary>
        /// 创建克隆
        /// </summary>
        /// <returns></returns>
        public override ICollectionItem CreateClone()
        {
            return new ParameterValue(this.Definition, base.Services, base.Parent);
        }
        /// <summary>
        /// 获取XAML
        /// </summary>
        /// <returns></returns>
        public override string GetXaml()
        {
            string arg = string.IsNullOrWhiteSpace(this.Value) ? this.Value : WebUtility.HtmlEncode(this.Value);
            return string.Format("<HMIShared:ParameterItem ParameterName=\"{0}\" ParameterValue=\"{1}\"></HMIShared:ParameterItem>", this.Name, arg);
        }

        /// <summary>
        /// 获取属性的特性集合
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public ICollection<Attribute>? GetAttributes(string propertyName)
        {
            if (!(propertyName != "Value") && this.parameterDefinition is IExplicitPropertyMetadataProvider)
            {
                return (this.parameterDefinition as IExplicitPropertyMetadataProvider)?.GetAttributes("DefaultValue");
            }
            return null;
        }
    }
}
