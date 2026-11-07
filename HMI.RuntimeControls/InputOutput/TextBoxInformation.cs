// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.RuntimeControls.InputOutput
{
	public class TextBoxInformation
	{
		public bool IsInEditMode
		{
			get;
			set;
		}
		public string Text
		{
			get;
			set;
		}
		public int SelectionStart
		{
			get;
			set;
		}
		public int SelectionLength
		{
			get;
			set;
		}
		public int Precision
		{
			get;
			set;
		}
	}
}
