using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TanuloProgram.Core;
using TanuloProgram.MVVM.Model;
using TanuloProgram.MVVM.View.LanguageLobbyViews;
using TanuloProgram.MVVM.ViewModel.LanguageViewModels;
using TanuloProgram.Services;

namespace TanuloProgram.MVVM.ViewModel.LanguageLobbyViewModels.LanguagePairing
{
    public class LanguagePairingViewModel : Core.ViewModel
    {
        private const string _DEFAULT_LANGUAGE = "Magyar";
        private const string _DEFAULT_STARTBUTTON_LABEL = "Új feladat indítása";
        private const string _CANNOT_START_ERROR_LABEL = "(Legalább 1 szónak, illetve mondatoknál legalább 1 mondatnak szerepelnie kell az adatbázisban.)";
        private const int _TASK_COUNT = 5;
        private LanguageType _language;
        private int _wordAmount;
        private ObservableCollection<IInsiderView> PairingsList;
        private ObservableCollection<Word> Words;
        private SliderProperties _upperSlider = new ();
        private SliderProperties _lowerSlider = new ();
        private CheckBoxProperties _checkBoxState = new ();
        private PairingViewModel _selectedVM;
        private string _typeChangeLabel;
        private string _startButtonLabel = "Indítás";
        private bool _maximumWords;
        private bool _maximumTasks;
        private string _errorLabel;
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

