using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TanuloProgram.Core;
using TanuloProgram.MVVM.Model;
using TanuloProgram.Services;

namespace TanuloProgram.MVVM.ViewModel.LanguageLobbyViewModels
{
    public class LanguageSubstitutionViewModel : Core.ViewModel
    {
        private const string _DEFAULT_LANGUAGES = "Magyar";
        private const string _DEFAULT_STARTBUTTON_LABEL = "Új feladat indítása";
        private const string _CANNOT_START_ERROR_LABEL = "(Legalább 1 szónak, illetve mondatoknál legalább 1 mondatnak szerepelnie kell az adatbázisban.)";
        private LanguageType _language;
        private int _wordAmount;
        private ObservableCollection<IInsiderView> SubsVMs;
        private ObservableCollection<Word> Words;
        private SliderProperties _upperSlider = new();
        private CheckBoxProperties _checkBoxState = new();
        private SubstitutionViewModel _selectedVM;
        private string _typeChangeLabel;
        private string _startButtonLabel = "Indítás";
        private string _errorLabel;
        private bool _maximumWords;
        private Visibility _selectedVisibility = Visibility.Hidden;
        private INavigationService _navigation;
        private bool _isHighlighted;
        public bool IsHighlighted
        {
            get => _isHighlighted;
            set
            {
                _isHighlighted = value;
                OnPropertyChange(); ;
            }
        }
        public string ErrorLabel
        {
            get => _errorLabel;
            set
            {
                _errorLabel = value;
                OnPropertyChange();
            }
        }
        public string StartButtonLabel
        {
            get => _startButtonLabel;
            set
            {
                _startButtonLabel = value;
                OnPropertyChange();
            }
        }

        public string TypeChangeLabel
        {
            get => _typeChangeLabel;
            set
            {
                _typeChangeLabel = value;
                OnPropertyChange();
            }
        }

        public bool MaximumWords
        {
            get => _maximumWords;
            set
            {
                _maximumWords = value;
                OnPropertyChange();
            }
        }

        public CheckBoxProperties CheckBoxState
        {
            get => _checkBoxState;
            set
            {
                _checkBoxState = value;
                OnPropertyChange();
            }
        }

        public SliderProperties UpperSlider
        {
            get => _upperSlider;
            set
            {
                _upperSlider = value;
                OnPropertyChange();
            }
        }

        public SubstitutionViewModel SelectedVM
        {
            get =>_selectedVM;
            set
            {
                _selectedVM = value;
                OnPropertyChange();
            }
        }

