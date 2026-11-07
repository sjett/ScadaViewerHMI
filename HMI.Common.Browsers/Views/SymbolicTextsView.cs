// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;
using HMI.Common.Browsers.ViewModels;
namespace HMI.Common.Browsers.Views
{
    [ExcludeFromCodeCoverage, TemplatePart(Name = "uxSymbolicTexts", Type = typeof(RadGridView))]
    public class SymbolicTextsView : BrowserView<IServerSymbolicTextBrowserViewModel>
    {
        private const string SymbolicTextsControlName = "uxSymbolicTexts";
        private RadGridView uxSymbolicTexts;
        static SymbolicTextsView()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(SymbolicTextsView), new FrameworkPropertyMetadata(typeof(SymbolicTextsView)));
        }
        public SymbolicTextsView()
        {
            base.DataContextChanged += new DependencyPropertyChangedEventHandler(this.SymbolicTextsView_DataContextChanged);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.uxSymbolicTexts = (base.Template.FindName("uxSymbolicTexts", this) as RadGridView);
            base.HookGridActionHandlers(this.uxSymbolicTexts);
            this.SetupColumns();
        }
        private void SymbolicTextsView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.SetupColumns();
        }
        protected override void RegisterEvents(IServerSymbolicTextBrowserViewModel viewModel)
        {
            base.RegisterEvents(viewModel);
            viewModel.LanguagesChanged += new EventHandler(this.ViewModel_LanguagesChanged);
        }
        protected override void UnregisterEvents(IServerSymbolicTextBrowserViewModel viewModel)
        {
            base.UnregisterEvents(viewModel);
            viewModel.LanguagesChanged -= new EventHandler(this.ViewModel_LanguagesChanged);
        }
        private void ViewModel_LanguagesChanged(object sender, EventArgs e)
        {
            this.SetupColumns();
        }
        private void SetupColumns()
        {
            if (base.ViewModel != null && this.uxSymbolicTexts != null)
            {
                this.RemoveTranslationColumns();
                foreach (ICultureViewModel current in base.ViewModel.Languages)
                {
                    GridViewDataColumn gridViewDataColumn = new GridViewDataColumn();
                    gridViewDataColumn.IsReadOnly = true;
                    gridViewDataColumn.DataMemberBinding = this.CreateTranslationBinding(current);
                    gridViewDataColumn.Header = current.Name;
                    Binding binding = this.CreateVisibilityBinding(current);
                    gridViewDataColumn.SetBinding(GridViewColumn.IsVisibleProperty, binding);
                    gridViewDataColumn.Width = new GridViewLength(1.0, GridViewLengthUnitType.Auto);
                    this.uxSymbolicTexts.Columns.Add(gridViewDataColumn);
                }
                return;
            }
        }
        private Binding CreateVisibilityBinding(ICultureViewModel language)
        {
            return new Binding
            {
                Source = language,
                Path = new PropertyPath("IsVisible", new object[0])
            };
        }
        private Binding CreateTranslationBinding(ICultureViewModel language)
        {
            return new Binding
            {
                Path = new PropertyPath("Translations[" + language.ID + "]", new object[0])
            };
        }
        private void RemoveTranslationColumns()
        {
            int count = this.uxSymbolicTexts.Columns.Count;
            this.RemoveLastColumns(count);
        }
        private void RemoveLastColumns(int columnCount)
        {
            for (int i = 1; i < columnCount; i++)
            {
                this.uxSymbolicTexts.Columns.RemoveAt(1);
            }
        }
    }
}
