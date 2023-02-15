using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SeedEncrypt.UI.Core.ViewModels;
using SeedEncryptWinApp.Converters;
using SeedEncryptWinApp.MarkupExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace SeedEncryptWinApp.Views
{
    public sealed partial class PasswordDialog : ContentDialog
    {
        public PasswordDialogViewModel ViewModel { get; } = App.Current.Services.GetRequiredService<PasswordDialogViewModel>();

        public PasswordDialog()
        {
            this.InitializeComponent();

            XamlRoot = App.Current.XamlRoot;
            Title = Lang.GetString("EncryptDialogTitle");
            PrimaryButtonText = Lang.GetString("BtnSubmit");
            CloseButtonText = Lang.GetString("BtnCancel");
            DefaultButton = ContentDialogButton.Primary;
            RequestedTheme = AppThemeConverter.Convert(App.Current.Theme);

            RepeatOuter.RegisterPropertyChangedCallback(PasswordBox.VisibilityProperty, OnRepeatOuterVisibilityChanged);
        }

        private void OnRepeatOuterVisibilityChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (ViewModel.IsRepeatState)
            {
                Password2.Focus(FocusState.Keyboard);
            }
        }
    }
}
