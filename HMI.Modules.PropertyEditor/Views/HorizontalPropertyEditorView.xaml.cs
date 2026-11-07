// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.ViewModels;
using HMI.Modules.PropertyEditor.Views;

namespace HMI.Modules.PropertyEditor.Views
{
    /// <summary>
    /// HorizontalPropertyEditorView.xaml 的交互逻辑
    /// </summary>
    public partial class HorizontalPropertyEditorView : UserControl
    {
        private IHorizontalPropertyEditorViewModel ViewModel
        {
            get
            {
                return (IHorizontalPropertyEditorViewModel)DataContext;
            }
            set
            {
                DataContext = value;
            }
        }

        public HorizontalPropertyEditorView()
        {
            InitializeComponent();
            
            this.uxPropertyGrid.KeyboardCommandProvider = new PropertyEditorKeyboardCommandProvider(this.uxPropertyGrid);
            DataContextChanged += new DependencyPropertyChangedEventHandler(HorizontalPropertyEditorView_DataContextChanged);
            this.uxPropertyGrid.BeginningEdit += new EventHandler<GridViewBeginningEditRoutedEventArgs>(uxPropertyGrid_BeginningEdit);
	
        }

        private void uxPropertyGrid_BeginningEdit(object sender, GridViewBeginningEditRoutedEventArgs e)
        {
            e.Cancel = true;
        }
        private void HorizontalPropertyEditorView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ViewModel = DataContext as IHorizontalPropertyEditorViewModel;
            PopulateGridViewColumns();
            ViewModel.UpdateGridViewColumns += new EventHandler(viewModel_UpdateGridViewColumns);
            ViewModel.GroupByChanged += new EventHandler<ValueChangedEventArgs<string>>(ViewModel_GroupByChanged);
            UpdateGrouping();
            UpdateExpandAllGroups();
        }
        private void UpdateExpandAllGroups()
        {
            this.uxPropertyGrid.AutoExpandGroups = ViewModel.ExpandAllGroups;
        }
        private void ViewModel_GroupByChanged(object sender, ValueChangedEventArgs<string> e)
        {
            UpdateGrouping();
        }
        private void UpdateGrouping()
        {
            this.uxPropertyGrid.GroupDescriptors.Clear();
            if (!ViewModel.IsCollectionGroupingEnabled)
            {
                return;
            }
            GroupDescriptor item = new GroupDescriptor
            {
                Member = "Key"
            };
            this.uxPropertyGrid.GroupDescriptors.Add(item);
        }
        private void viewModel_UpdateGridViewColumns(object sender, EventArgs e)
        {
            PopulateGridViewColumns();
        }
        private void PopulateGridViewColumns()
        {
            foreach (HMI.Infrastructure.ViewModels.ColumnDefinition current in ViewModel.ViewColumns)
            {
                if (this.uxPropertyGrid.Columns[current.Name] == null)
                {
                    PropertyEditorGridViewColumn item = new PropertyEditorGridViewColumn
                    {
                        UniqueName = current.Name,
                        Header = current.Header,
                        Name = current.Name,
                        CellStyle = (Style)FindResource("PropertyCellStyle"),
                        CellTemplate = (DataTemplate)FindResource("PropertyEditorCellTemplate"),
                        PropertyIndex = current.PropertyIndex,
                        MinWidth = 70.0,
                        DataMemberBinding = new Binding("Value." + current.Name)
                    };
                    this.uxPropertyGrid.Columns.Add(item);
                }
            }
        }
    }
}
