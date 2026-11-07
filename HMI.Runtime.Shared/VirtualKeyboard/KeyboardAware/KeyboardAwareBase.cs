// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.VirtualKeyboard.KeyboardAware
{
    using System;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using HMI.Runtime.Shared.KeyboardControl;
    using HMI.Runtime.Shared.VirtualKeyboard;

    public abstract class KeyboardAwareBase : IVirtualKeyboardAware, IDisposable
    {
        protected FrameworkElement element;
        private VirtualKeyboardSettings virtualKeyboardSettings;
        protected WeakReference weakReference;

        protected KeyboardAwareBase(FrameworkElement element)
        {
            this.element = element;
            this.weakReference = new WeakReference(element, false);
            this.InitializeKeyboardSettings();
        }

        protected abstract void AddOrRemoveSign();
        protected abstract void AddOrReplaceOnKey(string key);
        private static TextBox CreateInvisibleTextBox()
        {
            TextBox box = new TextBox();
            box.Width=(0.0);
            box.Height=(0.0);
            return box;
        }

        protected abstract void DeleteLastCharacter();
        public void Dispose()
        {
            if (this.element != null)
            {
                KeyboardService.DetachKeyboardFrom(this.element);
            }
        }

        protected Panel FindFirstPanel(FrameworkElement owner)
        {
            return (this.FindTopFrameworkElementParentOf(owner) as Panel);
        }

        private FrameworkElement FindTopFrameworkElementParentOf(FrameworkElement owner)
        {
            if (owner is Panel)
            {
                return owner;
            }
            DependencyObject parent = owner.Parent;
            if ((parent == null) || !(parent is FrameworkElement))
            {
                return null;
            }
            DependencyObject grandparent = ((FrameworkElement)parent).Parent;
            if ((grandparent != null) && (grandparent is FrameworkElement))
            {
                return this.FindTopFrameworkElementParentOf((FrameworkElement)parent);
            }
            return (FrameworkElement)parent;
        }

        protected abstract void FocusElement();
        protected void ForceValidationAndValueChanged()
        {
            Panel panel = this.FindFirstPanel(this.element);
            if (panel != null)
            {
                KeyboardController.Instance.FreezeKeyboard = true;
                TextBox box = CreateInvisibleTextBox();
                panel.Children.Add(box);
                box.Focus();
                this.FocusElement();
                panel.Children.Remove(box);
                SynchronizationContext.Current.Post(s => KeyboardController.Instance.FreezeKeyboard = false, null);
            }
        }

        protected abstract VirtualKeyboardType GetKeyboardType();
        protected static PressedKeyInformation GetKeyInformation(string key)
        {
            return new PressedKeyInformation { 
                Key = KeyHandler.GetKeyFromString(key), 
                KeyboardModifiers = 0
            };
        }

        protected abstract int GetSelectedLength();
        private void InitializeKeyboardSettings()
        {
            this.virtualKeyboardSettings = new VirtualKeyboardSettings();
            this.virtualKeyboardSettings.VirtualKeyboardType = this.GetKeyboardType();
        }

        protected static bool IsUntreatedKey(PressedKeyInformation pressedKeyInformation)
        {
            if (pressedKeyInformation.Key != System.Windows.Input.Key.LeftAlt
                && pressedKeyInformation.Key != System.Windows.Input.Key.RightAlt)
            {
                return (pressedKeyInformation.Key == System.Windows.Input.Key.LeftShift
                    || pressedKeyInformation.Key == System.Windows.Input.Key.RightShift);
            }
            return true;
        }

        public void SendKey(string key)
        {
            PressedKeyInformation keyInformation = GetKeyInformation(key);
            if ((this.weakReference.Target != null) && !IsUntreatedKey(keyInformation))
            {
                switch (keyInformation.Key)
                {
                    case System.Windows.Input.Key.Back:
                    case System.Windows.Input.Key.Delete:
                        this.DeleteLastCharacter();
                        break;

                    case System.Windows.Input.Key.Enter:
                        this.ForceValidationAndValueChanged();
                        break;

                    case System.Windows.Input.Key.Subtract:
                        this.AddOrRemoveSign();
                        break;

                    default:
                        this.AddOrReplaceOnKey(key);
                        break;
                }
                this.SetCarretPositionAtEnd();
            }
        }

        protected abstract void SetCarretPosition(int carretPosition);
        protected abstract void SetCarretPositionAtEnd();

        public VirtualKeyboardSettings VirtualKeyboardSettings
        {
            get
            {
                return this.virtualKeyboardSettings;
            }
        }
    }
}
