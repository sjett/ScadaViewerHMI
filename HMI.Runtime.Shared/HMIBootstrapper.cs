// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Communication;
using HMI.Runtime.Core;
using HMI.Runtime.Core.Helper.Models;
using HMI.Runtime.Shared.Common.Internationalization;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace HMI.Runtime.Shared
{
    public sealed class HMIBootstrapper : IDisposable
    {
        private readonly Window shell;
        private ServiceProvider? serviceProvider;

        public HMIBootstrapper(Window shell)
        {
            this.shell = shell ?? throw new ArgumentNullException(nameof(shell));
        }

        public void Run()
        {
            if (serviceProvider is not null)
            {
                throw new InvalidOperationException("The HMI runtime has already been started.");
            }

            ServerSettings serverSettings = LoadServerSettings();
            ServiceCollection services = new();
            services.AddSingleton(serverSettings);
            services.AddSingleton<IHMITrace, HMITrace>();
            services.AddSingleton<IHmiApiClientFactory, RapidScadaHmiApiClientFactory>();
            services.AddSingleton<HMIConnector>();
            services.AddSingleton<IHMIConnector>(provider => provider.GetRequiredService<HMIConnector>());
            services.AddSingleton<IHMITranslationProvider>(provider => provider.GetRequiredService<HMIConnector>());
            services.AddSingleton<ILanguageTranslator, LanguageTranslator>();

            ServiceProvider provider = services.BuildServiceProvider(
                new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
            try
            {
                RuntimeServices.SetServiceProvider(provider);
                serviceProvider = provider;
                shell.Show();
            }
            catch
            {
                serviceProvider = null;
                RuntimeServices.ClearServiceProvider(provider);
                provider.Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            ServiceProvider? provider = Interlocked.Exchange(ref serviceProvider, null);
            if (provider is null)
            {
                return;
            }

            RuntimeServices.ClearServiceProvider(provider);
            provider.Dispose();
        }

        private static ServerSettings LoadServerSettings()
        {
            string settingsPath = Path.Combine(AppContext.BaseDirectory, "ServerSettings.json");
            if (!File.Exists(settingsPath))
            {
                throw new FileNotFoundException("The runtime connection settings file was not found.", settingsPath);
            }

            using FileStream settingsStream = File.OpenRead(settingsPath);
            return JsonSerializer.Deserialize<ServerSettings>(
                settingsStream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidDataException($"The runtime connection settings file '{settingsPath}' is empty.");
        }
    }
}
