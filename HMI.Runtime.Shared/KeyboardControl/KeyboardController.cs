// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HMI.Runtime.Core;
using HMI.Runtime.Shared.VirtualKeyboard;
using HMI.Runtime.Shared.VirtualKeyboard.KeyboardAware;
namespace HMI.Runtime.Shared.KeyboardControl
{
	public class KeyboardController
	{
		private bool isUsingVirtualKeyboard;
		private WeakReference pendingKeyboardAware;
		private VirtualKeyboardType pendingVirtualKeyboardType;
		private IMachineSettingsProvider machineSettingsProvider;
		public event EventHandler IsUsingVirtualKeyboardChanged;
		internal bool FreezeKeyboard
		{
			get;
			set;
		}
		internal bool MachineSettingsApplied
		{
			get;
			private set;
		}
		internal bool IsUsingVirtualKeyboard
		{
			get
			{
				return this.isUsingVirtualKeyboard;
			}
			set
			{
				bool flag = this.isUsingVirtualKeyboard != value;
				this.isUsingVirtualKeyboard = value;
				if (flag && this.IsUsingVirtualKeyboardChanged != null)
				{
					this.IsUsingVirtualKeyboardChanged.Invoke(null, EventArgs.Empty);
				}
			}
		}
		public static KeyboardController Instance
		{
			get;
			private set;
		}
		static KeyboardController()
		{
			KeyboardController.Instance = new KeyboardController();
		}
		private KeyboardController()
		{
			this.machineSettingsProvider = new MachineSettingsProvider(new Action<MachineSettings>(this.MachineSettingsChanged));
		}
		private void MachineSettingsChanged(MachineSettings clientMachineSettings)
		{
			if (clientMachineSettings == null)
			{
				return;
			}
			this.IsUsingVirtualKeyboard = clientMachineSettings.HasVirtualKeyboard;
			this.MachineSettingsApplied = clientMachineSettings.MachineSettingsApplied;
			if (this.IsUsingVirtualKeyboard)
			{
				this.CheckPendingControls();
			}
		}
		public void OpenKeyboardFor(IVirtualKeyboardAware owner)
		{
			if (this.FreezeKeyboard)
			{
				return;
			}
			if (!this.IsUsingVirtualKeyboard)
			{
				this.pendingKeyboardAware = new WeakReference(owner, false);
				return;
			}
			this.DettachKeyboard();
			this.AttachKeyboardFor(owner);
		}
		public void OpenKeyboardFor(TextBox textBox, VirtualKeyboardType vkType)
		{
			if (this.FreezeKeyboard)
			{
				return;
			}
			if (!this.IsUsingVirtualKeyboard)
			{
				this.pendingKeyboardAware = new WeakReference(textBox, false);
				this.pendingVirtualKeyboardType = vkType;
				return;
			}
			this.DettachKeyboard();
			switch (vkType)
			{
			case VirtualKeyboardType.Numeric:
			{
				NumericKeyboardAwareTextBox owner = new NumericKeyboardAwareTextBox(textBox);
				this.AttachKeyboardFor(owner);
				return;
			}
			case VirtualKeyboardType.Alphanumeric:
			{
				AlphanumericKeyboardAwareTextBox owner2 = new AlphanumericKeyboardAwareTextBox(textBox);
				this.AttachKeyboardFor(owner2);
				return;
			}
			default:
				return;
			}
		}
		public void DettachKeyboard()
		{
			if (this.FreezeKeyboard)
			{
				return;
			}
			IVirtualKeyboard attachedKeyboard = this.GetAttachedKeyboard();
			if (attachedKeyboard != null)
			{
				this.DettachKeyboard(attachedKeyboard);
			}
		}
		private void CheckPendingControls()
		{
			if (this.pendingKeyboardAware == null)
			{
				return;
			}
			IVirtualKeyboardAware virtualKeyboardAware = this.pendingKeyboardAware.Target as IVirtualKeyboardAware;
			if (virtualKeyboardAware != null)
			{
				this.OpenKeyboardFor(virtualKeyboardAware);
				return;
			}
			TextBox textBox = this.pendingKeyboardAware.Target as TextBox;
			if (textBox != null)
			{
				this.OpenKeyboardFor(textBox, this.pendingVirtualKeyboardType);
			}
		}
		private IVirtualKeyboard GetAttachedKeyboard()
		{
			Panel container = HMIKeyboardWindow.Container;
			if (container == null)
			{
				return null;
			}
			if (container.Children.Contains(HMIVirtualKeyboard.HMIKeyboardInstance))
			{
				return HMIVirtualKeyboard.HMIKeyboardInstance;
			}
			if (container.Children.Contains(HMINumericKeyboard.HMINumericKeyboardInstance))
			{
				return HMINumericKeyboard.HMINumericKeyboardInstance;
			}
			return null;
		}
		private void DettachKeyboard(IVirtualKeyboard keyboard)
		{
			keyboard.HideKeyboard();
			keyboard.Owner = null;
			HMIKeyboardWindow.Container.Children.Remove(keyboard.KeyboardControl);
			HMIKeyboardWindow.Container = null;
		}
		private void AttachKeyboardFor(IVirtualKeyboardAware owner)
		{
			Panel panel = this.FindKeyboardContainer();
			if (panel == null)
			{
				throw new Exception("Owner must be contained by a Panel.");
			}
			IVirtualKeyboard keyboardFor = this.GetKeyboardFor(owner);
			if (keyboardFor != null)
			{
				panel.Children.Add(keyboardFor.KeyboardControl);
				HMIKeyboardWindow.Container = panel;
				keyboardFor.ShowKeyboard();
			}
		}
		private IVirtualKeyboard GetKeyboardFor(IVirtualKeyboardAware owner)
		{
			if (owner.VirtualKeyboardSettings.VirtualKeyboardType == VirtualKeyboardType.Alphanumeric)
			{
				HMIVirtualKeyboard.HMIKeyboardInstance.Owner = owner;
				return HMIVirtualKeyboard.HMIKeyboardInstance;
			}
			if (owner.VirtualKeyboardSettings.VirtualKeyboardType == VirtualKeyboardType.Numeric)
			{
				HMINumericKeyboard.HMINumericKeyboardInstance.Owner = owner;
				return HMINumericKeyboard.HMINumericKeyboardInstance;
			}
			return null;
		}
		private Panel FindKeyboardContainer()
		{
			FrameworkElement frameworkElement = Application.Current.MainWindow as FrameworkElement;
			if (frameworkElement == null)
			{
				return null;
			}
			return Enumerable.FirstOrDefault<Panel>(frameworkElement.GetChildrenByType<Panel>());
		}
	}
}
