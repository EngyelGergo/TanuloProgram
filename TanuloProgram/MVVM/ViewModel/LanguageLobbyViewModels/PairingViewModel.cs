using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Linq;
using TanuloProgram.Core;
using TanuloProgram.MVVM.Model;
using TanuloProgram.Services;

namespace TanuloProgram.MVVM.ViewModel.LanguageLobbyViewModels
{
    public enum ButtonColors
    {
        Default,
        Red,
        Orange,
        Yellow,
        Green,
        Violet
    }
    public class PairingViewModel : Core.ViewModel , IInsiderView
    {
        private const string _DEFAULT_LANGUAGE = "Magyar";
        private const string _PRE_TEXT = "Kattints!";
        public List<Word> ViewElements { get; set; }
        private List<string> ButtonOneList;
        private List<string> ButtonTwoList;
        private List<string> ButtonThreeList;
        private List<string> ButtonFourList;
        private List<string> ButtonFiveList;
        private List<string> _nativePairs;
        private List<string> _foreignPairs;
        private int _buttonOneListActuall = -1;
        private int _buttonTwoListActuall = -1;
        private int _buttonThreeListActuall = -1;
        private int _buttonFourListActuall = -1;
        private int _buttonFiveListActuall = -1;
        public int numberTasks;
        public int actuallTask;
        private string _foreignLanguage;
        private string _selectedLanguage;
        private string[] _labelNativeOne = ["",""];
        private string[] _labelNativeTwo = ["", ""];
        private string[] _labelNativeThree = ["", ""];
        private string[] _labelNativeFour = ["", ""];
        private string[] _labelNativeFive = ["", ""];
        private string _foreignTitleLabel;
        private string _actuallViewLabel;
        private string _nativeTitleLabel;
        private string _itemsTButtonOne;
        private string _itemsTButtonTwo;
        private string _itemsTButtonThree;
        private string _itemsTButtonFour;
        private string _itemsTButtonFive;
        private Visibility _firstItemVisibility;
        private Visibility _secondItemVisibility;
        private Visibility _thirdItemVisibility;
        private Visibility _fourthItemVisibility;
        private Visibility _fifthItemVisibility;
        private SolidColorBrush _labelColorOne;
        private SolidColorBrush _labelColorTwo;
        private SolidColorBrush _labelColorThree;
        private SolidColorBrush _labelColorFour;
        private SolidColorBrush _labelColorFive;

