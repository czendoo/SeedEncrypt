using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Windows.ApplicationModel;
using SeedEncrypt.UI.Core.ViewModels;

namespace SeedEncryptWinApp.Views
{
    public sealed partial class AboutPage : Page
    {
        public AboutViewModel ViewModel { get; } = App.Current.Services.GetRequiredService<AboutViewModel>();

        public AboutPage()
        {
            InitializeComponent();

            var v = Package.Current.Id.Version;

            ViewModel.Version = $"{v.Major}.{v.Minor}.{v.Revision}";
        }
    }
}
