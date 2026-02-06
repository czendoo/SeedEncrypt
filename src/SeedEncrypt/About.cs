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
            yield return "Salt is UTF-8 bytes of 'seedencrypt:salt:{password}'";
            yield return "A 2 * wordCount byte key is derived";
            yield return "Key derivation step 1: Argon2d v19 / memory 256 MiB / time 8 / lanes 4 / threads = CPU count / salt as above";
            yield return "Key derivation step 2: Rfc2898 PBKDF2 / HMACSHA512 / 2^20 iterations / salt as above / input = passwordBytes || argon2Hash";
            yield return "Each BIP39 word index is XORed with 11 bits taken from the derived key";
            yield return "BIP39 checksum bits are recomputed (SHA-256 of entropy) so the resulting seed phrase is valid";
        }
    }
}
