using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TanuloProgram.Core;
using TanuloProgram.MVVM.Model;
using TanuloProgram.Services;

namespace TanuloProgram.MVVM.ViewModel.LanguageLobbyViewModels
{
    public class LanguageCompletionViewModel : Core.ViewModel
    {
        private const string _DEFAULT_LANGUAGE = "Magyar";
        private const string _DEFAULT_STARTBUTTON_LABEL = "Új feladat indítása";
        private const string _CANNOT_START_ERROR_LABEL = "(Csak akkor indítható, ha legalább 4 szó szerepel az adatbázisban és legalább 1 mondat.)";
        private LanguageType _language;
        private int _allSentences;
        private int _allWords = 0;
        private ObservableCollection<IInsiderView> VMs;
        private ObservableCollection<Word> Words;
        private SliderProperties _upperSlider = new();
        private CheckBoxProperties _checkBoxState = new();
        private CompletionViewModel _selectedVM;
        private string _typeChangeLabel;
        private string _startButtonLabel = "Indítás";
        private bool _maximumSentence;
        private string _errorLabel;
        private Visibility _selectedVisibility = Visibility.Hidden;
        private INavigationService _navigation;
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

        public bool MaximumSentence
        {
            get => _maximumSentence;
            set
            {
                _maximumSentence = value;
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

        public CompletionViewModel SelectedVM
        {
            get => _selectedVM;
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
        public RelayCommand StartCompletionCommand { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand PreviousCommand { get; set; }
        public RelayCommand ResultViewCommand { get; set; }

        public LanguageCompletionViewModel(INavigationService navService, string tableName)
        {
            CustomMethods.ResizeApplicationMainWindow(WindowState.Maximized, WindowStyle.ToolWindow);
            Navigation = navService;

            Words = new SQLiteConnectionSimple().LoadData(tableName);
            _allSentences = CalculateMaxSententece(Words);
            _allWords = CalculateAllWords(Words);

            _language = CustomMethods.SetCurrentLanguages(tableName, _DEFAULT_LANGUAGE);
            SetCurrentLanguage(_language);

            CalculateSliderValue(_allSentences);

            BackToPreviousViewCommand = new RelayCommand(o => { Navigation.NavigateTo<LanguageLobbyViewModel>(tableName);}, o => true);
            StartCompletionCommand = new RelayCommand(o => 
            {
                if (UpperSlider.Value > 0 && _allWords >= 4) 
                {
                    ErrorLabel = "";
                    CreateCompletions();
                }
                else ErrorLabel = _CANNOT_START_ERROR_LABEL;
            }, o => true);
            NextCommand = new RelayCommand(o => { SelectedVM = Paging.NextView(SelectedVM, VMs) as CompletionViewModel; }, o => true);
            PreviousCommand = new RelayCommand(o => { SelectedVM = Paging.PreviousView(SelectedVM, VMs) as CompletionViewModel; }, o => true);
            ResultViewCommand = new RelayCommand(o => { Navigation.NavigateTo<LanguageResultViewModel>(ResultOfTest() + $":{tableName}"); }, o => true);
        }

        private int CalculateAllWords(ObservableCollection<Word> words)
        {
            int count = 0;
            foreach (Word w in Words)
            {
                if (w.IsWord == 1)
                {
                    count++;
                }
            }
            return count;
        }

        private void SetCurrentLanguage(LanguageType language)
        {
            TypeChangeLabel = language.ForeignLanguage;
        }

        private int CalculateMaxSententece(ObservableCollection<Word> words)
        {
            int count = 0;
            foreach (Word w in Words)
            {
                if (w.IsWord == 0)
                {
                    count++;
                }
            }
            return count;
        }

        private string ResultOfTest()
        {
            int succeeded = 0;
            foreach (CompletionViewModel viewModel in VMs)
            {
                int result = AnalyseViewModelResult(viewModel);
                succeeded += result;
            }
            LogFile.LogData(LogDataType.CompletionRuns);
            LogFile.LogData(LogDataType.CompletionGoodAnswer, succeeded);
            LogFile.LogData(LogDataType.CompletionBadAnswer, (VMs.Count - succeeded));
            LogFile.LogData(LogDataType.CompletionResult, $"{VMs.Count}/{succeeded}");
            return $"{VMs.Count}/{succeeded}";
        }

        private int AnalyseViewModelResult(CompletionViewModel viewModel)
        {
            int succeeded = 0;
            string answer = "";
            if (!string.IsNullOrEmpty(viewModel.AnswerText))
            {
                answer = viewModel.AnswerText.RemoveSentence().ToLower();
            }             
            string solution = viewModel.missingWord.RemoveSentence().ToLower();
            if (answer == solution)
            {
                ++succeeded;
                return succeeded;
            }        
            return succeeded;
        }


        private void CreateCompletions()
        {
            int i = 0;
            VMs = new();
            Random rnd = new();
            List<int> uniqueNumbers = CustomMethods.GenerateUniqueIndex(UpperSlider.Value, false,true, Words);
            int amount = uniqueNumbers.Count;
            foreach (int chosenNum in uniqueNumbers)
            {
                VMs.Add(new CompletionViewModel(amount, i + 1, chosenNum, uniqueNumbers, Words, _language, _language.ForeignLanguage));
                i++;
            }
            SelectedVM = VMs[0] as CompletionViewModel;
            StartButtonLabel = _DEFAULT_STARTBUTTON_LABEL;
            SelectedVisibility = Visibility.Visible;
        }

        private void CalculateSliderValue(int amount)
        {
            UpperSlider.Range = amount;
            UpperSlider.MaximumValue = UpperSlider.Range;
            if (amount > 0) { UpperSlider.MinimumValue = 1; }
            else UpperSlider.MinimumValue = 0;

            if (UpperSlider.Range <= UpperSlider.Value)
                UpperSlider.Value = (UpperSlider.Value >= UpperSlider.Range) ? (UpperSlider.Value = UpperSlider.MaximumValue) : 1;
            else if (MaximumSentence)
                UpperSlider.Value = UpperSlider.MaximumValue;
            else
            {
                if (amount > 0) UpperSlider.Value = 1;
                else UpperSlider.Value = 0;
            }
        }
    }
}
