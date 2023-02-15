using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SeedEncryptWinApp.Converters
{
    public enum LanguageToStringType { Short, Long }

    public class SeedLangToString
    {
        public LanguageToStringType ConvertType { get; set; }

        public static string ToShortString(Language lang)
        {
            switch (lang)
            {
                case Language.ChineseTraditional:
                    return "zh-Hant";
                case Language.ChineseSimplified:
                    return "zh-Hans";
                case Language.English:
                    return "en";
                case Language.Japanese:
                    return "jp";
                case Language.Spanish:
                    return "es";
                case Language.French:
                    return "fr";
                case Language.PortugueseBrazil:
                    return "pt-BR";
                case Language.Czech:
                    return "cz";
                default:
                    return lang.ToString();
            }
        }

        public static string ToLongString(Language lang)
        {
            switch (lang)
            {
                case Language.ChineseTraditional:
                    return "Chinese Traditional";
                case Language.ChineseSimplified:
                    return "Chinese Simplified";
                case Language.English:
                    return "English";
                case Language.Japanese:
                    return "Japanese";
                case Language.Spanish:
                    return "Spanish";
                case Language.French:
                    return "French";
                case Language.PortugueseBrazil:
                    return "Portuguese Brazil";
                case Language.Czech:
                    return "Czech";
                default:
                    return lang.ToString();
            }
        }
    }
}
