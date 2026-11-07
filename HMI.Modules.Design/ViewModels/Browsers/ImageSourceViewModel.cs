// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.ViewModels;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	public class ImageSourceViewModel : MediaSourceViewModel, IMediaSourceViewModel, IBrowserItemViewModel, IImageSourceViewModel
	{
		private BitmapImage _bitmapSource;
		public ImageSource BitmapSource
		{
			get
			{
				if (this._bitmapSource == null)
				{
					this.LoadBitmapSource();
				}
				return this._bitmapSource;
			}
		}
		public ImageSourceViewModel(string imageFileName) : base(imageFileName)
		{
		}
		private void LoadBitmapSource()
		{
			this._bitmapSource = LoadBitmapSource(base.FullPath);
		}

        private BitmapImage LoadBitmapSource(string filePath)
        {
            try
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(filePath);
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return null;
            }
        }
    }
}