        public SolidColorBrush LabelColorOne
        {
            get { return _labelColorOne; }
            set
            {
                if (_labelColorOne != value)
                {
                    _labelColorOne = value;
                    OnPropertyChange(nameof(LabelColorOne));
                }
            }
        }
        public SolidColorBrush LabelColorTwo
        {
            get { return _labelColorTwo; }
            set
            {
                if (_labelColorTwo != value)
                {
                    _labelColorTwo = value;
                    OnPropertyChange(nameof(LabelColorTwo));
                }
            }
        }
        public SolidColorBrush LabelColorThree
        {
            get { return _labelColorThree; }
            set
            {
                if (_labelColorThree != value)
                {
                    _labelColorThree = value;
                    OnPropertyChange(nameof(LabelColorThree));
                }
            }
        }
        public SolidColorBrush LabelColorFour
        {
            get { return _labelColorFour; }
            set
            {
                if (_labelColorFour != value)
                {
                    _labelColorFour = value;
                    OnPropertyChange(nameof(LabelColorFour));
                }
            }
        }
        public SolidColorBrush LabelColorFive
        {
            get { return _labelColorFive; }
            set
            {
                if (_labelColorFive != value)
                {
                    _labelColorFive = value;
                    OnPropertyChange(nameof(LabelColorFive));
                }
            }
        }
        private SolidColorBrush _buttonColorOne;
        private SolidColorBrush _buttonColorTwo;
        private SolidColorBrush _buttonColorThree;
        private SolidColorBrush _buttonColorFour;
        private SolidColorBrush _buttonColorFive;
        public SolidColorBrush ButtonColorOne
        {
            get { return _buttonColorOne; }
            set
            {
                if (_buttonColorOne != value)
                {
                    _buttonColorOne = value;
                    OnPropertyChange(nameof(ButtonColorOne));
                }
            }
        }
        public SolidColorBrush ButtonColorTwo
        {
            get { return _buttonColorTwo; }
            set
            {
                if (_buttonColorTwo != value)
                {
                    _buttonColorTwo = value;
                    OnPropertyChange(nameof(ButtonColorTwo));
                }
            }
        }
        public SolidColorBrush ButtonColorThree
        {
            get { return _buttonColorThree; }
            set
            {
                if (_buttonColorThree != value)
                {
                    _buttonColorThree = value;
                    OnPropertyChange(nameof(ButtonColorThree));
                }
            }
        }
        public SolidColorBrush ButtonColorFour
        {
            get { return _buttonColorFour; }
            set
            {
                if (_buttonColorFour != value)
                {
                    _buttonColorFour = value;
                    OnPropertyChange(nameof(ButtonColorFour));
                }
            }
        }
        public SolidColorBrush ButtonColorFive
        {
            get { return _buttonColorFive; }
            set
            {
                if (_buttonColorFive != value)
                {
                    _buttonColorFive = value;
                    OnPropertyChange(nameof(ButtonColorFive));
                }
            }
        }
        public Visibility FirstItemVisibility
        {
            get => _firstItemVisibility;
            set
            {
                _firstItemVisibility = value;
                OnPropertyChange(); ;
            }
        }
        public Visibility SecondItemVisibility
        {
            get => _secondItemVisibility;
            set
            {
                _secondItemVisibility = value;
                OnPropertyChange(); ;
            }
        }
        public Visibility ThirdItemVisibility
        {
            get => _thirdItemVisibility;
            set
            {
                _thirdItemVisibility = value;
                OnPropertyChange(); ;
            }
        }
        public Visibility FourthItemVisibility
        {
            get => _fourthItemVisibility;
            set
            {
                _fourthItemVisibility = value;
                OnPropertyChange(); ;
            }
        }
        public Visibility FifthItemVisibility
        {
            get => _fifthItemVisibility;
            set
            {
                _fifthItemVisibility = value;
                OnPropertyChange(); ;
            }
        }

        public string ItemsTButtonFive
        {
            get => _itemsTButtonFive;
            set
            {
                _itemsTButtonFive = value;
                OnPropertyChange(); ;
            }
        }
        public string ItemsTButtonFour
        {
            get => _itemsTButtonFour;
            set
            {
                _itemsTButtonFour = value;
                OnPropertyChange(); ;
            }
        }
        public string ItemsTButtonThree
        {
            get => _itemsTButtonThree;
            set
            {
                _itemsTButtonThree = value;
                OnPropertyChange(); ;
            }
        }
        public string ItemsTButtonTwo
        {
            get => _itemsTButtonTwo;
            set
            {
                _itemsTButtonTwo = value;
                OnPropertyChange(); ;
            }
        }
        public string ItemsTButtonOne
        {
            get => _itemsTButtonOne;
            set
            {
                _itemsTButtonOne = value;
                OnPropertyChange(); ;
            }
        }

        public string NativeTitleLabel
        {
            get => _nativeTitleLabel;
            set
            {
                _nativeTitleLabel = value;
                OnPropertyChange(); ;
            }
        }

        public string ActuallViewLabel
        {
            get => _actuallViewLabel;
            set
            {
                _actuallViewLabel = value;
                OnPropertyChange();
            }
        }

        public string ForeignTitleLabel
        {
            get => _foreignTitleLabel;
            set
            {
                _foreignTitleLabel = value;
                OnPropertyChange(); ;
            }
        }

