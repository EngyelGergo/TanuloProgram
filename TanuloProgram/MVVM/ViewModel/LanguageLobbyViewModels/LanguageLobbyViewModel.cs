using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TanuloProgram.Core;
using TanuloProgram.MVVM.View.LanguageLobbyViews;
using TanuloProgram.MVVM.ViewModel.LanguageLobbyViewModels.LanguagePairing;
using TanuloProgram.MVVM.ViewModel.LanguageViewModels;
using TanuloProgram.Services;

namespace TanuloProgram.MVVM.ViewModel.LanguageLobbyViewModels
{
    public class LanguageLobbyViewModel : Core.ViewModel
    {
        private const string _PRE_TITLE= " nyelv tanulása:";
        private MainWindow _mainWindow { get; set; }

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
        public RelayCommand StartPairingViewCommand { get; set; }
        public RelayCommand StartSubstitutionCommand { get; set; }
        public RelayCommand StartCompletionCommand { get; set; }

        public LanguageLobbyViewModel(INavigationService navService, string tableName)
        {
            CustomMethods.ResizeApplicationMainWindow(600, 900, WindowState.Normal, WindowStyle.SingleBorderWindow);
            Navigation = navService;
            TitleLabel = tableName.Replace("_"," ") + _PRE_TITLE;
            BackToPreviousViewCommand = new RelayCommand(o => { Navigation.NavigateTo<SelectedLanguageViewModel>(tableName); }, o => true);
            StartPairingViewCommand =  new RelayCommand(o => { Navigation.NavigateTo<LanguagePairingViewModel>(tableName); }, o => true);
            StartSubstitutionCommand = new RelayCommand(o => { Navigation.NavigateTo<LanguageSubstitutionViewModel>(tableName); }, o => true);
            StartCompletionCommand = new RelayCommand(o => { Navigation.NavigateTo<LanguageCompletionViewModel>(tableName); }, o => true);
        }
    }
}
