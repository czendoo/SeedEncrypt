using NBitcoin;
using SeedEncrypt;
using SeedEncryptConsole.UI;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SeedEncryptConsole.UI.UI;

namespace SeedEncryptConsole
{
    class MainCommand : Command<MainCommand.Settings>
    {
        Mnemonic? _mnemonic;
        Puzzle? _puzzle;

        public class Settings : CommandSettings
        {
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            ShowLogo();

            bool quit = false;

            var options = new Option[]
            {
                new Option("Generate new seed phrase") { Action = () => Generate() },
                new Option("Enter seed phrase") { Action = () => Create() },
                new Option("Encrypt") { Action = () => Encrypt(), Enabled = () => _mnemonic != null },
                new Option("Decrypt") { Action = () => Decrypt() },
                new Option("Show seed numbered") { Action = () => ShowSeedNumbered(), Enabled = () => _mnemonic != null },
                new Option("Show all") { Action = () => ShowAll(), Enabled = () => _mnemonic != null },
                new Option("Reset") { Action = () => Reset(), Enabled = () => _mnemonic != null },
                new Option("Clear screen") { Action = () => ClearScreen() },
                new Option("About") { Action = () => ShowAbout() },
                new Option("Quit") { Action = () => quit = true, Confirm = () => _mnemonic != null },
            };

            while (!quit)
            {
                PromptOptions(options);
            }

            return 0;
        }

        void Reset()
        {
            if (AnsiConsole.Confirm("Reset the runtime state of the application?", false))
            {
                _mnemonic = null;
                _puzzle = null;
            }
        }

        void Generate()
        {
            Wordlist wordList = PromptWordList();
            WordCount wordCount = PromptWordCount();

            WriteInfo($"Generate seed phrase: language={wordList.Name}, wordCount={(int)wordCount}");

            _mnemonic = new Mnemonic(wordList, wordCount);

            ShowSeed(_mnemonic);
        }

        private void Create()
        {
            PromptOptions(new[] {
                new Option("Type words") { Action = () => CreateByTyping() },
                new Option("Paste whole seed phrase from clipboard") { Action = () => ImportSeedAsText() },
                new Option("Cancel"),
                });
        }

        private void CreateByTyping()
        {
            Wordlist wordList = PromptWordList();
            WordCount wordCount = PromptWordCount();

            string[] words = new string[(int)wordCount];

            int ix = 0;

            WriteLine();
            WriteInfo("#");
            WriteInfo("# HELP");
            WriteInfo("# Enter one or more words seperated by space");
            WriteInfo("# Enter blank line for OPTIONS");
            WriteInfo("#");
            WriteLine();

            while (ix < words.Length)
            {
                string text = AnsiConsole.Prompt(
                    new TextPrompt<string>($"[{ColorPrompt}]Enter one or more words ({ix + 1} - {words.Length})[/]").AllowEmpty());

                string[] values = wordList.Split(text);

                if (values.Length == 0)
                {
                    int result = 0;

                    Option option = PromptOptions(new[]
                    {
                        new Option("Clear last") { Action = () => ix--, Enabled = () => ix > 0, Confirm = () => true },
                        new Option("Clear all") { Action = () => ix = 0, Enabled = () => ix > 0, Confirm = () => true },
                        new Option("Cancel") { Action = () => result = 1 },
                        new Option("Exit the seed phrase editor") { Action = () => result = 2, Confirm = () => ix > 0 },
                    });

                    if (result == 1) continue;
                    else if (result == 2) break;
                }

                foreach (string value in values)
                {
                    if (!wordList.WordExists(value, out int wordIx))
                    {
                        WriteWarning($"Word '{value}' is not in the bep39 word list");
                        break;
                    }

                    words[ix++] = value;

                    WriteInfo($"{ix}. {value}");

                    if (ix == words.Length)
                        break;
                }
            }

            if (ix == words.Length)
            {
                try
                {
                    string seed = String.Join(wordList.Space, words);
                    _mnemonic = new Mnemonic(seed, wordList);

                    ShowSeed(_mnemonic);
                }
                catch (Exception ex)
                {
                    WriteError("Failed", ex.ToString());
                }
            }
        }

        void Encrypt()
        {
            if (_mnemonic is null)
            {
                WriteWarning("No seed phrase to encrypt");
                return;
            }

            WriteNote("#");
            WriteNote("# ENCRYPT");
            WriteNumberedLines(About.GetPuzzleHelpLines());
            WriteNote("#");

            string? password = PromptPassword(true);
            if (password is null) return;

            if (_mnemonic != null)
            {
                try
                {
                    AnsiConsole.Status()
                        .Start("Encrypting the seed phrase", ctx =>
                        {
                            Stopwatch stopwatch = Stopwatch.StartNew();
                            _puzzle = Puzzle.Create(_mnemonic, password);
                            stopwatch.Stop();

                            WriteNote($"Encryption time: {stopwatch.ElapsedMilliseconds} ms");
                        });
                }
                catch (Exception ex)
                {
                    WriteError("Failed to encrypt the seed phrase", ex.Message);
                    return;
                }

                ShowPuzzle(_puzzle!);
            }
        }