        public string[] LabelNativeOne
        {
            get => _labelNativeOne;
            set
            {
                _labelNativeOne = value;
                OnPropertyChange(); ;
            }
        }
        public string[] LabelNativeTwo
        {
            get => _labelNativeTwo;
            set
            {
                _labelNativeTwo = value;
                OnPropertyChange(); ;
            }
        }
        public string[] LabelNativeThree
        {
            get => _labelNativeThree;
            set
            {
                _labelNativeThree = value;
                OnPropertyChange(); ;
            }
        }
        public string[] LabelNativeFour
        {
            get => _labelNativeFour;
            set
            {
                _labelNativeFour = value;
                OnPropertyChange(); ;
            }
        }
        public string[] LabelNativeFive
        {
            get => _labelNativeFive;
            set
            {
                _labelNativeFive = value;
                OnPropertyChange(); ;
            }
        }

        public RelayCommand BTOneCommand { get; set; }
        public RelayCommand BTTwoCommand { get; set; }
        public RelayCommand BTThreeCommand { get; set; }
        public RelayCommand BTFourCommand { get; set; }   
        public RelayCommand BTFiveCommand { get; set; }
        public RelayCommand OneRightButtonClickCommand { get; set; }
        public RelayCommand TwoRightButtonClickCommand { get; set; }
        public RelayCommand ThreeRightButtonClickCommand { get; set; }
        public RelayCommand FourRightButtonClickCommand { get; set; }
        public RelayCommand FiveRightButtonClickCommand { get; set; }


        public PairingViewModel(List<Word> viewWords, int numberTasks, int actuallTask, string foreignLanguage, string selectedLanguage)
        {
            ViewElements = viewWords;

            this.numberTasks = numberTasks;
            this.actuallTask = actuallTask;

            _foreignLanguage = foreignLanguage;
            _selectedLanguage = selectedLanguage;
            FillPairs(viewWords);

            LanguageEvent.HighlightEvent += OnHighlightEvent;

            SolidColorBrush white = Application.Current.Resources["PrimaryTextColor"] as SolidColorBrush;
            LabelColorOne = white;
            LabelColorTwo = white;
            LabelColorThree = white;
            LabelColorFour = white;
            LabelColorFive = white;
            if (selectedLanguage != new LanguageType().Variable)
            {
                SetQuestions();
                SetAnswersList();
            }
            else
            {
                SetQuestionAsVariable();
                SetAnswersListAsVariable();
            }
            ShowAvailableTasks();
            SetLanguagesLabels();

            BTOneCommand = new RelayCommand(o => {SetNextButton(ButtonOneList, ItemsTButtonOne, 1); }, o => true);
            BTTwoCommand = new RelayCommand(o => { SetNextButton(ButtonTwoList, ItemsTButtonTwo, 2); }, o => true);
            BTThreeCommand = new RelayCommand(o => { SetNextButton(ButtonThreeList, ItemsTButtonThree, 3); }, o => true);
            BTFourCommand = new RelayCommand(o => { SetNextButton(ButtonFourList, ItemsTButtonFour, 4); }, o => true);
            BTFiveCommand = new RelayCommand(o => { SetNextButton(ButtonFiveList, ItemsTButtonFive, 5); }, o => true);

            OneRightButtonClickCommand = new RelayCommand(o => { SetPrewButton(ButtonOneList, ItemsTButtonOne, 1); }, o => true);
            TwoRightButtonClickCommand = new RelayCommand(o => { SetPrewButton(ButtonTwoList, ItemsTButtonTwo, 2); }, o => true);
            ThreeRightButtonClickCommand = new RelayCommand(o => { SetPrewButton(ButtonThreeList, ItemsTButtonThree, 3); }, o => true);
            FourRightButtonClickCommand = new RelayCommand(o => { SetPrewButton(ButtonFourList, ItemsTButtonFour, 4); }, o => true);
            FiveRightButtonClickCommand = new RelayCommand(o => { SetPrewButton(ButtonFiveList, ItemsTButtonFive, 5); }, o => true);
        }

