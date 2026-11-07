// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using System.Collections.ObjectModel;

namespace HMI.Infrastructure.Models
{
    public class HorizontalEditorItem
    {
        public string Key
        {
            get;
            set;
        }
        public object Value
        {
            get;
            set;
        }
        public ObservableCollection<IProperty> Properties
        {
            get;
            set;
        }
        public HorizontalEditorItem()
        {
            Properties = new ObservableCollection<IProperty>();
        }
    }
}
