// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Core.Helper.Extensions;
using HMI.Runtime.Shared.Common;
using HMI.Runtime.Shared.Dialogs;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace HMI.RuntimeControls.General
{
	public class HMILogFileViewer : Button, IDisposable
	{
		private const string TraceFileName = "HMITrace.txt";

        private readonly DialogManager dialogManager;

		public static readonly DependencyProperty PopupModalityProperty = DependencyProperty.Register("PopupModality", typeof(DialogModality), typeof(HMILogFileViewer), new PropertyMetadata(DialogModality.Modeless, new PropertyChangedCallback(HMILogFileViewer.OnPopupModalityPropertyChanged)));
		
		[Category("HMI")]
		public DialogModality PopupModality
		{
			get
			{
				return (DialogModality)base.GetValue(HMILogFileViewer.PopupModalityProperty);
			}
			set
			{
				base.SetValue(HMILogFileViewer.PopupModalityProperty, value);
			}
		}
		private static void OnPopupModalityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		public HMILogFileViewer()
		{
			base.DefaultStyleKey=(typeof(HMILogFileViewer));
			this.dialogManager = new DialogManager(new Func<object>(this.CreateDialogContent), null, null);
		}
		~HMILogFileViewer()
		{
			this.Dispose(false);
		}
		private void HMIControl_MouseEnter(object sender, MouseEventArgs e)
		{
			base.Cursor=(Cursors.Hand);
		}
		private void HMIControl_MouseLeave(object sender, MouseEventArgs e)
		{
			base.Cursor=(Cursors.Arrow);
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			if (DesignerPropertiesExt.IsInDesignTool)
			{
				return;
			}
			this.RegisterMouseHandlers();
		}
		protected override void OnClick()
		{
			base.OnClick();
			this.HandleClick();
		}
		private void RegisterMouseHandlers()
		{
			base.MouseEnter+=(new MouseEventHandler(this.HMIControl_MouseEnter));
			base.MouseLeave+=(new MouseEventHandler(this.HMIControl_MouseLeave));
		}
		private void HandleClick()
		{
			this.dialogManager.Show("日志文件查看器", this.PopupModality);
		}
		private object CreateDialogContent()
		{
			string isolatedStorageFileContent = this.GetIsolatedStorageFileContent();
			TextBox textBox = new TextBox();
			textBox.Height=(500.0);
			textBox.Width=(800.0);
			textBox.IsReadOnly=(true);
            textBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
			textBox.VerticalScrollBarVisibility= ScrollBarVisibility.Auto;
			textBox.Margin = (new Thickness(5.0));
			textBox.Text=(isolatedStorageFileContent);
			TextBox textBox2 = textBox;
			textBox2.Focus();
			return textBox2;
		}
		private string GetIsolatedStorageFileContent()
		{
			IsolatedStorageFile isolatedStorageFile = null;
			IsolatedStorageFileStream isolatedStorageFileStream = null;
			StreamReader streamReader = null;
			string result = string.Empty;
			try
			{
				isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
				if (isolatedStorageFile.FileExists(TraceFileName))
				{
					isolatedStorageFileStream = isolatedStorageFile.OpenFile(TraceFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					streamReader = new StreamReader(isolatedStorageFileStream);
					result = streamReader.ReadToEnd();
				}
			}
			catch (Exception ex)
			{
				result = ex.ToString();
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
					streamReader.Dispose();
				}
				if (isolatedStorageFileStream != null)
				{
					isolatedStorageFileStream.Close();
					isolatedStorageFileStream.Dispose();
				}
				if (isolatedStorageFile != null)
				{
					isolatedStorageFile.Dispose();
				}
			}
			return result;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			this.DisposeMouseHandlers();
			this.StopStoryboards();
		}
		private void DisposeMouseHandlers()
		{
			base.MouseEnter-=(new MouseEventHandler(this.HMIControl_MouseEnter));
			base.MouseLeave-=(new MouseEventHandler(this.HMIControl_MouseLeave));
		}
	}
}
