// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Collections;
using HMI.Domain.Models;
using System.Windows.Input;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	public interface ICollectionBrowserViewModel : IBrowserViewModel
	{
		Type DefaultItemType
		{
			get;
			set;
		}
		bool IsCollectionReadOnly
		{
			get;
			set;
		}
		ICommand DeleteItemCommand
		{
			get;
		}
		ICommand MoveItemUpCommand
		{
			get;
		}
		ICommand MoveItemDownCommand
		{
			get;
		}
		ICommand AddNewItemCommand
		{
			get;
		}
		ICommand DuplicateItemCommand
		{
			get;
		}
		bool IsCollectionEditable
		{
			get;
		}
		IObservableCollection<ICollectionEditorItem> Items
		{
			get;
		}
		ICollectionEditorItem SelectedItem
		{
			get;
			set;
		}
		bool IsGroupingEnabled
		{
			get;
			set;
		}
		bool ExpandAllGroups
		{
			get;
			set;
		}
		string GroupBy
		{
			get;
			set;
		}
		Type[] AvailableItemTypes
		{
			get;
			set;
		}
		bool AllowDrop
		{
			get;
		}
		bool CanMoveItems
		{
			get;
		}
		void ApplyChanges();
		void CancelChanges();
		void Initialize(IBindingSource bindingSource);
	}
}
