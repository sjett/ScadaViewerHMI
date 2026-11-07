// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using HMI.Common.Browsers.Views;
using HMI.Common.Presentation.Views;
using HMI.Domain.Models;

namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage, 
		TemplatePart(Name = "uxBrowse", Type = typeof(RadButton)), 
		TemplatePart(Name = "uxEditor", Type = typeof(TextBox))]
	public abstract class BrowserEditorView : EditorView
	{
		private const string uxBrowseName = "uxBrowse";
		private const string uxEditorName = "uxEditor";
		
		protected TextBox Editor
		{
			get;
			private set;
		}
		protected RadButton BrowseButton
		{
			get;
			private set;
		}
		static BrowserEditorView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(BrowserEditorView), new FrameworkPropertyMetadata(typeof(BrowserEditorView)));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.Editor = this.FindTemplateChild<TextBox>(uxEditorName);
			if (this.Editor != null)
			{
				this.Editor.SetBinding(TextBox.TextProperty, base.ValueBinding);
			}
			this.BrowseButton = this.FindTemplateChild<RadButton>(uxBrowseName);
			if (this.BrowseButton != null && this.Editor != null)
			{
				this.BrowseButton.Click += new RoutedEventHandler(this.onBrowseButtonClick);
			}
		}
		private void onBrowseButtonClick(object sender, RoutedEventArgs e)
		{
			this.openBrowserWindow();
		}
		private void openBrowserWindow()
		{
			using (SelectorBrowserWindow selectorBrowserWindow = this.CreateBrowserWindow())
			{
				IBindingContextAware bindingContextAware = selectorBrowserWindow.DataContext as IBindingContextAware;
				if (bindingContextAware != null)
				{
					bindingContextAware.Context = base.Context;
				}
				selectorBrowserWindow.SelectedValue = this.Editor.Text;
				selectorBrowserWindow.ShowDialog();
				if (selectorBrowserWindow.DialogResult == true)
				{
					this.Editor.Text = selectorBrowserWindow.SelectedValue;
				}
			}
		}
		protected abstract SelectorBrowserWindow CreateBrowserWindow();
	}
}
