using NBitcoin;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SeedEncrypt
{
    public static class About
    {
        public static IEnumerable<string> GetPuzzleHelpLines()
        {
            yield return "A given seed phrase is encrypted by password to a fake seed phrase";
            yield return "Providing valid password, the fake seed phrase is decrypted and the original seed phrase is restored";
            yield return "Wrong password provides just another fake seed phrase";
            yield return "It's not possible to distinguish a fake seed phrase from a regular one";
            yield return "It's not possible to detect if the fake seed phrase are a result of encryption";
        }

        public static IEnumerable<string> GetPuzzleInfoLines()
        {
            yield return "Password is normalized using the Unicode normalization form C";
            yield return "Random secondary seed is generated";
            yield return "2 * wordCount byte key is derived";
            yield return "Key derivation step 1: Argon2d v19 / memory 256M / time 8 / lanes 4 / Secondary seed phrase used as a salt";
            yield return "Key derivation step 2: Rfc2898 Pbkdf2 / HMACSHA512 / 2^20 iterations / Secondary seed phrase used as a salt";
            yield return "New primary seed word indices = old word indices as array of USHORT XOR key";
            yield return "Checksum bits adjusted so the primary seed phrase is valid";
        }
    }
}
