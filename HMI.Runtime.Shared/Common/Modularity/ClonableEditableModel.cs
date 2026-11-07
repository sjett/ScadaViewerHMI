// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.Common.Modularity
{
    using System;
    using System.ComponentModel;

    public abstract class ClonableEditableModel<T> : EditableModel, IClonableEditableModel<T>, IModel, INotifyPropertyChanged, IEditableModel, IEditableObject where T: IEditableModel
    {
        protected ClonableEditableModel()
        {
        }

        public abstract T Clone();
    }
}

