// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Modules.Design.Attributes
{
    /// <summary>
    /// 可见性属性特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class VisibilityPropertyAttribute : Attribute
    {

        public string IsVisibilePropertyName
        {
            get;
            private set;
        }
        /// <summary>
        /// 可见性属性特性
        /// </summary>
        /// <param name="isVisibilePropertyName"></param>
        public VisibilityPropertyAttribute(string isVisibilePropertyName)
        {
            IsVisibilePropertyName = isVisibilePropertyName;
        }
    }
}