        public void OnHighlightEvent(object sender, HighlightEventArgs e)
        {
            if (LabelNativeOne[1] == _foreignLanguage)
                LabelColorOne = e.ColorCode;
            if (LabelNativeTwo[1] == _foreignLanguage)
                LabelColorTwo = e.ColorCode;
            if (LabelNativeThree[1] == _foreignLanguage)
                LabelColorThree = e.ColorCode;
            if (LabelNativeFour[1] == _foreignLanguage)
                LabelColorFour = e.ColorCode;
            if (LabelNativeFive[1] == _foreignLanguage)
                LabelColorFive = e.ColorCode;
        }

        private void FillPairs(List<Word> receivedWords)
        {
            _nativePairs = receivedWords.Select(word => word.NativeWord).ToList();
            _foreignPairs = receivedWords.Select(word => word.ForeignWord).ToList();
        }

        private void SetQuestions()
        {
            int labelNeed = ViewElements.Count;
            for (int i = 0; i < labelNeed; i++)
            {
                switch (i)
                {
                    case 0:
                        LabelNativeOne = [(_selectedLanguage == _DEFAULT_LANGUAGE) ? ViewElements[i].NativeWord : ViewElements[i].ForeignWord, _selectedLanguage];
                        break;
                    case 1:
                        LabelNativeTwo = [(_selectedLanguage == _DEFAULT_LANGUAGE) ? ViewElements[i].NativeWord : ViewElements[i].ForeignWord, _selectedLanguage];
                        break;
                    case 2:
                        LabelNativeThree = [(_selectedLanguage == _DEFAULT_LANGUAGE) ? ViewElements[i].NativeWord : ViewElements[i].ForeignWord, _selectedLanguage];
                        break;
                    case 3:
                        LabelNativeFour = [(_selectedLanguage == _DEFAULT_LANGUAGE) ? ViewElements[i].NativeWord : ViewElements[i].ForeignWord, _selectedLanguage];
                        break;
                    case 4:
                        LabelNativeFive = [(_selectedLanguage == _DEFAULT_LANGUAGE) ? ViewElements[i].NativeWord : ViewElements[i].ForeignWord, _selectedLanguage];
                        break;
                }
            }
        }
        private void SetQuestionAsVariable()
        {
            int labelNeed = ViewElements.Count;
            for (int i = 0; i < labelNeed; i++)
            {
                switch (i)
                {
                    case 0:
                        LabelNativeOne = RandomElement(ViewElements, i);
                        break;
                    case 1:
                        LabelNativeTwo = RandomElement(ViewElements, i);
                        break;
                    case 2:
                        LabelNativeThree = RandomElement(ViewElements, i);
                        break;
                    case 3:
                        LabelNativeFour = RandomElement(ViewElements, i);
                        break;
                    case 4:
                        LabelNativeFive = RandomElement(ViewElements, i);
                        break;
                }
            }
        }
        private void SetAnswersList()
        {
            int itemNeed = ViewElements.Count;
            int count = 0;
            ButtonOneList = [];
            ButtonTwoList = [];
            ButtonThreeList = [];
            ButtonFourList = [];
            ButtonFiveList = [];
            while (count < itemNeed)
            {
                if (count == 0)
                {
                    ButtonOneList = (_selectedLanguage == _DEFAULT_LANGUAGE) ? _foreignPairs : _nativePairs;
                    ButtonOneList.Shuffle();
                }
                else if (count == 1)
                {
                    ButtonTwoList = (_selectedLanguage == _DEFAULT_LANGUAGE) ? _foreignPairs : _nativePairs;
                    ButtonTwoList.Shuffle();
                }             
                else if (count == 2)
                {
                    ButtonThreeList = (_selectedLanguage == _DEFAULT_LANGUAGE) ? _foreignPairs : _nativePairs;
                    ButtonThreeList.Shuffle();
                }   
                else if (count == 3)
                {
                    ButtonFourList = (_selectedLanguage == _DEFAULT_LANGUAGE) ? _foreignPairs : _nativePairs;
                    ButtonFourList.Shuffle();
                }             
                else if (count == 4)
                {
                    ButtonFiveList = (_selectedLanguage == _DEFAULT_LANGUAGE) ? _foreignPairs : _nativePairs;
                    ButtonFiveList.Shuffle();
                }   
                ++count;
            }         
        }

