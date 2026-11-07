// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using HMI.Runtime.Core;
namespace HMI.Runtime.Shared
{
	public class HMIStaticViewModelBase<TState> : SecureControlViewModelBase, IHMIStaticControlBaseViewModel<TState> where TState : HMIStateCondition
	{
		private new IHMIConnector connector;
		private IEnumerable<TState> states;
		private TState currentState;
		public event EventHandler StateChanged;
		public TState CurrentState
		{
			get
			{
				return currentState;
			}
			private set
			{
                currentState = value;
			}
		}
		public HMIStaticViewModelBase(IHMIConnector connector, IEnumerable<TState> states, ISecurityChecker securityChecker, DenyAccessBehaviour securityDenyAccessBehaviour) : base(securityChecker, securityDenyAccessBehaviour)
		{
			if (connector == null)
			{
				throw new ArgumentNullException("connector");
			}
			if (states == null)
			{
				throw new ArgumentNullException("states");
			}
			this.connector = connector;
			this.states = states;
		}
		~HMIStaticViewModelBase()
		{
            Dispose(false);
		}
		private void wfStateConditionBase_StateChanged(object sender, EventArgs e)
		{
            currentState = base.CheckAndSetState(states.Cast<HMIStateConditionBase>()) as TState;
            OnStateChanged();
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && base.IsInitialized)
			{
                RemoveStateChangedEventHandlers();
			}
			base.Dispose(disposing);
		}
		private void RemoveStateChangedEventHandlers()
		{
			using (IEnumerator<TState> enumerator = states.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TState current = enumerator.Current;
					current.StateChanged -= new EventHandler(wfStateConditionBase_StateChanged);
					current.Dispose();
				}
			}
		}
		protected override void OnInitializing()
		{
			base.OnInitializing();
            AddStateChangedEventHandlers();
		}
		private void AddStateChangedEventHandlers()
		{
			using (IEnumerator<TState> enumerator = states.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TState current = enumerator.Current;
					current.StateChanged += new EventHandler(wfStateConditionBase_StateChanged);
					current.Initialize(connector);
				}
			}
		}
		protected virtual void OnStateChanged()
		{
			EventHandler stateChanged = StateChanged;
			if (stateChanged != null)
			{
				stateChanged.Invoke(this, EventArgs.Empty);
			}
		}
	}
}
