using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TanuloProgram.Core;
using TanuloProgram.MVVM.ViewModel.LanguageViewModels;
using TanuloProgram.Services;

namespace TanuloProgram.MVVM.ViewModel.LanguageLobbyViewModels
{
    public enum Classification
    {
        None,
        Cry,
        Sad,
        Medium,
        Okey,
        Smart,
        Perfect
    }

    public class LanguageResultViewModel : Core.ViewModel
    {
        private const string _basicCryRoot = "Images\\cry_basic.png";
        private const string _basicSadRoot = "Images\\sad_basic.png";
        private const string _basicMediumRoot = "Images\\medium_basic.png";
        private const string _basicOkeyRoot = "Images\\okey_basic.png";
        private const string _basicSmartRoot = "Images\\smart_basic.png";
        private const string _basicPerfectRoot = "Images\\perfect_basic.png";
        private string[] _cryLabels = 
            { 
            "Hát ez még csukott szemmel is jobban ment volna!",
            "Elfelejtetted kitölteni vagy hogy sikerült ez?",
            "Ez borzalmas lett..."
            };
        private string[] _sadLabels =
            {
            "Ez most nagyon gyengére sikerült.",
            "Jézusom...",
            "Erre még bőven rá fér a gyakorlás!"
            };
        private string[] _mediumLabels =
            {
            "Nem rossz, de nem is igazán jó.",
            "Némi tanulást még nem ártana!",
            "Erre mondják azt hogy közepes"
            };
        private string[] _okeyLabels =
            {
            "Ez már valami!",
            "Valaki gyakorolta a szavakat! Szép munka.",
            "Nem tökéletes, de a nagyrésze ment!"
            };
        private string[] _smartLabels =
            {
            "Csodálatos eredmény!",
            "Ezt nevezik profi munkának!",
            "Ez igen!"
            };
        private string[] _perfectLabels =
            {
            "A szavak fekete öves nagymestere vagy !",
            "Tökéletes eredmény!",
            "Ez az ember képtelen a hibákra!"
            };

        private Classification _classification;
        private INavigationService _navigation;
        private string _resultLabel;
        private string _reactionLabel;
        private string _imageUrl = "default_image.jpg";

        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;
                OnPropertyChange();
            }
        }

        public BitmapImage ImageBitmap
        {
            get
            {
                if (!string.IsNullOrEmpty(_imageUrl))
                {
                    return new BitmapImage(new Uri(_imageUrl));
                }
                else
                {
                    return new BitmapImage(new Uri("default_image.jpg", UriKind.Relative));
                }
            }
        }

        public string ReactionLabel
        {
            get => _reactionLabel;
            set
            {
                _reactionLabel = value;
                OnPropertyChange(); ;
            }
        }

        public string ResultLabel
        {
            get => _resultLabel;
            set
            {
                _resultLabel = value;
                OnPropertyChange(); ;
            }
        }

        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChange();
            }
        }

        public RelayCommand BackToLanguageLobbyCommand { get; set; }

        public LanguageResultViewModel(INavigationService navService, string result)
        {
            string[] res = result.Split(":");
            ResultLabel = res[0];
            _classification = ResultClassification(res[0]);
            SetClassification(_classification);
            Navigation = navService;
            BackToLanguageLobbyCommand = new RelayCommand(o => { Navigation.NavigateTo<LanguageLobbyViewModel>(res[1]); }, o => true);
        }


        private double PercentageCalculate(string result)
        {
            string[] resultPoints = result.Split('/');
            double maxPoint = Convert.ToDouble(resultPoints[0]);
            double resultPoint = Convert.ToDouble(resultPoints[1]);
            return (resultPoint / maxPoint);
        }

        private Classification ResultClassification(string result)
        {
            if (string.IsNullOrEmpty(result))
                return Classification.None;

            double resultPercentage = PercentageCalculate(result);
            if (resultPercentage < 0.2) 
                return Classification.Cry;
            else if (resultPercentage < 0.4)
                return Classification.Sad;
            else if (resultPercentage <= 0.6)
                return Classification.Medium;
            else if (resultPercentage < 0.8)
                return Classification.Okey;
            else if (resultPercentage != 1)
                return Classification.Smart;
            else
                return Classification.Perfect;
        }

        private void SetClassification(Classification classification)
        {
            Random random = new Random();
            switch (classification)
            {
                case Classification.None:
                    ResultLabel = "Hiba történt a kiértékeléskor";
                    break;
                case Classification.Cry:
                    ImageUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _basicCryRoot);
                    ReactionLabel = _cryLabels[random.Next(0, _cryLabels.Length)];
                    break;
                case Classification.Sad:
                    ImageUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _basicSadRoot);
                    ReactionLabel = _sadLabels[random.Next(0, _sadLabels.Length)];
                    break;
                case Classification.Medium:
                    ImageUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _basicMediumRoot);
                    ReactionLabel = _mediumLabels[random.Next(0, _mediumLabels.Length)];
                    break;
                case Classification.Okey:
                    ImageUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _basicOkeyRoot);
                    ReactionLabel = _okeyLabels[random.Next(0, _okeyLabels.Length)];
                    break;
                case Classification.Smart:
                    ImageUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _basicSmartRoot);
                    ReactionLabel = _smartLabels[random.Next(0, _smartLabels.Length)];
                    break;
                case Classification.Perfect:
                    ImageUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _basicPerfectRoot);
                    ReactionLabel = _perfectLabels[random.Next(0, _perfectLabels.Length)];
                    break;
            }
        }
    }
}
