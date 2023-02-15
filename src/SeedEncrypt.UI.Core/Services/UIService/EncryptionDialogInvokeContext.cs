using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedEncrypt.UI.Core.Services.UIService
{
    public class EncryptionDialogInvokeContext
    {
        public string Password { get; init; }

        public string? ErrorMessage { get; set; }

        public EncryptionDialogInvokeContext(string password)
        {
            Password = password;
        }
    }
}
