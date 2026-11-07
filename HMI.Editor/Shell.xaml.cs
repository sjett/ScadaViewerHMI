// SPDX-License-Identifier: GPL-3.0-only
using NLog;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml.Linq;
using Telerik.Windows.Controls;

using HMI.Common.Instrumentation.Events;
using HMI.Common.Presentation.Events.Interactions;
using HMI.Common.Xml;

using HMI.Editor.Common;

using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using HMI.Infrastructure.Services.Logging;

using HMI.Modules.Application.Events;

using HMI.Editor.Resources;
using HMI.Editor.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Editor
{
    /// <summary>
    /// Shell.xaml 的交互逻辑
    /// </summary>
    public partial class Shell : RadRibbonWindow
    {
        private IShellViewModel viewModel;
        private static readonly Logger logger = LogManager.GetLogger("Shell");
        private bool? isRibbonBarMinimized;
        private INotificationService notificationService;
        private bool disablePageSelection;
        private bool ignoreActivePageChanging;

        public bool IsBusy
        {
            get
            {
                return this.uxBusyIndicator.IsBusy;
            }
        }
        private bool CanHandlePaneVisibilityChange
        {
            get
            {
                return !base.IsLoaded || !base.IsActive;
            }
        }

        public Shell(
            IShellViewModel viewModel, 
            ILoggerFactory loggerFactory,
            INotificationService notificationService,
            IEventAggregator eventAggregator, 
            ILanguageService languageService)
        {
            this.viewModel = viewModel;
            viewModel.CommandSink.ProjectChanged += new EventHandler<ValueChangedEventArgs<IProjectViewModel>>(this.viewModel_ProjectChanged);
            viewModel.CommandSink.ActivePageChanged += new EventHandler<ValueChangedEventArgs<IProjectPageViewModel>>(this.CommandSink_ActivePageChanged);
            languageService.SetApplicationLanguage();
            this.notificationService = notificationService;
            this.RegisterToApplicationExitEvent(eventAggregator);
            this.InitializeComponent();
            this.AttachLoggers(loggerFactory);
            if (!this.LoadPreferences())
            {
                base.Left = 0.0;
                base.Top = 0.0;
                base.Width = SystemParameters.FullPrimaryScreenWidth;
                base.Height = SystemParameters.FullPrimaryScreenHeight + SystemParameters.CaptionHeight;
            }
            base.DataContext = viewModel;
            base.Loaded += new RoutedEventHandler(this.WindowMain_Loaded);
            base.Closing += new CancelEventHandler(this.WindowMain_Closing);
            this.SubscribeToUIEvents(eventAggregator);
        }
        private void SubscribeToUIEvents(IEventAggregator eventAggregator)
        {
            eventAggregator.SubscribeAndHandleOpenFileInteractionEvent();
        }
        public void ShowBusyIndicator(string message)
        {
            if (!this.isRibbonBarMinimized.HasValue)
            {
                this.isRibbonBarMinimized = new bool?(this.uxRibbonBar.IsMinimized);
            }
            this.uxBusyIndicator.BusyContent=(message);
            this.uxBusyIndicator.IsBusy=(true);
            this.uxRibbonBar.IsEnabled = false;
            this.uxRibbonBar.IsMinimized = false;
        }
        public void HideBusyIndicator()
        {
            this.uxRibbonBar.IsEnabled = true;
            this.uxBusyIndicator.IsBusy=(false);
            if (this.isRibbonBarMinimized.HasValue)
            {
                this.uxRibbonBar.IsMinimized = this.isRibbonBarMinimized.Value;
            }
            this.isRibbonBarMinimized = null;
        }
        private void RegisterToApplicationExitEvent(IEventAggregator eventAggregator)
        {
            ApplicationExitEvent @event = eventAggregator.GetEvent<ApplicationExitEvent>();
            if (@event != null)
            {
                @event.Subscribe(new System.Action<object>(this.OnApplicationExit), 0);
            }
        }
        private void OnApplicationExit(object obj)
        {
            try
            {
                base.Closing -= new CancelEventHandler(this.WindowMain_Closing);
                this.SavePreferences();
                Application.Current.Shutdown();
            }
            catch (System.Exception ex)
            {
                Shell.logger.ErrorException(ex.Message, ex);
            }
        }
        private void CommandSink_ActivePageChanged(object sender, ValueChangedEventArgs<IProjectPageViewModel> e)
        {
            if (this.ignoreActivePageChanging)
            {
                return;
            }
            this.ignoreActivePageChanging = true;
            RadPane selectedItem = this.uxDocumentPaneGroup.EnumeratePanes().SingleOrDefault((RadPane p) => this.HasPageAsSource(p, e.NewValue));
            this.uxDocumentPaneGroup.SelectedItem = selectedItem;
            this.ignoreActivePageChanging = false;
        }
        private void viewModel_ProjectChanged(object sender, ValueChangedEventArgs<IProjectViewModel> e)
        {
            if (e.OldValue != null)
            {
                this.ClearDocuments();
                e.OldValue.Pages.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.project_PagesCollectionChanged);
            }
            if (e.NewValue != null)
            {
                e.NewValue.Pages.CollectionChanged += new NotifyCollectionChangedEventHandler(this.project_PagesCollectionChanged);
                this.disablePageSelection = true;
                this.AddDocuments(e.NewValue.Children);
                this.disablePageSelection = false;
                this.viewModel.CommandSink.SelectedItem=(e.NewValue.Children.FirstOrDefault<IExplorerNodeViewModel>());
            }
        }
        private void project_PagesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    this.AddDocuments(e.NewItems.Cast<IProjectPageViewModel>());
                    return;
                case NotifyCollectionChangedAction.Remove:
                    this.RemoveDocuments(e.OldItems.Cast<IProjectPageViewModel>());
                    return;
                case NotifyCollectionChangedAction.Reset:
                    this.ClearDocuments();
                    this.AddDocuments(e.NewItems.Cast<IProjectPageViewModel>());
                    return;
            }
            throw new System.NotSupportedException();
        }
        private void RemoveDocuments(System.Collections.Generic.IEnumerable<IProjectPageViewModel> pages)
        {
            foreach (IProjectPageViewModel current in pages)
            {
                this.RemovePage(current);
            }
        }
        private void RemovePage(IProjectPageViewModel page)
        {
            System.Collections.Generic.List<RadPane> list = (
                from p in this.uxDocumentPaneGroup.EnumeratePanes()
                where this.HasPageAsSource(p, page)
                select p).ToList<RadPane>();
            foreach (RadPane current in list)
            {
                this.uxDocumentPaneGroup.RemovePane(current);
            }
        }
        private bool HasPageAsSource(RadPane pane, IProjectPageViewModel page)
        {
            FrameworkElement frameworkElement = pane.Content as FrameworkElement;
            return frameworkElement != null && frameworkElement.DataContext == page;
        }
        private void AddDocuments(System.Collections.Generic.IEnumerable<IExplorerNodeViewModel> nodes)
        {
            IOrderedEnumerable<IExplorerNodeViewModel> orderedEnumerable = nodes.OrderBy(delegate(IExplorerNodeViewModel x)
            {
                if (x is ITemplatePageViewModel)
                {
                    return 2;
                }
                if (!(x is IProjectPageViewModel))
                {
                    return 0;
                }
                return 1;
            });
            foreach (IExplorerNodeViewModel current in orderedEnumerable)
            {
                this.AddDocument(current);
            }
        }
        private void AddDocument(IExplorerNodeViewModel node)
        {
            this.CreateDocumentForPage(node as IProjectPageViewModel);
            this.AddDocuments(node.Children);
        }
        private void CreateDocumentForPage(IProjectPageViewModel page)
        {
            if (page == null)
            {
                return;
            }
            DesignerDocument designerDocument = this.CreateDocument(page);
            RadDocumentPane radDocumentPane = this.CreateDocumentPane(page, designerDocument);
            this.uxDocumentPaneGroup.AddItem(radDocumentPane, Telerik.Windows.Controls.Docking.DockPosition.Center);
            designerDocument.Focus();
        }
        private RadDocumentPane CreateDocumentPane(IProjectPageViewModel page, DesignerDocument document)
        {
            RadDocumentPane radDocumentPane = new RadDocumentPane();
            radDocumentPane.CanUserClose=(false);
            radDocumentPane.DataContext = page;
            radDocumentPane.SetBinding(HeaderedContentControl.HeaderProperty, "ID");
            radDocumentPane.SetBinding(FrameworkElement.ToolTipProperty, "ID");
            radDocumentPane.Content = document;
            radDocumentPane.ContextMenuTemplate=(null);
            return radDocumentPane;
        }
        private DesignerDocument CreateDocument(IProjectPageViewModel page)
        {
            return new DesignerDocument
            {
                DataContext = page
            };
        }
        private void ClearDocuments()
        {
            RadPane[] panes = this.uxDocumentPaneGroup.EnumeratePanes().ToArray<RadPane>();
            for (int i = 0; i < panes.Length; i++)
            {
                RadPane radPane = panes[i];
                this.uxDocumentPaneGroup.RemovePane(radPane);
            }
        }
        private void AttachLoggers(ILoggerFactory loggerFactory)
        {
            TextBoxLogger item = new TextBoxLogger(this.uxLog);
            //loggerFactory.BuildLoggers.Add(item);
            loggerFactory.RuntimeLoggers.Add(item);
        }
        private void SavePreferences()
        {
            double top;
            double left;
            double width;
            double height;
            if (base.WindowState != WindowState.Normal)
            {
                top = base.RestoreBounds.Top;
                left = base.RestoreBounds.Left;
                width = base.RestoreBounds.Width;
                height = base.RestoreBounds.Height;
            }
            else
            {
                top = base.Top;
                left = base.Left;
                width = base.Width;
                height = base.Height;
            }
            Preferences.SetPreferences(this, new JObject
			{
				new JProperty("Top", top.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)),
				new JProperty("Left", left.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)),
				new JProperty("Width", width.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)),
				new JProperty("Height", height.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)),
				new JProperty("WindowState", (base.WindowState == WindowState.Minimized) ? WindowState.Normal : base.WindowState)
			});
        }
        private bool LoadPreferences()
        {
            var preferences = Preferences.GetPreferences(this);
            if (preferences != null)
            {
                bool result;
                try
                {
                    base.Left = double.Parse(((string?)preferences["Left"]), System.Globalization.CultureInfo.InvariantCulture);
                    base.Top = double.Parse(((string?)preferences["Top"]), System.Globalization.CultureInfo.InvariantCulture);
                    base.Width = double.Parse(((string?)preferences["Width"]), System.Globalization.CultureInfo.InvariantCulture);
                    base.Height = double.Parse(((string?)preferences["Height"]), System.Globalization.CultureInfo.InvariantCulture);
                    if (((string?)preferences["WindowState"]) != null)
                    {
                        WindowState windowState = (WindowState)System.Enum.Parse(typeof(WindowState), ((string)preferences["WindowState"]));
                        base.WindowState = windowState;
                    }
                    result = true;
                }
                catch (System.Exception ex)
                {
                    this.notificationService.ShowWarning(ErrorTexts.ErrorLoadingApplicationPreferences, ex);
                    return false;
                }
                return result;
            }
            return false;
        }
        private void WindowMain_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            ApplicationCommands.Close.Execute(null, this);
        }
        private void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            this.AttachToSelectionChangedEvent();
            this.SetDockPanelVisibilityBindings();
        }
        private void AttachToSelectionChangedEvent()
        {
            this.uxDocumentPaneGroup.SelectionChanged += (new RadSelectionChangedEventHandler(this.uxDocumentPaneGroup_SelectionChanged));
        
        }
        private void SetDockPanelVisibilityBindings()
        {
            this.uxToolboxPane.SetBinding(RadPane.IsHiddenProperty, this.CreateTwoWayInvertedBinding("IsControlToolboxVisible"));
            this.uxPropertyInspectorPane.SetBinding(RadPane.IsHiddenProperty, this.CreateTwoWayInvertedBinding("IsPropertyEditorVisible"));
            this.uxExplorerPane.SetBinding(RadPane.IsHiddenProperty, this.CreateTwoWayInvertedBinding("IsProjectExplorerVisible"));
            this.uxLogPane.SetBinding(RadPane.IsHiddenProperty, this.CreateTwoWayInvertedBinding("IsLogVisible"));
            this.uxProjectPane.SetBinding(RadPane.IsHiddenProperty, this.CreateTwoWayInvertedBinding("IsProjectEnginingVisible"));
            this.uxAcionEffectPane.SetBinding(RadPane.IsHiddenProperty, this.CreateTwoWayInvertedBinding("IsActionEffectEditorVisible"));
        }
        private Binding CreateTwoWayInvertedBinding(string path)
        {
            return new Binding(path)
            {
                Source = this.viewModel,
                Mode = BindingMode.TwoWay,
                Converter = new InvertedBooleanConverter()
            };
        }
        private void uxDocumentPaneGroup_SelectionChanged(object sender, RoutedEventArgs e)
        {
            this.UpdateActiveDocument();
        }
        private void UpdateActiveDocument()
        {
            if (this.ignoreActivePageChanging)
            {
                return;
            }
            this.ignoreActivePageChanging = true;
            RadPane selectedPane = this.uxDocumentPaneGroup.SelectedPane;
            if (selectedPane == null)
            {
                this.viewModel.CommandSink.SelectedItem=(null);
            }
            else
            {
                IProjectPageViewModel projectPageViewModel = selectedPane.DataContext as IProjectPageViewModel;
                if (!this.disablePageSelection && this.viewModel.CommandSink.ActivePage != projectPageViewModel)
                {
                    this.viewModel.CommandSink.SelectedItem = (projectPageViewModel);
                }
            }
            this.ignoreActivePageChanging = false;
        }

    }
}
