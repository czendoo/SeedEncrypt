using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using SeedEncrypt.UI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeedEncryptWinApp.Converters
{
    public class AppThemeConverter : IValueConverter
    {
        public AppThemeConverter()
        {            
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is AppTheme theme)
            {
                return Convert(theme);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public static ElementTheme Convert(AppTheme theme)
        {
            switch (theme)
            {
                case AppTheme.Default:
                    return ElementTheme.Default;
                case AppTheme.Light:
                    return ElementTheme.Light;
                case AppTheme.Dark:
                    return ElementTheme.Dark;
                default:
                    throw new Exception($"Unknow theme: {theme}");
            }
        }
    }
}
