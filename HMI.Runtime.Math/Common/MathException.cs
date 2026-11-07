// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.MathExpressions.Common
{
	public class MathException : Exception
	{
		public MathException()
		{
		}
		public MathException(string message) : base(message)
		{
		}
		public MathException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
