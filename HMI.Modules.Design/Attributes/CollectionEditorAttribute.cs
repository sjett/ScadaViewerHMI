// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Modules.Design.Views;
using System.ComponentModel;

namespace HMI.Modules.Design.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class CollectionEditorAttribute : PropertyEditorAttribute
	{
		private readonly Type itemType;
		
		public bool ExpandAllGroups
		{
			get;
			set;
		}
		public bool IsGroupingEnabled
		{
			get;
			set;
		}
		public string GroupBy
		{
			get;
			set;
		}
		public ListSortDirection? GroupSortDirection
		{
			get;
			set;
		}
		public Type[] AvailableTypes
		{
			get;
			set;
		}
		public bool UseHorizontalEditing
		{
			get;
			set;
		}
		public bool IsCollectionReadOnly
		{
			get;
			set;
		}
		public CollectionEditorAttribute(Type defaultItemType)
		{
			this.itemType = defaultItemType;
		}
		protected override IEditorView BuildEditor(IProperty property, IBindingSource editedObject)
		{
			CollectionEditorView collectionEditorView = this.CreateCollectionEditor();
			collectionEditorView.IsGroupingEnabled = this.IsGroupingEnabled;
			collectionEditorView.ExpandAllGroups = this.ExpandAllGroups;
			collectionEditorView.GroupBy = this.GroupBy;
			collectionEditorView.GroupSortDirection = this.GroupSortDirection;
			collectionEditorView.IsCollectionReadOnly = this.IsCollectionReadOnly;
			collectionEditorView.DefaultItemType = this.itemType;
			collectionEditorView.AvailableTypes = this.AvailableTypes;
			return collectionEditorView;
		}
		protected CollectionEditorView CreateCollectionEditor()
		{
			if (this.UseHorizontalEditing)
			{
				return new HorizontalCollectionEditorView();
			}
			return new VerticalCollectionEditorView();
		}
		public override bool IsMultiSelectionCompatibleWith(IProperty property, IProperty otherProperty)
		{
			return false;
		}
		protected override void AssignBindings(IEditorView editor, IProperty property, IBindingSource bindingSource)
		{
			base.AssignBindings(editor, property, bindingSource);
			((CollectionEditorView)editor).BindingSource = bindingSource;
		}
	}
}
