// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Presentation.ViewModels;
using NLog;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Persistence;
namespace HMI.Common.Browsers.Views
{
    public abstract class BrowserView<TViewModel> : Control where TViewModel : class, ISelectorBrowserPageViewModel, IViewModel
    {
        private static readonly Logger logger = LogManager.GetLogger("BrowserView");
        private TViewModel oldViewModel;
        internal TViewModel ViewModel
        {
            get
            {
                return base.DataContext as TViewModel;
            }
            set
            {
                base.DataContext = value;
            }
        }
        public BrowserView()
        {
            base.DataContextChanged += new DependencyPropertyChangedEventHandler(this.TemplatedBrowserView_DataContextChanged);
        }
        private void TemplatedBrowserView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.oldViewModel != null)
            {
                this.UnregisterEvents(this.oldViewModel);
            }
            if (this.ViewModel != null)
            {
                this.RegisterEvents(this.ViewModel);
                this.oldViewModel = this.ViewModel;
            }
        }
        protected virtual void RegisterEvents(TViewModel viewModel)
        {
            viewModel.Error += new EventHandler<ExceptionEventArgs>(this.ViewModel_Error);
            viewModel.Closed += new EventHandler<DialogClosedEventArgs>(this.ViewModel_Closed);
        }
        protected virtual void UnregisterEvents(TViewModel viewModel)
        {
            viewModel.Error -= new EventHandler<ExceptionEventArgs>(this.ViewModel_Error);
            viewModel.Closed -= new EventHandler<DialogClosedEventArgs>(this.ViewModel_Closed);
        }
        protected virtual void ViewModel_Closed(object sender, DialogClosedEventArgs e)
        {
            this.UnregisterEvents(this.ViewModel);
        }
        private void ViewModel_Error(object sender, ExceptionEventArgs e)
        {
        }
        protected void HookGridActionHandlers(RadGridView itemsGrid)
        {
            itemsGrid.RowLoaded += new EventHandler<RowLoadedEventArgs>(this.itemsGrid_RowLoaded);
            itemsGrid.RowUnloaded += new EventHandler<RowUnloadedEventArgs>(this.itemsGrid_RowUnloaded);
            itemsGrid.PreviewKeyDown += new KeyEventHandler(this.itemsGrid_PreviewKeyDown);
        }
        protected void InitializeGridSettings(RadGridView itemsGrid, string settingsName)
        {
            itemsGrid.Filtered += delegate(object sender, GridViewFilteredEventArgs e)
            {
                this.SaveGridSettings(itemsGrid, settingsName);
            };
            itemsGrid.Sorted += delegate(object sender, GridViewSortedEventArgs e)
            {
                this.SaveGridSettings(itemsGrid, settingsName);
            };
            itemsGrid.Grouped += delegate(object sender, GridViewGroupedEventArgs e)
            {
                this.SaveGridSettings(itemsGrid, settingsName);
            };
            itemsGrid.ColumnReordered += delegate(object sender, GridViewColumnEventArgs e)
            {
                this.SaveGridSettings(itemsGrid, settingsName);
            };
            this.LoadCachedGridSettings(itemsGrid, settingsName);
        }
        private void LoadCachedGridSettings(RadGridView itemsGrid, string settingsName)
        {
            TViewModel viewModel = this.ViewModel;
            string cachedValue = viewModel.GetCachedValue<string>(settingsName, null);
            if (string.IsNullOrEmpty(cachedValue))
            {
                return;
            }
            try
            {
                this.LoadGridSettings(itemsGrid, cachedValue);
            }
            catch (Exception exception)
            {
                BrowserView<TViewModel>.logger.ErrorException("Could not load grid settings", exception);
            }
        }
        private void LoadGridSettings(RadGridView itemsGrid, string gridSettings)
        {
            byte[] bytes = Encoding.Default.GetBytes(gridSettings);
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                this.LoadGridSettings(itemsGrid, memoryStream);
            }
        }
        private void LoadGridSettings(RadGridView itemsGrid, Stream settingsStream)
        {
            PersistenceManager persistenceManager = new PersistenceManager();
            persistenceManager.Load(itemsGrid, settingsStream);
        }
        private void SaveGridSettings(RadGridView itemsGrid, string settingsName)
        {
            try
            {
                PersistenceManager persistenceManager = new PersistenceManager();
                using (Stream stream = persistenceManager.Save(itemsGrid))
                {
                    this.SaveGridSettings(stream, settingsName);
                }
            }
            catch (Exception exception)
            {
                BrowserView<TViewModel>.logger.ErrorException("Could not save grid settings", exception);
            }
        }
        private void SaveGridSettings(Stream settingsStream, string settingsName)
        {
            using (StreamReader streamReader = new StreamReader(settingsStream))
            {
                string value = streamReader.ReadToEnd();
                TViewModel viewModel = this.ViewModel;
                viewModel.SetCachedValue<string>(settingsName, value);
            }
        }
        private void itemsGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                e.Handled = true;
                this.ExecuteOkCommand();
            }
        }
        protected void ExecuteOkCommand()
        {
            TViewModel viewModel = this.ViewModel;
            viewModel.RaiseClosedEvent(true);
        }
        protected void HookDoubleClickRowHandlers(RadTreeView itemsTree)
        {
            itemsTree.ItemDoubleClick += new EventHandler<RadRoutedEventArgs>(this.itemsTree_ItemDoubleClick);
        }
        private void itemsTree_ItemDoubleClick(object sender, RadRoutedEventArgs e)
        {
            this.ExecuteOkCommand();
        }
        private void itemsGrid_RowUnloaded(object sender, RowUnloadedEventArgs e)
        {
            e.Row.PreviewMouseDoubleClick -= new MouseButtonEventHandler(this.Row_PreviewMouseDoubleClick);
        }
        private void itemsGrid_RowLoaded(object sender, RowLoadedEventArgs e)
        {
            e.Row.PreviewMouseDoubleClick += new MouseButtonEventHandler(this.Row_PreviewMouseDoubleClick);
        }
        private void Row_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.ExecuteOkCommand();
        }
    }
}
