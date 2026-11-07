// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows.Controls;
using HMI.Runtime.Core;
using HMI.Runtime.Core.Helper.Extensions;
namespace HMI.RuntimeControls.General
{
	public class HMIConnectorStatistics : Control, IDisposable
	{
		private IHMIConnector connector;
		private bool isDisposed;
		public HMIConnectorStatistics()
		{
			base.DefaultStyleKey=(base.GetType());
			this.CreateConnector();
		}
		private void CreateConnector()
		{
			this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
			base.DataContext=(this.connector.Statistics);
		}
		protected virtual void DisposeManagedResources()
		{
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
	}
}
