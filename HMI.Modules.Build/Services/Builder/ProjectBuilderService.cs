// SPDX-License-Identifier: GPL-3.0-only
using NLog;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using HMI.Infrastructure.Services.Logging;
using HMI.Modules.Build.Resources;
using HMI.Infrastructure.Presentation.Services;

namespace HMI.Modules.Build.Services.Builder;

public class ProjectBuilderService : IProjectBuilderService
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private static readonly string[] runtimeAssemblyFileNames =
    {
        "HMI.RuntimeControls.dll",
        "HMI.Runtime.Core.dll",
        "HMI.Runtime.Shared.dll",
        "HMI.MathExpressions.dll",
        "HMI.Runtime.Communication.dll",
        "ScadaCommon.dll",
        "ScadaCommon.Log.dll"
    };
    private readonly IContentReplacementService contentReplacementService;
    private readonly IFileSystemService fileSystemService;
    private readonly ILoggingService loggingService;
    private readonly IProjectManagerViewModel projectManagerViewModel;
    private readonly Stopwatch stopwatch;

    public IProjectViewModel Project
    {
        get
        {
            return projectManagerViewModel.Project;
        }
    }
    public ProjectBuilderService(
        IProjectManagerViewModel projectManagerViewModel,
        IContentReplacementService contentReplacementService,
        IFileSystemService fileSystemService,
        ILoggingService compositeLoggingService)
    {
        this.contentReplacementService = contentReplacementService ?? throw new ArgumentNullException(nameof(contentReplacementService));
        this.fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
        loggingService = compositeLoggingService ?? throw new ArgumentNullException(nameof(compositeLoggingService));
        this.projectManagerViewModel = projectManagerViewModel ?? throw new ArgumentNullException(nameof(projectManagerViewModel));
        stopwatch = new Stopwatch();
    }
    public bool Prepare(SynchronizationContext? synchronizationContext)
    {
        prepareStart("Preparing process");
        bool result;
        try
        {
            if (Project.Pages.Count == 0)
            {
                throw new InvalidOperationException("The project must contain at least one page before it can be built.");
            }

            ensureFolderExists();
            copyFiles();
            copyRuntimeLoggingConfiguration();
            copyRuntimeAssemblies();
            ensureServerSettingsFile();
            replaceAndCopy_App();

            synchronizationContext ??= new SynchronizationContext();
            synchronizationContext.Send(_ => generate_Pages(), null);

            generate_csproj();

            prepareEnd("Preparing process");
            result = true;
        }
        catch (Exception exception)
        {
            logger.Error(exception, BuildErrorTexts.ErrorPreparingFilesForBuild);
            result = false;
        }
        return result;

    }
    void prepareStart(string processName)
    {
        this.stopwatch.Reset();
        this.stopwatch.Start();
        this.loggingService.LogException(new Exception(string.Format("{0} started ...", processName)));
    }
    void ensureFolderExists()
    {
        this.fileSystemService.EnsureFolderExists(this.Project.AppProjectFolder);//app
        this.fileSystemService.EnsureFolderExists(this.Project.BinFolder);//app/bin
        this.fileSystemService.EnsureFolderExists(this.Project.BuildFolder);//app/build
        this.fileSystemService.EnsureFolderExists(this.Project.DebugFolder);//app/bin/debug
        this.fileSystemService.EnsureFolderExists(Path.Combine(this.Project.AppProjectFolder, "Publish"));
    }
    void copyFiles()
    {
        copyImages();
        copyAudio();
        copyXamls();
    }
    void copyRuntimeLoggingConfiguration()
    {
        string sourceFilePath = Path.Combine(Project.ProjectTemplatesFolder, "NLog.config");
        if (!File.Exists(sourceFilePath))
        {
            throw new FileNotFoundException("The runtime logging configuration template was not found.", sourceFilePath);
        }

        fileSystemService.Copy(sourceFilePath, Project.BuildFolder);
    }
    void copyRuntimeAssemblies()
    {
        foreach (string fileName in runtimeAssemblyFileNames)
        {
            string sourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (!File.Exists(sourceFilePath))
            {
                throw new FileNotFoundException($"Required runtime assembly '{fileName}' was not found.", sourceFilePath);
            }

            this.fileSystemService.Copy(sourceFilePath, this.Project.BuildFolder);
        }
    }
    void copyImages()
    {
        string destinationDirectory = ProjectFilesResolver.ResolveDeploymentFolder(this.Project, MediaType.Image);
        this.fileSystemService.CopyAllDirectoryFiles(this.Project.ProjectImagesFolder, destinationDirectory);
    }
    void copyAudio()
    {
        string destinationDirectory = ProjectFilesResolver.ResolveDeploymentFolder(this.Project, MediaType.Audio);
        this.fileSystemService.CopyAllDirectoryFiles(this.Project.ProjectMediaFolder, destinationDirectory);
    }
    void copyXamls()
    {
        string destinationDirectory = ProjectFilesResolver.ResolveDeploymentFolder(this.Project, MediaType.Xaml);
        this.fileSystemService.CopyAllDirectoryFiles(this.Project.ProjectXamlsFolder, destinationDirectory);
    }
    void ensureServerSettingsFile()
    {
        string settingsFilePath = Path.Combine(Project.AppProjectFolder, "ServerSettings.json");
        if (File.Exists(settingsFilePath))
        {
            return;
        }

        string serverName = string.IsNullOrWhiteSpace(Project.ServerName) ? "default" : Project.ServerName;
        var settings = new
        {
            DefaultServerName = serverName,
            SubscriptionPollingIntervalMilliseconds = 500,
            Servers = new[]
            {
                new
                {
                    Name = serverName,
                    Host = Project.Host,
                    Port = Project.Port,
                    Username = string.Empty,
                    Password = string.Empty,
                    Instance = string.Empty,
                    SecretKey = string.Empty,
                    Timeout = Project.Timeout
                }
            }
        };
        string json = System.Text.Json.JsonSerializer.Serialize(
            settings,
            new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        fileSystemService.WriteFileContent(settingsFilePath, json);
    }
    void replaceAndCopy_App()
    {
        replaceAndCopyAppXaml();
        replaceAndCopyAppCodeBehind();

        replaceAndCopyMainWindowXaml();
        replaceAndCopyMainWindowCodeBehind();
    }
    void replaceAndCopyAppXaml()
    {
        string contentFilePath = Path.Combine(this.Project.ProjectTemplatesFolder, "App.xaml");
        string targetFilePath = Path.Combine(this.Project.BuildFolder, "App.xaml");

        var valueHolder = new { PROJECTNAMESPACE = this.Project.ProjectNamespace };

        this.contentReplacementService.ReplaceFileContent(contentFilePath, targetFilePath, valueHolder);
    }
    void replaceAndCopyAppCodeBehind()
    {
        string contentFilePath = Path.Combine(this.Project.ProjectTemplatesFolder, "App.xaml.cs");
        string targetFilePath = Path.Combine(this.Project.BuildFolder, "App.xaml.cs");

        var valueHolder = new { PROJECTNAMESPACE = this.Project.ProjectNamespace };

        this.contentReplacementService.ReplaceFileContent(contentFilePath, targetFilePath, valueHolder);
    }
    void replaceAndCopyMainWindowXaml()
    {
        string contentFilePath = Path.Combine(this.Project.ProjectTemplatesFolder, "MainWindow.xaml");
        string targetFilePath = Path.Combine(this.Project.BuildFolder, "MainWindow.xaml");

        var valueHolder = new { PROJECTNAMESPACE = this.Project.ProjectNamespace, PROJECTNAME = this.Project.ProjectName };

        this.contentReplacementService.ReplaceFileContent(contentFilePath, targetFilePath, valueHolder);
    }
    void replaceAndCopyMainWindowCodeBehind()
    {
        string contentFilePath = Path.Combine(this.Project.ProjectTemplatesFolder, "MainWindow.xaml.cs");
        string targetFilePath = Path.Combine(this.Project.BuildFolder, "MainWindow.xaml.cs");

        var valueHolder = new { PROJECTNAMESPACE = this.Project.ProjectNamespace, PROJECTSTARTPAGE = this.Project.Pages.Count<IProjectPageViewModel>() > 0 ? this.Project.Pages.First<IProjectPageViewModel>().ID : string.Empty };

        this.contentReplacementService.ReplaceFileContent(contentFilePath, targetFilePath, valueHolder);
    }
    void generate_Pages()
    {
        foreach (IProjectPageViewModel current in this.Project.Pages)
        {
            copyAndReplace_Page_Cs(current);
            generate_Page_Xaml(current);
        }
    }
    void copyAndReplace_Page_Cs(IProjectPageViewModel page)
    {
        string contentFilePath = Path.Combine(this.Project.ProjectTemplatesFolder, "Page.xaml.cs");
        string targetFilePath = Path.Combine(this.Project.BuildFolder, page.ID + ".xaml.cs");

        var valueHolder = new { PROJECTNAMESPACE = this.Project.ProjectNamespace, PAGENAME = page.ID };

        this.contentReplacementService.ReplaceFileContent(contentFilePath, targetFilePath, valueHolder);
    }
    void generate_Page_Xaml(IProjectPageViewModel page)
    {
        string filePath = Path.Combine(this.Project.BuildFolder, page.ID + ".xaml");
        StringBuilder stringBuilder = new StringBuilder();
        addPageControlNamespace(stringBuilder, page);
        addPageNamespaces(stringBuilder, page);
        addPageControlContent(page, stringBuilder);
        addNavigationParameters(page, stringBuilder);
        addPageChildrenControlXaml(page, stringBuilder);
        addPageControlClose(stringBuilder, page);

        this.fileSystemService.WriteFileContent(filePath, stringBuilder.ToString());
    }
    void addPageControlNamespace(StringBuilder xamlBuilder, IProjectPageViewModel page)
    {
        xamlBuilder.AppendLine("<UserControl");
        xamlBuilder.AppendFormat("x:Class=\"{0}.{1}\"{2}", Project.ProjectNamespace, page.ID, Environment.NewLine);
        xamlBuilder.AppendLine("xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");
        xamlBuilder.AppendLine("xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
        //xamlBuilder.AppendLine("xmlns:Infrastructure=\"clr-namespace:HMI.Infrastructure.Converters;assembly=HMI.Infrastructure.Converters\"");
    }
    void addPageNamespaces(StringBuilder xamlBuilder, IProjectPageViewModel page)
    {
        IEnumerable<string> enumerable = getPageNamespaces(page);
        foreach (string current in enumerable)
        {
            xamlBuilder.AppendFormat("xmlns:{0}{1}", current, Environment.NewLine);
        }
        xamlBuilder.AppendLine(">");
    }
    IEnumerable<string> getPageNamespaces(IProjectPageViewModel page)
    {
        List<string> list = new List<string>();
        if (page.Children != null)
        {
            foreach (IExplorerNodeViewModel current in page.Children)
            {
                addPageChildrenControlNamespaceDefinitions(current, list);
            }
        }
        addHmiBaseNamespaceDefinitions(page, list);
        addTemplatePageNamespaceDefinitions(list, page);
        return list;
    }
    void addPageChildrenControlNamespaceDefinitions(IExplorerNodeViewModel node, List<string> namespaceDefinitions)
    {
        if (node.Children != null && node.Children.Count > 0)
        {
            foreach (IExplorerNodeViewModel current in node.Children)
            {
                addPageChildrenControlNamespaceDefinitions(current, namespaceDefinitions);
            }
        }

        ICommonControlViewModel? commonControlViewModel = node as ICommonControlViewModel;
        if (commonControlViewModel == null)
        {
            return;
        }
        IEnumerable<string> namespaces = commonControlViewModel.GetNamespaces();
        foreach (string namespaceDefinition in namespaces)
        {
            if (!string.IsNullOrWhiteSpace(namespaceDefinition) && !namespaceDefinitions.Contains(namespaceDefinition))
            {
                namespaceDefinitions.Add(namespaceDefinition);
            }
        }
    }
    void addHmiBaseNamespaceDefinitions(IProjectPageViewModel page, List<string> namespaceDefinitions)
    {
        if (page is ITemplatePageViewModel)
        {
            if (!namespaceDefinitions.Contains("HMINavigation=\"clr-namespace:HMI.RuntimeControls.Navigation;assembly=HMI.RuntimeControls\""))
            {
                namespaceDefinitions.Add("HMINavigation=\"clr-namespace:HMI.RuntimeControls.Navigation;assembly=HMI.RuntimeControls\"");
            }
            if (!namespaceDefinitions.Contains("HMIShared=\"clr-namespace:HMI.Runtime.Shared;assembly=HMI.Runtime.Shared\""))
            {
                namespaceDefinitions.Add("HMIShared=\"clr-namespace:HMI.Runtime.Shared;assembly=HMI.Runtime.Shared\"");
            }
        }
    }
    void addTemplatePageNamespaceDefinitions(List<string> namespaceDefinitions, IProjectPageViewModel page)
    {
        if (page is ITemplatePageViewModel templatePageViewModel)
        {
            foreach (IParameterDefinition current in templatePageViewModel.TemplateParameters)
            {
                current.AddNamespaceDefinitions(namespaceDefinitions);
            }
        }
    }
    void addPageControlContent(IProjectPageViewModel page, StringBuilder xamlBuilder)
    {
        addViewboxBegin(xamlBuilder, page);

        xamlBuilder
            .Append("<Canvas x:Name=\"LayoutRoot\"")
            .Append(" HorizontalAlignment=\"Left\"")
            .Append(" VerticalAlignment=\"Top\"")
            .Append(" Height=\"")
            .Append(page.Height.ToString(CultureInfo.InvariantCulture))
            .Append("\" Width=\"")
            .Append(page.Width.ToString(CultureInfo.InvariantCulture))
            .Append("\" Background=\"")
            .Append(page.Background.ToString())
            .Append("\" >");
    }
    void addViewboxBegin(StringBuilder xamlBuilder, IProjectPageViewModel page)
    {
        if (isStretchPages(page))
        {
            xamlBuilder.AppendLine("<Viewbox HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\">");
        }
    }
    void addNavigationParameters(IProjectPageViewModel page, StringBuilder xamlBuilder)
    {
        ITemplatePageViewModel? templatePageViewModel = page as ITemplatePageViewModel;
        if (templatePageViewModel != null)
        {
            xamlBuilder.AppendLine("<HMINavigation:HMINavigationParameter Height=\"0\" Width=\"0\" Opacity=\"0\" Visibility=\"Collapsed\">")
                .AppendLine("<HMINavigation:HMINavigationParameter.Parameters>");
            foreach (IParameterDefinition current in templatePageViewModel.TemplateParameters)
            {
                xamlBuilder.AppendLine(current.GetXaml());
            }
            xamlBuilder.AppendLine("</HMINavigation:HMINavigationParameter.Parameters>")
                .AppendLine("</HMINavigation:HMINavigationParameter>");
        }
    }
    void addPageChildrenControlXaml(IProjectPageViewModel page, StringBuilder xamlBuilder)
    {
        if (page.Children != null)
        {
            foreach (ICommonControlViewModel current in page.Children.OfType<ICommonControlViewModel>())
            {
                xamlBuilder.Append(current.GetXaml());
            }
        }
    }
    void addPageControlClose(StringBuilder xamlBuilder, IProjectPageViewModel page)
    {
        xamlBuilder.AppendLine("</Canvas>");
        addViewboxClose(xamlBuilder, page);
        xamlBuilder.AppendLine("</UserControl>");
    }
    void addViewboxClose(StringBuilder xamlBuilder, IProjectPageViewModel page)
    {
        if (isStretchPages(page))
        {
            xamlBuilder.AppendLine("</Viewbox>");
        }
    }
    bool isStretchPages(IProjectPageViewModel page)
    {
        return this.Project != null && page != null && (this.Project.StretchPages || page.StretchPages);
    }
    void generate_csproj()
    {
        string filePath = Path.Combine(this.Project.BuildFolder, this.Project.ProjectName + ".csproj");
        string text = copyAndReplace_csproj();
        text = replace_csproj_ProjectPages(text);
        text = replace_csproj_ProjectDlls(text);

        this.fileSystemService.WriteFileContent(filePath, text);
    }
    string copyAndReplace_csproj()
    {
        string contentFilePath = Path.Combine(this.Project.ProjectTemplatesFolder, "CSProject.csproj");

        var valueHolder = new { PROJECTNAME= this.Project.ProjectName, PROJECTNAMESPACE= this.Project.ProjectNamespace};

        return this.contentReplacementService.ReplaceFileContent(contentFilePath, valueHolder);
    }
    string replace_csproj_ProjectPages(string projectFileContent)
    {
        var valueHolder = new { PROJECTPAGES = string.Empty };
        return this.contentReplacementService.ReplaceStringContent(projectFileContent, valueHolder);
    }
    string replace_csproj_ProjectDlls(string context)
    {
        StringBuilder stringBuilder = new StringBuilder();
        IEnumerable<AssemblyReference> enumerable = getAssemblyReferences();
        foreach (AssemblyReference current in enumerable)
        {
            string assemblyFileName = Path.GetFileName(current.AssemblyName);
            string assemblyName = Path.GetFileNameWithoutExtension(assemblyFileName);
            stringBuilder.Append("\t<Reference Include=\"").Append(assemblyName).AppendLine("\">");
            if (current.CopyLocal)
            {
                string arg = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyFileName);
                stringBuilder.AppendFormat("\t\t<HintPath>{0}</HintPath>", arg);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("\t\t<Private>true</Private>");
            }
            else
            {
                stringBuilder.AppendLine("\t\t<SpecificVersion>False</SpecificVersion>");
            }
            stringBuilder.AppendLine("\t</Reference>");
        }
        var valueHolder = new { PROJECTDLLS = stringBuilder.ToString() };

        return this.contentReplacementService.ReplaceStringContent(context, valueHolder);
    }
    IEnumerable<Domain.Models.AssemblyReference> getAssemblyReferences()
    {
        List<AssemblyReference> list = new List<AssemblyReference>();
        if (this.Project.Pages == null)
        {
            return list;
        }
        foreach (string fileName in runtimeAssemblyFileNames)
        {
            var runtimeReference = new AssemblyReference(fileName);
            if (!list.Contains(runtimeReference))
            {
                list.Add(runtimeReference);
            }
        }
        foreach (IProjectPageViewModel current in this.Project.Pages)
        {
            if (current.Children != null)
            {
                foreach (IExplorerNodeViewModel childNode in current.Children)
                {
                    getControlAssemblyReference(list, childNode);
                }
            }
            getTemplatePageParametersAssemblyReference(current, list);
        }
        return list;
    }
    void getControlAssemblyReference(List<Domain.Models.AssemblyReference> assemblyReferences, IExplorerNodeViewModel node)
    {
        if (node.Children != null && node.Children.Count > 0)
        {
            foreach (IExplorerNodeViewModel current in node.Children)
            {
                getControlAssemblyReference(assemblyReferences, current);
            }
        }
        ICommonControlViewModel? commonControlViewModel = node as ICommonControlViewModel;
        if (commonControlViewModel != null)
        {
            IEnumerable<AssemblyReference> requiredAssemblies = commonControlViewModel.GetRequiredAssemblies();
            assemblyReferences.AddRange(requiredAssemblies.Where(assembly => !assemblyReferences.Contains(assembly)));
        }
    }
    void getTemplatePageParametersAssemblyReference(IProjectPageViewModel page, List<AssemblyReference> assemblyReferences)
    {
        if (page is ITemplatePageViewModel templatePageViewModel)
        {
            foreach (IParameterDefinition current in templatePageViewModel.TemplateParameters)
            {
                current.AddReferences(assemblyReferences);
            }
        }
    }
    void prepareEnd(string processName)
    {
        this.stopwatch.Stop();
        this.loggingService.LogException(new Exception(string.Format(CultureInfo.CurrentCulture, BuildTexts.BuildEndedFormat, new object[]
        {
            processName,
            this.stopwatch.Elapsed
        })));
    }
    public bool Build()
    {
        return buildProject("Build", publish: false);
    }
    public bool Publish()
    {
        return buildProject("Publish", publish: true);
    }
    bool buildProject(string target, bool publish)
    {
        try
        {
            string projectFilePath = Path.Combine(this.Project.BuildFolder, this.Project.ProjectName + ".csproj");
            ProcessStartInfo startInfo = new()
            {
                FileName = "dotnet",
                WorkingDirectory = this.Project.BuildFolder,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
                StandardErrorEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
                CreateNoWindow = true
            };
            if (publish)
            {
                string publishDirectory = Path.Combine(this.Project.AppProjectFolder, "Publish");
                startInfo.ArgumentList.Add("publish");
                startInfo.ArgumentList.Add(projectFilePath);
                startInfo.ArgumentList.Add("--configuration");
                startInfo.ArgumentList.Add("Release");
                startInfo.ArgumentList.Add("--output");
                startInfo.ArgumentList.Add(publishDirectory);
                startInfo.ArgumentList.Add("--no-self-contained");
            }
            else
            {
                startInfo.ArgumentList.Add("build");
                startInfo.ArgumentList.Add(projectFilePath);
                startInfo.ArgumentList.Add("--configuration");
                startInfo.ArgumentList.Add("Debug");
            }

            using Process process = Process.Start(startInfo)
                ?? throw new InvalidOperationException("Failed to start the .NET SDK.");
            Task standardOutputTask = forwardProcessOutputAsync(process.StandardOutput, isError: false);
            Task standardErrorTask = forwardProcessOutputAsync(process.StandardError, isError: true);
            process.WaitForExit();
            Task.WhenAll(standardOutputTask, standardErrorTask).GetAwaiter().GetResult();
            return process.ExitCode == 0;
        }
        catch (Exception exception)
        {
            logger.Error(exception, $"Failed to {target.ToLowerInvariant()} generated runtime project.");
            return false;
        }
    }
    async Task forwardProcessOutputAsync(StreamReader streamReader, bool isError)
    {
        string? line;
        while ((line = await streamReader.ReadLineAsync().ConfigureAwait(false)) is not null)
        {
            if (isError)
            {
                logger.Error(line);
            }
            else
            {
                logger.Info(line);
            }

            loggingService.LogException(new Exception(line));
        }
    }

    public bool CompleteBuild()
    {
        return Prepare(SynchronizationContext.Current ?? new SynchronizationContext()) && Build() && Publish();
    }

}
