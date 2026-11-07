// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Telerik.Windows.Controls;
using HMI.Runtime.Core;
using HMI.Runtime.Shared.Common;
using HMI.Runtime.Core.Helper.Extensions;

namespace HMI.Runtime.Shared.Dialogs
{
	[TemplatePart(Name = "CanvasElement", Type = typeof(Canvas)), TemplatePart(Name = "GridElement", Type = typeof(Grid))]
	public class DialogControl : RadWindow, IDisposable
	{
		public enum MoveBehaviorType
		{
			AllAreas,
			CaptionBarOnly
		}
		protected const string ElementCanvas = "CanvasElement";
		protected const string ElementGrid = "GridElement";
		private bool isLocationCentered = true;
		private IHMIConnector connector;

		public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.Register("HeaderBackground", typeof(Brush), typeof(DialogControl), new PropertyMetadata(null, new PropertyChangedCallback(OnHeaderBackgroundPropertyChanged)));
		public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.Register("HeaderForeground", typeof(Brush), typeof(DialogControl), new PropertyMetadata(null, new PropertyChangedCallback(OnHeaderForegroundPropertyChanged)));
		public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string), typeof(DialogControl), new PropertyMetadata("Dialog Control", new PropertyChangedCallback(OnHeaderTextPropertyChanged)));
		public static readonly DependencyProperty MoveBehaviorProperty = DependencyProperty.Register("MoveBehavior", typeof(MoveBehaviorType), typeof(DialogControl), new PropertyMetadata(MoveBehaviorType.CaptionBarOnly, new PropertyChangedCallback(OnMoveBehaviorPropertyChanged)));
		//private readonly List<RadHtmlPlaceholder> collapsedHtmlControls = new List<RadHtmlPlaceholder>();
		
		public event EventHandler<DialogClosedEventArgs> DialogClosed;
		
		[Category("HMI")]
		public Brush HeaderBackground
		{
			get
			{
				return (Brush)GetValue(HeaderBackgroundProperty);
			}
			set
			{
                SetValue(HeaderBackgroundProperty, value);
			}
		}
		[Category("HMI")]
		public Brush HeaderForeground
		{
			get
			{
				return (Brush)GetValue(HeaderForegroundProperty);
			}
			set
			{
                SetValue(HeaderForegroundProperty, value);
			}
		}
		[Category("HMI")]
		public string HeaderText
		{
			get
			{
				return (string)GetValue(HeaderTextProperty);
			}
			set
			{
                SetValue(HeaderTextProperty, value);
			}
		}
		[Category("HMI")]
		public MoveBehaviorType MoveBehavior
		{
			get
			{
				return (MoveBehaviorType)GetValue(MoveBehaviorProperty);
			}
			set
			{
                SetValue(MoveBehaviorProperty, value);
			}
		}
		private static void OnHeaderBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnHeaderForegroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnHeaderTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (DesignerProperties.GetIsInDesignMode(d))
			{
				return;
			}
			//d.SetValue(Telerik.Windows.Controls.HeaderedContentControl.HeaderProperty, e.NewValue);
            d.SetValue(HeaderProperty, e.NewValue);
		}
		private static void OnMoveBehaviorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		public DialogControl()
		{
            Header = string.Empty;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;// Telerik.Windows.Controls.WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.NoResize;// Telerik.Windows.Controls.ResizeMode.NoResize;
            IsRestricted = true;
			if (DesignerProperties.GetIsInDesignMode(this))
			{
                Visibility = Visibility.Collapsed;
			}
            Loaded += new RoutedEventHandler(DialogControl_Loaded);
            SizeChanged += new SizeChangedEventHandler(OnSizeChanged);
		}
		private void DialogControl_Loaded(object sender, RoutedEventArgs e)
		{
			if (DesignerProperties.GetIsInDesignMode(this))
			{
				return;
			}
            PrepareForUpdatePosition();
			//Application.Current.App.Content.Resized+=(new EventHandler(this.Content_Resized));
            Application.Current.MainWindow.SizeChanged += new SizeChangedEventHandler(Content_Resized);
            StretchModalBackground();
		}
		public void Dispose()
		{
			GC.SuppressFinalize(this);
            Dispose(true);
		}
		private void Dispose(bool dispose)
		{
			if (!dispose)
			{
				return;
			}
			if (connector != null)
			{
                //connector.UnregisterLanguageChangedHandler(null, new OnLanguageChangedDelegate(this.OnHeaderSymbolicTextChanged));
                connector.Dispose();
			}
            Loaded -= new RoutedEventHandler(DialogControl_Loaded);
            SizeChanged -= new SizeChangedEventHandler(OnSizeChanged);
			if (Application.Current != null)
			{
				//Application.Current.Host.Content.Resized-=(new EventHandler(this.Content_Resized));
                Application.Current.MainWindow.SizeChanged -= new SizeChangedEventHandler(Content_Resized);
			}
            OnDispose();
		}
		protected virtual void OnDispose()
		{
		}
		//private void Content_Resized(object sender, EventArgs e)
        private void Content_Resized(object sender, SizeChangedEventArgs e)
		{
            PrepareForUpdatePosition();
            RepositionWindow();
		}
		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
            UpdateLeftAndTop();
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
            Initialize();
		}
		internal void Initialize()
		{
            //base.TabNavigation = System.Windows.Input.KeyboardNavigationMode.Cycle;
			if (DesignerPropertiesExt.IsInDesignTool)
			{
				return;
			}

            connector = RuntimeServices.GetRequiredService<IHMIConnector>();

            SetHeaderText();
		}
		private void SetHeaderText()
		{
			if (string.IsNullOrWhiteSpace(HeaderText))
			{
                HeaderText = string.Empty;
				return;
			}
            HeaderText = StringPlaceholdersResolver.Resolve(HeaderText, this);
			//try
			//{
			//	connector.RegisterLanguageChangedHandler(new string[]
			//	{
			//		HeaderText
			//	}, new OnLanguageChangedDelegate(this.OnHeaderSymbolicTextChanged));
			//}
			//catch (Exception ex)
			//{
			//	connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			//}
		}
		//private void OnHeaderSymbolicTextChanged(string[] symbolicNames, string[] translationTexts)
		//{
		//	try
		//	{
		//		string text;
		//		if (translationTexts[0] != null && !translationTexts[0].Equals("!null!", StringComparison.InvariantCultureIgnoreCase))
		//		{
		//			text = translationTexts[0];
		//		}
		//		else
		//		{
		//			text = symbolicNames[0];
		//		}
		//		base.SetValue(HeaderedContentControl.HeaderProperty, text);
		//	}
		//	catch (Exception ex)
		//	{
  //              connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
		//	}
		//}

		//public void ShowDialog()
		//{
		//	//this.HideHtmlViewers();
		//	base.ShowDialog();
		//}

		//public void Show(bool shouldHideHtmlViewers)
		//{
  //          //if (shouldHideHtmlViewers)
  //          //{
  //          //    this.HideHtmlViewers();
  //          //}
  //          Show();
		//}

		protected override void OnClosed(WindowClosedEventArgs args)
		{
            //this.ShowHtmlViewers();
            OnDialogClosed(ConvertToDialogResult(DialogResult));
			base.OnClosed(args);
		}
		private void PrepareForUpdatePosition()
		{
            UpdateLayout();
            SetContentMaxWidthAndMaxHeight();
		}
		private void SetContentMaxWidthAndMaxHeight()
		{
            MaxWidth = Application.Current.MainWindow.ActualWidth;
            MaxHeight = Application.Current.MainWindow.ActualHeight;
		}
		private void RepositionWindow()
		{
            StretchModalBackground();
            UpdateLeftAndTop();
		}
		protected override void OnLocationChanged(EventArgs args)
		{
			base.OnLocationChanged(args);
			if (!isLocationCentered)
			{
				return;
			}
            Dispatcher.BeginInvoke(new Action(SetIsLocationChanged));
		}
		private void SetIsLocationChanged()
		{
            double centeredLeft = (Application.Current.MainWindow.ActualWidth - Math.Min(ActualWidth, MaxWidth)) / 2.0;
            double centeredTop = (Application.Current.MainWindow.ActualHeight - Math.Min(ActualHeight, MaxHeight)) / 2.0;
            isLocationCentered = Left == centeredLeft && Top == centeredTop;
		}
		private void UpdateLeftAndTop()
		{
            UpdateLeft();
            UpdateTop();
		}
		private void UpdateLeft()
		{
            double actualWidth = Application.Current.MainWindow.ActualWidth;
			if (isLocationCentered)
			{
                Left = (actualWidth - Math.Min(ActualWidth, MaxWidth)) / 2.0;
			}
			if (Left < 0.0)
			{
                Left = 0.0;
			}
			if (Left > actualWidth - ActualWidth)
			{
                Left = Math.Max(actualWidth - ActualWidth, 0.0);
			}
		}
		private void UpdateTop()
		{
            double actualHeight = Application.Current.MainWindow.ActualHeight;
			if (isLocationCentered)
			{
                Top = (actualHeight - Math.Min(ActualHeight, MaxHeight)) / 2.0;
			}
			if (Top < 0.0)
			{
                Top = 0.0;
			}
			if (Top > actualHeight - ActualHeight)
			{
                Top = Math.Max(actualHeight - ActualHeight, 0.0);
			}
		}
		private void StretchModalBackground()
		{
			Canvas canvas = Parent as Canvas;
			if (canvas == null)
			{
				return;
			}
			Canvas canvas2 = canvas.Children.OfType<Canvas>().FirstOrDefault();
			if (canvas2 == null)
			{
				return;
			}
			Popup popup = canvas.Parent as Popup;
			if (popup == null)
			{
				return;
			}
            double actualWidth = Application.Current.MainWindow.ActualWidth;
            double actualHeight = Application.Current.MainWindow.ActualHeight;
			FrameworkElement frameworkElement = popup.Parent as FrameworkElement;
			if (frameworkElement == null)
			{
				canvas2.Width=actualWidth;
				canvas2.Height=actualHeight;
				return;
			}
			
			Rect? boundsRelativeTo = frameworkElement.GetBoundsRelativeTo(Application.Current.MainWindow);
			if (!boundsRelativeTo.HasValue)
			{
				canvas2.Width = actualWidth;
				canvas2.Height = actualHeight;
				return;
			}
			Rect value = boundsRelativeTo.Value;
			canvas2.Margin = new Thickness(-value.Left, -value.Top, 0.0, 0.0);
			canvas2.Width = actualWidth;
			canvas2.Height = actualHeight;
		}
		public void Close(DialogResult dialogResult)
		{
            OnDialogClosed(dialogResult);
            DialogResult = ConvertFromDialogResult(dialogResult);
            Close();
			//Application.Current.Host.Content.Resized-=(new EventHandler(this.Content_Resized));
            Application.Current.MainWindow.SizeChanged -= new SizeChangedEventHandler(Content_Resized);
		}
		public void Close(bool? dialogResult)
		{
            Close(ConvertToDialogResult(dialogResult));
		}
		private bool? ConvertFromDialogResult(DialogResult dialogResult)
		{
			switch (dialogResult)
			{
			case Dialogs.DialogResult.OK:
            case Dialogs.DialogResult.Yes:
				return new bool?(true);
            case Dialogs.DialogResult.Cancel:
            case Dialogs.DialogResult.No:
				return new bool?(false);
			default:
				return default;
			}
		}
		private DialogResult ConvertToDialogResult(bool? dialogResult)
		{
			if (!dialogResult.HasValue)
			{
                return Dialogs.DialogResult.None;
			}
			if (!dialogResult.Value)
			{
                return Dialogs.DialogResult.Cancel;
			}
            return Dialogs.DialogResult.OK;
		}
		private static bool InvertMatrix(ref Matrix matrix)
		{
			double num = matrix.M11 * matrix.M22 - matrix.M12 * matrix.M21;
			if (num.Equals(0.0))
			{
				return false;
			}
			Matrix matrix2 = matrix;
			matrix.M11=matrix2.M22 / num;
			matrix.M12=-1.0 * matrix2.M12 / num;
			matrix.M21=-1.0 * matrix2.M21 / num;
			matrix.M22=matrix2.M11 / num;
			matrix.OffsetX=(matrix2.OffsetY * matrix2.M21 - matrix2.OffsetX * matrix2.M22) / num;
			matrix.OffsetY=(matrix2.OffsetX * matrix2.M12 - matrix2.OffsetY * matrix2.M11) / num;
			return true;
		}
		protected virtual void OnDialogClosed(DialogResult dialogResult)
		{
			if (DialogClosed == null)
			{
				return;
			}
            DialogClosed.Invoke(this, new DialogClosedEventArgs(dialogResult));
		}
        //private void HideHtmlViewers()
        //{
        //    this.collapsedHtmlControls.Clear();
        //    IEnumerable<FrameworkElement> enumerable = UIElementExtensions.FindControls(typeof(RadHtmlPlaceholder), Application.Current.RootVisual as FrameworkElement);
        //    using (IEnumerator<FrameworkElement> enumerator = enumerable.GetEnumerator())
        //    {
        //        while (enumerator.MoveNext())
        //        {
        //            RadHtmlPlaceholder control = (RadHtmlPlaceholder)enumerator.Current;
        //            this.HideHtmlControl(control);
        //        }
        //    }
        //}
        //private void HideHtmlControl(RadHtmlPlaceholder control)
        //{
        //    string styleAttribute = control.HtmlPresenter.GetStyleAttribute("display");
        //    if (styleAttribute == "none")
        //    {
        //        return;
        //    }
        //    control.HtmlPresenter.SetStyleAttribute("display-old", styleAttribute);
        //    control.HtmlPresenter.SetStyleAttribute("display", "none");
        //    this.collapsedHtmlControls.Add(control);
        //}
        //private void ShowHtmlViewers()
        //{
        //    using (List<RadHtmlPlaceholder>.Enumerator enumerator = this.collapsedHtmlControls.GetEnumerator())
        //    {
        //        while (enumerator.MoveNext())
        //        {
        //            RadHtmlPlaceholder current = enumerator.Current;
        //            this.ShowHtmlControl(current);
        //        }
        //    }
        //    this.collapsedHtmlControls.Clear();
        //}
        //private void ShowHtmlControl(RadHtmlPlaceholder control)
        //{
        //    control.HtmlPresenter.SetStyleAttribute("display", "block");
        //}
	}
}
