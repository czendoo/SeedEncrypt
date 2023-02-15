using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SeedEncrypt.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedEncryptWinApp.TemplateSelectors
{
    public class WordEditDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate InitialTemplate { get; set; }

        public DataTemplate ValidTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is WordEditViewModel word)
            {
                if (word.Text == null) 
                    return InitialTemplate;

                return ValidTemplate;
            }

            return base.SelectTemplateCore(item);
        }
    }
}
