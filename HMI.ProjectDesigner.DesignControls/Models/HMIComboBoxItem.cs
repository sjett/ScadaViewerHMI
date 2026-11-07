// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using System.ComponentModel;
namespace HMI.Infrastructure.Models
{
	public class HMIComboBoxItem : CollectionItem
	{
		private readonly IContentReplacementService contentReplacementService;
		private readonly IProjectViewModel project;
		private readonly IProjectManagerService projectManagerService;
		private string symbolicText;
		private int value;
		private int height = 35;
		private System.Windows.Media.Imaging.BitmapImage image;
		private System.Windows.Media.Stretch stretch = System.Windows.Media.Stretch.Uniform;
		private string source;
		private string imageSource;

		public event System.EventHandler<ValueChangedEventArgs<string>> SymbolicTextChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> ValueChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> HeightChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Imaging.BitmapImage>> ImageChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Stretch>> StretchChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SourceChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> ImageSourceChanged;

		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string SymbolicText
		{
			get
			{
				return this.symbolicText;
			}
			set
			{
				if (this.symbolicText != value)
				{
					base.RaisePropertyChanging<string>(() => this.SymbolicText);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.symbolicText, value);
					this.symbolicText = value;
					this.SymbolicTextChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.SymbolicText);
					base.UpdateEditorDisplayText();
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories))]
		public int Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (this.value != value)
				{
					base.RaisePropertyChanging<int>(() => this.Value);
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.value, value);
					this.value = value;
					this.ValueChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.Value);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories)), MinimumValue(0.0)]
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				if (this.height != value)
				{
					base.RaisePropertyChanging<int>(() => this.Height);
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.height, value);
					this.height = value;
					this.HeightChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.Height);
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
		[Browsable(false)]
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
		[Browsable(true), MediaBrowser(MediaType.Image), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
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
		[Browsable(false), PropertyCategory("Image", typeof(StandardPropertyCategories))]
		public string ImageSource
		{
			get
			{
				return this.imageSource;
			}
			set
			{
				if (this.imageSource != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.imageSource, value);
					base.RaisePropertyChanging<string>(() => this.ImageSource);
					this.imageSource = value;
					this.ImageSourceChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ImageSource);
				}
			}
		}
		public HMIComboBoxItem(IServiceProvider services, IExplorerNodeViewModel parentNode) : base(services, parentNode)
		{
			this.contentReplacementService = services.Resolve<IContentReplacementService>();
			this.projectManagerService = services.Resolve<IProjectManagerService>();
			this.project = parentNode.Project;
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
				System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
				bitmapImage.BeginInit();
				string uriString = System.IO.Path.Combine(this.project.ProjectImagesFolder, e.NewValue);
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
		public override void DeepCopyFields(ICollectionItem target)
		{
			HMIComboBoxItem wFComboBoxItem = target as HMIComboBoxItem;
			wFComboBoxItem.SymbolicText = this.SymbolicText;
			wFComboBoxItem.Value = this.Value;
			wFComboBoxItem.Height = this.Height;
			wFComboBoxItem.Image = this.Image;
			wFComboBoxItem.Stretch = this.Stretch;
			wFComboBoxItem.Source = this.Source;
			wFComboBoxItem.ImageSource = this.ImageSource;
		}
		public override ICollectionItem CreateClone()
		{
			return new HMIComboBoxItem(base.Services, base.Parent);
		}
		public override string GetXaml()
		{
			this.ImageSource = ProjectFilesResolver.Resolve(this.Source, MediaType.Image);
			string text = string.Format("<{0}:{1} SymbolicText=\"[SymbolicText]\" ImageSource=\"[ImageSource]\" Value=\"[Value]\" Height=\"[Height]\"></{0}:{1}>", "HMIButtons", "HMIComboBoxItem");
			return this.contentReplacementService.ReplaceStringContent(text, this);
		}
		public override string ToString()
		{
			return this.SymbolicText ?? string.Empty;
		}
	}
}
