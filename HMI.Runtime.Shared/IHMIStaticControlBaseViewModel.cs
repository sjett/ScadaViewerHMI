// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Shared
{
	public interface IHMIStaticControlBaseViewModel<TState> : IHMIControlViewModelBase<TState> where TState : HMIStateCondition
	{
	}
}
