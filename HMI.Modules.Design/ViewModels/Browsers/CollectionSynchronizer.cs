// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;
using Microsoft.CSharp.RuntimeBinder;
using System.Collections;
using System.Runtime.CompilerServices;

namespace HMI.Modules.Design.ViewModels.Browsers
{
	/// <summary>
	/// 将集合编辑器中的顺序和内容变更同步到原始集合。
	/// </summary>
	public static class CollectionSynchronizer
	{
        static CallSite<Action<CallSite, object, int, int>> moveItemCallSite;

        public static void UpdateOriginalCollection(this IEnumerable<ICollectionEditorItem> editedCollection, IList originalCollection)
		{
			if (originalCollection == null)
			{
				return;
			}

			ICollectionItem[] editedItems = editedCollection.Select(t=> t.Item).ToArray<ICollectionItem>();

			RemoveDeletedItems(editedItems, originalCollection);
			ApplyEditedItemOrder(editedItems, originalCollection);
		}

        static void ApplyEditedItemOrder(IEnumerable<ICollectionItem> editedItems, IList originalCollection)
        {
            int targetIndex = 0;
            foreach (ICollectionItem editedItem in editedItems)
            {
                UpdateOrInsertItem(editedItem, originalCollection, targetIndex);
                targetIndex++;
            }
        }

        static void UpdateOrInsertItem(ICollectionItem editedItem, IList originalCollection, int targetIndex)
        {
            var originalItem = originalCollection
                                .Cast<ICollectionItem>()
                                .SingleOrDefault(item => item.Id == editedItem.Id);

            if (originalItem == null)
            {
                InsertNewItem(editedItem, targetIndex, originalCollection);
                return;
            }

            UpdateExistingItemAndMove(editedItem, originalItem, targetIndex, originalCollection);
        }
        static void InsertNewItem(ICollectionItem editedItem, int targetIndex, IList originalCollection)
        {
            originalCollection.Insert(targetIndex, editedItem);
        }
        static void UpdateExistingItemAndMove(ICollectionItem editedItem, ICollectionItem originalItem, int targetIndex, IList originalCollection)
        {
            editedItem.DeepCopyFields(originalItem);
            int currentIndex = originalCollection.IndexOf(originalItem);
            MoveItem(targetIndex, originalCollection, currentIndex);
        }
        static void MoveItem(int targetIndex, IList collection, int currentIndex)
        {
            try
            {
                if (moveItemCallSite == null)
                {
                    moveItemCallSite =
                        CallSite<Action<CallSite, object, int, int>>
                        .Create(
                            Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(
                                CSharpBinderFlags.ResultDiscarded,
                                "Move",
                                null,
                                typeof(CollectionSynchronizer),
                                new CSharpArgumentInfo[]
                                {
                                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
                                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
                                }));
                }
                moveItemCallSite.Target(moveItemCallSite, collection, currentIndex, targetIndex);
            }
            catch (RuntimeBinderException)
            {
                throw new InvalidOperationException("Collection must have a Move method!");
            }
        }
        static void RemoveDeletedItems(IEnumerable<ICollectionItem> editedItems, IList originalCollection)
        {
            HashSet<Guid> editedItemIds = new HashSet<Guid>(editedItems.Select(item => item.Id));

            for (int index = originalCollection.Count - 1; index >= 0; index--)
            {
                RemoveItemIfDeleted(originalCollection, editedItemIds, index);
            }
        }
        static void RemoveItemIfDeleted(IList originalCollection, HashSet<Guid> editedItemIds, int index)
        {
            var originalItem = (ICollectionItem)originalCollection[index];
            if (!editedItemIds.Contains(originalItem.Id))
            {
                originalCollection.RemoveAt(index);
            }
        }

	}
}
