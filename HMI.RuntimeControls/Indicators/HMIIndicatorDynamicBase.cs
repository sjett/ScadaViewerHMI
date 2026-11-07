// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using HMI.Runtime.Core.Helper;
namespace HMI.RuntimeControls.Indicators
{
	public class HMIIndicatorDynamicBase : HMIIndicatorBase
	{
		protected Grid? LayoutRoot;

		public static readonly DependencyProperty DefaultStateXamlPathProperty = DependencyProperty.Register("DefaultStateXamlPath", typeof(string), typeof(HMIIndicatorDynamicBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorDynamicBase.OnDefaultStateXamlPathPropertyChanged)));
		public static readonly DependencyProperty StateXamlPath1Property = DependencyProperty.Register("StateXamlPath1", typeof(string), typeof(HMIIndicatorDynamicBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorDynamicBase.OnStateXamlPath1PropertyChanged)));
		public static readonly DependencyProperty StateXamlPath2Property = DependencyProperty.Register("StateXamlPath2", typeof(string), typeof(HMIIndicatorDynamicBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorDynamicBase.OnStateXamlPath2PropertyChanged)));
		public static readonly DependencyProperty StateXamlPath3Property = DependencyProperty.Register("StateXamlPath3", typeof(string), typeof(HMIIndicatorDynamicBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorDynamicBase.OnStateXamlPath3PropertyChanged)));
		public static readonly DependencyProperty StateXamlPath4Property = DependencyProperty.Register("StateXamlPath4", typeof(string), typeof(HMIIndicatorDynamicBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorDynamicBase.OnStateXamlPath4PropertyChanged)));
		public static readonly DependencyProperty StateXamlPath5Property = DependencyProperty.Register("StateXamlPath5", typeof(string), typeof(HMIIndicatorDynamicBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorDynamicBase.OnStateXamlPath5PropertyChanged)));
		public static readonly DependencyProperty StateXamlPath6Property = DependencyProperty.Register("StateXamlPath6", typeof(string), typeof(HMIIndicatorDynamicBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorDynamicBase.OnStateXamlPath6PropertyChanged)));
		public static readonly DependencyProperty StateXamlPath7Property = DependencyProperty.Register("StateXamlPath7", typeof(string), typeof(HMIIndicatorDynamicBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorDynamicBase.OnStateXamlPath7PropertyChanged)));
		public static readonly DependencyProperty StateXamlPath8Property = DependencyProperty.Register("StateXamlPath8", typeof(string), typeof(HMIIndicatorDynamicBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIIndicatorDynamicBase.OnStateXamlPath8PropertyChanged)));
		
		[Category("HMI")]
		public string DefaultStateXamlPath
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorDynamicBase.DefaultStateXamlPathProperty);
			}
			set
			{
				base.SetValue(HMIIndicatorDynamicBase.DefaultStateXamlPathProperty, value);
			}
		}
		[Category("HMI")]
		public string StateXamlPath1
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorDynamicBase.StateXamlPath1Property);
			}
			set
			{
				base.SetValue(HMIIndicatorDynamicBase.StateXamlPath1Property, value);
			}
		}
		[Category("HMI")]
		public string StateXamlPath2
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorDynamicBase.StateXamlPath2Property);
			}
			set
			{
				base.SetValue(HMIIndicatorDynamicBase.StateXamlPath2Property, value);
			}
		}
		[Category("HMI")]
		public string StateXamlPath3
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorDynamicBase.StateXamlPath3Property);
			}
			set
			{
				base.SetValue(HMIIndicatorDynamicBase.StateXamlPath3Property, value);
			}
		}
		[Category("HMI")]
		public string StateXamlPath4
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorDynamicBase.StateXamlPath4Property);
			}
			set
			{
				base.SetValue(HMIIndicatorDynamicBase.StateXamlPath4Property, value);
			}
		}
		[Category("HMI")]
		public string StateXamlPath5
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorDynamicBase.StateXamlPath5Property);
			}
			set
			{
				base.SetValue(HMIIndicatorDynamicBase.StateXamlPath5Property, value);
			}
		}
		[Category("HMI")]
		public string StateXamlPath6
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorDynamicBase.StateXamlPath6Property);
			}
			set
			{
				base.SetValue(HMIIndicatorDynamicBase.StateXamlPath6Property, value);
			}
		}
		[Category("HMI")]
		public string StateXamlPath7
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorDynamicBase.StateXamlPath7Property);
			}
			set
			{
				base.SetValue(HMIIndicatorDynamicBase.StateXamlPath7Property, value);
			}
		}
		[Category("HMI")]
		public string StateXamlPath8
		{
			get
			{
				return (string)base.GetValue(HMIIndicatorDynamicBase.StateXamlPath8Property);
			}
			set
			{
				base.SetValue(HMIIndicatorDynamicBase.StateXamlPath8Property, value);
			}
		}
		private static void OnDefaultStateXamlPathPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
		}
		private static void OnStateXamlPath1PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
		}
		private static void OnStateXamlPath2PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
		}
		private static void OnStateXamlPath3PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
		}
		private static void OnStateXamlPath4PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
		}
		private static void OnStateXamlPath5PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
		}
		private static void OnStateXamlPath6PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
		}
		private static void OnStateXamlPath7PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
		}
		private static void OnStateXamlPath8PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
		}
		public HMIIndicatorDynamicBase()
		{
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.LayoutRoot = (base.GetTemplateChild("LayoutRoot") as Grid);
			this.DownloadXamlFiles();
		}

		protected virtual void HandleDownloadCompleted(object state, string downloadData)
		{
		}

		protected virtual void HandleXamlFile(object userToken, string xamlFileName)
		{
			if (this.LayoutRoot != null && !string.IsNullOrEmpty(xamlFileName.Trim()) )
			{
				string xamlFilePath = this.GetXamlFilePath(xamlFileName);
				this.HandleDownloadCompleted(userToken, xamlFilePath);

                return;
			}
		}
		protected virtual void DownloadXamlFiles()
		{
		}
		private string GetXamlFilePath(string fileName)
		{
			string xamlFilePath = Path.Combine("Xamls", fileName);
			
			return xamlFilePath;
		}
		protected static FrameworkElement GetBadXamlResource()
		{
			TextBlock textBlock = new TextBlock();
			textBlock.Text=("Bad Xaml provided");
			return textBlock;
		}
	}
}