        void Decrypt()
        {
            WriteNote("#");
            WriteNote("# DECRYPT");
            WriteNote("# Restore the original seed phrase");
            WriteNote("#");

            Mnemonic? mnemonic1 = PromptSeedPhrase("Enter the cipher seed");
            if (mnemonic1 == null)
                return;

            string? password = PromptPassword(false);
            if (password == null)
                return;

            try
            {
                AnsiConsole.Status()
                    .Start("Decrypting the seed phrase", ctx =>
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        var puzzle = SeedEncrypt.Puzzle.Create(mnemonic1, password);
                        stopwatch.Stop();

                        WriteNote($"Decryption time: {stopwatch.ElapsedMilliseconds} ms");

                        _mnemonic = puzzle.Primary;
                        _puzzle = null;
                    });
            }
            catch (Exception ex)
            {
                WriteError("Failed to decrypt the seed phrase", ex.Message);
                return;
            }

            ShowSeed(_mnemonic);
        }

        void ImportSeedAsText(string? value = null)
        {
            var mnemonic = PromptSeedPhrase();
            if (mnemonic == null)
                return;

            _mnemonic = mnemonic;

            ShowAll();
        }

        void ShowAll()
        {
            WriteLine();

            WriteNote("*");
            WriteNote("* Info");
            WriteNote("*");

            ShowSeed(_mnemonic);

            if (_puzzle != null)
            {
                ShowPuzzle(_puzzle);
            }
        }

        void ShowSeed(Mnemonic? mnemonic, string? header = null)
        {
            WriteLine();

            header ??= "Seed phrase";

            if (mnemonic != null)
            {
                if (mnemonic.IsValidChecksum)
                {
                    WriteHeader(header, "Valid", ColorSuccess);
                }
                else
                {
                    WriteHeader(header, "Invalid seed checksum", ColorError);
                }

                WriteRaw(mnemonic.ToString(), ConsoleColor.Magenta);
            }
            else
            {
                WriteNote("No seedphrase");
            }
        }

        void ShowSeedNumbered()
        {
            WriteLine();

            if (_mnemonic != null)
            {
                bool validChecksum = _mnemonic.IsValidChecksum == true;

                if (validChecksum)
                {
                    WriteHeader("Seed phrase numbered", "Valid", ColorSuccess);
                }
                else
                {
                    WriteHeader("Seed phrase", "Invalid seed checksum", ColorError);
                }

                int maxWordLen = _mnemonic.Words.Max(x => x.Length) + 2;

                for (int ix = 0; ix < _mnemonic.Words.Length; ix++)
                {
                    if (ix % 3 == 0) WriteLine();

                    string order = $"{(ix + 1)}.".PadRight(4);
                    string word = _mnemonic.Words[ix].PadRight(maxWordLen);

                    AnsiConsole.Markup($"[{ColorImportant}]{order}[/][{ColorInfo}]{word}[/]");
                }

                WriteLine();
            }
            else
            {
                WriteNote("No seedphrase");
            }
        }

        void ShowPuzzle(Puzzle puzzle)
        {
            ShowSeed(puzzle.Primary, "The cipher seed phrase");
        }

        void ShowAbout()
        {
            WriteLine();

            WriteNote("*");
            WriteNote("* About");
            WriteNote("*");

            WriteLine();
            WriteHeader("Encryption usage");
            WriteNumberedLines(About.GetPuzzleHelpLines());
            WriteLine();
            WriteHeader("Encryption algorithm");
            WriteNumberedLines(About.GetPuzzleInfoLines());
        }

        void WriteNumberedLines(IEnumerable<string> lines)
        {
            int ix = 0;
            foreach (var line in lines)
            {
                WriteNote(Markup.Escape($"# {++ix}. {line}"));
            }
        }

        void ShowLogo()
        {
            AnsiConsole.Write(
                new FigletText("CZ")
                    .LeftJustified()
                    .Color(Color.DeepSkyBlue3));

            AnsiConsole.Write(new Markup($"[{ColorInfo}]SeedEncrypt[/][{ColorNote}] > Bip39 seed phrase encryption tool[/]"));

            WriteLine();
        }

        void ClearScreen()
        {
            AnsiConsole.Clear();

            ShowLogo();
        }
    }
}