        private void SetAnswersListAsVariable()
        {
            int itemNeed = ViewElements.Count;
            int count = 0;
            ButtonOneList = [];
            ButtonTwoList = [];
            ButtonThreeList = [];
            ButtonFourList = [];
            ButtonFiveList = [];
            while (count < itemNeed)
            {
                if (count == 0)
                {
                    ButtonOneList = (LabelNativeOne[1] == _DEFAULT_LANGUAGE) ? _foreignPairs : _nativePairs;
                    ButtonOneList.Shuffle();
                }
                else if (count == 1)
                {
                    ButtonTwoList = (LabelNativeTwo[1] == _DEFAULT_LANGUAGE) ? _foreignPairs : _nativePairs;
                    ButtonTwoList.Shuffle();
                }
                else if (count == 2)
                {
                    ButtonThreeList = (LabelNativeThree[1] == _DEFAULT_LANGUAGE) ? _foreignPairs : _nativePairs;
                    ButtonThreeList.Shuffle();
                }
                else if (count == 3)
                {
                    ButtonFourList = (LabelNativeFour[1] == _DEFAULT_LANGUAGE) ? _foreignPairs : _nativePairs;
                    ButtonFourList.Shuffle();
                }
                else if (count == 4)
                {
                    ButtonFiveList = (LabelNativeFive[1] == _DEFAULT_LANGUAGE) ? _foreignPairs : _nativePairs;
                    ButtonFiveList.Shuffle();
                }
                ++count;
            }
        }
        private string[] RandomElement(List<Word> viewElements, int index)
        {
            Random random = new();
            int selected = random.Next(0, 2);
            return (selected == 0) ? [viewElements[index].NativeWord, _DEFAULT_LANGUAGE] : [viewElements[index].ForeignWord, _foreignLanguage];
        }
        private void SetNextButton(List<string>? _list, string item, int index)
        {
            int min = 0;
            int max = _list.Count;

            if (_list == null || max == 0) return;

            if (IsFirstElementDefault(index, item, _list, min, max)) return;

            int idxOfButton = WhichButtonWasPressed(index);

            string temp = GetMyNextItem(max, _list, idxOfButton, index);

            if (index == 1) ItemsTButtonOne = temp;
            else if (index == 2) ItemsTButtonTwo = temp;
            else if (index == 3) ItemsTButtonThree = temp;
            else if (index == 4) ItemsTButtonFour = temp;
            else if (index == 5) ItemsTButtonFive = temp;

        }

        private void SetPrewButton(List<string>? _list, string item, int index)
        {
            int min = 0;
            int max = _list.Count;

            if (_list == null || max == 0) return;

            if (IsFirstElementDefault(index, item, _list, min, max)) return;

            int idxOfButton = WhichButtonWasPressed(index);

            string temp = GetMyPreviousItem(max, _list, idxOfButton, index);

            if (index == 1) ItemsTButtonOne = temp;
            else if (index == 2) ItemsTButtonTwo = temp;
            else if (index == 3) ItemsTButtonThree = temp;
            else if (index == 4) ItemsTButtonFour = temp;
            else if (index == 5) ItemsTButtonFive = temp;
        }

        private void SetButtonMemoryIndex(int index, int value)
        {
            if (index == 1) _buttonOneListActuall = value;
            else if (index == 2) _buttonTwoListActuall = value;
            else if (index == 3) _buttonThreeListActuall = value;
            else if (index == 4) _buttonFourListActuall = value;
            else if (index == 5) _buttonFiveListActuall = value;
        }

