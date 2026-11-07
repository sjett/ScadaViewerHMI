// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.ObjectModel;
namespace HMI.Modules.ControlTree
{
	[Obsolete]
	public class PrimitivesViewModel : IPrimitivesViewModel
	{
		private readonly ObservableCollection<PrimitiveModel> _primitives = new ObservableCollection<PrimitiveModel>();
		public ObservableCollection<PrimitiveModel> Primitives
		{
			get
			{
				return _primitives;
			}
		}
	}
}
