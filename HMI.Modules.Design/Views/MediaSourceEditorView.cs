// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage]
	public abstract class MediaSourceEditorView : BrowserEditorView
	{
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (base.Editor != null)
			{
				base.Editor.IsEnabled = false;
			}
		}
	}
}
