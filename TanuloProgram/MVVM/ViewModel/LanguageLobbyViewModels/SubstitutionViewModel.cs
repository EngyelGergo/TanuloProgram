using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TanuloProgram.Services;

namespace TanuloProgram.MVVM.ViewModel.LanguageLobbyViewModels
{
    public class SubstitutionViewModel : Core.ViewModel , IInsiderView
    {
        private string _actuallViewLabel;
        private string _questionText;
        private string _answerText;
        private string _foreignLanguage;
        private string _selectedLanguage;
        private SolidColorBrush _questionLabelColor;
        
        public SolidColorBrush QuestionLabelColor
        {
            get { return _questionLabelColor; }
            set
            {
                if (_questionLabelColor != value)
                {
                    _questionLabelColor = value;
                    OnPropertyChange(nameof(QuestionLabelColor));
                }
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

        public SubstitutionViewModel(int tasks, int currentTask, string questionText, string foreignLanguage, string selectedLanguage)
        {
            _foreignLanguage = foreignLanguage;
            _selectedLanguage = selectedLanguage;
            ActuallViewLabel = $"{tasks}/{currentTask}";
            QuestionText = questionText ;
            LanguageEvent.HighlightEvent += OnHighlightEvent;

            SolidColorBrush white = Application.Current.Resources["PrimaryTextColor"] as SolidColorBrush;
            QuestionLabelColor = white;
        }

        public void OnHighlightEvent(object sender, HighlightEventArgs e)
        {
            if (_foreignLanguage == _selectedLanguage)
                QuestionLabelColor = e.ColorCode;
        }
    }
}