        private int WhichButtonWasPressed(int index)
        {
            if (index == 1) return _buttonOneListActuall;
            else if (index == 2) return _buttonTwoListActuall;
            else if (index == 3) return _buttonThreeListActuall;
            else if (index == 4) return _buttonFourListActuall;
            else if (index == 5) return _buttonFiveListActuall;
            else return -1;
        }

        private string GetMyNextItem(int max, List<string> list, int result_index, int idx)
        {
            string temp = "";

            if (result_index == max - 1)
            {
                temp = list[0];
                SetButtonMemoryIndex(idx, 0);
                if (idx == 1) ButtonColorOne = SetButtonBackgroundColor((ButtonColors)(1));
                else if (idx == 2) ButtonColorTwo = SetButtonBackgroundColor((ButtonColors)(1));
                else if (idx == 3) ButtonColorThree = SetButtonBackgroundColor((ButtonColors)(1));
                else if (idx == 4) ButtonColorFour = SetButtonBackgroundColor((ButtonColors)(1));
                else if (idx == 5) ButtonColorFive = SetButtonBackgroundColor((ButtonColors)(1));
                return temp;
            }
            else
            {
                temp = list[result_index + 1];
                SetButtonMemoryIndex(idx, result_index + 1);
                if (idx == 1) ButtonColorOne = SetButtonBackgroundColor((ButtonColors)_buttonOneListActuall + 1);
                else if (idx == 2) ButtonColorTwo = SetButtonBackgroundColor((ButtonColors)_buttonTwoListActuall + 1);
                else if (idx == 3) ButtonColorThree = SetButtonBackgroundColor((ButtonColors)_buttonThreeListActuall + 1);
                else if (idx == 4) ButtonColorFour = SetButtonBackgroundColor((ButtonColors)_buttonFourListActuall + 1);
                else if (idx == 5) ButtonColorFive = SetButtonBackgroundColor((ButtonColors)_buttonFiveListActuall + 1);
                return temp;
            }
        }

        private string GetMyPreviousItem(int max, List<string> list, int result_index, int idx)
        {
            string temp = "";

            if (result_index == 0)
            {
                temp = list[max - 1];
                SetButtonMemoryIndex(idx, max - 1);
                if (idx == 1) ButtonColorOne = SetButtonBackgroundColor((ButtonColors)(max));
                else if (idx == 2) ButtonColorTwo = SetButtonBackgroundColor((ButtonColors)(max));
                else if (idx == 3) ButtonColorThree = SetButtonBackgroundColor((ButtonColors)(max));
                else if (idx == 4) ButtonColorFour = SetButtonBackgroundColor((ButtonColors)(max));
                else if (idx == 5) ButtonColorFive = SetButtonBackgroundColor((ButtonColors)(max));
                return temp;
            }
            else
            {
                temp = list[result_index - 1];
                SetButtonMemoryIndex(idx, result_index - 1);
                if (idx == 1) ButtonColorOne = SetButtonBackgroundColor((ButtonColors)_buttonOneListActuall + 1);
                else if (idx == 2) ButtonColorTwo = SetButtonBackgroundColor((ButtonColors)_buttonTwoListActuall + 1);
                else if (idx == 3) ButtonColorThree = SetButtonBackgroundColor((ButtonColors)_buttonThreeListActuall + 1);
                else if (idx == 4) ButtonColorFour = SetButtonBackgroundColor((ButtonColors)_buttonFourListActuall + 1);
                else if (idx == 5) ButtonColorFive = SetButtonBackgroundColor((ButtonColors)_buttonFiveListActuall + 1);
                return temp;
            }
        }

