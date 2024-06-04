using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanuloProgram.Core;
using TanuloProgram.MVVM.Model;
using TanuloProgram.Services;

namespace TanuloProgram.MVVM.ViewModel.LanguageLobbyViewModels
{
    public class CompletionViewModel : Core.ViewModel , IInsiderView
    {
        private const string _meaning = "Jelentése: "; 
        private string _actuallViewLabel;
        private string _questionText;
        private string _answerText;
        private string _buttonText1;
        private string _buttonText2;
        private string _buttonText3;
        private string _buttonText4;
        private string _buttonText5;
        private string _nativeMeaning;
        public string missingWord = "";
        private ObservableCollection<Word> _w_list;
        public string NativeMeaning
        {
            get => _nativeMeaning;
            set
            {
                _nativeMeaning = value;
                OnPropertyChange();
            }
        }
        public string ButtonText1
        {
            get => _buttonText1;
            set
            {
                _buttonText1 = value;
                OnPropertyChange();
            }
        }
        public string ButtonText2
        {
            get => _buttonText2;
            set
            {
                _buttonText2 = value;
                OnPropertyChange();
            }
        }
        public string ButtonText3
        {
            get => _buttonText3;
            set
            {
                _buttonText3 = value;
                OnPropertyChange();
            }
        }
        public string ButtonText4
        {
            get => _buttonText4;
            set
            {
                _buttonText4 = value;
                OnPropertyChange();
            }
        }
        public string ButtonText5
        {
            get => _buttonText5;
            set
            {
                _buttonText5 = value;
                OnPropertyChange();
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

        public string QuestionText
        {
            get => _questionText;
            set
            {
                _questionText = value;
                OnPropertyChange();
            }
        }
        public string AnswerText
        {
            get => _answerText;
            set
            {
                _answerText = value;
                OnPropertyChange();
            }
        }
    
        public RelayCommand ButtonFirCommand { get; set; }
        public RelayCommand ButtonSecCommand { get; set; }
        public RelayCommand ButtonThiCommand { get; set; }
        public RelayCommand ButtonFouCommand { get; set; }
        public RelayCommand ButtonFifCommand { get; set; }

        public CompletionViewModel(int tasks, int currentTask,int selectedUniq, List<int> uniqueMembers, ObservableCollection<Word> w_list, LanguageType languages, string selectedLang)
        {
            ActuallViewLabel = $"{tasks}/{currentTask}";
            _w_list = new(w_list);
            SetNaviteValue(_w_list, selectedUniq);
            SetQuestionValue(selectedUniq, _w_list);
            SetButtonsValues(_w_list, uniqueMembers);
            ButtonFirCommand = new RelayCommand(o => { SetAnswer(ButtonText1); }, o => true);
            ButtonSecCommand = new RelayCommand(o => { SetAnswer(ButtonText2); }, o => true);
            ButtonThiCommand = new RelayCommand(o => { SetAnswer(ButtonText3); }, o => true);
            ButtonFouCommand = new RelayCommand(o => { SetAnswer(ButtonText4); }, o => true);
            ButtonFifCommand = new RelayCommand(o => { SetAnswer(ButtonText5); }, o => true);
        }

        private void SetNaviteValue(ObservableCollection<Word> w_list, int selectedUniq)
        {
            string text = _meaning + w_list[selectedUniq].NativeWord.FirstToUpper().InsertSentence();
            NativeMeaning = "(" + text + ")";
        }
        private void SetQuestionValue(int selectedUniq, ObservableCollection<Word> _w)
        {
            Random rnd = new();
            string[] questionParts = _w[selectedUniq].ForeignWord.Split(" ");
            int selectedIndex = rnd.Next(0, questionParts.Length);
            if (selectedIndex == 0)
                missingWord = questionParts[selectedIndex].FirstToUpper();
            else if (selectedIndex == questionParts.Length-1)
                missingWord = questionParts[selectedIndex].InsertSentence();
            else
                missingWord = questionParts[selectedIndex].Trim();
            questionParts[selectedIndex] = GenerateUnderlineWord(questionParts[selectedIndex]);
            QuestionText = string.Join(" ", questionParts).FirstToUpper().InsertSentence();
        }
        private void SetButtonsValues(ObservableCollection<Word> _w, List<int> uniqueMembers)
        {    
            Random rnd = new();
            List<string> buttonTexts = new List<string> { ButtonText1, ButtonText2, ButtonText3, ButtonText4, ButtonText5 };
            List<Word> delList= new List<Word>();
            int itemsCount = _w.Count;
            for (int i = 0; i < itemsCount; i++)
            {
                if (uniqueMembers.Contains(i))
                {
                    delList.Add(_w_list[i]);
                }
            }
            foreach (Word w in delList)
            {
                _w_list.Remove(w);
            }
            List<int> new_uniqueMembers = CustomMethods.GenerateUniqueIndex(4, true, false, _w);
            new_uniqueMembers.Add(-1);
            int nuMCount = new_uniqueMembers.Count;


            for (int i = 0; i < buttonTexts.Count; i++)
            {
                int selectedIdx = rnd.Next(0, nuMCount);
                int selected = new_uniqueMembers[selectedIdx];
                string temp = (selected == -1) ? missingWord : _w[selected].ForeignWord;
                buttonTexts[i] = temp;
                --nuMCount;
                new_uniqueMembers.RemoveAt(selectedIdx);
            }
            if (char.IsUpper(missingWord[0]))
            {
                ButtonText1 = buttonTexts[0].FirstToUpper();
                ButtonText2 = buttonTexts[1].FirstToUpper();
                ButtonText3 = buttonTexts[2].FirstToUpper();
                ButtonText4 = buttonTexts[3].FirstToUpper();
                ButtonText5 = buttonTexts[4].FirstToUpper();
            }
            else
            {
                ButtonText1 = buttonTexts[0].ToLower().RemoveSentence();
                ButtonText2 = buttonTexts[1].ToLower().RemoveSentence();
                ButtonText3 = buttonTexts[2].ToLower().RemoveSentence();
                ButtonText4 = buttonTexts[3].ToLower().RemoveSentence();
                ButtonText5 = buttonTexts[4].ToLower().RemoveSentence();
            }
            
        }

        private string GenerateUnderlineWord(string word)
        {
            string blank = "_";
            int lenght = word.Length;
            for (int i = 0;i < lenght; i++) 
            {
                blank += "_";
            }
            return blank;
        }

        private void SetButtonsValues(string questionText)
        {
            ButtonText1 = "Content";
            ButtonText2 = "Content";
            ButtonText3 = "Conteeeeeeeeent";
            ButtonText4 = "Content!!!";
            ButtonText5 = "Content";
        }

        private void SetAnswer(string text)
        {
            AnswerText = text;
        }
    }
}
