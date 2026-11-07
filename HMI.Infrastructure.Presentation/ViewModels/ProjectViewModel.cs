// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Common.Presentation.Events;
using HMI.Common.Presentation.ViewModels;
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Infrastructure.Presentation.Events;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using NLog;
using System.Collections.ObjectModel;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.IO;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Media;
using System.ComponentModel.DataAnnotations;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    public class ProjectViewModel : NodeViewModel, IExplorerNodeViewModel, IHierarchicalNode, INotifyPropertyChanged, IDisposable, IProjectViewModel, INotifyProperty, INotifyPropertyChanging, IViewModel, IProject
	{
		private readonly IFileSystemService fileSystemService;
		private readonly IMediaService mediaService;

		private readonly ObservableCollection<string> _projectImages = new ObservableCollection<string>();
		private readonly ObservableCollection<string> _projectMedia = new ObservableCollection<string>();
		private readonly ObservableCollection<string> _projectXamls = new ObservableCollection<string>();

		private double _fastBlinkSpeed = 0.5;
		private Color _appPageBackground = Colors.White;
		private bool _isModified;
		private double _mediumBlinkSpeed = 1.0;
		private Brush _pageDefaultBackground = (Brush)Brushes.White.GetAsFrozen();
		private double _slowBlinkSpeed = 2.0;
		private Brush _stateDefaultBackground = (Brush)new SolidColorBrush(Colors.LightGray).GetAsFrozen();
		private Brush _stateDefaultForeground = (Brush)new SolidColorBrush(Colors.Black).GetAsFrozen();

		private readonly IObservableCollection<IProjectPageViewModel> _pages = new FullyObservableCollection<IProjectPageViewModel>();
		private bool _stretchPages;

		private string _serverName = "";
		private string _host = "localhost";
		//private HttpProtocol _protocol;
		private int _port = 10000;
		private int _timeout = 10000;
		private string _username = "ScadaHmi";
		private string _password = "B9D0F030265751578B00E9801C69320C";
		private string _secretkey = "0E063D581B1DB27B8E984993DED65CE99284A706756C706C7F3B12C44BD55D4D";


        public event EventHandler? ServerSettingsChanged;

		public event EventHandler<ValueChangedEventArgs<bool>>? StretchPagesChanged;

		public event EventHandler<ValueChangedEventArgs<double>>? FastBlinkSpeedChanged;

		public event EventHandler<ValueChangedEventArgs<double>>? MediumBlinkSpeedChanged;

		public event EventHandler<ValueChangedEventArgs<double>>? SlowBlinkSpeedChanged;

		public event EventHandler<ValueChangedEventArgs<string>>? ServerNameChanged;

		public event EventHandler<ValueChangedEventArgs<string>>? HostChanged;

        public event EventHandler<ValueChangedEventArgs<Color>>? HtmlPageBackgroundChanged;

		[Browsable(false)]
		public ICollectionMonitor<IProjectPageViewModel> PagesMonitor
		{
			get;
			private set;
		}
		[Browsable(false)]
		public IActionMonitor ActionMonitor
		{
			get;
			private set;
		}
		[Browsable(false)]
		public string BinFolder
		{
			get
			{
				return AppProjectFolder + "\\Bin";
			}
		}
		[Browsable(false)]
		public override IProjectViewModel Project
		{
			get
			{
				return this;
			}
		}
		[Browsable(false)]
		public string BuildFolder
		{
			get
			{
				return AppProjectFolder + "\\Build";
			}
		}
		[Browsable(false)]
		[Obsolete]
		public string ClientBinFolder
		{
			get
			{
				return AppProjectFolder + "\\ClientBin";
			}
		}
		[Browsable(false)]
		public string DebugFolder
		{
			get
			{
				return BuildFolder + "\\Debug";
			}
		}
		[Browsable(false)]
		public IObservableCollection<IProjectPageViewModel> Pages
		{
			get
			{
				return _pages;
			}
		}
		[Browsable(false)]
		public IEnumerable<ITemplatePageViewModel> TemplatePages
		{
			get
			{
				return Pages.OfType<ITemplatePageViewModel>();
			}
		}
		[ReadOnly(true)]
		public string ProjectFolder
		{
			get;
			set;
		}
		[Browsable(false)]
		public ObservableCollection<string> ProjectImages
		{
			get
			{
				return _projectImages;
			}
		}
		[Browsable(false)]
		public ObservableCollection<string> ProjectMedia
		{
			get
			{
				return _projectMedia;
			}
		}
		[Browsable(false)]
		public ObservableCollection<string> ProjectXamls
		{
			get
			{
				return _projectXamls;
			}
		}
		[Browsable(false)]
		public string ProjectImagesFolder
		{
			get;
			private set;
		}
		[Browsable(false)]
		public string ProjectXamlsFolder
		{
			get;
			private set;
		}
		[Browsable(false)]
		public string ProjectMediaFolder
		{
			get;
			private set;
		}
		[Browsable(false)]
		public string ProjectName
		{
			get
			{
				return ID;
			}
			private set
			{
                ID = value;
			}
		}
		[Browsable(false)]
		public string OldProjectName
		{
			get;
			private set;
		}
		[Browsable(false)]
		public override bool IsEditable
		{
			get
			{
				return false;
			}
		}
		[Browsable(false)]
		public string ProjectNamespace
		{
			get;
			private set;
		}
		[Browsable(false)]
		public string ProjectTemplatesFolder
		{
			get;
			private set;
		}
		[Browsable(false)]
		public string ProjectTitle
		{
			get;
			set;
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool StretchPages
		{
			get
			{
				return _stretchPages;
			}
			set
			{
				if (_stretchPages != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(_stretchPages, value);
                    _stretchPages = value;
                    StretchPagesChanged?.Invoke(this, e);
                    RaisePropertyChanged(() => StretchPages);
				}
			}
		}
		[ReadOnly(true)]
        public string AppProjectFolder
		{
			get
			{
				return System.IO.Path.Combine(ProjectFolder, AppProjectName);
			}
		}
		[Browsable(false)]
        public string AppProjectName
		{
			get
			{
				return ProjectName + ".App";
			}
		}

		[Browsable(false)]
        public string HMISilverlightControlsZipFile
		{
			get;
			private set;
		}

		[Browsable(false)]
		public ILogger Logger
		{
			get;
			internal set;
		}
		[Browsable(false)]
		public bool IsModified
		{
			get
			{
				return _isModified;
			}
			set
			{
                _isModified = value;
                RaisePropertyChanged(() => IsModified);
			}
		}
		[ReadOnly(true)]
		public string ProjectFullPath
		{
			get
			{
				return System.IO.Path.Combine(ProjectFolder, ProjectName + ".Project");
			}
		}
		[DisplayName("StateDefaultBackground"), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public Brush StateDefaultBackground
		{
			get
			{
				return _stateDefaultBackground;
			}
			set
			{
				if (_stateDefaultBackground != value)
				{
                    RaisePropertyChanging(() => StateDefaultBackground);
                    _stateDefaultBackground = value;
                    RaisePropertyChanged(() => StateDefaultBackground);
				}
			}
		}
		[DisplayName("StateDefaultForeground"), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public Brush StateDefaultForeground
		{
			get
			{
				return _stateDefaultForeground;
			}
			set
			{
				if (_stateDefaultForeground != value)
				{
                    RaisePropertyChanging(() => StateDefaultForeground);
                    _stateDefaultForeground = value;
                    RaisePropertyChanged(() => StateDefaultForeground);
				}
			}
		}
		[DisplayName("StateFastBlinkSpeed(s)"), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public double FastBlinkSpeed
		{
			get
			{
				return _fastBlinkSpeed;
			}
			set
			{
				if (_fastBlinkSpeed != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(_fastBlinkSpeed, value);
                    RaisePropertyChanging(() => FastBlinkSpeed);
                    _fastBlinkSpeed = value;
                    FastBlinkSpeedChanged?.Invoke(this, e);
                    RaisePropertyChanged(() => FastBlinkSpeed);
				}
			}
		}
		[DisplayName("StateMediumBlinkSpeed(s)"), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public double MediumBlinkSpeed
		{
			get
			{
				return _mediumBlinkSpeed;
			}
			set
			{
				if (_mediumBlinkSpeed != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(_mediumBlinkSpeed, value);
                    RaisePropertyChanging(() => MediumBlinkSpeed);
                    _mediumBlinkSpeed = value;
                    MediumBlinkSpeedChanged?.Invoke(this, e);
                    RaisePropertyChanged(() => MediumBlinkSpeed);
				}
			}
		}
		[DisplayName("StateSlowBlinkSpeed(s)"), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public double SlowBlinkSpeed
		{
			get
			{
				return _slowBlinkSpeed;
			}
			set
			{
				if (_slowBlinkSpeed != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(_slowBlinkSpeed, value);
                    RaisePropertyChanging(() => SlowBlinkSpeed);
                    _slowBlinkSpeed = value;
                    SlowBlinkSpeedChanged?.Invoke(this, e);
                    RaisePropertyChanged(() => SlowBlinkSpeed);
				}
			}
		}
		[DisplayName("PageDefaultBackground"), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush PageDefaultBackground
		{
			get
			{
				return _pageDefaultBackground;
			}
			set
			{
				if (_pageDefaultBackground != value)
				{
                    RaisePropertyChanging(() => PageDefaultBackground);
                    _pageDefaultBackground = value;
                    RaisePropertyChanged(() => PageDefaultBackground);
				}
			}
		}
		[PropertyCategory("ServerSettings", typeof(StandardPropertyCategories))]
		public string ServerName
		{
			get
			{
				return _serverName;
			}
			set
			{
				if (_serverName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(_serverName, value);
                    RaisePropertyChanging(() => ServerName);
                    _serverName = value;
                    ServerNameChanged?.Invoke(this, e);
                    RaisePropertyChanged(() => ServerName);
					RaiseServerSettingsChanged();
				}
			}
		}

		[Editable(false)]
        string IProject.ServerName
		{
			get
			{
				return ServerName;
			}
		}

		[PropertyCategory("ServerSettings", typeof(StandardPropertyCategories))]
		public string Host
		{
			get
			{
				return _host;
			}
			set
			{
				if (_host != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(_host, value);
                    RaisePropertyChanging(() => Host);
                    _host = value;
                    HostChanged?.Invoke(this, e);
                    RaisePropertyChanged(() => Host);
					RaiseServerSettingsChanged();
				}
			}
		}

        [PropertyCategory("ServerSettings", typeof(StandardPropertyCategories)), MaximumValue(65535.0), MinimumValue(80.0)]
		public int Port
		{
			get
			{
				return _port;
			}
			set
			{
				if (_port != value)
				{
                    RaisePropertyChanging(() => Port);
                    _port = value;
                    RaisePropertyChanged(() => Port);
					RaiseServerSettingsChanged();
				}
			}
		}

        [PropertyCategory("ServerSettings", typeof(StandardPropertyCategories)), DefaultValue(10000), MinimumValue(100)]
        public int Timeout
        {
            get
            {
                return _timeout;
            }
            set
            {
                if (_timeout != value)
                {
                    RaisePropertyChanging(() => Timeout);
                    _timeout = value;
                    RaisePropertyChanged(() => Timeout);
                    RaiseServerSettingsChanged();
                }
            }
        }
        [PropertyCategory("ServerSettings", typeof(StandardPropertyCategories))]
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                if (_username != value)
                {
                    RaisePropertyChanging(() => Username);
                    _username = value;
                    RaisePropertyChanged(() => Username);
                    RaiseServerSettingsChanged();
                }
            }
        }

		[PropertyCategory("ServerSettings", typeof(StandardPropertyCategories))]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    RaisePropertyChanging(() => Password);
                    _password = value;
                    RaisePropertyChanged(() => Password);
                    RaiseServerSettingsChanged();
                }
            }
        }
        [PropertyCategory("ServerSettings", typeof(StandardPropertyCategories))]
        public string SecretKey
        {
            get
            {
                return _secretkey;
            }
            set
            {
                if (_secretkey != value)
                {
                    RaisePropertyChanging(() => SecretKey);
                    _secretkey = value;
                    RaisePropertyChanged(() => SecretKey);
                    RaiseServerSettingsChanged();
                }
            }
        }

        [PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Color AppPageBackground
		{
			get
			{
				return _appPageBackground;
			}
			set
			{
				if (_appPageBackground != value)
				{
					ValueChangedEventArgs<Color> e = new ValueChangedEventArgs<Color>(_appPageBackground, value);
                    RaisePropertyChanging(() => AppPageBackground);
                    _appPageBackground = value;
                    HtmlPageBackgroundChanged?.Invoke(this, e);
                    RaisePropertyChanged(() => AppPageBackground);
				}
			}
		}


		public ProjectViewModel(IServiceProvider container, string name, string folder) : base(container, null)
		{
			Contract.Requires<ArgumentNullException>(name != null, "name");
			Contract.Requires<ArgumentException>(name.Length != 0, "name");
			Contract.Requires<ArgumentNullException>(folder != null, "folder");
			Contract.Requires<ArgumentException>(folder.Length != 0, "folder");

            PagesMonitor = container.Resolve<ICollectionMonitor<IProjectPageViewModel>>();
            PagesMonitor.Start(Pages);

            fileSystemService = container.Resolve<IFileSystemService>();
            mediaService = container.Resolve<IMediaService>();
            ActionMonitor = container.Resolve<IActionMonitor>();

            InitializeDefaultStateAppearance();
            SetProjectProperties(name, folder);

			//HMISilverlightControlsZipFile = smethod_71();

            EnsureResourceFoldersExist();

            SubscribeToMediaChanged();

			this.OnMediaChanged(MediaType.Image);
			this.OnMediaChanged(MediaType.Audio);
			//this.OnMediaChanged(MediaType.Xaml);
		}
        private void SubscribeToMediaChanged()
        {
            MediaChangedEvent @event = EventAggregator.GetEvent<MediaChangedEvent>();
            @event.Subscribe(new Action<MediaType>(OnMediaChanged), ThreadOption.UIThread);
        }
        private void RaiseServerSettingsChanged()
        {
            ServerSettingsChanged?.Invoke(this, new EventArgs());
        }

		private string GetAssemblyFolder()
		{
			string location = Assembly.GetExecutingAssembly().Location;
			return new FileInfo(location).Directory.FullName;
		}

		public void SetProjectProperties(string name, string folder)
		{
            ProjectFolder = folder;
            ProjectImagesFolder = System.IO.Path.Combine( ProjectFolder , "Images");
            ProjectMediaFolder = System.IO.Path.Combine( ProjectFolder , "Media");
            ProjectXamlsFolder = System.IO.Path.Combine( ProjectFolder , "Xamls");

            ProjectName = name;
            ProjectNamespace = name;
            ProjectTemplatesFolder = GetProjectTemplatesFolder();
            ProjectTitle = name;
            OldProjectName = name;
		}
        private string GetProjectTemplatesFolder()
        {
            string path = GetAssemblyFolder();
            return System.IO.Path.Combine(path, "BuildTemplates");
        }
        public override bool IsValidDropTarget(IEnumerable<object> payload)
		{
			bool result;
			if (payload.Count() == 1 && payload.First() is IProjectPageViewModel)
			{
				result = !Children.Any(t=> t is IProjectPageViewModel && !(t is ITemplatePageViewModel));
            }
			else
			{
				result = false;
			}
			return result;
		}
		public void OnMediaChanged(MediaType mediaType)
		{
			switch (mediaType)
			{
			case MediaType.Image:
                    LoadImageFiles();
				return;
			case MediaType.Xaml:
					LoadXamlFiles();
				return;
			case MediaType.Audio:
                    LoadAudioFiles();
				return;
			default:
				return;
			}
		}
		private void LoadAudioFiles()
		{
			IObservable<string> mediaFiles = mediaService.GetMediaFiles(MediaType.Audio, ProjectMediaFolder);
            LoadProjectMediaFiles(mediaFiles);
		}
		private void LoadImageFiles()
		{
			IObservable<string> mediaFiles = mediaService.GetMediaFiles(MediaType.Image, ProjectImagesFolder);
            LoadProjectImageFiles(mediaFiles);
		}
		private void LoadXamlFiles()
		{
			IObservable<string> mediaFiles = mediaService.GetMediaFiles(MediaType.Xaml, ProjectXamlsFolder);
			LoadProjectXamlsFiles(mediaFiles);
		}
		private void EnsureResourceFoldersExist()
		{
            fileSystemService.EnsureFolderExists(ProjectImagesFolder);
            fileSystemService.EnsureFolderExists(ProjectMediaFolder);
            fileSystemService.EnsureFolderExists(ProjectXamlsFolder);
		}
		private void InitializeDefaultStateAppearance()
		{
            StateDefaultBackground = new SolidColorBrush(Colors.LightGray);
            StateDefaultForeground = new SolidColorBrush(Colors.Black);
		}
		public void LoadProjectImageFiles(IObservable<string> files)
		{
            ProjectImages.Clear();
			if (files == null)
			{
				return;
			}
            ProjectImages.Add(string.Empty);
            ProjectImages.AddRange(files.ToEnumerable<string>());
		}
		public void LoadProjectMediaFiles(IObservable<string> files)
		{
            ProjectMedia.Clear();
			if (files == null)
			{
				return;
			}
            ProjectMedia.Add(string.Empty);
            ProjectMedia.AddRange(files.ToEnumerable<string>());
		}
		public void LoadProjectXamlsFiles(IObservable<string> files)
		{
            ProjectXamls.Clear();
			if (files == null)
			{
				return;
			}
            ProjectXamls.Add(string.Empty);
            ProjectXamls.AddRange(files.ToEnumerable<string>());
		}
		

	}
}
