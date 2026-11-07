// SPDX-License-Identifier: GPL-3.0-only
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Resources;

namespace HMI.Modules.Design.Attributes
{
    /// <summary>
    /// 属性分类特性，用于本地化属性分类名称
    /// </summary>
    public class PropertyCategoryAttribute : CategoryAttribute
    {
        private readonly ResourceManager _resourceManager;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="category"></param>
        /// <param name="resourceManagerType"></param>
        public PropertyCategoryAttribute(string category, Type resourceManagerType) : base(category)
        {
            Contract.Requires<ArgumentNullException>(category != null, "category");
            Contract.Requires<ArgumentException>(category != string.Empty, "category");
            Contract.Requires<ArgumentNullException>(resourceManagerType != null, "resourceManagerType");
            _resourceManager = new ResourceManager(resourceManagerType);
        }
        /// <summary>
        /// 获取本地化字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override string GetLocalizedString(string value)
        {
            return _resourceManager.GetString(value) ?? base.GetLocalizedString(value);
        }
    }
}
