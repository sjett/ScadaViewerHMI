// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Shared
{
	public interface IHMIControlViewModelBase<T> : IViewModel, IDisposable, ISecureControlViewModel where T : HMIStateConditionBase
	{
		event EventHandler StateChanged;
		T CurrentState
		{
			get;
		}
	}
}
