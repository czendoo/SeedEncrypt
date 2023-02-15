using CommunityToolkit.Mvvm.ComponentModel;
using NBitcoin;
using SeedEncrypt.UI.Core;
using SeedEncrypt.UI.Core.Services;
using SeedEncrypt.UI.Core.Services.UIService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeedEncrypt.UI.Core.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        readonly IUIService _uiService;
        readonly IAppSettings _settings;

        [ObservableProperty]
        List<AppTheme> _themes = Enum.GetValues<AppTheme>().ToList();

        [ObservableProperty]
        AppTheme _theme;

        [ObservableProperty]
        List<Language> _seedLangs = Enum.GetValues<Language>().Where(x => x != Language.Unknown).ToList();

        [ObservableProperty]
        Language _seedLang;

        [ObservableProperty]
        List<WordCount> _wordCountList = Enum.GetValues<WordCount>().ToList();

        [ObservableProperty]
        WordCount _wordCount;

        [ObservableProperty]
        List<LangViewModel> _langs;

        [ObservableProperty]
        LangViewModel _lang;

        public SettingsViewModel(IUIService uiService, IAppSettings settings)
        {
            _uiService = uiService;
            _settings = settings;

            _theme = _settings.Theme;
            _seedLang = _settings.DefaultSeedLang;
            _wordCount = _settings.DefaultSeedWordCount;

            _langs = GetLanguages().ToList();
            _lang = _langs.FirstOrDefault(x => x.Id == _settings.Language) ?? _langs[0];
        }

        IEnumerable<LangViewModel> GetLanguages()
        {
            yield return new LangViewModel("", _uiService.GetString("Default"));
            foreach (var lang in _uiService.GetLangList())
                yield return new LangViewModel(lang);
        }

        partial void OnThemeChanged(AppTheme value)
        {
            _uiService.Theme = value;
            _settings.Theme = value;
        }

        partial void OnSeedLangChanged(Language value)
        {
            _settings.DefaultSeedLang = value;
        }

        partial void OnWordCountChanged(WordCount value)
        {
            _settings.DefaultSeedWordCount = value;
        }

        partial void OnLangChanged(LangViewModel value)
        {
            _settings.Language = value.Id;
        }
    }
}
