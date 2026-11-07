// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.Common.Modularity
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.ComponentModel;

    public interface ISerializableEditableModel<T> : IModel, INotifyPropertyChanged, IEditableModel, IEditableObject where T: IEditableModel
    {
        T Deserialize(JObject element);
        string Serialize();
    }
}

