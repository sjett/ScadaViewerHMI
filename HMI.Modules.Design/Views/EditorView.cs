// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using System.Windows.Controls;
using System.Windows.Data;

namespace HMI.Modules.Design.Views
{
    /// <summary>
    /// 编辑器视图基类
    /// </summary>
    public abstract class EditorView : Control, IEditorView
    {
        /// <summary>
        /// 值绑定
        /// </summary>
        public Binding ValueBinding
        {
            get;
            set;
        }
        /// <summary>
        /// 服务定位器
        /// </summary>
        public IServiceProvider ServiceLocator
        {
            get;
            set;
        }
        /// <summary>
        /// 绑定上下文
        /// </summary>
        public IBindingSource Context
        {
            get;
            set;
        }
    }
}
