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
    public partial class EnterSeedViewModel : ObservableObject
    {
        readonly IUIService _uiService;

        [ObservableProperty]
        bool _hasError;

        [ObservableProperty]
        bool _isBusy;

        [ObservableProperty]
        string? _errorMessage;

        [ObservableProperty]
        string? _text;

        public EnterSeedViewModel(IUIService uiService)
        {
            _uiService = uiService;
        }

        partial void OnErrorMessageChanged(string? value)
        {
            HasError = value != null;
        }

        public Task<EnterSeedResult?> Run()
        {
            EnterSeedResult? result = null;

            ErrorMessage = null;

            if (string.IsNullOrWhiteSpace(Text))
            {
                ErrorMessage = _uiService.GetString("ErrMsgProvideValidSeed");
            }
            else
            {
                try
                {
                    var mnemonic = new Mnemonic(Text);

                    result = new EnterSeedResult()
                    {
                        Mnemonic = mnemonic
                    };
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
                finally
                {
                    IsBusy = false;
                }
            }

            return Task.FromResult(result);
        }
    }
}
