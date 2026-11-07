// SPDX-License-Identifier: GPL-3.0-only
using NLog;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;
using HMI.Runtime.Shared;


namespace HMI.Runtime;//[PROJECTNAMESPACE]

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static readonly Logger Logger = LogManager.GetLogger("Runtime");
    private HMIBootstrapper? runtime;

    public App()
    {
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
    }

    protected override void OnStartup(StartupEventArgs args)
    {
        base.OnStartup(args);
        runtime = new HMIBootstrapper(new MainWindow());
        runtime.Run();
    }

    protected override void OnExit(ExitEventArgs args)
    {
        try
        {
            runtime?.Dispose();
        }
        finally
        {
            base.OnExit(args);
        }
    }

    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
        if (args.ExceptionObject is Exception exception)
        {
            Logger.Fatal(exception, "An unhandled application exception occurred.");
        }
    }

    private static void OnDispatcherUnhandledException(
        object sender,
        DispatcherUnhandledExceptionEventArgs args)
    {
        Logger.Fatal(args.Exception, "An unhandled dispatcher exception occurred.");
    }
}


