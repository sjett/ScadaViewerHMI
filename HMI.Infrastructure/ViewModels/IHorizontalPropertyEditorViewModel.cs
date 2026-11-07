// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Common.Presentation.ViewModels;
using HMI.Infrastructure.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HMI.Infrastructure.ViewModels
{
    public interface IHorizontalPropertyEditorViewModel : INotifyPropertyChanged, IViewModel
    {
        event EventHandler<ValueChangedEventArgs<string>> GroupByChanged;
        event EventHandler UpdateGridViewColumns;
        event EventHandler<ValueChangedEventArgs<HorizontalEditorItem>> SelectedItemChanged;
        ObservableCollection<HorizontalEditorItem> Items
        {
            get;
            set;
        }
        HorizontalEditorItem SelectedItem
        {
            get;
            set;
        }
        IEnumerable<ColumnDefinition> ViewColumns
        {
            get;
        }
        string GroupBy
        {
            get;
            set;
        }
        bool IsCollectionGroupingEnabled
        {
            get;
            set;
        }
        bool ExpandAllGroups
        {
            get;
            set;
        }
        void SetDisplayObjects(IEnumerable<object> displayObjects);
        void AddObjectToItems(object newItem);
    }
}
