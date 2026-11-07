// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Domain.Models;
using HMI.Modules.Design.Views;
namespace HMI.Modules.Design.Attributes
{
	public class MediaBrowserAttribute : PropertyEditorAttribute
	{
		private readonly MediaType _mediaType;
		public MediaBrowserAttribute(MediaType mediaType)
		{
			this._mediaType = mediaType;
		}
		protected override IEditorView BuildEditor(IProperty property, IBindingSource editedObject)
		{
			switch (this._mediaType)
			{
			case MediaType.Image:
				return new ImageSourceEditorView();
			case MediaType.Audio:
				return new AudioSourceEditorView();
			}
			return new XamlSourceEditorView();
		}
		public override bool IsMultiSelectionCompatibleWith(IProperty property, IProperty otherProperty)
		{
			var mediaBrowserAttribute = otherProperty.PropertyEditorBuilder as MediaBrowserAttribute;
			return mediaBrowserAttribute != null && mediaBrowserAttribute._mediaType == this._mediaType;
		}
	}
}
