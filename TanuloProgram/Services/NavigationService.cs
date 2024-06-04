using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using TanuloProgram.Core;
using TanuloProgram.MVVM.View;
using TanuloProgram.MVVM.ViewModel;
using TanuloProgram.MVVM.ViewModel.LanguageLobbyViewModels.LanguagePairing;

namespace TanuloProgram.Services
{
    public class NavigationService : ObservableObject, INavigationService
    {

        private readonly Func<Type,string, ViewModel> _viewModelFactory;
        private ViewModel _currentView;
        public ViewModel CurrentView
        {
            get => _currentView;
            private set
            {
                _currentView = value; ;
                OnPropertyChange();
            }
        } 
        public NavigationService(Func<Type,string, ViewModel> viewModelFactory)
        {
                _viewModelFactory = viewModelFactory;
        }
        public NavigationService()
        {
                
        }

        public void NavigateTo<TViewModel>(string s) where TViewModel : ViewModel
        {
            ViewModel viewModel = _viewModelFactory.Invoke(typeof(TViewModel),s);
            CurrentView = viewModel;
        }
    }
}
