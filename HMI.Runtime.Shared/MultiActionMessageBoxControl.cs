// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
namespace HMI.Runtime.Shared
{
	[TemplatePart(Name = "ButtonsStackPanelElement", Type = typeof(StackPanel))]
	public class MultiActionMessageBoxControl : MessageBoxBase, IDisposable
	{
		protected new const string ElementButtonsStackPanel = "ButtonsStackPanelElement";
		private readonly ObservableCollection<UserAction> buttonActions = new ObservableCollection<UserAction>();
		public ObservableCollection<UserAction> ButtonActions
		{
			get
			{
				return this.buttonActions;
			}
		}
		public MultiActionMessageBoxControl()
		{
			base.DefaultStyleKey=(typeof(MultiActionMessageBoxControl));
		}
		protected override void SetupMessageBoxButtons()
		{
			for (int i = 0; i < this.ButtonActions.Count; i++)
			{
				this.AddActionButton(i);
			}
		}
		private void AddActionButton(int i)
		{
			UserAction action = this.ButtonActions[i];
			double spacing = (i > 0) ? 10.0 : 0.0;
			object actionContent = action.GetActionContent();
			base.AddButton(actionContent, delegate(object sender, RoutedEventArgs e)
			{
				if (action.ActionCallback != null)
				{
					action.ActionCallback(action.State);
				}
                this.Close(Dialogs.DialogResult.None);
			}, spacing);
		}
	}
}
