using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedEncrypt.UI.Core.Services.UIService
{
    public class PuzzleResult
    {
        public Mnemonic? Mnemonic { get; set; }

        public string? Password { get; set; }
    }
}
