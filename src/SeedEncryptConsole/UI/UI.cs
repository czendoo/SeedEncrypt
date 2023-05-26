using NBitcoin;
using Spectre.Console.Rendering;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeedEncrypt;

namespace SeedEncryptConsole.UI
{
    public static class UI
    {
        public const string ColorInfo = "steelblue3";
        public const string ColorImportant = "turquoise2";
        public const string ColorError = "DeepPink3";
        public const string ColorSuccess = "springgreen3_1";
        public const string ColorNote = "steelblue";
        public const string ColorHeader = "deepskyblue2";
        public const string ColorDisabled = "grey35";
        public const string ColorWarning = "salmon1";
        public const string ColorPrompt = "lightseagreen";

        public static readonly Color BorderColorInfo = Color.SteelBlue3;
        public static readonly Color BorderColorSuccess = Color.SpringGreen3_1;

        public static void WriteHeader(string header, string note = "", string noteColor = ColorNote)
        {
            string markup = $"[{ColorHeader}][[ {header} ]][/]";
            if (!string.IsNullOrEmpty(note))
                markup += $" [{noteColor}]{note}[/]";
            AnsiConsole.MarkupLine(markup);
        }

        public static string? PromptPassword(bool encrypt)
        {
            string msg = encrypt ? "Enter strong password" : "Enter password";

            string password = AnsiConsole.Prompt(new TextPrompt<string>($"[{ColorWarning}]{msg}[/]").AllowEmpty()/*.Secret()*/);

            password = password.Trim();

            if (string.IsNullOrEmpty(password))
            {
                WriteWarning("Operation cancelled");
                return null;
            }

            if (encrypt)
            {
                if (password.Length < 16)
                    WriteWarning($"Consider longer password (16 letters): length={password.Length}");

                bool hasLowecase = false;
                bool hasUppercase = false;
                bool hasDigit = false;
                bool hasOther = false;

                foreach (char ch in password)
                {
                    if (Char.IsLower(ch))
                        hasLowecase = true;
                    else if (Char.IsUpper(ch))
                        hasUppercase = true;
                    else if (Char.IsDigit(ch))
                        hasDigit = true;
                    else hasOther = true;
                }

                if (!hasLowecase)
                    WriteWarning($"Consider adding some lower-case letters to the password");

                if (!hasUppercase)
                    WriteWarning($"Consider adding some upper-case letters to the password");

                if (!hasDigit)
                    WriteWarning($"Consider adding some digits to the password");

                if (!hasOther)
                    WriteWarning($"Consider adding some special characters to the password");
            }

            return password;
        }

        public static WordCount PromptWordCount() => (WordCount)AnsiConsole.Prompt(
            new SelectionPrompt<int>()
            .Title($"[{ColorPrompt}]Choose word count[/]")
            .AddChoices(Enum.GetValues<WordCount>().Cast<int>()));

        public static void WriteKeyValue(string key, string? value) =>
            AnsiConsole.MarkupLine($"[{ColorDisabled}]{key} > [/][darkseagreen4_1][/][{ColorNote}]{value}[/]");

        public static void WriteError(string message) =>
            AnsiConsole.MarkupLine($"[{ColorError}]{message}[/]");

        public static void WriteError(string message, string details) =>
            AnsiConsole.MarkupLine($"[{ColorError}]{message}: [/][steelblue]{details}[/]");

        public static void WriteWarning(string message) =>
            AnsiConsole.MarkupLine($"[{ColorWarning}]{message}[/]");

        public static void WriteSuccess(string message) =>
            AnsiConsole.MarkupLine($"[{ColorSuccess}]{message}[/]");

        public static void WriteInfo(string text) =>
            AnsiConsole.MarkupLine($"[{ColorInfo}]{text}[/]");

        public static void WriteImportantInfo(string text) => AnsiConsole.MarkupLine($"[{ColorImportant}]{text}[/]");

        public static void WriteNote(string text) =>
            AnsiConsole.MarkupLine($"[{ColorNote}]{text}[/]");

        public static void WriteLine() => AnsiConsole.WriteLine();

        public static void WriteRaw(string text, ConsoleColor color = ConsoleColor.DarkGray)
        {
            var oldColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
        }

        public static Language PromptLanguage() => AnsiConsole.Prompt(new SelectionPrompt<Language>()
            .Title($"[{ColorPrompt}]Select seed language[/]")
            .AddChoices(Enum.GetValues<Language>().Where(x => x != Language.Unknown)));

        public static Wordlist PromptWordList()
        {
            var language = PromptLanguage();
            var wordListTask = Wordlist.LoadWordList(language);
            wordListTask.Wait();
            return wordListTask.Result;
        }

        public static Option PromptOptions(IEnumerable<Option> options)
        {
            WriteLine();

            Option option = AnsiConsole.Prompt(new SelectionPrompt<Option>()
                .Title($"[{ColorPrompt}]Choose action[/]")
                .AddChoices(options.Where(x => x.Enabled()))
                );

            if (!option.Confirm() || AnsiConsole.Confirm($"{option.Name}?", false))
            {
                option.Action();
            }

            return option;
        }

        public static string? PromptPath(string? recentPath = null)
        {
            string path = AnsiConsole.Prompt(new TextPrompt<string>($"[{ColorPrompt}]Enter file path[/]")
                .AllowEmpty()
                );

            return string.IsNullOrWhiteSpace(path) ? null : path;
        }

        public static Mnemonic? PromptSeedPhrase(string? prompt = null, bool allowValidChecksum = false)
        {
            string sentence = AnsiConsole.Prompt(new TextPrompt<string>(
                $"[{ColorPrompt}]{prompt ?? "Enter seed phrase"}: [/]").AllowEmpty()
                );

            sentence = StringHelper.TrimAndRemoveEndings(sentence);
            if (string.IsNullOrEmpty(sentence))
            {
                WriteWarning("Cancelled");
                return null;
            }

            try
            {
                var mnemonic = new Mnemonic(sentence);

                if (!allowValidChecksum && !mnemonic.IsValidChecksum)
                {
                    WriteError("Invalid checksum");
                    return null;
                }

                return mnemonic;
            }
            catch (NotSupportedException)
            {
                WriteError("Failed to parse the seed phrase",
                    "Entered words not found in bip39 word lists");
                return null;
            }
            catch (Exception ex)
            {
                WriteError("Failed to parse the seed phrase", ex.Message);
                return null;
            }
        }
    }
}
