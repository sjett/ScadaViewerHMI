// SPDX-License-Identifier: GPL-3.0-only

using HMI.Common.Collections;
using HMI.Domain.Models;

namespace HMI.Infrastructure.ViewModels
{
    public interface IPropertyEditorViewModel
    {
        event EventHandler PropertiesUpdating;
        event EventHandler PropertiesUpdated;
        IObservableCollection<IProperty> PropertyDescriptors
        {
            get;
        }
    }
}
