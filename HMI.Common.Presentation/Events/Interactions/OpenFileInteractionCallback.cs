// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Presentation.Events.Interactions
{
	public delegate void OpenFileInteractionCallback(bool isFileSelected, IEnumerable<string> selectedFiles, object state);
}
