using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SimpleGame.ViewModel;

namespace SimpleGame.ViewModels
{
    class ModalBoxViewModel:ViewModelBase
    {
        public string Message { get; set; }
        public RelayCommand BackToSettingsCommand { get; }
        public ModalBoxViewModel(string message)
        {
            Message = message;
            BackToSettingsCommand=new RelayCommand(BackToSettings);
        }

        private void BackToSettings()
        {
            ViewModelLocator vm=new ViewModelLocator();
            vm.Main.CurrentViewModel=new GameSettingsViewModel();
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Width = 400;
                Application.Current.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }
    }
}
