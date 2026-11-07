// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.Views;
using HMI.Common.Presentation.Views;
using HMI.Domain.Models;
using HMI.Modules.Design.ViewModels.Browsers;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Telerik.Windows.Controls;

namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage, TemplatePart(Name = "uxBrowse", Type = typeof(RadButton))]
	public abstract class CollectionEditorView : EditorView
	{
		private const string BrowseButtonTemplatePartName = "uxBrowse";
		public static readonly DependencyProperty IsCollectionReadOnlyProperty;
		public static readonly DependencyProperty DefaultItemTypeProperty;
		public static readonly DependencyProperty IsGroupingEnabledProperty;
		public static readonly DependencyProperty ExpandAllGroupsProperty;
		public static readonly DependencyProperty GroupByProperty;
		public static readonly DependencyProperty GroupSortDirectionProperty;
		private RadButton browseButton;
		public bool IsCollectionReadOnly
		{
			get
			{
				return (bool)base.GetValue(CollectionEditorView.IsCollectionReadOnlyProperty);
			}
			set
			{
				base.SetValue(CollectionEditorView.IsCollectionReadOnlyProperty, value);
			}
		}
		public Type DefaultItemType
		{
			get
			{
				return (Type)base.GetValue(CollectionEditorView.DefaultItemTypeProperty);
			}
			set
			{
				base.SetValue(CollectionEditorView.DefaultItemTypeProperty, value);
			}
		}
		public bool IsGroupingEnabled
		{
			get
			{
				return (bool)base.GetValue(CollectionEditorView.IsGroupingEnabledProperty);
			}
			set
			{
				base.SetValue(CollectionEditorView.IsGroupingEnabledProperty, value);
			}
		}
		public bool ExpandAllGroups
		{
			get
			{
				return (bool)base.GetValue(CollectionEditorView.ExpandAllGroupsProperty);
			}
			set
			{
				base.SetValue(CollectionEditorView.ExpandAllGroupsProperty, value);
			}
		}
		public string GroupBy
		{
			get
			{
				return (string)base.GetValue(CollectionEditorView.GroupByProperty);
			}
			set
			{
				base.SetValue(CollectionEditorView.GroupByProperty, value);
			}
		}
		public ListSortDirection? GroupSortDirection
		{
			get
			{
				return (ListSortDirection?)base.GetValue(CollectionEditorView.GroupSortDirectionProperty);
			}
			set
			{
				base.SetValue(CollectionEditorView.GroupSortDirectionProperty, value);
			}
		}
		public Type[] AvailableTypes
		{
			get;
			set;
		}
		public IBindingSource BindingSource
		{
			get;
			set;
		}
		static CollectionEditorView()
		{
			CollectionEditorView.IsCollectionReadOnlyProperty = DependencyProperty.Register("IsCollectionReadOnly", typeof(bool), typeof(CollectionEditorView), new PropertyMetadata(false));
			CollectionEditorView.DefaultItemTypeProperty = DependencyProperty.Register("DefaultItemType", typeof(Type), typeof(CollectionEditorView), null);
			CollectionEditorView.IsGroupingEnabledProperty = DependencyProperty.Register("IsGroupingEnabled", typeof(bool), typeof(CollectionEditorView), null);
			CollectionEditorView.ExpandAllGroupsProperty = DependencyProperty.Register("ExpandAllGroups", typeof(bool), typeof(CollectionEditorView), new PropertyMetadata(true));
			CollectionEditorView.GroupByProperty = DependencyProperty.Register("GroupBy", typeof(string), typeof(CollectionEditorView), null);
			CollectionEditorView.GroupSortDirectionProperty = DependencyProperty.Register("GroupSortDirection", typeof(ListSortDirection?), typeof(CollectionEditorView), null);
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CollectionEditorView), new FrameworkPropertyMetadata(typeof(CollectionEditorView)));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.browseButton = this.FindTemplateChild<RadButton>(BrowseButtonTemplatePartName);
			this.browseButton.Click += new RoutedEventHandler(this.OnBrowseButtonClick);
		}
		private void OnBrowseButtonClick(object sender, RoutedEventArgs e)
		{
			ICollectionBrowserViewModel browserViewModel = CreateCollectionBrowserViewModel();
			InitializeCollectionBrowserViewModel(browserViewModel);
			ShowCollectionBrowser(browserViewModel);
		}
		private void ShowCollectionBrowser(ICollectionBrowserViewModel browserViewModel)
		{
			using (BrowserWindow browserWindow = this.CreateCollectionBrowser(browserViewModel))
			{
				browserWindow.ShowDialog();
				if (browserWindow.DialogResult == true)
				{
					browserViewModel.ApplyChanges();
				}
				else
				{
					browserViewModel.CancelChanges();
				}
			}
		}
		private void InitializeCollectionBrowserViewModel(ICollectionBrowserViewModel collectionBrowserViewModel)
		{
			collectionBrowserViewModel.IsCollectionReadOnly = this.IsCollectionReadOnly;
			collectionBrowserViewModel.DefaultItemType = this.DefaultItemType;
			collectionBrowserViewModel.IsGroupingEnabled = this.IsGroupingEnabled;
			collectionBrowserViewModel.ExpandAllGroups = this.ExpandAllGroups;
			collectionBrowserViewModel.GroupBy = this.GroupBy;
			if (this.AvailableTypes != null)
			{
				collectionBrowserViewModel.AvailableItemTypes = this.AvailableTypes.AsEnumerable<Type>().ToArray<Type>();
			}
			else
			{
				collectionBrowserViewModel.AvailableItemTypes = new Type[0];
			}
			collectionBrowserViewModel.Initialize(this.BindingSource);
		}
		protected abstract ICollectionBrowserViewModel CreateCollectionBrowserViewModel();
		protected abstract BrowserWindow CreateCollectionBrowser(ICollectionBrowserViewModel viewModel);
	}
}
