using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using SeedEncryptWinApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using Windows.UI.Popups;
using SeedEncryptWinApp.Converters;
using Windows.ApplicationModel.Resources;
using SeedEncrypt.UI.Core;
using SeedEncrypt.UI.Core.Services;
using SeedEncrypt.UI.Core.Services.UIService;
using Windows.ApplicationModel.DataTransfer;
using SeedEncrypt.UI.Core.ViewModels;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Provider;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.Extensions.DependencyInjection;
using SeedEncrypt;

namespace SeedEncryptWinApp.Services.UIService
{
    public class UIService : IUIService
    {
        readonly ILogger<UIService> _logger;

        public AppTheme Theme
        {
            get => App.Current.Theme;
            set => App.Current.Theme = value;
        }

        public UIService(ILogger<UIService> logger)
        {
            _logger = logger;
        }

        MainView MainView => (MainView)MainWindow.Current.Content;

        public void NavigateToAddSeedPage()
        {
            MainView.NavigateTo(typeof(EditSeedPage));
        }

        public async Task<EnterSeedResult> EnterSeedPhraseAsync()
        {
            EnterSeedDialog dialog = new();
            EnterSeedResult result = null;

            dialog.PrimaryButtonClick += async (sender, args) =>
            {
                var deferral = args.GetDeferral();

                dialog.IsPrimaryButtonEnabled = false;

                result = await dialog.ViewModel.Run();

                if (result == null)
                {
                    dialog.IsPrimaryButtonEnabled = true;
                    args.Cancel = true;
                }

                deferral.Complete();
            };

            await dialog.ShowAsync();

            return result;
        }

        public async Task<DecryptResult> DecryptSeedPhraseAsync()
        {
            DecryptViewModel viewModel = null;

            DecryptDialog dialog = new(viewModel);
            DecryptResult importResult = null;

            dialog.PrimaryButtonClick += async (sender, args) =>
            {
                var deferral = args.GetDeferral();

                dialog.IsPrimaryButtonEnabled = false;

                importResult = await dialog.ViewModel.Import();

                if (importResult == null)
                {
                    dialog.IsPrimaryButtonEnabled = true;
                    args.Cancel = true;
                }

                deferral.Complete();
            };

            await dialog.ShowAsync();

            return importResult;
        }

        public async Task<bool> ShowConfirmation(string message, string title)
        {
            ContentDialog dialog = new ContentDialog()
            {
                XamlRoot = App.Current.XamlRoot,
                Title = title,
                PrimaryButtonText = GetString("BtnOk"),
                CloseButtonText = GetString("BtnCancel"),
                DefaultButton = ContentDialogButton.Secondary,
                Content = message,
                RequestedTheme = AppThemeConverter.Convert(Theme),
            };

            var res = await dialog.ShowAsync();

            return res == ContentDialogResult.Primary;
        }

        public async Task ShowMessageBox(string message, string title)
        {
            ContentDialog dialog = new ContentDialog()
            {
                XamlRoot = App.Current.XamlRoot,
                Title = title,
                PrimaryButtonText = GetString("BtnOk"),
                DefaultButton = ContentDialogButton.Primary,
                Content = message,
                RequestedTheme = AppThemeConverter.Convert(Theme),
            };

            var res = await dialog.ShowAsync();
        }


        public async Task<bool> ShowLostSeedConfirmation()
        {
            bool res = await ShowConfirmation(GetString("ConfirmContinue"), GetString("ConfirmLostSeed"));

            return res;
        }

        public List<string> GetLangList()
        {
            return Windows.Globalization.ApplicationLanguages.ManifestLanguages.ToList();
        }

        public string GetString(string id) =>
            ResourceLoader.GetForViewIndependentUse().GetString(id);

        public void CopyTextToClipboard(string text)
        {
            DataPackage dataPackage = new()
            {
                RequestedOperation = DataPackageOperation.Copy,
            };

            dataPackage.SetText(text);

            Clipboard.SetContent(dataPackage);
        }

        public async Task ShowPasswordDialog(Action<PasswordDialogViewModel> configure)
        {
            PasswordDialog dialog = new();

            configure?.Invoke(dialog.ViewModel);

            if (dialog.ViewModel.Title != null)
                dialog.Title = dialog.ViewModel.Title;

            dialog.PrimaryButtonClick += async (sender, args) =>
            {
                dialog.IsPrimaryButtonEnabled = false;

                var deferral = args.GetDeferral();

                bool result = await dialog.ViewModel.InvokeAsync();

                if (!result)
                {
                    dialog.IsPrimaryButtonEnabled = true;
                    args.Cancel = true;
                }

                deferral.Complete();
            };

            await dialog.ShowAsync();
        }
    }
}
