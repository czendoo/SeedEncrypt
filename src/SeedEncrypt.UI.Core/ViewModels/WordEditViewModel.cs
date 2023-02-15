using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedEncrypt.UI.Core.ViewModels
{
    public partial class WordEditViewModel : ObservableObject
    {
        [ObservableProperty]
        int _order;

        [ObservableProperty]
        string? _text;

        [ObservableProperty]
        bool _isValid;

        partial void OnTextChanged(string? value)
        {
            IsValid = value != null;
        }
    }
}
