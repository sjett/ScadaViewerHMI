// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Runtime.Shared.Common;
namespace HMI.Runtime.Shared
{
	public abstract class ViewModelBase : PropertyChangedNotifier, IViewModel, IDisposable
	{
		public bool IsInitialized
		{
			get;
			private set;
		}
		public ViewModelBase()
		{
		}
		~ViewModelBase()
		{
            Dispose(false);
		}
		public void Dispose()
		{
            Dispose(true);
			GC.SuppressFinalize(this);
		}
		public void Initialize()
		{
			if (IsInitialized)
			{
				return;
			}
            OnInitializing();
            IsInitialized = true;
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && IsInitialized)
			{
                IsInitialized = false;
			}
		}
		protected virtual void OnInitializing()
		{
		}
	}
}
