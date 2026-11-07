// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers;
using HMI.Common.Configuration;
using HMI.Common.Presentation;
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Editor.Common;
using HMI.Editor.Resources;
using HMI.Editor.Services;
using HMI.Editor.ViewModels;
using HMI.Infrastructure;
using HMI.Infrastructure.Presentation;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using HMI.Infrastructure.Services.Logging;
using HMI.Modules.Application;
using HMI.Modules.Application.Events;
using HMI.Modules.Artboard;
using HMI.Modules.Build;
using HMI.Modules.ControlTree;
using HMI.Modules.Design;
using HMI.Modules.Explorer;
using HMI.Modules.PropertyEditor;
using HMI.Modules.Solution;
using HMI.ProjectDesigner.DesignControls;
using Newtonsoft.Json;
using NLog;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
namespace HMI.Editor;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static readonly Logger logger = LogManager.GetLogger("Editor");
    private IServiceProvider services = null!;

    public App()
    {
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            //Converters = { new BrushJsonConverter() },
            Formatting = Formatting.Indented,
            DefaultValueHandling = DefaultValueHandling.Ignore,//忽略默认值
            NullValueHandling = NullValueHandling.Ignore//忽略null值
        };

        DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(this.App_DispatcherUnhandledException);
        System.AppDomain.CurrentDomain.UnhandledException += new System.UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);

    }
    private void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
    {
        logger.Fatal("An unhandled AppDomain exception has occurred", (System.Exception)e.ExceptionObject);
    }
    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        logger.Fatal("An unhandled dispatcher exception has occurred", e.Exception);
    }
    private void ParseDefaultDoc(string argument)
    {
        Uri uri;
        if (Uri.TryCreate(argument, UriKind.Absolute, out uri))
        {
            string localPath = uri.LocalPath;
            Properties["LoadProjectName"] = localPath;
            return;
        }
        if (!string.IsNullOrWhiteSpace(argument))
        {
            Properties["LoadProjectName"] = argument;
        }
    }
    private void ParseArgument(string argument)
    {
        if (argument.Equals("/resetpreferences", System.StringComparison.InvariantCultureIgnoreCase))
        {
            Preferences.Reset();
        }
    }
    protected override void OnStartup(StartupEventArgs e)
    {
        //var splash = new SplashWindow();
        //splash.Show();

        logger.Info<System.Version>(LogTexts.StartingEditor, GetType().Assembly.GetName().Version);
        List<string> list = new List<string>();
        //if (AppDomain.CurrentDomain.SetupInformation.ActivationArguments != null
        //    && AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData != null
        //    && AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData.Length > 0)
        //{
        //    list.AddRange(AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData);
        //}
        if (e.Args.Any<string>())
        {
            list.AddRange(e.Args);
        }
        foreach (string current in list)
        {
            if (current.StartsWith("/"))
            {
                this.ParseArgument(current);
            }
            else
            {
                this.ParseDefaultDoc(current);
            }
        }
        EventManager.RegisterClassHandler(typeof(TextBox), UIElement.KeyUpEvent, new KeyEventHandler(this.TextBox_KeyUp));

        base.OnStartup(e);
        InitializeServices();
        MainWindow = services.GetRequiredService<Shell>();
        MainWindow.Show();

        this.PublishApplicationStartedEvent();
    }
    private void PublishApplicationStartedEvent()
    {
        logger.Info("Application started");
        
        IEventAggregator eventAggregator = services.GetRequiredService<IEventAggregator>();
        ApplicationStartedEvent @event = eventAggregator.GetEvent<ApplicationStartedEvent>();
        @event.Publish(null);
    }
    private void TextBox_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Return)
        {
            return;
        }
        TextBox textBox = sender as TextBox;
        if (textBox != null)
        {
            BindingExpression bindingExpression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
            if (bindingExpression != null)
            {
                bindingExpression.UpdateSource();
            }
        }
        e.Handled = true;
    }
    private void InitializeServices()
    {
        var settingsStore = new HmiSettingsStore();
        new LegacyRegistrySettingsImporter(settingsStore).ImportOnce();

        var collection = new ServiceCollection();
        collection.AddSingleton(settingsStore);
        collection.AddSingleton<IRegionManager, RegionManager>();
        collection.AddSingleton<IEventAggregator, EventAggregator>();
        collection.AddSingleton<IMergeDictionaryRegistry, MergeDictionaryRegistry>();
        collection.AddSingleton<INotificationService, NotificationService>();
        collection.AddSingleton<ISerializationService, SerializationService>();
        collection.AddSingleton<IResourceProviderService, ResourceProviderService>();
        collection.AddSingleton<IErrorHandlingService, ErrorHandlingService>();
        collection.AddSingleton<ILoggingService, CompositeLoggingService>();
        collection.AddSingleton<ILoggerFactory, LoggerFactory>();
        collection.AddSingleton<IFileSystemService, FileSystemService>();
        collection.AddSingleton<Shell>();
        collection.AddSingleton<Func<Shell>>(provider => () => provider.GetRequiredService<Shell>());
        collection.AddSingleton<IShellViewModel, ShellViewModel>();
        collection.AddSingleton<IProjectManagerViewModel, ProjectManagerViewModel>();
        collection.AddSingleton<ISolution>(provider => provider.GetRequiredService<IProjectManagerViewModel>());
        collection.AddSingleton<IProjectManagerService, ProjectManagerService>();
        collection.AddSingleton<IClipboardManagerService, ClipboardManagerService>();
        collection.AddSingleton<ISerializerFactory, SerializerFactory>();
        collection.AddSingleton<IMruService, MruService>();
        collection.AddSingleton<ILanguageService, LanguageService>();
        collection.AddSingleton<IMediaService, MediaService>();
        collection.AddSingleton<IControlFactory, ControlFactory>();
        collection.AddSingleton<IFilterDescriptorFactory, FilterDescriptorFactory>();
        if (SynchronizationContext.Current is { } synchronizationContext)
        {
            collection.AddSingleton(synchronizationContext);
        }

        SdkBootstrapper.ConfigureContainer(collection);

        IHmiModule[] modules =
        [
            new PresentationModule(),
            new BrowsersModule(),
            new DesignModule(),
            new ApplicationModule(),
            new SolutionModule(),
            new ArtboardModule(),
            new DesignControlsModule(),
            new ControlTreeModule(),
            new ExplorerModule(),
            new PropertyEditorModule(),
            new BuildModule()
        ];

        foreach (var module in modules)
        {
            module.RegisterTypes(collection);
        }

        services = collection.BuildServiceProvider();
        RegionManager.SetCurrent(services.GetRequiredService<IRegionManager>());

        foreach (var module in modules)
        {
            module.OnInitialized(services);
        }
    }
    //protected override void OnInitialized()
    //{
    //    base.OnInitialized();
    //    // 初始化后可执行默认导航

    //}
}
