// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Modules.Design.ViewModels;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage]
	public class ThicknessEditorView : EditorView, IDisposable
	{
		public static readonly DependencyProperty ThicknessValueProperty;
		private bool isDisposed;
		internal IThicknessViewModel ViewModel
		{
			get
			{
				return base.DataContext as IThicknessViewModel;
			}
		}
		public Thickness ThicknessValue
		{
			get
			{
				return (Thickness)base.GetValue(ThicknessEditorView.ThicknessValueProperty);
			}
			set
			{
				base.SetValue(ThicknessEditorView.ThicknessValueProperty, value);
			}
		}
		static ThicknessEditorView()
		{
			ThicknessEditorView.ThicknessValueProperty = DependencyProperty.Register("ThicknessValue", typeof(Thickness), typeof(ThicknessEditorView), new PropertyMetadata(default(Thickness), new PropertyChangedCallback(OnThicknessValueChanged)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ThicknessEditorView), new FrameworkPropertyMetadata(typeof(ThicknessEditorView)));
		}
        static void OnThicknessValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
        {
            ThicknessEditorView editor = dependencyObject as ThicknessEditorView;
            if (editor == null)
            {
                return;
            }
            if (!(change.NewValue is Thickness))
            {
                return;
            }
            Thickness thickness = (Thickness)change.NewValue;
            editor.ViewModel.Thickness = thickness;
        }
        public ThicknessEditorView()
		{
			base.DataContext = new ThicknessViewModel();
			SubscribeToViewModelChanges();
		}
        void SubscribeToViewModelChanges()
        {
            if (ViewModel == null)
            {
                return;
            }
            ViewModel.ThicknessChanged += new EventHandler<ValueChangedEventArgs<Thickness>>(OnViewModelThicknessChanged);
        }
        public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			base.SetBinding(ThicknessEditorView.ThicknessValueProperty, base.ValueBinding);
		}
		void OnViewModelThicknessChanged(object sender, ValueChangedEventArgs<Thickness> e)
		{
			base.SetValue(ThicknessEditorView.ThicknessValueProperty, e.NewValue);
		}
		protected virtual void DisposeManagedResources()
		{
			UnsubscribeFromViewModelChanges();
		}
        void UnsubscribeFromViewModelChanges()
        {
            if (ViewModel == null)
            {
                return;
            }
            ViewModel.ThicknessChanged -= new EventHandler<ValueChangedEventArgs<Thickness>>(OnViewModelThicknessChanged);
        }
        protected virtual void DisposeUnmanagedResources()
		{
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!this.isDisposed)
			{
				if (disposing)
				{
					this.DisposeManagedResources();
				}
				this.DisposeUnmanagedResources();
			}
			this.isDisposed = true;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		~ThicknessEditorView()
		{
			this.Dispose(false);
		}
	}
}
