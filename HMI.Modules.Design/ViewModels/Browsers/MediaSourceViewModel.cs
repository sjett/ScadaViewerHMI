// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Presentation.ViewModels;
using HMI.Modules.Design.Resources;
using System.IO;

namespace HMI.Modules.Design.ViewModels.Browsers
{
	public abstract class MediaSourceViewModel : ViewModel, IMediaSourceViewModel, IBrowserItemViewModel
	{
		private readonly string _itemText;
		
		public string Name
		{
			get;
			private set;
		}
		public string FullPath
		{
			get;
			private set;
		}
		string IBrowserItemViewModel.ItemText
		{
			get
			{
				return this._itemText;
			}
		}
		protected MediaSourceViewModel(string mediaFileName)
		{
			if (!string.IsNullOrWhiteSpace(mediaFileName))
			{
				this.Name = Path.GetFileName(mediaFileName);
				this.FullPath = mediaFileName;
				this._itemText = this.Name;
				return;
			}
			this.Name = BrowserTexts.NoItemSelected;
			this.FullPath = null;
			this._itemText = null;
		}
	}
}
