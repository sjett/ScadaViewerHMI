// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using HMI.Runtime.Core;
using HMI.Runtime.Core.Helper.Extensions;
using HMI.Runtime.Shared.Common;
using HMI.Runtime.Shared.Dialogs;

namespace HMI.Runtime.Shared
{
    /// <summary>

    /// </summary>
    [TemplatePart(Name = "LayoutRoot", Type = typeof(Grid))]
	public abstract class HMIStatesControlBase : Control, IDisposable
	{
		protected const string ElementLayoutRoot = "LayoutRoot";
		protected Grid layoutRoot;
		private IHMIConnector connector;
		private ISecurityChecker securityChecker;
		private Brush backgroundBackup;
		private Brush foregroundBackup;
		private readonly DialogManager dialogManager;
		private ObservableCollection<ParameterItem> popupParameters;
		private DispatcherTimer stateTimer;
		private HMIStaticStateCondition oldState;
		private bool showAlternateProperties;
		protected bool? isInitiallyVisible;
		protected bool? isInitiallyEnabled;
		private bool wasOnApplyTemplateHandled;

		public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMIStatesControlBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnObjectIDPropertyChanged)));
		public static readonly DependencyProperty PopupModalityProperty = DependencyProperty.Register("PopupModality", typeof(DialogModality), typeof(HMIStatesControlBase), new PropertyMetadata(DialogModality.Modeless, new PropertyChangedCallback(OnPopupModalityPropertyChanged)));
		public static readonly DependencyProperty PopupSourceProperty = DependencyProperty.Register("PopupSource", typeof(string), typeof(HMIStatesControlBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(PopupSourcePropertyChanged)));
		public static readonly DependencyProperty PopupTitleProperty = DependencyProperty.Register("PopupTitle", typeof(string), typeof(HMIStatesControlBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(PopupTitlePropertyChanged)));
		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMIStatesControlBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnSecurityAuthorizationNamePropertyChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMIStatesControlBase), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(OnSecurityDenyAccessBehaviourPropertyChanged)));
		public static readonly DependencyProperty ShowPopupProperty = DependencyProperty.Register("ShowPopup", typeof(bool), typeof(HMIStatesControlBase), new PropertyMetadata(true, new PropertyChangedCallback(ShowPopupPropertyChanged)));
		public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMIStatesControlBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnSignalPrefixPropertyChanged)));
		
		protected IHMIConnector Connector
		{
			get
			{
				if (connector == null)
				{
                    connector = RuntimeServices.GetRequiredService<IHMIConnector>();
				}
				return connector;
			}
		}
		protected bool IsInDesignMode
		{
			get
			{
				return DesignerPropertiesExt.IsInDesignTool;
			}
		}
		[Category("HMI")]
		public string ObjectID
		{
			get
			{
				return (string)GetValue(ObjectIDProperty);
			}
			set
			{
                SetValue(ObjectIDProperty, value);
			}
		}
		[Category("HMI")]
		public DialogModality PopupModality
		{
			get
			{
				return (DialogModality)GetValue(PopupModalityProperty);
			}
			set
			{
				base.SetValue(PopupModalityProperty, value);
			}
		}
		[Category("HMI")]
		public ObservableCollection<ParameterItem> PopupParameters
		{
			get
			{
				return popupParameters;
			}
		}
		[Category("HMI")]
		public string PopupSource
		{
			get
			{
				return (string)GetValue(PopupSourceProperty);
			}
			set
			{
                SetValue(PopupSourceProperty, value);
			}
		}
		[Category("HMI")]
		public string PopupTitle
		{
			get
			{
				return (string)GetValue(PopupTitleProperty);
			}
			set
			{
                SetValue(PopupTitleProperty, value);
			}
		}
		[Category("HMI")]
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)GetValue(SecurityAuthorizationNameProperty);
			}
			set
			{
                SetValue(SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)GetValue(SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(SecurityDenyAccessBehaviourProperty, value);
			}
		}
		protected ISecurityChecker SecurityChecker
		{
			get
			{
				if (securityChecker == null)
				{
                    securityChecker = new SecurityChecker(Connector, SecurityAuthorizationName);
				}
				return securityChecker;
			}
		}
		[Category("HMI")]
		public bool ShowPopup
		{
			get
			{
				return (bool)GetValue(ShowPopupProperty);
			}
			set
			{
                SetValue(ShowPopupProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalPrefix
		{
			get
			{
				return (string)GetValue(SignalPrefixProperty);
			}
			set
			{
                SetValue(SignalPrefixProperty, value);
			}
		}
		public HMIStatesControlBase()
		{
            popupParameters = new ObservableCollection<ParameterItem>();
            CreateStateTimer();
            dialogManager = new DialogManager(null, null, null);
		}
		private void CreateStateTimer()
		{
			DispatcherTimer dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Interval=TimeSpan.FromSeconds(100.0);
            stateTimer = dispatcherTimer;
            stateTimer.Tick +=new EventHandler(stateTimer_Tick);
		}
		~HMIStatesControlBase()
		{
            Dispose(false);
		}
		private static void OnObjectIDPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIStatesControlBase wFStatesControlBase = d as HMIStatesControlBase;
			if (wFStatesControlBase == null)
			{
				return;
			}
			wFStatesControlBase.HandlePropertyChanged(e.Property);
		}
		private static void OnPopupModalityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void PopupSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void PopupTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSecurityAuthorizationNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSecurityDenyAccessBehaviourPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void ShowPopupPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSignalPrefixPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIStatesControlBase wFStatesControlBase = d as HMIStatesControlBase;
			if (wFStatesControlBase == null)
			{
				return;
			}
			wFStatesControlBase.HandlePropertyChanged(e.Property);
		}
		public override void OnApplyTemplate()
		{
            wasOnApplyTemplateHandled = true;
            TrySetInitialVariables();
            RestoreInitialVisibility();
            GetLayoutRoot();
            BackupStateProperties();
            ResolvePlaceholders();
            InitializeViewModel();
            ApplyTemplate();
            SetDataContext();
		}
		private void ApplyTemplate()
		{
			if (ShouldPrepareTemplate(this))
			{
                TrimSignalAndObjectId(this);
                PrepairTemplate();
			}
		}
		public Grid GetLayoutRoot()
		{
			return layoutRoot = GetTemplateChild("LayoutRoot") as Grid;
		}
		protected virtual void BackupStateProperties()
		{
            backgroundBackup = Background;
            foregroundBackup = Foreground;
		}
		protected virtual bool ShouldPrepareTemplate(HMIStatesControlBase owner)
		{
			return owner.IsInDesignMode || HasViewModel();
		}
		protected virtual void TrimSignalAndObjectId(HMIStatesControlBase owner)
		{
			owner.ObjectID = owner.ObjectID.Trim();
			owner.SignalPrefix = owner.SignalPrefix.Trim();
		}
		protected virtual void PrepairTemplate()
		{
            SetDataContext();
            SetIsEnableAndIsVissible();
            RegisterEvents();
		}
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);
            TryShowPopupDialog();
		}
		private void ResolvePlaceholders()
		{
            ResolveStatesPlaceholders();
            ResolvePopupPlaceholders();
		}
		private void ResolveStatesPlaceholders()
		{
			using (IEnumerator<HMIStateConditionBase> enumerator = CastStatesToBase().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HMIStateConditionBase current = enumerator.Current;
					current.ResolvePlaceholders(this);
				}
			}
		}
		private void ResolvePopupPlaceholders()
		{
            ResolvePopupParametersPlaceholders();
            ResolvePopupSourcePlaceholders();
            ResolvePopupTitlePlaceholders();
		}
		private void ResolvePopupParametersPlaceholders()
		{
			using (IEnumerator<ParameterItem> enumerator = popupParameters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ParameterItem current = enumerator.Current;
					current.ParameterName = StringPlaceholdersResolver.Resolve(current.ParameterName, this);
					current.ParameterValue = StringPlaceholdersResolver.Resolve(current.ParameterValue, this);
				}
			}
		}
		private void ResolvePopupSourcePlaceholders()
		{
            PopupSource = StringPlaceholdersResolver.Resolve(PopupSource, this);
		}
		private void ResolvePopupTitlePlaceholders()
		{
            PopupTitle = StringPlaceholdersResolver.Resolve(PopupTitle, this);
		}
		private void SetPropertiesForNullState()
		{
            StopTimerAnimation();
            RestoreBackupStateProperties();
            UpdateControlProperties();
		}
		private void SetPropertiesForNewState(HMIStaticStateCondition state)
		{
            StopTimerAnimation();
            UpdateControlProperties();
            ApplyStateProperties();
            StartTimerAnimation(state);
		}
		private void StopTimerAnimation()
		{
			if (stateTimer.IsEnabled)
			{
                stateTimer.Stop();
			}
		}
		private void StartTimerAnimation(HMIStaticStateCondition state)
		{
			if (ShouldTimerAnimationStart(state))
			{
                StartTimer(state);
			}
		}
		private bool ShouldTimerAnimationStart(HMIStaticStateCondition state)
		{
			return state.IsBlinking && state.BlinkingFrequency > 0.0;
		}
		private void StartTimer(HMIStaticStateCondition state)
		{
            showAlternateProperties = true;
            stateTimer.Interval=TimeSpan.FromSeconds(state.BlinkingFrequency);
            stateTimer.Start();
		}
		private void TimerTick(HMIStaticStateCondition state)
		{
			if (showAlternateProperties)
			{
                ApplyStateAlternateProperties();
			}
			else
			{
                ApplyStateProperties();
			}
            showAlternateProperties = !showAlternateProperties;
		}
		private void stateTimer_Tick(object sender, EventArgs e)
		{
			HMIStaticStateCondition currentState = GetCurrentState();
			if (currentState != null)
			{
                TimerTick(currentState);
			}
		}
		private bool TryShowPopupDialog()
		{
			if (ShowPopup && !string.IsNullOrEmpty(PopupSource))
			{
				try
				{
                    dialogManager.Show(this, PopupSource, PopupParameters, PopupTitle, PopupModality);
				}
				catch (Exception ex)
				{
                    connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", GetType().Name, ex.Message), new object[0]);
				}
				return false;
			}
			return true;
		}
		protected void ForceOnApplyTemplate()
		{
			if (IsInDesignMode)
			{
				return;
			}
			SynchronizationContext.Current.Post(delegate(object x)
			{
				if (wasOnApplyTemplateHandled)
				{
					return;
				}
                TrySetInitialVariables();
                Visibility = Visibility.Visible;
			}, null);
		}
		protected void TrySetInitialVisibility()
		{
			if (!isInitiallyVisible.HasValue)
			{
                isInitiallyVisible = new bool?(Visibility == Visibility.Visible);
			}
		}
		protected void TrySetInitialEnabled()
		{
			if (!isInitiallyEnabled.HasValue)
			{
                isInitiallyEnabled = new bool?(IsEnabled);
			}
		}
		protected void TrySetInitialVariables()
		{
            TrySetInitialVisibility();
            TrySetInitialEnabled();
		}
		protected void RestoreInitialVisibility()
		{
			if (IsInDesignMode)
			{
				return;
			}
            Visibility = isInitiallyVisible.Value ? Visibility.Visible : Visibility.Collapsed;
		}
		protected virtual void InitializeControlStates()
		{
            UpdateControlProperties();
            SetStatesProperties(GetCurrentState());
		}
		protected void UpdateControlProperties()
		{
            SetEnabled();
            SetVisibility();
		}
		protected object CreateViewModel()
		{
			if (!IsInDesignMode)
			{
				return CreateRuntimeViewModel(Connector, securityChecker);
			}
			return CreateDesignTimeViewModel();
		}
		public virtual void Dispose()
		{
            Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected abstract void InitializeViewModel();
		protected abstract void SetIsEnableAndIsVissible();
		protected abstract void SetDataContext();
		protected abstract void RegisterEvents();
		protected abstract void DoDispose();
		protected abstract void UnregisterEvents();
		protected abstract void ApplyStateAlternateProperties();
		protected abstract void ApplyStateProperties();
		protected abstract object CreateDesignTimeViewModel();
		protected abstract object CreateRuntimeViewModel(IHMIConnector connector, ISecurityChecker securityChecker);
		protected abstract IEnumerable<HMIStateConditionBase> CastStatesToBase();
		protected abstract bool HasViewModel();
		protected virtual void DisposeTimer()
		{
			if (stateTimer != null && stateTimer.IsEnabled)
			{
                stateTimer.Stop();
                stateTimer.Tick-=new EventHandler(stateTimer_Tick);
                stateTimer = null;
				return;
			}
		}
		protected abstract void DisposeModel();
		protected abstract HMIStaticStateCondition GetCurrentState();
		protected abstract void SetEnabled();
		protected abstract void SetVisibility();
		protected virtual void TrimAndPrepareTemplate()
		{
            TrimSignalAndObjectId(this);
            PrepairTemplate();
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
            dialogManager.Dispose();
            DisposeTimer();
            DisposeConnector();
            DisposeSecurityChecker();
            DoDispose();
			this.StopStoryboards();
		}
		protected void DisposeConnector()
		{
			if (Connector != null)
			{
                Connector.Dispose();
			}
		}
		protected void DisposeSecurityChecker()
		{
			if (SecurityChecker != null)
			{
                SecurityChecker.Dispose();
			}
		}
		private void RemoveFromUI(Control control)
		{
			Grid grid = GetLayoutRoot();
			if (grid != null && grid.Children.Contains(control))
			{
				grid.Children.Remove(control);
			}
		}
		protected virtual void RestoreBackupStateProperties()
		{
            Background = backgroundBackup;
            Foreground = foregroundBackup;
		}
		protected virtual void DoDispose(HMIStatesControlBase owner)
		{
            DisposeModel();
            DisposeSecurityChecker();
            DisposeConnector();
		}
		protected void SetStatesProperties(HMIStaticStateCondition state)
		{
			if (state == null)
			{
                oldState = null;
                SetPropertiesForNullState();
				return;
			}
			if (state.Equals(oldState))
			{
				return;
			}
            oldState = state;
            SetPropertiesForNewState(state);
		}
		protected virtual void HandlePropertyChanged(DependencyProperty property)
		{
		}
	}
}
