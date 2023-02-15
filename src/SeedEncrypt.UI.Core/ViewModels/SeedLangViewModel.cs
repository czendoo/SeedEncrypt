using CommunityToolkit.Mvvm.ComponentModel;
using NBitcoin;
using NBitcoin.OpenAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedEncrypt.UI.Core.ViewModels
{
    public class SeedLangViewModel : ObservableObject
    {
        public Language Lang { get; set; }

        public string ShortName { get; set; }

        public string LongName { get; set; }

        public SeedLangViewModel(Language lang, string shortName, string longName)
        {
            Lang = lang;
            ShortName = shortName;
            LongName = longName;
        }
    }
}
