// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HMI.Runtime.Shared.KeyboardControl;

namespace HMI.Runtime.Shared.VirtualKeyboard
{
	public partial class HMIVirtualKeyboard : UserControl, IVirtualKeyboard
	{
		private bool shiftHit;
		private static Panel parentGrid;
		private static HMIVirtualKeyboard wfKeyboardInstance;
		private IVirtualKeyboardHelper helper;
		private bool hasNCHitTest;
		private Point initialNCHitTest;
		private bool isFirstDisplay = true;
		private bool dragStarted;

		public static readonly DependencyProperty KeyboardSizeProperty = DependencyProperty.Register("KeyboardSize", typeof(double), typeof(HMIVirtualKeyboard), new PropertyMetadata(600.0, new PropertyChangedCallback(OnKeyboardSizePropertyChanged)));
		
		public bool ShiftHit
		{
			get
			{
				return shiftHit;
			}
			set
			{
				shiftHit = value;
				ChangeLayout();
			}
		}
		public static int KeyNum
		{
			get;
			set;
		}
		[Category("HMI")]
		public double KeyboardSize
		{
			get
			{
				return (double)GetValue(KeyboardSizeProperty);
			}
			set
			{
				SetValue(KeyboardSizeProperty, value);
			}
		}
		public static HMIVirtualKeyboard HMIKeyboardInstance
		{
			get
			{
				if (wfKeyboardInstance == null)
				{
					InitializeInstance();
				}
				wfKeyboardInstance.InitKeyboard();
				return wfKeyboardInstance;
			}
			set
			{
				wfKeyboardInstance = value;
			}
		}
		public Control KeyboardControl
		{
			get
			{
				return this;
			}
		}
		public IVirtualKeyboardAware Owner
		{
			get;
			set;
		}
		private static void OnKeyboardSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void InitializeInstance()
		{
			wfKeyboardInstance = new HMIVirtualKeyboard();
			wfKeyboardInstance.SetValue(Panel.ZIndexProperty, 2);
			wfKeyboardInstance.Closed.Storyboard.Completed+=new EventHandler(StoryboardClosed_Completed);
		}
		public HMIVirtualKeyboard()
		{
			this.InitializeComponent();
			InitKeyboard();
			KeysDictionary.KeyboardLanguageChanged += new KeyboardLanguageChangedHandler(InitKeyboard);
			helper = new VirtualKeyboardHelper();
			this.uxPopupElement.Opened+=new EventHandler(uxPopupElement_Opened);
			this.uxGridElement.MouseLeftButtonDown+=new MouseButtonEventHandler(uxGridElement_MouseLeftButtonDown);
			this.uxGridElement.MouseLeftButtonUp+=new MouseButtonEventHandler(uxGridElement_MouseLeftButtonUp);
			this.uxGridElement.MouseMove+=new MouseEventHandler(uxGridElement_MouseMove);
		}
        private void Content_Resized(object sender, SizeChangedEventArgs e)
		{
			this.uxCanvasElement.Height=Application.Current.MainWindow.ActualHeight;
            this.uxCanvasElement.Width = Application.Current.MainWindow.ActualWidth;
		}
		private void Key_VirtualKeyboardKeyPressed(object sender, VirtualKeyPressedEventArgs e)
		{
			if (e.Key == "Shift")
			{
				ToggleShift();
			}
			ChangeKeyboardLanguageIfNecessary(e.Key);
			if (Owner != null && !IsLanguageButton(e.Key))
			{
				Owner.SendKey(e.Key);
			}
		}
		public void KeyboardClose(object sender, MouseButtonEventArgs e)
		{
			if (KeyboardExistsAsChild())
			{
				HideKeyboard();
			}
			e.Handled=true;
		}
		private static void StoryboardClosed_Completed(object sender, EventArgs e)
		{
			KeyboardController.Instance.DettachKeyboard();
			wfKeyboardInstance.uxPopupElement.IsOpen=false;
			wfKeyboardInstance.uxKeyboardSizeControl.Visibility= Visibility.Collapsed;
			MatrixTransform matrixTransform = new MatrixTransform();
			matrixTransform.Matrix=Matrix.Identity;
			wfKeyboardInstance.RenderTransform=matrixTransform;
		}
		private void uxGridElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.Handled)
			{
				return;
			}
			hasNCHitTest = true;
			initialNCHitTest = e.GetPosition(this.uxGridElement);
			this.uxGridElement.CaptureMouse();
			e.Handled=true;
		}
		private void uxGridElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (e.Handled)
			{
				return;
			}
			if (hasNCHitTest)
			{
				hasNCHitTest = false;
				dragStarted = false;
				this.uxGridElement.ReleaseMouseCapture();
				e.Handled=true;
			}
		}
		private void uxGridElement_MouseMove(object sender, MouseEventArgs e)
		{
			if (hasNCHitTest)
			{
				Point position = e.GetPosition(this.uxGridElement);
				Point point = new Point(position.X - initialNCHitTest.X, position.Y - initialNCHitTest.Y);
                if (dragStarted || Math.Abs(point.X) > 3.0 || Math.Abs(point.Y) > 3.0)
				{
					dragStarted = true;
					GeneralTransform generalTransform = this.uxGridElement.TransformToVisual(this.uxCanvasElement);
					Point point2 = generalTransform.Transform(point);
					if (point2.X < 0.0 || point2.X + this.uxGridElement.ActualWidth > this.uxCanvasElement.ActualWidth)
					{
						point.X=0.0;
					}
					if (point2.Y < 0.0 || point2.Y + this.uxGridElement.ActualHeight > this.uxCanvasElement.ActualHeight)
					{
						point.Y=0.0;
					}
					this.uxGridElement.SetValue(Canvas.TopProperty, (double)this.uxGridElement.GetValue(Canvas.TopProperty) + point.Y);
					this.uxGridElement.SetValue(Canvas.LeftProperty, (double)this.uxGridElement.GetValue(Canvas.LeftProperty) + point.X);
				}
			}
		}
		private void uxPopupElement_Opened(object sender, EventArgs e)
		{
			VisualStateManager.GoToState(this, "Open", true);
		}
		private void InitKeyboard()
		{
			shiftHit = false;
			int languageDefault = KeysDictionary.KeyboardLayouts[KeysDictionary.KeyboardLayout];
			for (int i = 0; i < this.LayoutRoot.Children.Count; i++)
			{
				VirtualKey virtualKey = this.LayoutRoot.Children[i] as VirtualKey;
				if (virtualKey != null)
				{
					SetButtonsText(languageDefault, virtualKey);
					SetLanguageLayout(virtualKey);
				}
				if (this.LayoutRoot.Children[i] != null && this.LayoutRoot.Children[i].Equals(this.DoubleKeyContainer))
				{
					virtualKey = (VirtualKey)this.DoubleKeyContainer.Children[0];
					SetButtonsText(languageDefault, virtualKey);
				}
			}
		}
		private void ChangeKeyboardLanguageIfNecessary(string keyHit)
		{
			if (keyHit != null)
			{
				if (keyHit == "EN")
				{
					ChangeKeyboardlayout(keyHit);
					this.smaler_Key.Visibility= Visibility.Collapsed;
					return;
				}
				if (keyHit == "RU")
				{
					ChangeKeyboardlayout(keyHit);
					return;
				}
				if (keyHit == "AR")
				{
					ChangeKeyboardlayout(keyHit);
					return;
				}
				if (!(keyHit == "DE"))
				{
					return;
				}
				ChangeKeyboardlayout(keyHit);
				this.smaler_Key.Visibility= Visibility.Visible;
			}
		}
		private static void ChangeKeyboardlayout(string keyHit)
		{
			if (KeysDictionary.KeyboardLayouts.ContainsKey(keyHit))
			{
				KeysDictionary.KeyboardLayout = keyHit;
			}
		}
		private static void SetButtonsText(int languageDefault, VirtualKey button)
		{
			if (!HMIVirtualKeyboard.IsModifierKey(button.Text))
			{
				button.Text = KeysDictionary.KeyValues[button.KeyNum + languageDefault];
			}
		}
		private static void SetLanguageLayout(VirtualKey button)
		{
			if (HMIVirtualKeyboard.IsLanguageButton(button.Text))
			{
				if (IsSelectedLanguage(button))
				{
					button.IsEnabled=false;
					return;
				}
				button.IsEnabled=true;
			}
		}
		private static bool IsSelectedLanguage(VirtualKey button)
		{
			return button.Text.Equals(KeysDictionary.KeyboardLayout);
		}
		private static bool IsLanguageButton(string buttonText)
		{
			return buttonText.Equals("AR") || buttonText.Equals("EN") || buttonText.Equals("RU") || buttonText.Equals("DE");
		}
		public void ChangeLayout()
		{
			if (shiftHit)
			{
				for (int i = 0; i < this.LayoutRoot.Children.Count; i++)
				{
					UIElement uIElement = this.LayoutRoot.Children[i];
					if (uIElement != null && IsHMIKeyType(uIElement))
					{
						VirtualKey buttonContent = (VirtualKey)uIElement;
						SetButtonContent(buttonContent);
					}
					if (this.LayoutRoot.Children[i] != null && this.LayoutRoot.Children[i].Equals(this.DoubleKeyContainer))
					{
						VirtualKey buttonContent2 = (VirtualKey)this.DoubleKeyContainer.Children[0];
						SetButtonContent(buttonContent2);
					}
				}
				return;
			}
			InitKeyboard();
		}
		private static void SetButtonContent(VirtualKey key)
		{
			string text = key.Text;
			if (!IsModifierKey(text))
			{
				int num = KeysDictionary.KeyboardLayouts[KeysDictionary.KeyboardLayout];
				key.Text = KeysDictionary.KeyValues[key.KeyNum + 48 + num];
			}
		}
		private static bool IsHMIKeyType(UIElement element)
		{
			return element.GetType().Equals(typeof(VirtualKey));
		}
		private static bool IsModifierKey(string buttonText)
		{
			return buttonText.Equals("Backspace".ToString()) || buttonText.Equals("Delete".ToString()) || buttonText.Equals("Enter".ToString()) || buttonText.Equals("Shift".ToString()) || buttonText.Equals("Alt".ToString()) || buttonText.Equals("Space".ToString()) || buttonText.Equals("Escape".ToString()) || buttonText.Equals("EN".ToString()) || buttonText.Equals("AR".ToString()) || buttonText.Equals("RU".ToString()) || buttonText.Equals("DE".ToString());
		}
		private static bool KeyboardExistsAsChild()
		{
			return HMIKeyboardWindow.Container.Children.Contains(HMIKeyboardInstance);
		}
		internal static void PrepareForRezise()
		{
			wfKeyboardInstance.ToggleResizeControl();
		}
		public void StartWarningAnimation()
		{
			(Resources["WarningAnimation"] as Storyboard).Begin();
			if (wfKeyboardInstance != null)
			{
				(this.uxKeyboardSizeControl.Resources["WarningAnimation"] as Storyboard).Begin();
			}
		}
		private void ToggleShift()
		{
			ShiftHit = !ShiftHit;
		}
		public void ShowKeyboard()
		{
            Application.Current.MainWindow.SizeChanged += new SizeChangedEventHandler(Content_Resized);
			this.uxCanvasElement.Background=null;
			this.uxCanvasElement.Height=Application.Current.MainWindow.ActualHeight;
            this.uxCanvasElement.Width = Application.Current.MainWindow.ActualWidth;
			Transform transform = helper.CreateRenderTransform(TransformToVisual(null));
			if (transform != null)
			{
				RenderTransform=transform;
			}
			this.uxPopupElement.IsOpen=true;
			if (isFirstDisplay)
			{
				isFirstDisplay = false;
                helper.CenterBottomControl(this.uxGridElement, this.uxCanvasElement, 1);//Application.Current.Host.Content.ZoomFactor
			}
		}
		public void HideKeyboard()
		{
            Application.Current.MainWindow.SizeChanged -= new SizeChangedEventHandler(Content_Resized);
			VisualStateManager.GoToState(this, "Closed", true);
			this.uxKeyboardSizeControl.Hide();
		}
		public void ToggleResizeControl()
		{
			if (!this.uxKeyboardSizeControl.IsOpened)
			{
				if (this.uxKeyboardSizeControl.ActualHeight == 0.0 || double.IsNaN(this.uxKeyboardSizeControl.ActualHeight))
				{
					this.uxKeyboardSizeControl.Measure(new Size(this.uxCanvasElement.Width, this.uxCanvasElement.Height));
				}
				this.uxKeyboardSizeControl.SetValue(Canvas.TopProperty, 0.0);
				this.uxKeyboardSizeControl.SetValue(Canvas.LeftProperty, this.uxCanvasElement.Width - this.uxKeyboardSizeControl.Width);
				this.uxKeyboardSizeControl.Visibility= Visibility.Visible;
				this.uxKeyboardSizeControl.Owner = this;
				this.uxKeyboardSizeControl.KeyboardSize = this.KeyboardViewBox.ActualWidth;
				this.uxKeyboardSizeControl.Show();
				return;
			}
			this.uxKeyboardSizeControl.Hide();
		}
	
	}
}
