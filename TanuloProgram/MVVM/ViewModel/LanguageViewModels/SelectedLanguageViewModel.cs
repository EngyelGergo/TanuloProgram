using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanuloProgram.Core;
using TanuloProgram.MVVM.ViewModel.LanguageLobbyViewModels;
using TanuloProgram.Services;

namespace TanuloProgram.MVVM.ViewModel.LanguageViewModels
{
    public class SelectedLanguageViewModel : Core.ViewModel
    {
        private const string _PRE = " adatbázis";
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
        public RelayCommand LoadLanguageCommand { get; set; }
        public RelayCommand NT_LWAV_Command { get; set; }
        public RelayCommand NT_LWUV_Command { get; set; }
        public SelectedLanguageViewModel(INavigationService navigation, string tableName)
        {
            TitleLabel = tableName + _PRE;
            Navigation = navigation;
            BackToPreviousViewCommand = new RelayCommand(o => 
            { 
                Navigation.NavigateTo<LanguageViewModel>();
                LanguageEvent.RefresfhMainWindowEvent += OnRefreshEvent;
            }, o => true);
            NT_LWAV_Command = new RelayCommand(o =>{ Navigation.NavigateTo<LanguageWordAddViewModel>(tableName); }, o => true);
            NT_LWUV_Command = new RelayCommand(o => { Navigation.NavigateTo<LanguageWordUpdateViewModel>(tableName); }, o => true);
            LoadLanguageCommand = new RelayCommand(o => { Navigation.NavigateTo<LanguageLobbyViewModel>(tableName); }, o => true);
        }

        private void OnRefreshEvent(object sender, RefreshEventArgs e)
        {
            CustomMethods.ResizeApplicationMainWindow(e.Height, e.Width);
        }
    }
}
