// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 分组容器接口
    /// </summary>
    public interface IGroupContainer
    {
        /// <summary>
        /// 将选中项提升为组
        /// </summary>
        void PromoteSelectionToGroup();
        /// <summary>
        /// 将组降级为选中项
        /// </summary>
        /// <param name="groupControl">组</param>
        void DemoteGroupToSelection(IControlGroupContainerViewModel groupControl);
    }
}
