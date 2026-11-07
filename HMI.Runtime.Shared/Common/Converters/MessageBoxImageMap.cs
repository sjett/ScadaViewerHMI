// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows.Media;
namespace HMI.Runtime.Shared.Common.Converters
{
	public class MessageBoxImageMap
	{
		public MessageBoxType MessageBoxType
		{
			get;
			set;
		}
		public ImageSource Image
		{
			get;
			set;
		}
	}
}
