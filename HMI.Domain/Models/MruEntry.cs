// SPDX-License-Identifier: GPL-3.0-only
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HMI.Domain.Models
{
    /// <summary>
    /// 最近使用的文件条目
    /// </summary>
    public class MruEntry : IEquatable<MruEntry>, INotifyPropertyChanged
    {
        private string? _name;
        private string? _path;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 路径
        /// </summary>
        public string? Path
        {
            get
            {
                return _path;
            }
            set
            {
                if (_path != value)
                {
                    _path = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="itemName"></param>
        public MruEntry(string itemName)
        {
            Name = System.IO.Path.GetFileNameWithoutExtension(itemName);
            Path = itemName;
        }
        /// <summary>
        /// 比较是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(MruEntry? other)
        {
            return other != null && Path == other.Path;
        }
    }
}
