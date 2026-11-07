// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Dialogs;
namespace HMI.RuntimeControls.General.UserLoginDialogs
{
	public class HMIUserLoginPasswordChange : DialogControl
	{
		private const string SNDialogCaption = "HMISN_Ctrl_HMIUserLogin_DialogCaption";

		public event Action<string, string> ChangePasswordTriggered
		{
			add
			{
				if (base.Content is HMIUserLoginPasswordChangeContent)
				{
					(base.Content as HMIUserLoginPasswordChangeContent)!.ChangePasswordTriggered += value;
				}
			}
			remove
			{
				if (base.Content is HMIUserLoginPasswordChangeContent)
				{
					(base.Content as HMIUserLoginPasswordChangeContent)!.ChangePasswordTriggered -= value;
				}
			}
		}
		public HMIUserLoginPasswordChange()
		{
			base.DefaultStyleKey=(base.GetType());
		}
		public HMIUserLoginPasswordChange(Style contentStyle, bool isPasswordExpired, string userTryingToLoggin) : this()
		{
			base.HeaderText = SNDialogCaption;
			HMIUserLoginPasswordChangeContent wFUserLoginPasswordChangeContent = new HMIUserLoginPasswordChangeContent(isPasswordExpired, userTryingToLoggin);
			wFUserLoginPasswordChangeContent.CloseWindow += new Action(this.dialogContent_CloseWindow);
			wFUserLoginPasswordChangeContent.Style=(contentStyle);
			base.Content=(wFUserLoginPasswordChangeContent);
		}
		private void dialogContent_CloseWindow()
		{
			base.Close();
		}
		protected override void OnDispose()
		{
			base.OnDispose();
			HMIUserLoginPasswordChangeContent wFUserLoginPasswordChangeContent = base.Content as HMIUserLoginPasswordChangeContent;
			if (wFUserLoginPasswordChangeContent != null)
			{
				wFUserLoginPasswordChangeContent.CloseWindow -= new Action(this.dialogContent_CloseWindow);
				wFUserLoginPasswordChangeContent.Dispose();
			}
		}
	}
}
