using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using TanuloProgram.Core;
using TanuloProgram.Services;
using TanuloProgram.MVVM.ViewModel;
using TanuloProgram.MVVM.ViewModel.LanguageViewModels;
using TanuloProgram.MVVM.ViewModel.LanguageLobbyViewModels;
using TanuloProgram.MVVM.View.LanguageLobbyViews;
using System.Security.Cryptography.X509Certificates;
using TanuloProgram.MVVM.ViewModel.LanguageLobbyViewModels.LanguagePairing;

namespace TanuloProgram
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvide;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindow>(provider => new MainWindow()
            {
                DataContext = provider.GetRequiredService<MainViewModel>()  
            });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MathViewModel>();
            services.AddSingleton<LanguageCreateViewModel>();
            services.AddTransient<LanguageViewModel>();
            services.AddTransient<LanguageWordAddViewModel>();
            services.AddTransient<LanguageWordUpdateViewModel>();
            services.AddTransient<SelectedLanguageViewModel>();
            //services.AddSingleton<PairingViewModel>();
            services.AddSingleton<LanguageLobbyViewModel>();
            services.AddSingleton<LanguagePairingView>();
            services.AddSingleton<LanguageResultViewModel>();
            services.AddSingleton<LanguageCompletionViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<Func<Type, string, ViewModel>>((serviceProvider) =>
            {
                return (viewModelType, s) =>
                {
                    var navigationService = serviceProvider.GetRequiredService<INavigationService>();

                    if (viewModelType == typeof(LanguageWordAddViewModel))
                    {
                        return new LanguageWordAddViewModel(navigationService, s);
                    }
                    else if (viewModelType == typeof(LanguageWordUpdateViewModel))
                    {
                        return new LanguageWordUpdateViewModel(navigationService, s);
                    }
                    else if (viewModelType == typeof(LanguageLobbyViewModel))
                    {
                        return new LanguageLobbyViewModel(navigationService, s);
                    }
                    else if (viewModelType == typeof(LanguagePairingViewModel))
                    {
                        return new LanguagePairingViewModel(navigationService, s);
                    }
                    else if (viewModelType == typeof(LanguageSubstitutionViewModel))
                    {
                        return new LanguageSubstitutionViewModel(navigationService, s);
                    }
                    else if (viewModelType == typeof(LanguageResultViewModel))
                    {
                        return new LanguageResultViewModel(navigationService,s);
                    }
                    else if (viewModelType == typeof(LanguageCompletionViewModel))
                    {
                        return new LanguageCompletionViewModel(navigationService, s);
                    }
                    else if (viewModelType == typeof(SelectedLanguageViewModel))
                    {
                        return new SelectedLanguageViewModel(navigationService, s);
                    }
                    else
                    {
                        return serviceProvider.GetRequiredService(viewModelType) as ViewModel;
                    }
                };
            });
            _serviceProvide = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            DatabaseFacade facade = new DatabaseFacade(new DbDataContext());
            facade.EnsureCreated();
            var mainWindow = _serviceProvide.GetRequiredService<MainWindow>();     
            LogFile.LogData(LogDataType.ProgramStart);
            LogFile.LoggingLoadAllLanguage();
            mainWindow.Show();
            base.OnStartup(e);
        }


    }

}
