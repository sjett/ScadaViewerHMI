// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Common.Presentation.Events.Interactions
{
	public delegate void SaveFileInteractionCallback(bool isFileSaved, string fileName, object state);
}
