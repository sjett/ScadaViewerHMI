// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.Common.Modularity
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.ComponentModel;

    public abstract class SerializableEditableModel<T> : EditableModel, ISerializableEditableModel<T>, IModel, INotifyPropertyChanged, IEditableModel, IEditableObject where T: IEditableModel
    {
        protected SerializableEditableModel()
        {
        }

        public abstract T Deserialize(JObject element);
        public abstract string Serialize();
    }
}

