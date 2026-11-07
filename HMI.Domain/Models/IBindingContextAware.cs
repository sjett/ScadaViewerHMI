// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMI.Domain.Models
{
    /// <summary>
    /// 绑定上下文感知接口
    /// </summary>
    public interface IBindingContextAware
    {
        /// <summary>
        /// 绑定上下文
        /// </summary>
        IBindingSource Context
        {
            get;
            set;
        }
    }
}
