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
using SeedEncrypt.UI.Core.ViewModels;

namespace SeedEncryptWinApp.Views
{
    public sealed partial class MainView : UserControl
    {
        public const string TagHomePage = "Home";
        public const string TagAboutPage = "About";
        public const string TagSettingsPage = "Settings";

        public Frame ContentRoot => ContentFrame;

        public MainViewModel ViewModel => App.Current.Services.GetRequiredService<MainViewModel>();

        List<(string Tag, Type PageType)> _pages = new List<(string Tag, Type PageType)>()
        {
            (TagHomePage, typeof(EditSeedPage)),
            (TagAboutPage, typeof(AboutPage)),
            (TagSettingsPage, typeof(SettingsPage)),
        };

        public MainView()
        {
            InitializeComponent();
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs args)
        {
            if (!_pages.Any(x => x.PageType == args.SourcePageType))
            {
                NavView.SelectedItem = null;
            }
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer is NavigationViewItem item)
            {
                NavigateTo(item.Tag as string);
            }
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavView.MenuItems.First() is NavigationViewItem firstItem)
            {
                firstItem.IsSelected = true;
                NavigateTo(firstItem.Tag as string);
            }
        }

        void NavigateTo(string tag)
        {
            var pageInfo = _pages.FirstOrDefault(x => x.Tag == tag);

            if (pageInfo.PageType != null)
            {
                NavigateTo(pageInfo.PageType);
            }
        }

        public void NavigateTo(Type pageType)
        {
            ContentFrame.Navigate(pageType);
        }
    }
}
