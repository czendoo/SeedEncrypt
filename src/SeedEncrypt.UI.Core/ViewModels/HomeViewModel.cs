using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SeedEncrypt.UI.Core.Services.UIService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedEncrypt.UI.Core.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        readonly ILogger<HomeViewModel> _logger;
        readonly IUIService _navService;

        public HomeViewModel(ILogger<HomeViewModel> logger, IUIService navService)
        {
            _logger = logger;
            _navService = navService;
        }

        [RelayCommand]
        public void AddNewSeed()
        {
            _navService.NavigateToAddSeedPage();
        }
    }
}
