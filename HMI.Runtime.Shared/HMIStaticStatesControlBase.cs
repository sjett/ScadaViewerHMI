// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Core;
using HMI.Runtime.Shared.Common;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HMI.Runtime.Shared
{
	[TemplatePart(Name = "LayoutRoot", Type = typeof(Grid))]
	public abstract class HMIStaticStatesControlBase : HMIStatesControlBase
	{
		private IHMIStaticControlBaseViewModel<HMIStateCondition> viewModel;

		public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register("LineThickness", typeof(double), typeof(HMIStaticStatesControlBase), new PropertyMetadata(2.0, new PropertyChangedCallback(OnLineThicknessPropertyChanged)));
		
		public IHMIStaticControlBaseViewModel<HMIStateCondition> ViewModel
		{
			get
			{
				if (this.viewModel == null)
				{
					this.viewModel = (IHMIStaticControlBaseViewModel<HMIStateCondition>)CreateViewModel();
				}
				return this.viewModel;
			}
		}
		[Category("HMI")]
		public double LineThickness
		{
			get
			{
				return (double)GetValue(LineThicknessProperty);
			}
			set
			{
                SetValue(LineThicknessProperty, value);
			}
		}
		protected HMIStateCondition CurrentState
		{
			get
			{
				return ViewModel.CurrentState;
			}
		}
		[Category("HMI")]
		public ObservableCollection<HMIStateCondition> States
		{
			get;
			private set;
		}
		public HMIStaticStatesControlBase()
		{
            layoutRoot = null;
            States = new ObservableCollection<HMIStateCondition>();
            ForceOnApplyTemplate();
		}
		~HMIStaticStatesControlBase()
		{
            Dispose(false);
		}
		private static void OnLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIStaticStatesControlBase? wFStaticStatesControlBase = d as HMIStaticStatesControlBase;
			if (wFStaticStatesControlBase != null && (double)e.NewValue < 0.0)
			{
				wFStaticStatesControlBase.ClearValue(e.Property);
				return;
			}
		}
		private void HMIControl_MouseEnter(object sender, MouseEventArgs e)
		{
			if (string.IsNullOrEmpty(PopupSource))
			{
				return;
			}
            Cursor = Cursors.Hand;
		}
		private void HMIControl_MouseLeave(object sender, MouseEventArgs e)
		{
            Cursor = Cursors.Arrow;
		}
		private void ViewModel_EnableChanged(object sender, EventArgs e)
		{
            SetEnabled();
		}
		private void ViewModel_VisibleChanged(object sender, EventArgs e)
		{
            SetVisibility();
		}
		private void ViewModel_StateChanged(object sender, EventArgs e)
		{
			base.SetStatesProperties(ViewModel.CurrentState);
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
            BackupStateProperties();
			if (ShouldPrepareTemplate(this))
			{
                TrimAndPrepareTemplate();
			}
            InitializeViewModel();
            SetDataContext();
            InitializeControlStates();
			if (IsInDesignMode)
			{
				return;
			}
            RegisterMouseHandlers();
		}
		protected override void SetEnabled()
		{
			if (IsInDesignMode)
			{
				return;
			}
			bool flag = ViewModel.CurrentState != null ? ViewModel.CurrentState.IsEnabled : isInitiallyEnabled.Value;
            IsEnabled = ViewModel.IsEnabled && flag;
			VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled", true);
		}
		protected override void SetVisibility()
		{
			if (IsInDesignMode)
			{
				return;
			}
			bool flag = ViewModel.CurrentState != null ? ViewModel.CurrentState.IsVisible : isInitiallyVisible.Value;
            Visibility = !ViewModel.IsVisible || !flag ? Visibility.Collapsed : Visibility.Visible;
		}
		protected override bool HasViewModel()
		{
			return viewModel != null;
		}
		protected override object CreateDesignTimeViewModel()
		{
			return new HMIStatesControlBaseDesignViewModel<HMIStateCondition>();
		}
		protected override object CreateRuntimeViewModel(IHMIConnector connector, ISecurityChecker securityChecker)
		{
			return new HMIStaticViewModelBase<HMIStateCondition>(connector, States, SecurityChecker, SecurityDenyAccessBehaviour);
		}
		protected override void InitializeViewModel()
		{
            ViewModel.Initialize();
            UpdateControlProperties();
		}
		protected override void RegisterEvents()
		{
            ViewModel.EnableChanged += new EventHandler(ViewModel_EnableChanged);
            ViewModel.VisibleChanged += new EventHandler(ViewModel_VisibleChanged);
            ViewModel.StateChanged += new EventHandler(ViewModel_StateChanged);
		}
		protected override void UnregisterEvents()
		{
            ViewModel.EnableChanged -= new EventHandler(ViewModel_EnableChanged);
            ViewModel.VisibleChanged -= new EventHandler(ViewModel_VisibleChanged);
            ViewModel.StateChanged -= new EventHandler(ViewModel_StateChanged);
		}
		protected override void SetIsEnableAndIsVissible()
		{
            SetEnabled();
            SetVisibility();
		}
		protected override void SetDataContext()
		{
            DataContext = ViewModel;
		}
		protected override void ApplyStateAlternateProperties()
		{
            Background = CurrentState.AlternateBackground;
            Foreground = CurrentState.AlternateForeground;
		}
		protected override void ApplyStateProperties()
		{
            Background = CurrentState.Background;
            Foreground = CurrentState.Foreground;
		}
		protected override void DoDispose()
		{
            DisposeModel();
            DisposeMouseHandlers();
			this.StopStoryboards();
		}
		protected override void DisposeModel()
		{
			if (viewModel == null)
			{
				return;
			}
            UnregisterEvents();
            viewModel.Dispose();
		}
		private void DisposeMouseHandlers()
		{
            MouseEnter -= new MouseEventHandler(HMIControl_MouseEnter);
            MouseLeave -= new MouseEventHandler(HMIControl_MouseLeave);
		}
		protected override HMIStaticStateCondition GetCurrentState()
		{
			return CurrentState;
		}
		protected override IEnumerable<HMIStateConditionBase> CastStatesToBase()
		{
			return States.Cast<HMIStateConditionBase>();
		}
		private void RegisterMouseHandlers()
		{
            MouseEnter += new MouseEventHandler(HMIControl_MouseEnter);
            MouseLeave += new MouseEventHandler(HMIControl_MouseLeave);
		}
	}
}
