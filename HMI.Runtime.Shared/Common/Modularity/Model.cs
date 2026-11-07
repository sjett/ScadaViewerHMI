// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.Common.Modularity
{
    using System;
    using System.ComponentModel;
    using HMI.Runtime.Shared.Common;

    public abstract class Model : PropertyChangedNotifier, IModel, INotifyPropertyChanged
    {
        protected Model()
        {
        }
    }
}

