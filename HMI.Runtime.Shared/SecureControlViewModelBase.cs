// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using HMI.Runtime.Core;
namespace HMI.Runtime.Shared
{
	public abstract class SecureControlViewModelBase : ViewModelBase, IViewModel, IDisposable, ISecureControlViewModel
	{
		private bool isEnabled;
		private bool isVisible;
		private ISecurityChecker securityChecker;
		protected IHMIConnector connector;
		public event EventHandler EnableChanged;
		public event EventHandler VisibleChanged;
		public bool IsEnabled
		{
			get
			{
				return this.isEnabled;
			}
			protected set
			{
				this.isEnabled = value;
				base.RaisePropertyChanged<bool>(() => this.IsEnabled);
			}
		}
		public bool IsVisible
		{
			get
			{
				return this.isVisible;
			}
			protected set
			{
				this.isVisible = value;
				base.RaisePropertyChanged<bool>(() => this.IsVisible);
			}
		}
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get;
			private set;
		}
		public SecureControlViewModelBase(ISecurityChecker securityChecker, DenyAccessBehaviour securityDenyAccessBehaviour)
		{
			if (securityChecker == null)
			{
				throw new ArgumentNullException("securityChecker");
			}
			this.securityChecker = securityChecker;
			this.SecurityDenyAccessBehaviour = securityDenyAccessBehaviour;
			this.SetEnableState();
			this.SetVisibileState();
		}
		~SecureControlViewModelBase()
		{
			this.Dispose(false);
		}
		private void securityChecker_AuthorizationChanged(object sender, EventArgs e)
		{
			this.SetEnableState();
			this.SetVisibileState();
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && base.IsInitialized)
			{
				this.securityChecker.AuthorizationChanged -= new EventHandler(this.securityChecker_AuthorizationChanged);
			}
			base.Dispose(disposing);
		}
		protected override void OnInitializing()
		{
			base.OnInitializing();
			this.securityChecker.AuthorizationChanged += new EventHandler(this.securityChecker_AuthorizationChanged);
		}
		protected virtual void OnEnableChanged()
		{
			EventHandler enableChanged = this.EnableChanged;
			if (enableChanged != null)
			{
				enableChanged.Invoke(this, EventArgs.Empty);
			}
		}
		protected virtual void OnVisibleChanged()
		{
			EventHandler visibleChanged = this.VisibleChanged;
			if (visibleChanged != null)
			{
				visibleChanged.Invoke(this, EventArgs.Empty);
			}
		}
		private bool DetermineEnableState()
		{
			return this.securityChecker.IsAuthorized || this.SecurityDenyAccessBehaviour == DenyAccessBehaviour.Hide;
		}
		private bool DetermineVisibileState()
		{
			return this.securityChecker.IsAuthorized || this.SecurityDenyAccessBehaviour == DenyAccessBehaviour.Disable;
		}
		private void SetEnableState()
		{
			this.IsEnabled = this.DetermineEnableState();
			this.OnEnableChanged();
		}
		private void SetVisibileState()
		{
			this.IsVisible = this.DetermineVisibileState();
			this.OnVisibleChanged();
		}
		protected HMIStateConditionBase CheckAndSetState(IEnumerable<HMIStateConditionBase> states)
		{
			HMIStateConditionBase result = null;
			SecureControlViewModelBase.SetState(states, ref result);
			return result;
		}
		private static void SetState(IEnumerable<HMIStateConditionBase> states, ref HMIStateConditionBase thisState)
		{
			HMIStateConditionBase satifiedCondition = SecureControlViewModelBase.GetSatifiedCondition(states);
			if (satifiedCondition != null)
			{
				thisState = satifiedCondition;
				return;
			}
			thisState = null;
		}
		private static HMIStateConditionBase GetSatifiedCondition(IEnumerable<HMIStateConditionBase> states)
		{
			using (IEnumerator<HMIStateConditionBase> enumerator = states.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HMIStateConditionBase current = enumerator.Current;
					if (current.IsSatisfied())
					{
						return current;
					}
				}
			}
			return null;
		}
		protected void RegisterSignal(string serverName, string signalName, OnSignalChangedDelegate signalChangedDelegate)
		{
			try
			{
				this.connector.RegisterSignalChangedHandler(serverName, signalName, signalChangedDelegate);
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		protected void UnregisterSignal(string serverName, string signalName, OnSignalChangedDelegate signalChangedDelegate)
		{
			try
			{
				this.connector.UnregisterSignalChangedHandler(serverName, signalName, signalChangedDelegate);
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
	}
}
