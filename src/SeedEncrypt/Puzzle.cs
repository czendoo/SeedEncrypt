using Isopoh.Cryptography.Argon2;
using NBitcoin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SeedEncrypt
{
    public class Puzzle
    {       
        public const int IterationCount = 1 << 20;

        public Mnemonic Primary { get; init; }

        public Mnemonic Secondary { get; init; }

        Puzzle(Mnemonic primary, Mnemonic secondary)
        {
            Primary = primary;
            Secondary = secondary;
        }

        public static Puzzle Create(Mnemonic primary, string password, Mnemonic? secondary = null)
        {
            int wordCount = primary.Words.Length;
            int bitCount = wordCount * 11;
            int entropyByteCount = bitCount / 8 - ((bitCount % 8) == 0 ? 1 : 0);
            int entropyBitCount = entropyByteCount * 8;
            int checksumBitCount = bitCount - entropyBitCount;

            secondary ??= new Mnemonic(primary.WordList, (WordCount)wordCount);
            int[] indices2 = secondary.Indices.ToArray();
            indices2[0] &= 2046;
            RecountChecksum(indices2, wordCount, entropyByteCount, entropyBitCount, checksumBitCount);
            string seedPhrase2 = primary.WordList.GetSentence(indices2);
            secondary = new Mnemonic(seedPhrase2, primary.WordList);


            int[] indices = primary.Indices.ToArray();
            byte[] salt = Encoding.UTF8.GetBytes(seedPhrase2);
            byte[] key = CreateKey(salt, password, wordCount * 2);

            for (int i = 0; i < wordCount; i++)
            {
                indices[i] ^= ((key[2 * i] << 8) | key[2 * i + 1]) & 2047;
            }

            RecountChecksum(indices, wordCount, entropyByteCount, entropyBitCount, checksumBitCount);

            primary = new Mnemonic(primary.WordList.GetSentence(indices), primary.WordList);

            return new Puzzle(primary, secondary);
        }

        static void RecountChecksum(int[] indices, int wordCount, int entropyByteCount, int entropyBitCount, int checksumBitCount)
        {
            BitArray entropy = new BitArray(entropyBitCount);

            for (int i = 0; i < entropyBitCount; i++)
            {
                entropy.Set(i, (indices[i / 11] & (1 << (10 - (i % 11)))) != 0);
            }

            byte[] entropyBytes = new byte[entropyByteCount];

            entropy.CopyTo(entropyBytes, 0);

            for (int i = 0; i < entropyBytes.Length; i++)
                entropyBytes[i] = ReverseBitOrder(entropyBytes[i]);

            var checksum = SHA256.HashData(entropyBytes);

            indices[wordCount - 1] = (indices[wordCount - 1] >> checksumBitCount) << checksumBitCount;
            indices[wordCount - 1] |= checksum[0] >> (8 - checksumBitCount);
        }

        static byte ReverseBitOrder(byte b)
        {
            return (byte)(
                (((b >> 0) & 1) << 7) |
                (((b >> 1) & 1) << 6) |
                (((b >> 2) & 1) << 5) |
                (((b >> 3) & 1) << 4) |
                (((b >> 4) & 1) << 3) |
                (((b >> 5) & 1) << 2) |
                (((b >> 6) & 1) << 1) |
                (((b >> 7) & 1) << 0));
        }

        static byte[] CreateKey(byte[] salt, string password, int keySize)
        {
            password = password.Normalize(NormalizationForm.FormC);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            using var argon = new Argon2(new Argon2Config()
            {
                MemoryCost = 256 * 1024,
                TimeCost = 8,
                Lanes = 4,
                Threads = Environment.ProcessorCount,
                Salt = salt,
                Password = passwordBytes,
                Type = Argon2Type.HybridAddressing,
                Version = Argon2Version.Nineteen,
                HashLength = keySize
            });

            using var hash = argon.Hash();

            byte[] key = new byte[passwordBytes.Length + hash.Buffer.Length];

            Buffer.BlockCopy(passwordBytes, 0, key, 0, passwordBytes.Length);
            Buffer.BlockCopy(hash.Buffer, 0, key, passwordBytes.Length, hash.Buffer.Length);

            return Rfc2898DeriveBytes.Pbkdf2(
                key,
                salt,
                IterationCount,
                HashAlgorithmName.SHA512,
                keySize);
        }
    }
}
