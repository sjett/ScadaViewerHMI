// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.General.ViewModels
{
	public class ImageViewModel : CommonControlViewModel
	{
		private IProjectManagerService projectManagerService;
		private string source;
		private System.Windows.Media.Stretch stretch;
		private System.Windows.Media.Imaging.BitmapImage image;

		public event System.EventHandler<ValueChangedEventArgs<string>> SourceChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Stretch>> StretchChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Imaging.BitmapImage>> ImageChanged;

		[Browsable(true), MediaBrowser(MediaType.Image), PropertyCategory("Image", typeof(StandardPropertyCategories))]
		public string Source
		{
			get
			{
				return this.source;
			}
			set
			{
				if (this.source != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.source, value);
					base.RaisePropertyChanging<string>(() => this.Source);
					this.source = value;
					this.SourceChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.Source);
				}
			}
		}
		[Browsable(true), PropertyCategory("Image", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Stretch Stretch
		{
			get
			{
				return this.stretch;
			}
			set
			{
				if (this.stretch != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Stretch> e = new ValueChangedEventArgs<System.Windows.Media.Stretch>(this.stretch, value);
					base.RaisePropertyChanging<System.Windows.Media.Stretch>(() => this.Stretch);
					this.stretch = value;
					this.StretchChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Stretch>(() => this.Stretch);
				}
			}
		}
		[Browsable(false)]
		public System.Windows.Media.Imaging.BitmapImage Image
		{
			get
			{
				return this.image;
			}
			private set
			{
				if (this.image != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Imaging.BitmapImage>(() => this.Image);
					ValueChangedEventArgs<System.Windows.Media.Imaging.BitmapImage> e = new ValueChangedEventArgs<System.Windows.Media.Imaging.BitmapImage>(this.image, value);
					this.image = value;
					this.ImageChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Imaging.BitmapImage>(() => this.Image);
				}
			}
		}
		public ImageViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType, IProjectManagerService projectManagerService) : base(container, parentValue, controlType)
		{
			this.projectManagerService = projectManagerService;
			this.SourceChanged += new System.EventHandler<ValueChangedEventArgs<string>>(this.ImageViewModel_SourceChanged);
		}
		private void ImageViewModel_SourceChanged(object sender, ValueChangedEventArgs<string> e)
		{
			if (string.IsNullOrEmpty(e.NewValue))
			{
				this.Image = null;
				return;
			}
			this.Image = this.ReadImage(e);
		}
		private System.Windows.Media.Imaging.BitmapImage ReadImage(ValueChangedEventArgs<string> e)
		{
			System.Windows.Media.Imaging.BitmapImage result;
			try
			{
				string uriString = System.IO.Path.Combine(this.Project.ProjectImagesFolder, e.NewValue);
				System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.UriSource = new Uri(uriString);
				bitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
				bitmapImage.EndInit();
				bitmapImage.Freeze();
				result = bitmapImage;
			}
			catch
			{
				result = null;
			}
			return result;
		}
		public override string GetXamlControlName()
		{
			return "Image";
		}
		public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		{
			return Enumerable.Empty<AssemblyReference>();
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return Enumerable.Empty<string>();
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("Source", ProjectFilesResolver.Resolve(this.Source, MediaType.Image));
			base.AddXamlAttribute("Stretch", this.Stretch);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
		}
	}
}
