// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics;
using System.Reflection;
using System.Security;

namespace HMI.Infrastructure.Common
{
    public static class ExceptionExtensions
	{
		[DebuggerHidden]
		public static void RethrowWithNoStackTraceLoss(this Exception exception)
		{
            FieldInfo fieldInfo = typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);
			fieldInfo.SetValue(exception, exception.StackTrace);
			throw exception;
		}
	}
}
