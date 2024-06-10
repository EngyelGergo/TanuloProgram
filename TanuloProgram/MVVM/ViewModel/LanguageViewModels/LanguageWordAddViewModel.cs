using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TanuloProgram.Core;
using TanuloProgram.MVVM.Model;
using TanuloProgram.Services;

namespace TanuloProgram.MVVM.ViewModel.LanguageViewModels
{
    #nullable disable
    public class LanguageWordAddViewModel : Core.ViewModel
    {
        private const string _SUCCESFUL = "Hozzáadva egy új pár az adatbázishoz!";
        private const string _SUCCESFUL_MULTIPLE = "Hozzáadva több új elempár az adatbázishoz!";
        private const string _MULTIPLE_ERROR = "Probléma az egyik taggal, próbálja újra";
        private const string _FILL_ERROR = "Töltse ki mindkét mezőt!";
        private const string _EXISTS_ERROR = "A beírt elempár már létezik az adatbázisban!";
        private object _languageLabel;
        private bool _isSentenceChecked;
        private bool _isWordChecked = true; // Alapértelmezetten a "szó" van kiválasztva
        private string _replyLabel = "";
        private string _foreignWord = "";
        private string _nativeWord = "";
        private bool _isTextBoxFocused;
        private string LanguageType { get; set; }

        public bool IsTextBoxFocused
        {
            get { return _isTextBoxFocused; }
            set
            {
                if (_isTextBoxFocused != value)
                {
                    _isTextBoxFocused = value;
                    OnPropertyChange();
                }
            }
        }

