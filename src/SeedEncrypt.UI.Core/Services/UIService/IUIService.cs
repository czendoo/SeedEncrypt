using NBitcoin;
using SeedEncrypt.UI.Core;
using SeedEncrypt.UI.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeedEncrypt.UI.Core.Services.UIService
{
    public interface IUIService
    {
        void NavigateToAddSeedPage();

        Task ShowPasswordDialog(Action<PasswordDialogViewModel> configure);

        Task<DecryptResult> DecryptSeedPhraseAsync();

        Task<EnterSeedResult> EnterSeedPhraseAsync();

        Task<bool> ShowConfirmation(string message, string title);

        Task ShowMessageBox(string message, string title);

        Task<bool> ShowLostSeedConfirmation();

        AppTheme Theme { get; set; }

        List<string> GetLangList();

        string GetString(string id);

        void CopyTextToClipboard(string text);
    }
}