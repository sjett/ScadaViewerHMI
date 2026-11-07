// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Runtime.Core;
namespace HMI.Runtime.Shared
{
    public interface ISecurityChecker : IDisposable
    {
        event EventHandler AuthorizationChanged;
        bool IsAuthorized
        {
            get;
        }
        string SecurityAuthorizationName
        {
            get;
        }
    }

    public class SecurityChecker : ISecurityChecker
	{
		private IHMISecurity wfSecurity;
		public event EventHandler AuthorizationChanged;
		public bool IsAuthorized
		{
			get;
			private set;
		}
		public string SecurityAuthorizationName
		{
			get;
			private set;
		}
		public SecurityChecker(IHMISecurity wfSecurity, string securityAuthorizationName)
		{
			if (wfSecurity == null)
			{
				throw new ArgumentException();
			}
			this.SecurityAuthorizationName = securityAuthorizationName;
			this.wfSecurity = wfSecurity;
			this.HoldCurrentAuthorizationStatus();
			this.wfSecurity.UserContextChanged += new OnUserContextChangedEventHandler(this.wfSecurity_UserContextChanged);
		}
		~SecurityChecker()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void wfSecurity_UserContextChanged(string userName)
		{
			this.HandleUserContextChanged();
		}
		private void HoldCurrentAuthorizationStatus()
		{
			this.IsAuthorized = this.CheckAuthorization();
		}
		private void HandleUserContextChanged()
		{
			if (this.AuthorizationHasChanged())
			{
				this.HandleAuthorizationHasChanged();
			}
		}
		private bool AuthorizationHasChanged()
		{
			return this.IsAuthorized != this.CheckAuthorization();
		}
		private void HandleAuthorizationHasChanged()
		{
			this.HoldCurrentAuthorizationStatus();
			this.OnAuthorizationChanged();
		}
		private bool CheckAuthorization()
		{
			return string.IsNullOrEmpty(this.SecurityAuthorizationName) || this.wfSecurity.CheckProjectAuthorization(this.SecurityAuthorizationName);
		}
		protected virtual void OnAuthorizationChanged()
		{
			if (this.AuthorizationChanged != null)
			{
				this.AuthorizationChanged.Invoke(this, EventArgs.Empty);
			}
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.wfSecurity != null)
			{
				this.wfSecurity.UserContextChanged -= new OnUserContextChangedEventHandler(this.wfSecurity_UserContextChanged);
			}
		}
	}
}
