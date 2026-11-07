// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Domain.Models
{
    /// <summary>
    /// 程序集引用
    /// </summary>
    public class AssemblyReference : IEquatable<AssemblyReference>
    {
        /// <summary>
        /// 程序集名称
        /// </summary>
        public string AssemblyName
        {
            get;
            set;
        }
        /// <summary>
        /// 是否本地复制
        /// </summary>
        public bool CopyLocal
        {
            get;
            set;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="copyLocal"></param>
        public AssemblyReference(string assemblyName, bool copyLocal = true)
        {
            AssemblyName = assemblyName;
            CopyLocal = copyLocal;
        }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(AssemblyReference other)
        {
            return other != null && AssemblyName == other.AssemblyName && CopyLocal == other.CopyLocal;
        }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as AssemblyReference);
        }
        /// <summary>
        /// 获取哈希码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return AssemblyName.GetHashCode() ^ CopyLocal.GetHashCode();
        }
        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return AssemblyName;
        }
    }
}