        public Visibility SelectedVisibility
        {
            get => _selectedVisibility;
            set
            {
                _selectedVisibility = value;
                OnPropertyChange();
            }
        }

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
        public RelayCommand StartSubstitutionCommand { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand PreviousCommand { get; set; }
        public RelayCommand LeftButtonClickCommand { get; set; }
        public RelayCommand RightButtonClickCommand { get; set; }
        public RelayCommand ResultViewCommand { get; set; }
        public RelayCommand HighlightOnCommand { get; set; }
        public RelayCommand HighlightOffCommand { get; set; }

        public LanguageSubstitutionViewModel(INavigationService navService, string tableName)
        {
            CustomMethods.ResizeApplicationMainWindow(WindowState.Maximized, WindowStyle.ToolWindow);
            Navigation = navService;
            Words = new SQLiteConnectionSimple().LoadData(tableName);
            _wordAmount = Words.Count;
            IsHighlighted = false;

            LanguageEvent.OnlyWordEvent += OnOnlyWordEvent;
            LanguageEvent.OnlySentenceEvent += OnOnlySentenceEvent;
            LanguageEvent.NonSelectedElementEvent += OnNonSelecteedElementEvent;
            _language = CustomMethods.SetCurrentLanguages(tableName, _DEFAULT_LANGUAGES);
             SetCurrentLanguges(_language);
            CalculateSliderValues(_wordAmount);

            BackToPreviousViewCommand = new RelayCommand(o => {
                LanguageEvent.OnlyWordEvent -= OnOnlyWordEvent;
                LanguageEvent.OnlySentenceEvent -= OnOnlySentenceEvent;
                LanguageEvent.NonSelectedElementEvent -= OnNonSelecteedElementEvent;
                Navigation.NavigateTo<LanguageLobbyViewModel>(tableName);
            }, o => true);
            LeftButtonClickCommand = new RelayCommand(o => { TypeChangeLabel = CustomMethods.SelectNextLanguageType(_language,TypeChangeLabel); }, o => true);
            RightButtonClickCommand = new RelayCommand(o => { TypeChangeLabel = CustomMethods.SelectPreviousLanguageType(_language, TypeChangeLabel); }, o => true);
            StartSubstitutionCommand = new RelayCommand(o => 
            {
                if (UpperSlider.Value > 0)
                {
                    ErrorLabel = "";
                    if (SubsVMs != null && SubsVMs.Count > 0)
                        UnsubscribeFromEvents(SubsVMs);
                    IsHighlighted = false;
                    CreateSubstitutions();
                }
                else ErrorLabel = _CANNOT_START_ERROR_LABEL;

            }, o => true);
            NextCommand = new RelayCommand(o => { SelectedVM = Paging.NextView(SelectedVM, SubsVMs) as SubstitutionViewModel; }, o => true);
            PreviousCommand = new RelayCommand(o => { SelectedVM = Paging.PreviousView(SelectedVM, SubsVMs) as SubstitutionViewModel; }, o => true);
            ResultViewCommand = new RelayCommand(o =>
            {
                LanguageEvent.OnlyWordEvent -= OnOnlyWordEvent;
                LanguageEvent.OnlySentenceEvent -= OnOnlySentenceEvent;
                LanguageEvent.NonSelectedElementEvent -= OnNonSelecteedElementEvent;
                Navigation.NavigateTo<LanguageResultViewModel>(ResultOfTest() + $":{tableName}");
            }, o => true);
            HighlightOnCommand = new RelayCommand(o => {
                LanguageEvent.TriggerHighlightEvent(this, Application.Current.Resources["HightlightColor"] as SolidColorBrush);
            }, o => true);
            HighlightOffCommand = new RelayCommand(o => {
                LanguageEvent.TriggerHighlightEvent(this, Application.Current.Resources["PrimaryTextColor"] as SolidColorBrush);
            }, o => true);
        }

        private void UnsubscribeFromEvents(ObservableCollection<IInsiderView> subList)
        {
            foreach (SubstitutionViewModel item in subList)
            {
                LanguageEvent.HighlightEvent -= item.OnHighlightEvent;
            }
        }
        private string ResultOfTest()
        {
            Dictionary<string, List<string>> nativePair = new Dictionary<string, List<string>>();

            foreach (var word in Words)
            {
                if (!nativePair.ContainsKey(word.NativeWord))
                {
                    nativePair[word.NativeWord] = new List<string>();
                }
                nativePair[word.NativeWord].Add(word.ForeignWord);
            }
            Dictionary<string, List<string>> foreignPair = new Dictionary<string, List<string>>();

            foreach (var word in Words)
            {
                if (!foreignPair.ContainsKey(word.ForeignWord))
                {
                    foreignPair[word.ForeignWord] = new List<string>();
                }

                foreignPair[word.ForeignWord].Add(word.NativeWord);
            }

            int succeeded = 0;
            int amountOfWords = 0;
            if (TypeChangeLabel == _language.NativeLanguage)
            {
                foreach (SubstitutionViewModel viewModel in SubsVMs)
                {
                    int result = CheckViewModel(viewModel, nativePair);
                    succeeded += result;
                }
            }
            else if (TypeChangeLabel == _language.ForeignLanguage)
            {
                foreach (SubstitutionViewModel viewModel in SubsVMs)
                {
                    int result = CheckViewModel(viewModel, foreignPair);
                    succeeded += result;
                }
            }
            else if (TypeChangeLabel == _language.Variable)
            {
                var mergedPair = nativePair
                .Concat(foreignPair)
                .GroupBy(x => x.Key)
                .ToDictionary(g => g.Key, g => g.First().Value);
                foreach (SubstitutionViewModel viewModel in SubsVMs)
                {
                    int result = CheckViewModel(viewModel, mergedPair);
                    succeeded += result;
                }
            }
            UnsubscribeFromEvents(SubsVMs);
            LogFile.LogData(LogDataType.SubstitutionRuns);
            LogFile.LogData(LogDataType.SubstitutionGoodAnswer, succeeded);
            LogFile.LogData(LogDataType.SubstitutionBadAnswer, (SubsVMs.Count - succeeded));
            LogFile.LogData(LogDataType.SubstitutionResult, $"{SubsVMs.Count}/{succeeded}");
            return $"{SubsVMs.Count}/{succeeded}";

        }

        private int CheckViewModel(SubstitutionViewModel viewModel, Dictionary<string, List<string>> wordPair)
        {
            string answer = viewModel.AnswerText?.LowcaseOneLineText();
            string question = viewModel.QuestionText?.LowcaseOneLineText();

            if (wordPair.TryGetValue(question, out List<string> possibleAnswers))
            {
                if (possibleAnswers.Contains(answer))
                    return 1; 
            }

            return 0;
        }

        private void CreateSubstitutions()
        {
            int i = 0;
            SubsVMs = new();
            Random rnd = new();
            List<int> uniqueNumbers = CustomMethods.GenerateUniqueIndex(UpperSlider.Value, CheckBoxState.IsWordChecked, CheckBoxState.IsSentenceChecked, Words);
            int amount = uniqueNumbers.Count;
            foreach (int chosenNum in uniqueNumbers)
            {
                if (TypeChangeLabel == _language.NativeLanguage)
                    SubsVMs.Add(new SubstitutionViewModel(amount, i + 1, Words[chosenNum].NativeWord,_language.ForeignLanguage, _language.NativeLanguage));
                else if (TypeChangeLabel == _language.ForeignLanguage)
                    SubsVMs.Add(new SubstitutionViewModel(amount, i + 1, Words[chosenNum].ForeignWord, _language.ForeignLanguage, _language.NativeLanguage));
                else
                {
                    int num = rnd.Next(0, 2);
                    string chosenWord = (num == 0) ? Words[chosenNum].NativeWord: Words[chosenNum].ForeignWord;
                    string selected = (num == 0) ? _language.NativeLanguage : _language.ForeignLanguage;
                    SubsVMs.Add(new SubstitutionViewModel(amount, i + 1, chosenWord, _language.ForeignLanguage, selected));
                }
                i++;
            }
            SelectedVM = SubsVMs[0] as SubstitutionViewModel;
            StartButtonLabel = _DEFAULT_STARTBUTTON_LABEL;
            SelectedVisibility = Visibility.Visible;
        }


        public void OnOnlyWordEvent(object sender, EventArgs e)
        {
            int count = 0;
            foreach (Word w in Words)
            {
                if (w.IsWord == 1)
                {
                    count++;
                }
            }
            CalculateSliderValues(count);
        }
        private void OnOnlySentenceEvent(object? sender, EventArgs e)
        {
            int count = 0;
            foreach (Word w in Words)
            {
                if (w.IsWord == 0)
                {
                    count++;
                }
            }
            CalculateSliderValues(count);
        }
        private void OnNonSelecteedElementEvent(object? sender, EventArgs e)
        {
            CalculateSliderValues(Words.Count);
        }

        private void CalculateSliderValues(int amount)
        {
            UpperSlider.Range = amount;
            UpperSlider.MaximumValue = UpperSlider.Range;
            if (amount > 0) { UpperSlider.MinimumValue = 1; }
            else UpperSlider.MinimumValue = 0;

            if (UpperSlider.Range <= UpperSlider.Value)
                UpperSlider.Value = (UpperSlider.Value >= UpperSlider.Range) ? (UpperSlider.Value = UpperSlider.MaximumValue) : 1;
            else if (MaximumWords)
                UpperSlider.Value = UpperSlider.MaximumValue;
            else
            {
                if (amount > 0) UpperSlider.Value = 1;
                else UpperSlider.Value = 0;
            }
        }

        private void SetCurrentLanguges(LanguageType languages)
        {
            TypeChangeLabel = languages.NativeLanguage;
        }
    }
}
