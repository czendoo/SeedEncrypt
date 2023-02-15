// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SeedEncryptWinApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditSeedPage : Page
    {
        public EditSeedViewModel ViewModel { get; } = App.Current.Services.GetRequiredService<EditSeedViewModel>();

        public EditSeedPage()
        {
            NavigationCacheMode = NavigationCacheMode.Required;

            InitializeComponent();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.WordsPerRowOrCol))
            {
                InitWordCountPerCol();
            }
        }

        void AutoSuggestBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                ViewModel.AddCurrentWord();
            }
        }

        void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
        }

        void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                ViewModel.OnCurrentWordChangedByUser();
            }
        }

        void ItemsWrapGrid_Loaded(object sender, RoutedEventArgs e)
        {
            InitWordCountPerCol();
        }

        void InitWordCountPerCol()
        {
            var grid = WordsGridView.ItemsPanelRoot as ItemsWrapGrid;

            if (grid != null)
            {
                grid.MaximumRowsOrColumns = ViewModel.WordsPerRowOrCol;
            }
        }

        private async void WordCountCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems?.First() is int count)
            {
                await ViewModel.OnSeedWordCountChangedByUser(count);
            }
        }

        private async void LangCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems?.First() is SeedLangViewModel lang)
            {
                await ViewModel.OnSeedLangChangedByUser(lang);
            }
        }
    }
}
