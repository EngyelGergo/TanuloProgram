using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TanuloProgram.Core;
using TanuloProgram.MVVM.ViewModel.LanguageLobbyViewModels;
using TanuloProgram.Services;
using static TanuloProgram.Services.LanguageEvent;

namespace TanuloProgram.MVVM.ViewModel.LanguageViewModels
{
    #nullable disable
    public class LanguageViewModel : Core.ViewModel
    {
        private string _displayedSelectedItem = "";
        public ObservableCollection<string> LanguageList { get; set; }

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

        private string _selectedItem;
        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value != null)
                {
                    _selectedItem = value.Replace(" ", "_");
                    _displayedSelectedItem = value.Replace("_", " ");
                }

                else
                {
                    _selectedItem = value;
                    _displayedSelectedItem = value;
                }    
                OnPropertyChange(nameof(SelectedItem));
            }
        }
 

        public RelayCommand BackToPreviousViewCommand { get; set; }
        public RelayCommand DropSelectedTableCommand { get; set; }
        public RelayCommand LoadLanguageCommand { get; set; }
        public RelayCommand NT_LCV_Command { get; set; }

        public LanguageViewModel(INavigationService navigation)
        {
            RefresfhMainWindowEvent += OnRefreshEvent;
            ElementAddedEvent += OnElementAdded;
            LanguageList = new SQLiteConnectionSimple().LoadTables().Count > 0 ? new SQLiteConnectionSimple().LoadTables() : new();
            RemoveSQLiteTable(LanguageList);
            RemoveUnderlineFromTables();
            Navigation = navigation;
            BackToPreviousViewCommand = new RelayCommand(o => { Navigation.NavigateTo<MainViewModel>(); }, o => true);
            NT_LCV_Command = new RelayCommand(o => 
            {
                TriggerRefreshEvent(this, 300, 450);
                Navigation.NavigateTo<LanguageCreateViewModel>(); 
            }, o => true);

            DropSelectedTableCommand = new RelayCommand(o => 
            {
                if (!string.IsNullOrEmpty(SelectedItem))
                {
                    MessageBoxResult result = MessageBox.Show($"Biztosan törölni szeretnéd a(z) \"{_displayedSelectedItem}\" adatbázist?", "Figyelem", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        new SQLiteConnectionSimple().DeleteTable(SelectedItem);
                        LanguageList.Remove(_displayedSelectedItem);
                        LogFile.LogData(LogDataType.DeletedAdatbase);
                    }       
                }
            }, o => true);
            LoadLanguageCommand = new RelayCommand(o => 
            {
                if (!string.IsNullOrEmpty(SelectedItem))
                {
                    LogFile.LogLanguageSet(SelectedItem);
                    LogFile.LogData(LogDataType.MostChoosenLanguageAmount, SelectedItem);
                    LogFile.LogData(LogDataType.MostChoosenLanguage, SelectedItem);
                    RefresfhMainWindowEvent -= OnRefreshEvent;
                    Navigation.NavigateTo<SelectedLanguageViewModel>(SelectedItem);
                }
            }, o => true);
        }

        private void OnRefreshEvent(object sender, RefreshEventArgs e)
        {
            CustomMethods.ResizeApplicationMainWindow(e.Height, e.Width);
        }
        private void OnElementAdded(object sender, StringElementEventArg e)
        {
            LanguageList.Add(e.Element);
        }
        private void OnElementDeleted(object sender, StringElementEventArg e)
        {
            LanguageList.Remove(e.Element);
        }
        private void RemoveUnderlineFromTables()
        {
            int c = LanguageList.Count;
            for (int i = 0; i < c; i++)
            {
                LanguageList[i] = LanguageList[i].Replace("_", " ");
            }
        }

        private void RemoveSQLiteTable(ObservableCollection<string> languageList)
        {
            if (languageList.Count == 0 || languageList == null) return;
            if (languageList.Contains(CustomMethods.SQL_MAIN_TABLE))
                languageList.Remove(CustomMethods.SQL_MAIN_TABLE);
        }
    }
}
