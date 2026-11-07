// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.ObjectModel;
namespace HMI.Modules.ControlTree
{
	[Obsolete]
	public interface IPrimitivesViewModel
	{
		ObservableCollection<PrimitiveModel> Primitives
		{
			get;
		}
	}
}
