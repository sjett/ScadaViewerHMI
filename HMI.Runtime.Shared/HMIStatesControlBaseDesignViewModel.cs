// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Shared
{
	public class HMIStatesControlBaseDesignViewModel<TState> : IHMIControlViewModelBase<TState>, IHMIStaticControlBaseViewModel<TState>, IViewModel, IDisposable, ISecureControlViewModel where TState : HMIStateCondition
	{
		public event EventHandler StateChanged;
		public event EventHandler EnableChanged;
		public event EventHandler VisibleChanged;
		public TState CurrentState
		{
			get
			{
				return default;
			}
		}
		public bool IsEnabled
		{
			get
			{
				return true;
			}
		}
		public bool IsVisible
		{
			get
			{
				return true;
			}
		}
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return DenyAccessBehaviour.Hide;
			}
		}
		public bool IsInitialized
		{
			get;
			private set;
		}
		public void Initialize()
		{
            IsInitialized = true;
		}
		public void Dispose()
		{
		}
		protected virtual void OnStateChanged()
		{
		}
		protected virtual void OnEnableChanged()
		{
		}
		protected virtual void OnVisibleChanged()
		{
		}
	}
}
