using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedEncrypt.UI.Core.ViewModels
{
    public class LangViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public LangViewModel(string id)
        {
            CultureInfo ci = new CultureInfo(id);

            Id = id;
            Name = ci.DisplayName;
        }

        public LangViewModel(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
