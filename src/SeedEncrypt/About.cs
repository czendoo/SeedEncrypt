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
            yield return "A given seed phrase is transformed into a valid seed phrase using a password";
            yield return "The transformed seed phrase has a valid BIP39 checksum, making it indistinguishable from a regular seed phrase";
            yield return "Applying the same password to the transformed seed phrase restores the original seed phrase";
            yield return "Using an incorrect password produces a different valid seed phrase";
            yield return "There is no way to detect whether a seed phrase is original or password-transformed";
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
