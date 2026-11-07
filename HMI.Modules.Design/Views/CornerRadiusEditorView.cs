// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Modules.Design.ViewModels;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage]
	public class CornerRadiusEditorView : EditorView, IDisposable
	{
		public static readonly DependencyProperty CornerRadiusValueProperty;
		private bool isDisposed;
		internal ICornerRadiusViewModel ViewModel
		{
			get
			{
				return base.DataContext as ICornerRadiusViewModel;
			}
		}
		public CornerRadius CornerRadiusValue
		{
			get
			{
				return (CornerRadius)base.GetValue(CornerRadiusEditorView.CornerRadiusValueProperty);
			}
			set
			{
				base.SetValue(CornerRadiusEditorView.CornerRadiusValueProperty, value);
			}
		}
		static CornerRadiusEditorView()
		{
			CornerRadiusEditorView.CornerRadiusValueProperty = DependencyProperty.Register("CornerRadiusValue", typeof(CornerRadius), typeof(CornerRadiusEditorView), new PropertyMetadata(default(CornerRadius), new PropertyChangedCallback(OnCornerRadiusValueChanged)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CornerRadiusEditorView), new FrameworkPropertyMetadata(typeof(CornerRadiusEditorView)));
		}
        static void OnCornerRadiusValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
        {
            CornerRadiusEditorView editor = dependencyObject as CornerRadiusEditorView;
            if (editor == null)
            {
                return;
            }
            if (!(change.NewValue is CornerRadius))
            {
                return;
            }
            CornerRadius cornerRadius = (CornerRadius)change.NewValue;
            editor.ViewModel.CornerRadius = cornerRadius;
        }
        public CornerRadiusEditorView()
		{
			base.DataContext = new CornerRadiusViewModel();
			SubscribeToViewModelChanges();
		}
        void SubscribeToViewModelChanges()
        {
            if (ViewModel == null)
            {
                return;
            }
            ViewModel.CornerRadiusChanged += new EventHandler<ValueChangedEventArgs<CornerRadius>>(OnViewModelCornerRadiusChanged);
        }
        public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			base.SetBinding(CornerRadiusEditorView.CornerRadiusValueProperty, base.ValueBinding);
		}
		void OnViewModelCornerRadiusChanged(object sender, ValueChangedEventArgs<CornerRadius> e)
		{
			base.SetValue(CornerRadiusEditorView.CornerRadiusValueProperty, e.NewValue);
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
            ViewModel.CornerRadiusChanged -= new EventHandler<ValueChangedEventArgs<CornerRadius>>(OnViewModelCornerRadiusChanged);
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
		~CornerRadiusEditorView()
		{
			this.Dispose(false);
		}
	}
}