        private bool IsFirstElementDefault(int index, string item, List<string> list, int min, int max)
        {
            if (item != _PRE_TEXT) return false;

            Random random = new Random();
            int num = random.Next(min, max);
            if (index == 1)
            {
                ItemsTButtonOne = list[num];
                _buttonOneListActuall = num;
                ButtonColorOne = SetButtonBackgroundColor((ButtonColors)num + 1);
            }
            else if (index == 2)
            {
                ItemsTButtonTwo = list[num];
                _buttonTwoListActuall = num;
                ButtonColorTwo = SetButtonBackgroundColor((ButtonColors)num + 1);
            }
            else if (index == 3)
            {
                ItemsTButtonThree = list[num];
                _buttonThreeListActuall = num;
                ButtonColorThree = SetButtonBackgroundColor((ButtonColors)num + 1);
            }
            else if (index == 4)
            {
                ItemsTButtonFour = list[num];
                _buttonFourListActuall = num;
                ButtonColorFour = SetButtonBackgroundColor((ButtonColors)num + 1);
            }
            else if (index == 5)
            {
                ItemsTButtonFive = list[num];
                _buttonFiveListActuall = num;
                ButtonColorFive = SetButtonBackgroundColor((ButtonColors)num + 1);
            }
            return true;     
        }

        private void ShowAvailableTasks()
        {
            if (ButtonOneList.Count > 0)
            {
                ItemsTButtonOne = _PRE_TEXT;
                ButtonColorOne = SetButtonBackgroundColor(ButtonColors.Default);
            }  
            else
                FirstItemVisibility = Visibility.Collapsed;

            if (ButtonTwoList.Count > 0)
            {
                ItemsTButtonTwo = _PRE_TEXT;
                ButtonColorTwo = SetButtonBackgroundColor(ButtonColors.Default);
            }     
            else
                SecondItemVisibility = Visibility.Collapsed;

            if (ButtonThreeList.Count > 0)
            {
                ItemsTButtonThree = _PRE_TEXT;
                ButtonColorThree = SetButtonBackgroundColor(ButtonColors.Default);
            }        
            else
                ThirdItemVisibility = Visibility.Collapsed;

            if (ButtonFourList.Count > 0)
            {
                ItemsTButtonFour = _PRE_TEXT;
                ButtonColorFour = SetButtonBackgroundColor(ButtonColors.Default);
            }
            else
                FourthItemVisibility = Visibility.Collapsed;

            if (ButtonFiveList.Count > 0)
            {
                ItemsTButtonFive = _PRE_TEXT;
                ButtonColorFive = SetButtonBackgroundColor(ButtonColors.Default);
            }               
            else
                FifthItemVisibility = Visibility.Collapsed;
        }

        private SolidColorBrush SetButtonBackgroundColor(ButtonColors color)
        {
            switch (color)
            {
                case ButtonColors.Default:
                    return Application.Current.Resources["ButtonMouseOverColor"] as SolidColorBrush;
                case ButtonColors.Red:
                    return CustomMethods.MakeColor("#B67575");
                case ButtonColors.Orange:
                    return CustomMethods.MakeColor("#E29C34"); 
                case ButtonColors.Yellow:
                    return CustomMethods.MakeColor("#837F04");
                case ButtonColors.Green:
                    return CustomMethods.MakeColor("#449A3A");
                case ButtonColors.Violet:
                    return CustomMethods.MakeColor("#8A4E9A");
                default:
                    return null;
            }
        }
    
        private void SetLanguagesLabels()
        {
            if (_selectedLanguage == _DEFAULT_LANGUAGE)
            {
                NativeTitleLabel = _DEFAULT_LANGUAGE;
                ForeignTitleLabel = _foreignLanguage;
            }
            else if (_selectedLanguage == _foreignLanguage)
            {
                NativeTitleLabel = _foreignLanguage;
                ForeignTitleLabel = _DEFAULT_LANGUAGE;
            }
            else if (_selectedLanguage == "Változó")
            {
                NativeTitleLabel = "Változó";
                ForeignTitleLabel = "Változó";
            }

            SetActuallViewLabel(actuallTask, numberTasks);
        }

        public void SetActuallViewLabel(int task, int amountTasks)
        {
            ActuallViewLabel = $"{task}/{amountTasks}";
        }
    }
}
