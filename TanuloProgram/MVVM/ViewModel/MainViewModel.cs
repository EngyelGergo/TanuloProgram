using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TanuloProgram.Core;
using TanuloProgram.Services;
using TanuloProgram.MVVM.ViewModel.LanguageViewModels;

namespace TanuloProgram.MVVM.ViewModel
{
	#nullable disable
    public class MainViewModel : Core.ViewModel
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

		public RelayCommand AppShutdownCommand { get; set; }
        public RelayCommand NavigateToMathCommand { get; set; }
		public RelayCommand NavigateToLanguageViewCommand { get; set; }

 
        public MainViewModel(INavigationService navService)
        {
            Navigation = navService;
			NavigateToLanguageViewCommand = new RelayCommand(o => { Navigation.NavigateTo<LanguageViewModel>(); }, o => true);
            NavigateToMathCommand = new RelayCommand(o => { Navigation.NavigateTo<MathViewModel>(); }, o => true);
			AppShutdownCommand = new RelayCommand(o => { Application.Current.Shutdown(); }, o => true);
        }

    }
}
