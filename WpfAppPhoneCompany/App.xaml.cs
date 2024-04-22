using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfAppPhoneCompany.Data;
using PresentationLayer.Services;
using WpfAppPhoneCompany.ViewModels;

namespace WpfAppPhoneCompany
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //public static Window ActiveWindow => Application.Current.Windows
        //       .OfType<Window>()
        //       .FirstOrDefault(w => w.IsActive);

        //public static Window FocusedWindow => Application.Current.Windows
        //   .OfType<Window>()
        //   .FirstOrDefault(w => w.IsFocused);

        //public static Window CurrentWindow => FocusedWindow ?? ActiveWindow;

        public static bool IsDesignTime { get; private set; } = true;

        private static IHost __Host;

        public static IHost Host => __Host
            ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IServiceProvider Services => Host.Services;

        internal static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
           .AddDatabase(host.Configuration.GetSection("Database"))
           .AddServices()
           .AddViewModels()
        ;

        protected override async void OnStartup(StartupEventArgs e)
        {
            IsDesignTime = false;

            var host = Host;

            using (var scope = Services.CreateScope())
                await scope.ServiceProvider.GetRequiredService<DbInitializer>().InitializeAsync();

            base.OnStartup(e);
            await host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using var host = Host;
            base.OnExit(e);
            await host.StopAsync();
        }
    }
}
