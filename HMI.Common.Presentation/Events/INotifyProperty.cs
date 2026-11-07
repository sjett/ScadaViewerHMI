// SPDX-License-Identifier: GPL-3.0-only
using System.ComponentModel;

namespace HMI.Common.Presentation.Events
{
    /// <summary>
    /// 属性更改通知接口
    /// </summary>
    public interface INotifyProperty : INotifyPropertyChanged, INotifyPropertyChanging
    {
    }
}
