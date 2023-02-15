using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedEncrypt.UI.Core.Services
{
    public interface IAppSettings
    {
        AppTheme Theme { get; set; }

        WordCount DefaultSeedWordCount { get; set; }

        Language DefaultSeedLang { get; set; }

        string Language { get; set; }
    }
}
