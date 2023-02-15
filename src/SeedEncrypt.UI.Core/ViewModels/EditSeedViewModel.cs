using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NBitcoin;
using Newtonsoft.Json.Linq;
using SeedEncrypt;
using SeedEncrypt.UI.Core.Services;
using SeedEncrypt.UI.Core.Services.UIService;
using SeedEncryptWinApp.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SeedEncrypt.UI.Core.ViewModels
{
    public partial class EditSeedViewModel : ObservableObject
    {
        readonly IUIService _uiService;
        readonly IAppSettings _settings;
        Wordlist _wordList;

        [ObservableProperty]
        string? _currentWord;

        [ObservableProperty]
        List<string>? _suggestedWords;

        [ObservableProperty]
        ObservableCollection<WordEditViewModel> _words = new();

        [ObservableProperty]
        List<int> _wordCountList = Enum.GetValues(typeof(WordCount)).Cast<int>().ToList();

        [ObservableProperty]
        List<SeedLangViewModel> _seedLangList;

        [ObservableProperty]
        SeedLangViewModel? _seedLang;

        [ObservableProperty]
        int _wordsPerRowOrCol;

        [ObservableProperty]
        bool _notInList;

        [ObservableProperty]
        bool _isChecksumValid;

        [ObservableProperty]
        bool _isComplete;

        [ObservableProperty]
        bool _isEmpty;

        [ObservableProperty]
        string? _password;

        [ObservableProperty]
        int _nonemptyWordCount;

        [ObservableProperty]
        int _seedWordCount = 24;

        [ObservableProperty]
        string? _primaryCipher;

        [ObservableProperty]
        string? _secondaryCipher;

        [ObservableProperty]
        bool _isEncrypted;

        Puzzle? _puzzle;
        Puzzle? Puzzle
        {
            get => _puzzle;
            set
            {
                if (value == null)
                {
                    PrimaryCipher = null;
                    SecondaryCipher = null;
                    IsEncrypted = false;
                }
                else
                {
                    PrimaryCipher = value.Primary.ToString();
                    SecondaryCipher = value.Secondary.ToString();
                    IsEncrypted = true;
                }

                _puzzle = value;
            }
        }

        public EditSeedViewModel(IUIService uiService, IAppSettings settings)
        {
            _uiService = uiService;
            _settings = settings;

            _seedWordCount = (int)_settings.DefaultSeedWordCount;
            
            _seedLangList = Enum
                .GetValues(typeof(Language))
                .Cast<Language>()
                .Where(x => x != Language.Unknown)
                .Select(x => new SeedLangViewModel(x, SeedLangToString.ToShortString(x), SeedLangToString.ToLongString(x)))
                .ToList();

            var seedLang = _settings.DefaultSeedLang;

            _seedLang = _seedLangList.FirstOrDefault(x => x.Lang == seedLang) ?? _seedLangList[0];
            _wordList = LoadWordList(_seedLang.Lang);

            InitWords();            
        }

        partial void OnSeedLangChanged(SeedLangViewModel? value)
        {
            if (SeedLang != null)
            {
                _wordList = LoadWordList(SeedLang.Lang);

                if (!IsEmpty)
                {
                    foreach (var word in Words)
                        word.Text = null;

                    CheckComplete();
                }
            }
        }

        Wordlist LoadWordList(Language lang)
        {
            var loadLangTask = Wordlist.LoadWordList(lang);
            
            loadLangTask.Wait();

            return loadLangTask.Result as Wordlist;
        }

        public void OnCurrentWordChangedByUser()
        {
            string? currentWord = CurrentWord?.Normalize(NormalizationForm.FormKD);

            NotInList = false;

            SuggestedWords = string.IsNullOrEmpty(currentWord) ? null :
                _wordList.GetWords().Where(x => x.StartsWith(currentWord, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        [RelayCommand]
        public void AddCurrentWord()
        {
            string? currentWord = CurrentWord?.Normalize(NormalizationForm.FormKD).ToLower();

            NotInList = !_wordList.GetWords().Any(x => x.Equals(currentWord, StringComparison.OrdinalIgnoreCase));

            if (NotInList)
                return;

            if (Words.FirstOrDefault(x => x.Text == null) is WordEditViewModel word)
            {
                word.Text = currentWord;

                CheckComplete();
            }

            CurrentWord = null;
            SuggestedWords = null;
        }

        void InitWords()
        {
            if (Words.Count != SeedWordCount)
            {
                while (Words.Count > SeedWordCount)
                {
                    Words.RemoveAt(Words.Count - 1);
                }

                while (Words.Count < SeedWordCount)
                {
                    Words.Add(new WordEditViewModel()
                    {
                        Order = Words.Count + 1,
                    });
                }

                WordsPerRowOrCol = 3;

                OnWordsChanged();
                CheckComplete();
            }
        }

        [RelayCommand]
        public async void Generate()
        {
            if (IsEmpty || await _uiService.ShowLostSeedConfirmation())
            {
                Apply(new Mnemonic(_wordList, (WordCount)Words.Count));
            }
        }

        [RelayCommand]
        public void Apply(Mnemonic mnemonic)
        {
            if (_wordList.Name != mnemonic.WordList.Name)
            {
                var lang = LangNameToLang(mnemonic.WordList.Name);
                SeedLang = SeedLangList.FirstOrDefault(x => x.Lang == lang);
            }

            if (Words.Count != mnemonic.Words.Length)
                SeedWordCount = mnemonic.Words.Length;

            int i = 0;

            foreach (var word in Words)
            {
                word.Text = mnemonic.Words[i++];
            }

            CheckComplete();
            OnWordsChanged();
        }

        void CheckComplete()
        {
            int emptyCount = Words.Count(x => x.Text == null);

            if (emptyCount == 0)
            {
                var mnemonic = ToMnemonic();

                IsComplete = true;
                IsChecksumValid = mnemonic.IsValidChecksum;
                IsEmpty = false;
            }
            else
            {
                IsComplete = false;
                IsChecksumValid = false;
                IsEmpty = emptyCount == Words.Count;
            }
        }

        void OnWordsChanged()
        {
            Puzzle = null;
        }

        public string ToSeedPhraseString() => string.Join(_wordList.Space, Words.Select(x => x.Text));

        public Mnemonic ToMnemonic() => new Mnemonic(ToSeedPhraseString());

        [RelayCommand]
        public async void ClearAll()
        {
            if (!IsEmpty && (await _uiService.ShowLostSeedConfirmation()))
            {
                foreach (var word in Words)
                {
                    word.Text = null;
                }

                CheckComplete();
            }
        }

        [RelayCommand]
        public void DeleteLastWord()
        {
            if (Words.LastOrDefault(x => x.Text != null) is WordEditViewModel word)
            {
                word.Text = null;

                CheckComplete();
                OnWordsChanged();
            }
        }

        [RelayCommand]
        public async void Encrypt()
        {
            Puzzle? puzzle = null;

            try
            {
                await _uiService.ShowPasswordDialog((Action<PasswordDialogViewModel>)(vm =>
                {
                    vm.Title = _uiService.GetString("EncryptDialogTitle");
                    vm.Action = ctx =>
                    {
                        puzzle = Puzzle.Create(ToMnemonic(), ctx.Password);
                    };
                }));

                Puzzle = puzzle;
            }
            catch (Exception ex)
            {
                await _uiService.ShowMessageBox(ex.Message, "Failed to encrypt the seed phrase");
            }
        }

        [RelayCommand]
        public void CopySeedPhraseAsPlainText()
        {
            _uiService.CopyTextToClipboard(ToSeedPhraseString());
        }

        [RelayCommand]
        public void CopyPrimaryCipher()
        {
            if (PrimaryCipher != null)
                _uiService.CopyTextToClipboard(PrimaryCipher);
        }

        [RelayCommand]
        public void CopySecondaryCipher()
        {
            if (SecondaryCipher != null)
                _uiService.CopyTextToClipboard(SecondaryCipher);
        }

        [RelayCommand]
        public async void Decrypt()
        {
            if (!IsEmpty && !(await _uiService.ShowLostSeedConfirmation()))
                return;

            try
            {
                var res = await _uiService.DecryptSeedPhraseAsync();

                if (res?.Mnemonic == null)
                    return;

                if (res.Password != null)
                    Password = res.Password;

                Apply(res.Mnemonic);
            }
            catch
            {
            }
        }

        [RelayCommand]
        public async void EnterSeed()
        {
            if (!IsEmpty && !(await _uiService.ShowLostSeedConfirmation()))
                return;

            try
            {
                var res = await _uiService.EnterSeedPhraseAsync();

                if (res?.Mnemonic == null)
                    return;

                Apply(res.Mnemonic);
            }
            catch
            {
            }
        }

        public static Language LangNameToLang(string langName)
        {
            switch (langName)
            {
                case "chinese_traditional":
                    return Language.ChineseTraditional;
                case "chinese_simplified":
                    return Language.ChineseSimplified;
                case "english":
                    return Language.English;
                case "japanese":
                    return Language.Japanese;
                case "spanish":
                    return Language.Spanish;
                case "french":
                    return Language.French;
                case "portuguese_brazil":
                    return Language.PortugueseBrazil;
                case "czech":
                    return Language.Czech;
                default:
                    throw new Exception($"Unsupported language: {langName}");

            }
        }

        public async Task OnSeedWordCountChangedByUser(int wordCount)
        {
            if (SeedWordCount == wordCount)
                return;

            if (!IsEmpty)
            {
                bool cancel = !await _uiService.ShowConfirmation(
                    _uiService.GetString("ConfirmChangeWordCount"),
                    _uiService.GetString("ConfirmSeedNotEmpty"));

                if (cancel)
                {
                    OnPropertyChanged(nameof(SeedWordCount));
                    return;
                }
            }

            SeedWordCount = wordCount;
        }

        public async Task OnSeedLangChangedByUser(SeedLangViewModel seedLang)
        {
            if (SeedLang == seedLang)
                return;

            if (!IsEmpty && !await _uiService.ShowLostSeedConfirmation())
            {
                OnPropertyChanged(nameof(SeedLang));
                return;
            }

            SeedLang = seedLang;
        }

        partial void OnSeedWordCountChanged(int value)
        {
            InitWords();
        }
    }
}