        public bool MaximumTasks
        {
            get => _maximumTasks;
            set
            {
                _maximumTasks = value;
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

        public SliderProperties LowerSlider
        {
            get => _lowerSlider;
            set
            {
                _lowerSlider = value;
                OnPropertyChange();
            }
        }

        public PairingViewModel SelectedVM
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
        public RelayCommand StartPairingCommand { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand PreviousCommand { get; set; }
        public RelayCommand LeftButtonClickCommand { get; set; }
        public RelayCommand RightButtonClickCommand { get; set; }
        public RelayCommand ResultViewCommand { get; set; }
        public RelayCommand HighlightOnCommand { get; set; }
        public RelayCommand HighlightOffCommand { get; set; }


        public LanguagePairingViewModel(INavigationService navService, string tableName)
        {
            IsHighlighted = false;
            CustomMethods.ResizeApplicationMainWindow(WindowState.Maximized, WindowStyle.ToolWindow);
            Navigation = navService;
            Words = new SQLiteConnectionSimple().LoadData(tableName);
            StartButtonLabel = _startButtonLabel;
            _wordAmount = Words.Count;
            LanguageEvent.OnlyWordEvent += OnOnlyWordEvent;
            LanguageEvent.OnlySentenceEvent += OnOnlySentenceEvent;
            LanguageEvent.NonSelectedElementEvent += OnNonSelecteedElementEvent;
            _language = CustomMethods.SetCurrentLanguages(tableName, _DEFAULT_LANGUAGE);
            SetCurrentLanguage(_language);
            InitializingViewElements();

            BackToPreviousViewCommand = new RelayCommand(o => {
                LanguageEvent.OnlyWordEvent -= OnOnlyWordEvent;
                LanguageEvent.OnlySentenceEvent -= OnOnlySentenceEvent;
                LanguageEvent.NonSelectedElementEvent -= OnNonSelecteedElementEvent;
                Navigation.NavigateTo<LanguageLobbyViewModel>(tableName); 
            }, o => true);
            StartPairingCommand = new RelayCommand(o => 
            {
                if (UpperSlider.Value > 0 && LowerSlider.Value > 0) 
                {
                    ErrorLabel = "";
                    if (PairingsList != null && PairingsList.Count > 0)
                        UnsubscribeFromEvents(PairingsList);
                    IsHighlighted = false;
                    CreatePairing();
                }
                else ErrorLabel = _CANNOT_START_ERROR_LABEL;
                
            }, o => true);
            NextCommand = new RelayCommand(o => {SelectedVM = Paging.NextView(SelectedVM, PairingsList) as PairingViewModel; }, o => true);
            PreviousCommand = new RelayCommand(o => { SelectedVM = Paging.PreviousView(SelectedVM, PairingsList) as PairingViewModel; }, o => true);
            LeftButtonClickCommand = new RelayCommand(o => {TypeChangeLabel = CustomMethods.SelectNextLanguageType(_language,TypeChangeLabel); }, o => true);
            RightButtonClickCommand = new RelayCommand(o => { TypeChangeLabel = CustomMethods.SelectPreviousLanguageType(_language, TypeChangeLabel); }, o => true);
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

        private void UnsubscribeFromEvents(ObservableCollection<IInsiderView> pairingList)
        {
            foreach (PairingViewModel item in pairingList)
            {
                LanguageEvent.HighlightEvent -= item.OnHighlightEvent;
            }
        }
        private int NativeWordAmount(PairingViewModel viewModel)
        {
            int amount = 0;
            if (!string.IsNullOrEmpty(viewModel.LabelNativeOne[0]))
                amount++;
            if (!string.IsNullOrEmpty(viewModel.LabelNativeTwo[0]))
                amount++;
            if (!string.IsNullOrEmpty(viewModel.LabelNativeThree[0]))
                amount++;
            if (!string.IsNullOrEmpty(viewModel.LabelNativeFour[0]))
                amount++;
            if (!string.IsNullOrEmpty(viewModel.LabelNativeFive[0]))
                amount++;
            return amount;
        }
        private (int, int) CheckViewModel(PairingViewModel viewModel, Dictionary<string, List<string>> wordPair)
        {
            int succeeded = 0;
            int amountOfWords = NativeWordAmount(viewModel);

            if (!string.IsNullOrEmpty(viewModel.LabelNativeOne[0]) && wordPair.ContainsKey(viewModel.LabelNativeOne[0]) && wordPair[viewModel.LabelNativeOne[0]].Contains(viewModel.ItemsTButtonOne))
                succeeded++;
            if (!string.IsNullOrEmpty(viewModel.LabelNativeTwo[0]) && wordPair.ContainsKey(viewModel.LabelNativeTwo[0]) && wordPair[viewModel.LabelNativeTwo[0]].Contains(viewModel.ItemsTButtonTwo))
                succeeded++;
            if (!string.IsNullOrEmpty(viewModel.LabelNativeThree[0]) && wordPair.ContainsKey(viewModel.LabelNativeThree[0]) && wordPair[viewModel.LabelNativeThree[0]].Contains(viewModel.ItemsTButtonThree))
                succeeded++;
            if (!string.IsNullOrEmpty(viewModel.LabelNativeFour[0]) && wordPair.ContainsKey(viewModel.LabelNativeFour[0]) && wordPair[viewModel.LabelNativeFour[0]].Contains(viewModel.ItemsTButtonFour))
                succeeded++;
            if (!string.IsNullOrEmpty(viewModel.LabelNativeFive[0]) && wordPair.ContainsKey(viewModel.LabelNativeFive[0]) && wordPair[viewModel.LabelNativeFive[0]].Contains(viewModel.ItemsTButtonFive))
                succeeded++;
            return (succeeded, amountOfWords);
        }

        private string ResultOfTest()
        {
            //Dictionary<string, string> nativePair = Words.ToDictionary(w => w.NativeWord, w => w.ForeignWord);
            // Egy Dictionary<string, List<string>> létrehozása
            Dictionary<string, List<string>> nativePair = new Dictionary<string, List<string>>();

            // Words listából Dictionary<string, List<string>> létrehozása
            foreach (var word in Words)
            {
                // Ha a kulcs még nem létezik a Dictionary-ben, akkor hozzunk létre egy új List<string>-et
                if (!nativePair.ContainsKey(word.NativeWord))
                {
                    nativePair[word.NativeWord] = new List<string>();
                }

                // Adjuk hozzá a kulcshoz tartozó List<string>-hez az új értéket
                nativePair[word.NativeWord].Add(word.ForeignWord);
            }
            //Dictionary<string, string> foreignPair = Words.ToDictionary(w => w.ForeignWord, w => w.NativeWord);
            Dictionary<string, List<string>> foreignPair = new Dictionary<string, List<string>>();

            // Words listából Dictionary<string, List<string>> létrehozása
            foreach (var word in Words)
            {
                // Ha a kulcs még nem létezik a Dictionary-ben, akkor hozzunk létre egy új List<string>-et
                if (!foreignPair.ContainsKey(word.ForeignWord))
                {
                    foreignPair[word.ForeignWord] = new List<string>();
                }

                // Adjuk hozzá a kulcshoz tartozó List<string>-hez az új értéket
                foreignPair[word.ForeignWord].Add(word.NativeWord);
            }
            int succeeded = 0;
            int amountOfWords = 0;
            if (TypeChangeLabel == _language.NativeLanguage)
            {             
                foreach (PairingViewModel viewModel in PairingsList)
                {
                    (int,int) result = CheckViewModel(viewModel, nativePair);
                    succeeded += result.Item1;
                    amountOfWords += result.Item2;
                }
            }
            else if (TypeChangeLabel == _language.ForeignLanguage)
            {              
                foreach (PairingViewModel viewModel in PairingsList)
                {
                    (int, int) result = CheckViewModel(viewModel, foreignPair);
                    succeeded += result.Item1;
                    amountOfWords += result.Item2;
                }
            }
            else if (TypeChangeLabel == _language.Variable)
            {
                var mergedPair = nativePair.Concat(foreignPair).ToDictionary(x => x.Key, x => x.Value);
                foreach (PairingViewModel viewModel in PairingsList)
                {
                    (int, int) result = CheckViewModel(viewModel, mergedPair);
                    succeeded += result.Item1;
                    amountOfWords += result.Item2;
                }
            }
            foreach (PairingViewModel viewModel in PairingsList)
            {
                LanguageEvent.HighlightEvent -= viewModel.OnHighlightEvent;
            }    
            LogFile.LogData(LogDataType.PairRuns);
            LogFile.LogData(LogDataType.PairGoodAnswer, succeeded);
            LogFile.LogData(LogDataType.PairBadAnswer, (amountOfWords-succeeded));
            LogFile.LogData(LogDataType.PairResult, $"{amountOfWords}/{succeeded}");
            return $"{amountOfWords}/{succeeded}";

        }

        private void SetCurrentLanguage(LanguageType languages)
        {
            TypeChangeLabel = languages.NativeLanguage;
        }

        private void InitializingViewElements()
        {     
            int amountOfTask = _wordAmount / _TASK_COUNT;
            UpperSlider.Range = _wordAmount;
            UpperSlider.MaximumValue = UpperSlider.Range;
            if (_wordAmount > 0) { UpperSlider.MinimumValue = 1; }
            else UpperSlider.MinimumValue = 0;

            if (UpperSlider.Range <= UpperSlider.Value)
                UpperSlider.Value = (UpperSlider.Value >= UpperSlider.Range) ? (UpperSlider.Value = UpperSlider.MaximumValue) : 1;
            else if (MaximumWords)
                UpperSlider.Value = UpperSlider.MaximumValue;
            else
                if (_wordAmount > 0) UpperSlider.Value = 1;
                else UpperSlider.Value = 0;

            LowerSlider.Range = (_wordAmount % _TASK_COUNT == 0) ? amountOfTask : amountOfTask + 1;
            LowerSlider.MaximumValue = LowerSlider.Range;
            if (_wordAmount > 0) { LowerSlider.MinimumValue = 1; }
            else LowerSlider.MinimumValue = 0;

            if (LowerSlider.Range <= LowerSlider.Value)
                LowerSlider.Value = (LowerSlider.Value >= LowerSlider.Range) ? (LowerSlider.Value = LowerSlider.MaximumValue) : 1;
            else if (MaximumTasks)
                LowerSlider.Value = LowerSlider.MaximumValue;
            else
                if (_wordAmount > 0) LowerSlider.Value = 1;
                else LowerSlider.Value = 0;
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
            _wordAmount = count;
            InitializingViewElements();
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
            _wordAmount = count;
            InitializingViewElements();
        }
        private void OnNonSelecteedElementEvent(object? sender, EventArgs e)
        {
            _wordAmount = Words.Count;
            InitializingViewElements();
        }

        private void CreatePairing()
        {
            StartButtonLabel = _DEFAULT_STARTBUTTON_LABEL;
            SelectedVisibility = Visibility.Visible;
            PairingsList = new ObservableCollection<IInsiderView> ();
            CreateTheTask();
            SelectedVM = PairingsList[0] as PairingViewModel;
        }

        private void CreateTheTask()
        {
            int numberTasks = LowerSlider.Value;
            int numberElements = UpperSlider.Value;
            List<Word> tempList = new ();
            List<Word> listWords = new ();
            HashSet<int> uniqueNumbers = new ();
            // Specific unique index of words or sentence selected from the list
            uniqueNumbers = GenerateUniqueIndex(numberElements);
            foreach (var h in uniqueNumbers)
            {
                tempList.Add(Words[h]);
            }
            for (int i = 1; i < numberTasks + 1; i++)
            {
                for (int j = 0; j < _TASK_COUNT; j++)
                {      
                    if (tempList.Count > 0)
                    {
                        listWords.Add(tempList[0]);
                        tempList.RemoveAt(0);
                    }
                    else
                    {
                        break;
                    }
                }
                if (listWords.Count > 0)
                {
                    PairingsList.Add(new PairingViewModel(listWords, numberTasks, i, _language.ForeignLanguage, TypeChangeLabel));
                    listWords.Clear();
                }                    
                else
                {
                    foreach (var element in PairingsList) 
                    {
                        if (element is PairingViewModel pairingViewModel)
                        {
                            pairingViewModel.numberTasks--;
                            pairingViewModel.SetActuallViewLabel(pairingViewModel.actuallTask, pairingViewModel.numberTasks);
                        }
                        
                    }
                }
            }

   
        }
        private HashSet<int> GenerateUniqueIndex(int numberElements)
        {
            HashSet<int> uniqueNumbers = new ();
            Random random = new ();
            while (uniqueNumbers.Count < numberElements)
            {
                int randomNumber = random.Next(0, Words.Count);
                if (!uniqueNumbers.Contains(randomNumber))
                {
                    if (CheckBoxState.IsWordChecked && Words[randomNumber].IsWord == 1)
                    {
                        uniqueNumbers.Add(randomNumber);
                    }
                    else if (CheckBoxState.IsSentenceChecked && Words[randomNumber].IsWord == 0)
                    {
                        uniqueNumbers.Add(randomNumber);
                    }
                    else if (!CheckBoxState.IsWordChecked && !CheckBoxState.IsSentenceChecked)
                    {
                        uniqueNumbers.Add(randomNumber);
                    }
                }
            }
            return uniqueNumbers;
        }
    }
}
