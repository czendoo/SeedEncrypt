using CommunityToolkit.Mvvm.ComponentModel;
using NBitcoin;
using SeedEncrypt;
using SeedEncrypt.UI.Core.Services.UIService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SeedEncrypt.UI.Core.ViewModels
{
    public partial class DecryptViewModel : ObservableObject
    {
        readonly IUIService _uiService;

        [ObservableProperty]
        string? _password;

        [ObservableProperty]
        bool _showPassword = true;

        [ObservableProperty]
        bool _hasError;

        [ObservableProperty]
        bool _isBusy;

        [ObservableProperty]
        string? _errorMessage;

        [ObservableProperty]
        string? _primary;

        [ObservableProperty]
        string? _secondary;

        public DecryptViewModel(IUIService uiService)
        {
            _uiService = uiService;
        }

        partial void OnErrorMessageChanged(string? value)
        {
            HasError = value != null;
        }

        public async Task<DecryptResult?> Import()
        {
            if (string.IsNullOrWhiteSpace(Primary))
            {
                ErrorMessage = _uiService.GetString("ErrMsgProvideValidPrimarySeed");
                return null;
            }

            if (string.IsNullOrWhiteSpace(Secondary))
            {
                ErrorMessage = _uiService.GetString("ErrMsgProvideValidSecondarySeed");
                return null;
            }

            Mnemonic primary;
            Mnemonic secondary;
            
            try
            {
                primary = new Mnemonic(Primary);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }

            try
            {
                secondary = new Mnemonic(Secondary);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = _uiService.GetString("ErrMsgProvideValidPassword");
                return null;
            }

            ErrorMessage = null;

            try
            {
                DecryptResult? result = null;
                IsBusy = true;

                await Task.Run(() =>
                {
                    var puzzle = Puzzle.Create(primary, Password, secondary);
                    result = new DecryptResult()
                    {
                        Mnemonic = puzzle.Primary,
                        Password = Password,
                    };
                });

                return result;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
