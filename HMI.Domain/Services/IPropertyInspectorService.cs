// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI.Domain.Services
{
    /// <summary>
    /// 属性检查服务
    /// </summary>
    public interface IPropertyInspectorService
    {
        /// <summary>
        /// 获取属性集合
        /// </summary>
        /// <param name="projectViewModel">项目视图模型</param>
        /// <returns></returns>
        ICollection<IProperty> GetProperties(params object[] projectViewModel);
    }
}
