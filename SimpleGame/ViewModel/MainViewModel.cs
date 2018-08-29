using GalaSoft.MvvmLight;
using SimpleGame.ViewModels;

namespace SimpleGame.ViewModel
{
  
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel == value)
                {
                    return;
                }

                _currentViewModel = value;
                RaisePropertyChanged();
            }
        }

        private int _width;
        public int Width
        {
            get => _width;
            set
            {
                if (_width == value)
                {
                    return;
                }

                _width = value;
                RaisePropertyChanged();
            }
        }
        private int _height;
        public int Height
        {
            get => _height;
            set
            {
                if (_height == value)
                {
                    return;
                }

                _height = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel()
        {
            Width = 300;
            Height = 600;
            CurrentViewModel=GameSettingsViewModel.GetInstance();            
        }
    }
}