// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using HMI.Common.Browsers.ViewModels;
using HMI.Infrastructure.Services;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	internal class XamlSourceViewModel : MediaSourceViewModel, IMediaSourceViewModel, IXamlSourceViewModel, IBrowserItemViewModel
	{
		private readonly IFileSystemService _fileSystemService;
		private bool _isXamlLoaded;
		private UIElement _xamlSource;
		public UIElement XamlSource
		{
			get
			{
				if (!this._isXamlLoaded)
				{
					this.LoadXamlSource();
				}
				return this._xamlSource;
			}
		}
		public bool IsXamlLoaded
		{
			get
			{
				return this._isXamlLoaded;
			}
		}
		public bool IsValidXamlSource
		{
			get
			{
				return this.XamlSource != null;
			}
		}
		public XamlSourceViewModel(string xamlFileName, IFileSystemService fileSystemService) : base(xamlFileName)
		{
			this._fileSystemService = fileSystemService;
		}
		private void LoadXamlSource()
		{
			try
			{
				using (Stream stream = this._fileSystemService.OpenRead(base.FullPath))
				{
					this._xamlSource = (UIElement)XamlReader.Load(stream);
				}
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
				this._xamlSource = null;
			}
			this._isXamlLoaded = true;
		}
	}
}
