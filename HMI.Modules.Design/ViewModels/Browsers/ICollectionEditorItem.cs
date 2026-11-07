// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	public interface ICollectionEditorItem : INotifyProperty
	{
		string Key
		{
			get;
			set;
		}
		string DisplayText
		{
			get;
			set;
		}
		ICollectionItem Item
		{
			get;
			set;
		}
	}
}
