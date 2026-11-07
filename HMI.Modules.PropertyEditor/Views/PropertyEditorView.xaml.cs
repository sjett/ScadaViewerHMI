// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;
using HMI.Domain.Models;
using HMI.Infrastructure.ViewModels;
//using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Threading;

namespace HMI.Modules.PropertyEditor.Views
{
    /// <summary>
    /// PropertyEditorView.xaml 的交互逻辑
    /// </summary>
    public partial class PropertyEditorView : UserControl
    {
        private IDisposable? defferedPropertyUpdates;
        private Dictionary<object, bool>? expandedGroups;

        private IEnumerable<IGroup> Groups
        {
            get
            {
                return this.uxPropertyTree.Items.Groups.Cast<IGroup>();
            }
        }

        public PropertyEditorView()
        {
            InitializeComponent();
            DataContextChanged += new DependencyPropertyChangedEventHandler(PropertyEditorView_DataContextChanged);
            this.uxPropertyTree.KeyboardCommandProvider = new PropertyEditorKeyboardCommandProvider(this.uxPropertyTree);

        }

        private void PropertyEditorView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //分组状态，控件本身自带，这里没什么作用，另外Rx.Net 不能用于telerik Grid DeferRefresh，无法同步
            StopHandlingPropertyUpdates(e.OldValue as IPropertyEditorViewModel);
            StartHandlingPropertyUpdates(e.NewValue as IPropertyEditorViewModel);
        }
        private void StopHandlingPropertyUpdates(IPropertyEditorViewModel? viewModel)
        {
            if (viewModel != null)
            {
                defferedPropertyUpdates?.Dispose();
            }
        }
        private void StartHandlingPropertyUpdates(IPropertyEditorViewModel? newViewModel)
        {
            if (newViewModel == null)
            {
                return;
            }

            newViewModel.PropertiesUpdating += (s, e) =>
            {
                SaveOpenedCategories();
            };

            newViewModel.PropertiesUpdated += (s, e) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    LoadOpenedCategories();
                }), DispatcherPriority.Background);
            };

            //var source = Observable.FromEventPattern(
            //   addHandler => newViewModel.PropertiesUpdating += addHandler, 
            //   removeHandler => newViewModel.PropertiesUpdating -= removeHandler);

            //var propertiesUpdated = Observable.FromEventPattern(
            //    addHandler => newViewModel.PropertiesUpdated += addHandler,
            //    removeHandler => newViewModel.PropertiesUpdated -= removeHandler);

            //DataItemCollection items = this.uxPropertyTree.Items;

            //var deferredRefresh = Observable.Using(
            //    items.DeferRefresh,
            //    (IDisposable refresh) => 
            //        propertiesUpdated
            //        .Take(1)
            //        .Do(pattern =>
            //        {
            //            LoadOpenedCategories();
            //        }));

            //defferedPropertyUpdates = source
            //    .Do(pattern => SaveOpenedCategories())
            //    .Do(pattern => deferredRefresh.Subscribe())
            //    .Subscribe();
        }
        private void LoadOpenedCategories()
        {
            if (expandedGroups == null)
            {
                return;
            }
            foreach (IGroup current in Groups)
            {
                RestoreExpandedState(current);
            }
        }
        private void RestoreExpandedState(IGroup group)
        {
            bool flag;
            if (expandedGroups!.TryGetValue(group.Key, out flag) && flag)
            {
                this.uxPropertyTree.ExpandGroup(group);
            }
        }
        private void SaveOpenedCategories()
        {
            var generator = this.uxPropertyTree.ItemContainerGenerator;
            expandedGroups = Groups.ToDictionary((group) => group.Key, (group) => IsExpanded(group, generator));
        }
        private static bool IsExpanded(IGroup group, IContainerGenerator generator)
        {
            var gridViewGroupRow = generator.ContainerFromItem(group) as GridViewGroupRow;
            return gridViewGroupRow != null && gridViewGroupRow.IsExpanded;
        }
        private void uxPropertyTree_BeginningEdit(object sender, GridViewBeginningEditRoutedEventArgs e)
        {
            e.Cancel = true;
        }
        private void VisibleProperties_OnFilter(object sender, FilterEventArgs e)
        {
            var property = e.Item as IProperty;
            e.Accepted = property != null && property.IsVisible;
        }

    }
}
