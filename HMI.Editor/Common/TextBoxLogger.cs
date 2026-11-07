// SPDX-License-Identifier: GPL-3.0-only
//using Microsoft.Build.Framework;
//using Microsoft.Build.Logging;
using System;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using HMI.Infrastructure.Services.Logging;

namespace HMI.Editor.Common
{
	public class TextBoxLogger : /*ConsoleLogger,*/ IRuntimeLogger
	{
		private TextBox textBox;
		private SynchronizationContext synchronizationContext;
		public TextBoxLogger(TextBox textBoxLogger) //: base(LoggerVerbosity.Normal)
		{
            textBox = textBoxLogger;
			//base.WriteHandler = new WriteHandler(this.Write);
            synchronizationContext = SynchronizationContext.Current;
		}
		//private void Write(string text)
		//{
  //          AppendText(text);
		//}
		private void AppendText(string message)
		{
            synchronizationContext.Post(delegate(object state)
			{
                textBox.AppendText(message);
                textBox.ScrollToEnd();
			}, null);
		}
		public void LogException(Exception exception)
		{
            StringBuilder stringBuilder = new StringBuilder();
			while (exception != null)
			{
				stringBuilder.Append(string.Format("Source: {0}\nType: {1}\nMessage: {2}\nStack Trace: {3}\n\n", new object[]
				{
					exception.Source,
					exception.GetType().Name,
					exception.Message,
					exception.StackTrace
				}));
				exception = exception.InnerException;
			}
			stringBuilder.AppendLine();
            AppendText(stringBuilder.ToString());
		}
	}
}
