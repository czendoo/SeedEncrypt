using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using SeedEncryptWinApp.Services.UIService;
using Microsoft.UI;
using SeedEncryptWinApp.Converters;
using SeedEncrypt.UI.Core.Services;
using Newtonsoft.Json.Linq;
using SeedEncrypt.UI.Core;
using SeedEncryptWinApp.Services.Settings;
using SeedEncrypt.UI.Core.Services.UIService;
using SeedEncrypt.UI.Core.ViewModels;
using SeedEncrypt;

namespace SeedEncryptWinApp
{
    public partial class App : Application
    {
        public MainWindow Window { get; protected set; }

        public static new App Current => Application.Current as App;

        public IServiceProvider Services { get; }

        public MainViewModel ViewModel { get; }

        public IAppSettings Settings { get; }

        public XamlRoot XamlRoot => MainWindow.Current.Content.XamlRoot;

        public App()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Debug(LogEventLevel.Verbose)
                .WriteTo.File(@"Logs\app.log", fileSizeLimitBytes: 1024 * 512, rollOnFileSizeLimit: true, retainedFileCountLimit: 1)
                .CreateLogger();

            Services = ConfigureServices();

            ViewModel = Services.GetRequiredService<MainViewModel>();
            Settings = Services.GetRequiredService<IAppSettings>();

            _theme = Settings.Theme;
            
            InitializeComponent();
        }

        AppTheme _theme;
        public AppTheme Theme
        {
            get => _theme;
            set
            {
                if (MainWindow.Current.Content is FrameworkElement el)
                {
                    el.RequestedTheme = AppThemeConverter.Convert(value);
                }

                _theme = value;
            }
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            Window = new MainWindow();
            Window.Activate();
        }

        static IServiceProvider ConfigureServices()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddLogging(builder => builder.AddSerilog());
            services.AddSingleton<IUIService, UIService>();
            services.AddTransient<IAppSettings, AppSettigns>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<EditSeedViewModel>();
            services.AddTransient<PasswordDialogViewModel>();
            services.AddTransient<DecryptViewModel>();
            services.AddTransient<EnterSeedViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<AboutViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
