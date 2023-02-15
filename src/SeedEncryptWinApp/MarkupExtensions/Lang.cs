using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace SeedEncryptWinApp.MarkupExtensions
{
    public class Lang : MarkupExtension
    {
        public string Id { get; set; }

        public Lang()
        {            
        }

        protected override object ProvideValue() => GetString(Id);

        public static string GetString(string id)
        {
            string str = ResourceLoader.GetForViewIndependentUse().GetString(id);
            return string.IsNullOrEmpty(str) ? $"[{id}]" : str;
        }
    }
}
