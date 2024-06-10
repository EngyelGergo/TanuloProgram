using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TanuloProgram.Core;
using TanuloProgram.Services;
using System.Text.RegularExpressions;

namespace TanuloProgram.MVVM.ViewModel.LanguageViewModels
{
    #nullable disable
    public class LanguageCreateViewModel : Core.ViewModel
    {
        private const string _SPECIAL_ELEMENT_ERROR = "Ez a név nem választható!";
        private const string _EXISTS_TABLE_ERROR = "Az elem már létezik: ";
        private const string _EMPTY_STRING_ERROR = "Töltse ki a mezőt!";
        private const string _NOT_APPROPRIATE_ERROR = "Nem felel meg az előírtaknak!";
        private string _textBoxText;
        private string _replyLabel = "";
        private string _errorMessageLabel = "";
        public string ErrorMessageLabel
        {
            get => _errorMessageLabel;
            set
            {
                _errorMessageLabel = value;
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
        public string TextBoxText
        {
            get { return _textBoxText; }
            set
            {
                if (_textBoxText != value)
                {
                    _textBoxText = value;
                    OnPropertyChange(nameof(TextBoxText));
                }
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

        public RelayCommand NavigateToLanguageViewCommand { get; set; }
        public RelayCommand CreateTableCommand { get; set; }


        public LanguageCreateViewModel(INavigationService navigation)
        {
            Navigation = navigation;
            NavigateToLanguageViewCommand = new RelayCommand(o => 
            {
                LanguageEvent.TriggerRefreshEvent(this, 600, 900);
                Navigation.NavigateTo<LanguageViewModel>();
                TextBoxText = "";
            }, o => true);
            CreateTableCommand = new RelayCommand(o =>
            {
                if (IsApplicable(TextBoxText))
                {
                    new SQLiteConnectionSimple().CreateTable(TextBoxText.Trim().Replace(" ","_"));
                    CustomMethods.ResizeApplicationMainWindow(600, 900);
                    LanguageEvent.TriggerCollenctionAddEvent(this, TextBoxText);
                    TextBoxText = "";
                    ErrorMessageLabel = "";
                    LogFile.LogData(LogDataType.NewAdatbase);                  
                    Navigation.NavigateTo<LanguageViewModel>();
                    
                }
            }, o => true);
        }

        private bool IsApplicable(string textBoxText)
        {
            if (string.IsNullOrEmpty(textBoxText) ||string.IsNullOrWhiteSpace(textBoxText))
            {
                ErrorMessageLabel = _EMPTY_STRING_ERROR;
                return false;
            }
            string input = textBoxText;
            string pattern = @"^[a-zA-Z0-9 áéíóöőúüűÁÉÍÓÖŐÚÜŰ]+$"; // Only word and digits allowed
            bool containsOnlyLettersAndDigits = Regex.IsMatch(input, pattern);
            if (!containsOnlyLettersAndDigits)
            {
                ErrorMessageLabel = _NOT_APPROPRIATE_ERROR;
                return false;
            }
            if (textBoxText == CustomMethods.SQL_MAIN_TABLE.Replace("_"," "))
            {
                ErrorMessageLabel = _SPECIAL_ELEMENT_ERROR;
                return false;
            }
            string search_textBoxText = textBoxText.Trim().Replace(" ", "_").ToLower();
            if(SQLiteConnectionSimple.IsTableExists(search_textBoxText)) 
            {
                ErrorMessageLabel = _EXISTS_TABLE_ERROR + textBoxText;
                return false;
            }
            return true;

        }
    }
}
