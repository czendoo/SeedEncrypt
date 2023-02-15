using Microsoft.UI.Xaml.Data;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SeedEncryptWinApp.Converters
{
    public class SeedLangToStringConverter : IValueConverter
    {
        public LanguageToStringType ConvertType { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Language lang)
            {
                switch (ConvertType)
                {
                    case LanguageToStringType.Short:
                        return SeedLangToString.ToShortString(lang);
                    case LanguageToStringType.Long:
                        return SeedLangToString.ToLongString(lang);
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
