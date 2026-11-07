// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.Common.Modularity
{
    using System.ComponentModel;

    public interface IClonableEditableModel<T> : IModel, INotifyPropertyChanged, IEditableModel, IEditableObject where T: IEditableModel
    {
        T Clone();
    }
}

