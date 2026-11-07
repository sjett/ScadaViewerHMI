// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Runtime.Shared.Dialogs;
namespace HMI.RuntimeControls.General.UserLoginDialogs
{
	public class HMIUserLogin1Dialog : DialogControl
	{
		private const string SNDialogCaption = "HMISN_Ctrl_HMIUserLogin_DialogCaption";

		public event Action<string, string, bool> LoginTriggered
		{
			add
			{
				if (base.Content is HMIUserLogin1DialogContent)
				{
					(base.Content as HMIUserLogin1DialogContent).LoginTriggered += value;
				}
			}
			remove
			{
				if (base.Content is HMIUserLogin1DialogContent)
				{
					(base.Content as HMIUserLogin1DialogContent).LoginTriggered -= value;
				}
			}
		}
		public event Action LogoutTriggered
		{
			add
			{
				if (base.Content is HMIUserLogin1DialogContent)
				{
					(base.Content as HMIUserLogin1DialogContent).LogoutTriggered += value;
				}
			}
			remove
			{
				if (base.Content is HMIUserLogin1DialogContent)
				{
					(base.Content as HMIUserLogin1DialogContent).LogoutTriggered -= value;
				}
			}
		}
		public event Action ChangePasswordTriggered
		{
			add
			{
				if (base.Content is HMIUserLogin1DialogContent)
				{
					(base.Content as HMIUserLogin1DialogContent).ChangePasswordTriggered += value;
				}
			}
			remove
			{
				if (base.Content is HMIUserLogin1DialogContent)
				{
					(base.Content as HMIUserLogin1DialogContent).ChangePasswordTriggered -= value;
				}
			}
		}
		public HMIUserLogin1Dialog()
		{
			base.DefaultStyleKey=(base.GetType());
		}
		public HMIUserLogin1Dialog(Style contentStyle, bool canConnectWithDomainUser, bool canChangePassword) : this()
		{
			base.HeaderText = SNDialogCaption;
			HMIUserLogin1DialogContent wFUserLogin1DialogContent = new HMIUserLogin1DialogContent(canConnectWithDomainUser, canChangePassword);
			wFUserLogin1DialogContent.CloseWindow += new Action(this.dialogContent_CloseWindow);
			wFUserLogin1DialogContent.Style=(contentStyle);
			base.Content=(wFUserLogin1DialogContent);
		}
		private void dialogContent_CloseWindow()
		{
			base.Close();
		}
		protected override void OnDispose()
		{
			base.OnDispose();
			HMIUserLogin1DialogContent wFUserLogin1DialogContent = base.Content as HMIUserLogin1DialogContent;
			if (wFUserLogin1DialogContent != null)
			{
				wFUserLogin1DialogContent.CloseWindow -= new Action(this.dialogContent_CloseWindow);
				wFUserLogin1DialogContent.Dispose();
			}
		}
		//public override void Show()
		//{
		//	base.Show(true);
		//}
	}
}