        public string NativeWord
        {
            get => _nativeWord;
            set
            {
                _nativeWord = value;
                OnPropertyChange();
            }
        }
        public string ForeignWord
        {
            get => _foreignWord;
            set
            {
                _foreignWord = value;
                OnPropertyChange();
            }
        }
        public string ReplyLabel
        {
            get => _replyLabel;
            set
            {
                _replyLabel = value;
                OnPropertyChange();
            }
        }
        public bool IsWordChecked
        {
            get { return _isWordChecked; }
            set
            {
                if (_isWordChecked != value)
                {
                    _isWordChecked = value;
                    OnPropertyChange(nameof(IsWordChecked));
                }
            }
        }
        public bool IsSentenceChecked
        {
            get { return _isSentenceChecked; }
            set
            {
                if (_isSentenceChecked != value)
                {
                    _isSentenceChecked = value;
                    OnPropertyChange(nameof(IsSentenceChecked));
                }
            }
        }
        public object LanguageLabel
        {
            get => _languageLabel;
            set
            {
                _languageLabel = value;
                OnPropertyChange();
            }
        }
        private object _titleLabel;
        public object TitleLabel
        {
            get => _titleLabel;
            set
            {
                _titleLabel = value;
                OnPropertyChange();
            }
        }

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChange();
            }
        }

        public RelayCommand BackToPreviousViewCommand { get; set; }
        public RelayCommand OnWordCheckedCommand { get; set; }
        public RelayCommand OnSentenceCheckedCommand { get; set; }
        public RelayCommand OnWordUncheckedCommand { get; set; }
        public RelayCommand OnSentenceUncheckedCommand { get; set; }
        public RelayCommand ReplyLabelCommand { get; set; }
        public RelayCommand CreateWordCommand { get; set; }

        public LanguageWordAddViewModel(INavigationService navService, string languageType = "dummy")
        {
            IsTextBoxFocused = true;
            LanguageType = languageType.Replace("_", " ");
            LanguageLabel = LanguageType;
            TitleLabel = $"Új szó létrehozása: {LanguageType} nyelven";
            Navigation = navService;
            BackToPreviousViewCommand = new RelayCommand(o => 
            { 
                Navigation.NavigateTo<SelectedLanguageViewModel>(languageType);
                
            }, o => true);
            OnWordCheckedCommand = new RelayCommand(o => { OnWordChecked(); }, o => true);
            OnSentenceCheckedCommand = new RelayCommand(o => { OnSentenceChecked(); }, o => true);
            OnWordUncheckedCommand = new RelayCommand(o => { OnWordUnchecked(); }, o => true);
            OnSentenceUncheckedCommand = new RelayCommand(o => { OnSentenceUnchecked(); }, o => true);
            CreateWordCommand = new RelayCommand(o => { WordsChecks(languageType); }, o => true);

        }

        public void SetReplyLabel(string result)
        {
            ReplyLabel = result;
        }

        public void WordsChecks(string lang)
        {
            if (!AreTextBoxesFilled())
            {
                SetReplyLabel(_FILL_ERROR);
                FocusTextBox();
                return;
            }
            string nativ = NativeWord.Trim();
            string foreign = ForeignWord.Trim();

            if (IsWordChecked && nativ.Contains(',')) 
                ProcessMultipleWords(lang, nativ, foreign);
            else 
                ProcessSingleWord(lang, nativ, foreign);
        }

        private void ProcessSingleWord(string lang, string nativ, string foreign)
        {
            {
                if (IsNotValidPairs(lang, nativ, foreign, _EXISTS_ERROR)) return;
                GenerateNewItemPairs(nativ, foreign, IsWordChecked);

                ClearTextBoxs();
                SetReplyLabel(_SUCCESFUL);
                FocusTextBox();
            }
        }

        private void ProcessMultipleWords(string lang, string nativ, string foreign)
        {
            List<string> separatorWords = nativ.Split(',')
                                    .Select(x => x.Trim())
                                    .Where(x => !string.IsNullOrWhiteSpace(x))
                                    .ToList();
            if  (separatorWords.Count <= 0) 
            {
                SetReplyLabel(_FILL_ERROR);
                FocusTextBox();
                return ;
            }
            foreach (var word in separatorWords)
            {
                if (IsNotValidPairs(lang, word, foreign, _MULTIPLE_ERROR)) return;
            }
            separatorWords.ForEach(word =>
            {
                GenerateNewItemPairs(word, foreign, IsWordChecked);
            });

            ClearTextBoxs();
            SetReplyLabel(_SUCCESFUL_MULTIPLE);
            FocusTextBox();
        }

        private bool AreTextBoxesFilled()
        {
            return !string.IsNullOrWhiteSpace(NativeWord) && !string.IsNullOrWhiteSpace(ForeignWord);
        }
        private void FocusTextBox()
        {
            IsTextBoxFocused = false;
            IsTextBoxFocused = true;
        }

        private bool IsNotValidPairs(string lang, string nativ, string foreign, string error)
        {
            if (SQLiteConnectionSimple.ElementPairExists(lang, nativ.ToLower(), foreign.ToLower()))
            {
                SetReplyLabel(error);
                FocusTextBox();
                return true;
            }
            else return false;
        }

        private void GenerateNewItemPairs(string nativeWord, string foreignWord, bool isWordChecked)
        {
            string table = LanguageType.Replace(" ", "_");
            Word w = SaveCurrentDataFromUI(nativeWord, foreignWord, isWordChecked);
            new SQLiteConnectionSimple().SaveData(table, w);
            
            LogFile.LogData(LogDataType.NewElementCreating);   
        }

        private Word SaveCurrentDataFromUI(string _native, string _foreign,bool _isWord)
        {
            Word w = new ();
            w.NativeWord = _native;
            w.ForeignWord = _foreign;
            w.IsWord = _isWord ? 1 : 0;
            return w ;
        }

        private void ClearTextBoxs()
        {
            NativeWord = "";
            ForeignWord = "";
        }

        public void OnWordChecked()
        {
            IsWordChecked = true;
            IsSentenceChecked = false;
        }

        public void OnWordUnchecked()
        {
            if (!IsWordChecked && IsSentenceChecked)
            {
                return;
            }
            IsWordChecked = true;
        }

        public void OnSentenceChecked()
        {
            IsSentenceChecked = true;
            IsWordChecked = false;
        }

        public void OnSentenceUnchecked()
        {
            if (!IsSentenceChecked && IsWordChecked)
            {
                return;
            }
            IsSentenceChecked = true;
        }



    }
}
