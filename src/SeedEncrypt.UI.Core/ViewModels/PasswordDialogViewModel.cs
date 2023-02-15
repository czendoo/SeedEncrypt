using CommunityToolkit.Mvvm.ComponentModel;
using SeedEncrypt;
using SeedEncrypt.UI.Core.Services.UIService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedEncrypt.UI.Core.ViewModels
{
    public partial class PasswordDialogViewModel : ObservableObject
    {
        readonly IUIService _uiService;

        [ObservableProperty]
        bool _isRepeatEnabled = true;

        [ObservableProperty]
        bool _isRepeatState;

        [ObservableProperty]
        string? _title;

        [ObservableProperty]
        string? _password;

        [ObservableProperty]
        string? _password2;

        [ObservableProperty]
        bool _revealPassword = true;

        [ObservableProperty]
        bool _revealPassword1;

        [ObservableProperty]
        bool _revealPassword2;

        [ObservableProperty]
        bool _hasError;

        [ObservableProperty]
        string? _errorMessage;

        [ObservableProperty]
        bool _isBusy;

        public Action<EncryptionDialogInvokeContext>? Action { get; set; }

        public PasswordDialogViewModel(IUIService uiService)
        {
            _uiService = uiService;

            RevealPassword1 = RevealPassword;
            RevealPassword2 = RevealPassword;
        }

        partial void OnRevealPasswordChanged(bool value)
        {
            RevealPassword1 = value && !IsRepeatState;
            RevealPassword2 = value;
        }

        partial void OnIsRepeatStateChanged(bool value)
        {
            if (value)
            {
                RevealPassword1 = false;
                RevealPassword2 = RevealPassword;
            }
            else
            {
                RevealPassword1 = RevealPassword;
            }
        }

        partial void OnPasswordChanged(string? value)
        {
            IsRepeatState = false;
            Password2 = null;
            SetError(null);
        }

        partial void OnPassword2Changed(string? value)
        {
            SetError(null);
        }

        public async Task<bool> InvokeAsync()
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                HasError = true;
                ErrorMessage = _uiService.GetString("ErrMsgProvideValidPassword");
                return false;
            }

            if (IsRepeatEnabled)
            {
                if (IsRepeatState)
                {
                    if (!string.Equals(Password, Password2))
                    {
                        HasError = true;
                        ErrorMessage = "Password mismatch";
                        return false;
                    }
                }
                else
                {
                    IsRepeatState = true;
                    return false;
                }
            }

            IsBusy = true;

            try
            {
                EncryptionDialogInvokeContext ctx = new (Password);

                await Task.Run(() =>
                {
                    Action?.Invoke(ctx);
                });

                if (ctx.ErrorMessage != null)
                {
                    ErrorMessage = ctx.ErrorMessage;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        void SetError(string? errorMessage)
        {
            ErrorMessage = errorMessage;
            HasError = errorMessage != null;
        }
    }
}
