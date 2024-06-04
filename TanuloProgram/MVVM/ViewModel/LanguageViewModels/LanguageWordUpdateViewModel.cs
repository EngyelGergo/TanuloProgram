using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using TanuloProgram.Core;
using TanuloProgram.MVVM.Model;
using TanuloProgram.Services;

namespace TanuloProgram.MVVM.ViewModel.LanguageViewModels
{
    public class LanguageWordUpdateViewModel : Core.ViewModel
    {
        public ObservableCollection<Word> LanguageWords { get; set; }
        private Word? LastWord { get; set; }
        private const string _EXIST_ERROR = "Nem sikerült, mert ez az elempár már létezik!";
        private const string _NULL_ERROR = "Nem lehet egyik mező sem üres!";
        private const string _UPDATE_LABEL = "Egy elem módosítva lett.";
        private const string _DELETE_LABEL = "Egy elem törölve lett.";
        private string LanguageType { get; set; }


        private int _selectedIndex = -1;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    OnPropertyChange(nameof(SelectedIndex));
                    OnPropertyChange(nameof(NativeWordTB));
                    OnPropertyChange(nameof(ForeignWordTB));
                    OnPropertyChange(nameof(NativeTBEnable));
                    OnPropertyChange(nameof(ForeignTBEnable));
                    OnPropertyChange(nameof(WordEnable));
                    OnPropertyChange(nameof(SentenceEnable));
                    OnPropertyChange(nameof(DeleteEnable));
                    OnPropertyChange(nameof(ModifyEnable));
                    if (_selectedIndex != -1 && LanguageWords[SelectedIndex].IsWord == 1)
                    {
                        ReplyLabel = "";
                        OnWordChecked();
                    }
                    else if(_selectedIndex != -1)
                    {
                        ReplyLabel = "";
                        OnSentenceChecked();
                    }
                }
            }
        }

        public bool DeleteEnable
        {
            get
            {
                if (SelectedIndex >= 0 && LanguageWords.Count > SelectedIndex)
                {
                    return true;
                }
                return false;
            }
            set { }
        }

        public bool ModifyEnable
        {
            get
            {
                if (SelectedIndex >= 0 && LanguageWords.Count > SelectedIndex)
                {
                    return true;
                }
                return false;
            }
            set { }
        }

        public bool WordEnable
        {
            get
            {
                if (SelectedIndex >= 0 && LanguageWords.Count > SelectedIndex)
                {
                    return true;
                }
                return false;
            }
            set { }
        }
        public bool SentenceEnable
        {
            get
            {
                if (SelectedIndex >= 0 && LanguageWords.Count > SelectedIndex)
                {
                    return true;
                }
                return false;
            }
            set { }
        }

        public bool NativeTBEnable
        {
            get
            {
                if (SelectedIndex >= 0 && LanguageWords.Count > SelectedIndex)
                {
                    return true;
                }
                return false;
            }
            set { }
        }

        public bool ForeignTBEnable
        {
            get
            {
                if (SelectedIndex >= 0 && LanguageWords.Count > SelectedIndex)
                {
                    return true;
                }
                return false;
            }
            set { }
        }

        private string _nativeWordTB = "";
        private int _nativeWordID = -1;
        public string NativeWordTB
        {
            get
            {
                if (SelectedIndex >= 0 && LanguageWords.Count > SelectedIndex && _nativeWordID != SelectedIndex)
                {
                    _nativeWordID = SelectedIndex;
                    _nativeWordTB = LanguageWords[SelectedIndex].NativeWord;
                    return _nativeWordTB;
                }
                return _nativeWordTB;
            }
            set
            {
               _nativeWordTB = value;
                OnPropertyChange();
            }
        }

        private string _foreignWordTB = "";
        private int _foreignWordID = -1;
        public string ForeignWordTB
        {
            get
            {
                if (SelectedIndex >= 0 && LanguageWords.Count > SelectedIndex && _foreignWordID != SelectedIndex)
                {
                    _foreignWordID = SelectedIndex;
                    _foreignWordTB = LanguageWords[SelectedIndex].ForeignWord;
                    return _foreignWordTB;
                }
                return _foreignWordTB;
            }
            set 
            {
                _foreignWordTB = value;
                OnPropertyChange();
            }
        }

        private bool _isWordChecked;
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

        private bool _isSentenceChecked;
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

        private object _languageLabel;
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

        private string _replyLabel = "";
        public string ReplyLabel
        {
            get => _replyLabel;
            set
            {
                _replyLabel = value;
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
        public RelayCommand DeleteElementCommand { get; set; }
        public RelayCommand OnWordCheckedCommand { get; set; }
        public RelayCommand OnSentenceCheckedCommand { get; set; }
        public RelayCommand OnWordUncheckedCommand { get; set; }
        public RelayCommand OnSentenceUncheckedCommand { get; set; }

        public RelayCommand EditElementCommand { get; set; }

        public LanguageWordUpdateViewModel(INavigationService navService, string languageType = "dummy")
        {
            LanguageType = languageType;
            if (LanguageType !=  "dummy")
            {  
                LanguageWords = new SQLiteConnectionSimple().LoadData(languageType);
                LanguageWords = new ObservableCollection<Word>(LanguageWords.OrderBy(word => word.NativeWord));
            }
            LanguageLabel = LanguageType.Replace("_", " ");
            TitleLabel = $"{LanguageType.Replace("_", " ")} nyelv szerkesztése";
            Navigation = navService;
            BackToPreviousViewCommand = new RelayCommand(o => { Navigation.NavigateTo<SelectedLanguageViewModel>(languageType);}, o => true);
            DeleteElementCommand = new RelayCommand(o => { SetReplyLabel(_DELETE_LABEL); DeleteElement(); }, o => true);
            OnWordCheckedCommand = new RelayCommand(o => { OnWordChecked(); }, o => true);
            OnSentenceCheckedCommand = new RelayCommand(o => { OnSentenceChecked(); }, o => true);
            OnWordUncheckedCommand = new RelayCommand(o => { OnWordUnchecked(); }, o => true);
            OnSentenceUncheckedCommand = new RelayCommand(o => { OnSentenceUnchecked(); }, o => true);
            EditElementCommand = new RelayCommand(o => { UpdateElement(languageType) ;}, o => true);
        }

        private void SetReplyLabel(string txt)
        {
            ReplyLabel = txt;
        }

        private void UpdateElement(string tableName)
        {    
            Word w_new;
            Word w_old = LanguageWords[SelectedIndex];
            if (string.IsNullOrEmpty(NativeWordTB) || string.IsNullOrWhiteSpace(NativeWordTB) || string.IsNullOrEmpty(ForeignWordTB) || string.IsNullOrWhiteSpace(ForeignWordTB))
            {
                NativeWordTB = w_old.NativeWord;
                ForeignWordTB = w_old.ForeignWord;
                if ((w_old.IsWord == 1) ? true : false) OnWordChecked();
                else OnSentenceChecked();
                SetReplyLabel(_NULL_ERROR);
                return;
            }
            if (LastWord != null && LastWord.NativeWord == w_old.NativeWord && LastWord.ForeignWord == w_old.ForeignWord)
            {
                w_new = CreateWord(w_old.Id, IsWordChecked, w_old.NativeWord, w_old.ForeignWord);
                LastWord = w_new;
            }
            else
            {
                if (SQLiteConnectionSimple.ElementPairExists(tableName, NativeWordTB, ForeignWordTB, IsWordChecked ? 1 : 0))
                {
                    NativeWordTB = w_old.NativeWord;
                    ForeignWordTB = w_old.ForeignWord;
                    IsWordChecked = (w_old.IsWord == 1) ? true : false;
                    if ((w_old.IsWord == 1) ? true : false) OnWordChecked();
                    else OnSentenceChecked();
                    SetReplyLabel(_EXIST_ERROR);
                    return;
                }
                    w_new = CreateWord(w_old.Id, IsWordChecked, NativeWordTB.Trim(), ForeignWordTB.Trim());
            }
            LanguageWords[SelectedIndex] = w_new;
            new SQLiteConnectionSimple().UpdateTable(LanguageType, w_new);
            LogFile.LogData(LogDataType.ChangedElement);
            SetReplyLabel(_UPDATE_LABEL);
            DefaultItemsState();
        }

        private Word CreateWord(int id, bool _isWord, string _native, string _foreign)
        {
            Word w = new();
            w.Id = id;
            w.NativeWord = _native;
            w.ForeignWord = _foreign;
            w.IsWord = _isWord ? 1 : 0;
            return w;
        }

        private void DeleteElement()
        {
            Word w = LanguageWords[SelectedIndex];
            LanguageWords.RemoveAt(SelectedIndex);
            new SQLiteConnectionSimple().DeleteData(LanguageType, w.Id);
            LogFile.LogData(LogDataType.DeletedElement);
            DefaultItemsState();
        }

        private void OnWordChecked()
        {
            if (_selectedIndex != -1)
            {
                IsWordChecked = true;
                IsSentenceChecked = false;
            }
        }
        private void OnWordUnchecked()
        {
            if (!IsWordChecked && IsSentenceChecked && _selectedIndex != -1)
            {
                return;
            }
            else if (_selectedIndex != -1)
            {
                IsWordChecked = true;
            }
        }

        private void OnSentenceChecked()
        {
            if (_selectedIndex != -1)
            {
                IsSentenceChecked = true;
                IsWordChecked = false; 
            }
        }
        private void OnSentenceUnchecked()
        {
            if (!IsSentenceChecked && IsWordChecked && _selectedIndex != -1)
            {
                return;
            }
            else if (_selectedIndex != -1) 
            {
                IsSentenceChecked = true;
            }
            
        }

        private void DefaultItemsState()
        {
            SelectedIndex = -1;
            IsWordChecked = false;
            IsSentenceChecked = false;
            _nativeWordID = -1;
            _foreignWordID = -1;
            NativeWordTB = "";
            ForeignWordTB = "";
        }

    }
}
