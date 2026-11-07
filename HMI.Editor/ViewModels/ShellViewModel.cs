// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.ViewModels;
using HMI.Infrastructure.Controls;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Windows.Controls;
using HMI.Infrastructure.Services.Logging;

namespace HMI.Editor.ViewModels
{
    public class ShellViewModel : ViewModel, IShellViewModel
	{
        private readonly IFileSystemService fileSystemService;
        private readonly IProjectManagerViewModel projectManagerViewModel;
        private bool isLoadingPreferences;
        private bool isControlToolboxVisible = true;
        private bool isProjectExplorerVisible = true;
        private bool isPropertyEditorVisible = true;
        private bool isLogVisible = false;
        private bool isProjectEnginingVisible = true;
        private bool isActionEffectEditorVisible = true;

        private readonly AIPromptViewModel aIPromptViewModel;
        public ShellViewModel(ILoggingService loggingService,
			IFileSystemService fileSystemService, 
			IProjectManagerViewModel projectManagerViewModel, 
			IGuideSettingsViewModel guideSettingsViewModel, IServiceProvider provider)
        {
            Contract.Requires<ArgumentNullException>(loggingService != null);
            Contract.Requires<ArgumentNullException>(fileSystemService != null);
            Contract.Requires<ArgumentNullException>(projectManagerViewModel != null);
            Contract.Requires<ArgumentNullException>(guideSettingsViewModel != null, "guideSettingsViewModel");

            LoggingService = loggingService;
            this.fileSystemService = fileSystemService;
            this.projectManagerViewModel = projectManagerViewModel;
            GuideSettings = guideSettingsViewModel;
            Documents = new ObservableCollection<UserControl>();
            AllDocuments = new ObservableCollection<UserControl>();
            StaticSymbols = new ObservableCollection<IStaticSymbol>();
            InitializeProjectFolder();
            LoadPreferences();

            aIPromptViewModel = new AIPromptViewModel(provider);
        }
        public IGuideSettingsViewModel GuideSettings
		{
			get;
			private set;
		}
        public ILoggingService LoggingService
        {
            get;
            private set;
        }
        public ObservableCollection<UserControl> Documents
        {
            get;
            private set;
        }
        public ObservableCollection<UserControl> AllDocuments
        {
            get;
            private set;
        }
        public ObservableCollection<IStaticSymbol> StaticSymbols
        {
            get;
            private set;
        }
        public IProjectManagerViewModel CommandSink
        {
            get
            {
                return this.projectManagerViewModel;
            }
        }
        public AIPromptViewModel AIPrompt
        {
            get
            {
                return this.aIPromptViewModel;
            }
        }
        public bool IsControlToolboxVisible
		{
			get
			{
				return isControlToolboxVisible;
			}
			set
			{
				if (isControlToolboxVisible != value)
				{
                    isControlToolboxVisible = value;
					base.RaisePropertyChanged<bool>(() => IsControlToolboxVisible);
                    SavePreferences();
				}
			}
		}
        public bool IsProjectEnginingVisible
        {
            get
            {
                return isProjectEnginingVisible;
            }
            set
            {
                if (isProjectEnginingVisible != value)
                {
                    isProjectEnginingVisible = value;
                    base.RaisePropertyChanged<bool>(() => IsProjectEnginingVisible);
                    SavePreferences();
                }
            }
        }
        public bool IsProjectExplorerVisible
		{
			get
			{
				return isProjectExplorerVisible;
			}
			set
			{
				if (isProjectExplorerVisible != value)
				{
                    isProjectExplorerVisible = value;
					base.RaisePropertyChanged<bool>(() => IsProjectExplorerVisible);
                    SavePreferences();
				}
			}
		}
		public bool IsPropertyEditorVisible
		{
			get
			{
				return isPropertyEditorVisible;
			}
			set
			{
				if (isPropertyEditorVisible != value)
				{
                    isPropertyEditorVisible = value;
					base.RaisePropertyChanged<bool>(() => IsPropertyEditorVisible);
                    SavePreferences();
				}
			}
		}
        public bool IsActionEffectEditorVisible
        {
            get
            {
                return isActionEffectEditorVisible;
            }
            set
            {
                if (isActionEffectEditorVisible != value)
                {
                    isActionEffectEditorVisible = value;
                    base.RaisePropertyChanged<bool>(() => IsActionEffectEditorVisible);
                    SavePreferences();
                }
            }
        }
        public bool IsLogVisible
		{
			get
			{
				return isLogVisible;
			}
			set
			{
				if (isLogVisible != value)
				{
                    isLogVisible = value;
					base.RaisePropertyChanged<bool>(() => IsLogVisible);
                    SavePreferences();
				}
			}
		}
		private void InitializeProjectFolder()
		{
            fileSystemService.EnsureFolderExists(fileSystemService.DefaultProjectFolder);
		}
		private void SavePreferences()
		{
			if (isLoadingPreferences)
			{
				return;
			}
            var obj = new JObject();
            obj["IsProjectEnginingVisible"] = this.IsProjectEnginingVisible;
            obj["IsControlToolboxVisible"] = this.IsControlToolboxVisible;
            obj["IsProjectExplorerVisible"] = this.IsProjectExplorerVisible;
            obj["IsPropertyEditorVisible"] = this.IsPropertyEditorVisible;
            obj["IsActionEffectEditorVisible"] = this.IsActionEffectEditorVisible;
            obj["IsLogVisible"] = this.IsLogVisible;
            Preferences.SetPreferences(this, obj);
		}
		private void LoadPreferences()
		{
            isLoadingPreferences = true;
			try
			{
				var preferences = Preferences.GetPreferences(this);
				if (preferences != null)
				{
                    this.IsProjectEnginingVisible = ((bool?)preferences["IsProjectEnginingVisible"]) ?? true;
                    this.IsControlToolboxVisible = ((bool?)preferences["IsControlToolboxVisible"]) ?? true;
                    this.IsProjectExplorerVisible = ((bool?)preferences["IsProjectExplorerVisible"]) ?? true;
                    this.IsPropertyEditorVisible = ((bool?)preferences["IsPropertyEditorVisible"]) ?? true;
                    this.IsActionEffectEditorVisible = ((bool?)preferences["IsActionEffectEditorVisible"]) ?? true;
                    this.IsLogVisible = ((bool?)preferences["IsLogVisible"]) ?? false;
                }
			}
			catch
			{
			}
            isLoadingPreferences = false;
		}
	}
}
