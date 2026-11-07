// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Modules.Design.Attributes
{
    /// <summary>
    /// 编辑器顺序属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EditorOrderAttribute : Attribute
    {
        /// <summary>
        /// 默认顺序
        /// </summary>
        public static readonly EditorOrderAttribute Default = new EditorOrderAttribute(0.0);

        /// <summary>
        /// 顺序
        /// </summary>
        public double Order
        {
            get;
            private set;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="order"></param>
        public EditorOrderAttribute(double order)
        {
            Order = order;
        }
    }
}
