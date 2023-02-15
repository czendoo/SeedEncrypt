using SeedEncrypt.UI.Core;
using SeedEncrypt.UI.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SeedEncryptWinApp.Services.Settings
{
    public class AppSettigns : IAppSettings
    {
        ApplicationDataContainer _settings;

        public AppSettigns()
        {
            _settings = ApplicationData.Current.LocalSettings;
        }

        public AppTheme Theme 
        {
            get
            {
                string value = _settings.Values["AppTheme"] as string;
                return Enum.TryParse<AppTheme>(value, out AppTheme theme) ? theme : AppTheme.Default;
            }
            set => _settings.Values["AppTheme"] = value.ToString();
        }
        
        public NBitcoin.WordCount DefaultSeedWordCount 
        {
            get
            {
                string value = _settings.Values["DefaultSeedWordCount"] as string;
                return Enum.TryParse<NBitcoin.WordCount>(value, out NBitcoin.WordCount count) ? count : NBitcoin.WordCount.TwentyFour;
            }
            set => _settings.Values["DefaultSeedWordCount"] = value.ToString();
        }

        public NBitcoin.Language DefaultSeedLang 
        {
            get
            {
                string value = _settings.Values["DefaultSeedLang"] as string;
                return Enum.TryParse<NBitcoin.Language>(value, out NBitcoin.Language lang) ? lang : NBitcoin.Language.English;

            }
            set => _settings.Values["DefaultSeedLang"] = value.ToString();
        }

        public string Language 
        {
            get => Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride;
            set => Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = value;
        }
    }
}
