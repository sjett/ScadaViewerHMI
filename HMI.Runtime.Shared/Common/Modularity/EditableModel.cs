// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.Common.Modularity
{
    using System;
    using System.ComponentModel;

    public abstract class EditableModel : Model, IModel, INotifyPropertyChanged, IEditableModel, IEditableObject
    {
        protected EditableModel()
        {
        }

        public abstract void BeginEdit();
        public abstract void CancelEdit();
        public abstract void EndEdit();
    }
}

