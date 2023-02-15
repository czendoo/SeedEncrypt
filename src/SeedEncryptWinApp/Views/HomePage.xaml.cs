// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SeedEncrypt.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SeedEncryptWinApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomeViewModel ViewModel { get; } = App.Current.Services.GetRequiredService<HomeViewModel>();

        public HomePage()
        {
            this.InitializeComponent();
        }

        void BtnAdd_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //ContentDialog dlg = new ContentDialog()
            //{
            //    XamlRoot = XamlRoot,
            //    Title = "Test dialog",
            //    Content = "Hello World",
            //    CloseButtonText = "Ok",
            //};

            //await dlg.ShowAsync();

            Frame.Navigate(typeof(EditSeedPage));
        }

        [RelayCommand]
        public void AddSeedPhrase()
        {
        }
    }
}
