using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeedEncrypt;
using SeedEncrypt.UI.Core.Services;
using SeedEncrypt.UI.Core.Services.UIService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedEncrypt.UI.Core.ViewModels
{
    public partial class AboutViewModel : ObservableObject
    {
        readonly IUIService _uiService;

        [ObservableProperty]
        string? _version;

        public string EncryptionHelp { get; }

        public string EncryptionInfo { get; }

        public AboutViewModel(IUIService uiService)
        {
            int ix;
            _uiService = uiService;

            ix = 1;
            EncryptionHelp = String.Join(Environment.NewLine, About.GetPuzzleHelpLines().Select(line => $"{ix++}. {line}"));

            ix = 1;
            EncryptionInfo = String.Join(Environment.NewLine, About.GetPuzzleInfoLines().Select(line => $"{ix++}. {line}"));
        }

        [RelayCommand]
        public void CopyEncryptionInfo()
        {
            _uiService.CopyTextToClipboard(EncryptionInfo);
        }
    }
}
