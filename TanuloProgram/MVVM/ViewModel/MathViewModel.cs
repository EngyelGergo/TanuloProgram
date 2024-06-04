using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanuloProgram.Core;
using TanuloProgram.Services;
using TanuloProgram.MVVM.ViewModel.LanguageViewModels;

namespace TanuloProgram.MVVM.ViewModel
{
    #nullable disable
    public class MathViewModel : Core.ViewModel
    {
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

        public RelayCommand NavigateToLanguage { get; set; }


        public MathViewModel(INavigationService navigation)
        {
            Navigation = navigation;
            NavigateToLanguage = new RelayCommand(o => { Navigation.NavigateTo<LanguageViewModel>(); }, o => true);
                
        }
    }
}
