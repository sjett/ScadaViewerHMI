// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Shared
{
	public interface ISecureControlViewModel : IViewModel, IDisposable
	{
		event EventHandler EnableChanged;
		event EventHandler VisibleChanged;
		bool IsEnabled
		{
			get;
		}
		bool IsVisible
		{
			get;
		}
		DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get;
		}
	}
}
