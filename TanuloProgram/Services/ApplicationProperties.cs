using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanuloProgram.Core;
using TanuloProgram.MVVM.ViewModel.LanguageLobbyViewModels.LanguagePairing;

namespace TanuloProgram.Services
{
    /// <summary>
    /// LanguagePairingViewModel specific checkbox properties
    /// </summary>
    public class CheckBoxProperties : ObservableObject
    {
        // Indicates whether the word checkbox is checked or not.
        private bool _isWordChecked;
        // Indicates whether the sentencee checkbox is checked or not.
        private bool _isSentenceChecked;
        // Gets or sets a value indicating a word checkbox IsChecked property value.
        public bool IsWordChecked
        {
            get { return _isWordChecked; }
            set
            {
                if (_isWordChecked != value)
                {
                    _isWordChecked = value;
                    // if word changed to true change sentence to false
                    if (value) _isSentenceChecked = false;
                    OnPropertyChange(nameof(IsWordChecked));
                    OnPropertyChange(nameof(IsSentenceChecked));
                    LanguageEvent.OnlyWordEvent.Invoke(this, null);
                }
            }
        }
        // Gets or sets a value indicating a word checkbox IsChecked property value.
        public bool IsSentenceChecked
        {
            get { return _isSentenceChecked; }
            set
            {
                if (_isSentenceChecked != value)
                {
                    _isSentenceChecked = value;
                    // if sentence changed to true change word to false
                    if (value) _isWordChecked = false;
                    OnPropertyChange(nameof(IsSentenceChecked));
                    OnPropertyChange(nameof(IsWordChecked));
                    LanguageEvent.OnlySentenceEvent.Invoke(this, null);
                }
            }
        }

        public RelayCommand DefaultItems { get; set; }

        public CheckBoxProperties()
        {
            DefaultItems = new RelayCommand(o => { DefaultState(); }, o => true);
        }

        private void DefaultState()
        {
            if (!IsWordChecked && !IsSentenceChecked)
            {
                LanguageEvent.NonSelectedElementEvent.Invoke(this, null);
            }
        }
    }
    public class SliderProperties : ObservableObject
    {
        private int _minimumValue;
        private int _maximumValue;
        private int _value;
        private bool _enable = true;

        public int Range { get; set; }
        public RelayCommand CheckedCommand { get; set; }
        public RelayCommand UncheckedCommand { get; set; }


        public bool Enable
        {
            get => _enable;
            set
            {
                _enable = value;
                OnPropertyChange(); ;
            }
        }

        public int MinimumValue
        {
            get => _minimumValue;
            set
            {
                _minimumValue = value;
                OnPropertyChange(); ;
            }
        }

        public int MaximumValue
        {
            get => _maximumValue;
            set
            {
                _maximumValue = value;
                OnPropertyChange(); ;
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChange();
                OnPropertyChange(nameof(ValueLabel));
            }
        }

        public string ValueLabel
        {
            get => $"{Value} db";
        }

        public SliderProperties()
        {
            CheckedCommand = new RelayCommand(o => { SetMaximum(); }, o => true);
            UncheckedCommand = new RelayCommand(o => { InactivateMaximum(); }, o => true);
        }

        private void InactivateMaximum()
        {
            Value = MinimumValue;
            Enable = true;
        }

        private void SetMaximum()
        {
            Value = Range;
            Enable = false;
        }
    }

}
