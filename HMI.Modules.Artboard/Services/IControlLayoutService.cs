// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Artboard.Services
{
	public interface IControlLayoutService
	{
		void MakeSameWidth(ICommonControlViewModel firstItem, IEnumerable<ICommonControlViewModel> selectedItems);
		void MakeSameHeight(ICommonControlViewModel firstItem, IEnumerable<ICommonControlViewModel> selectedItems);
		void AlignLeft(ICommonControlViewModel referenceItem, IEnumerable<ICommonControlViewModel> selectedItems);
		void AlignCenter(ICommonControlViewModel referenceItem, IEnumerable<ICommonControlViewModel> selectedItems);
		void AlignRight(ICommonControlViewModel referenceItem, IEnumerable<ICommonControlViewModel> selectedItems);
		void AlignTop(ICommonControlViewModel referenceItem, IEnumerable<ICommonControlViewModel> selectedItems);
		void AlignMiddle(ICommonControlViewModel referenceItem, IEnumerable<ICommonControlViewModel> selectedItems);
		void AlignBottom(ICommonControlViewModel referenceItem, IEnumerable<ICommonControlViewModel> selectedItems);
		void DistributeHorizontally(ICommonControlViewModel referenceItem, IEnumerable<ICommonControlViewModel> selectedItems);
		void DistributeVertically(ICommonControlViewModel referenceItem, IEnumerable<ICommonControlViewModel> selectedItems);
	}
}
