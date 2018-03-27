using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace BoardGameAid.Core.ViewModels
{
    public class MainViewModel
        : MvxViewModel
    {
        string hello = "Hello MvvmCross";
        public string Hello
        {
            get { return hello; }
            set => SetProperty(ref hello, value);
        }

        #region constructor and services

        private IMvxNavigationService _navigationService;

        public MainViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #endregion


        #region commands

        private IMvxAsyncCommand _showEditPlayersCommand;
        private IMvxAsyncCommand _startSecretHitlerCommand;
        private IMvxAsyncCommand _startResistanceCommand;


        public IMvxAsyncCommand ShowEditPlayersCommand
        {
            get
            {
                return _showEditPlayersCommand ?? (_showEditPlayersCommand = new MvxAsyncCommand(() => _navigationService.Navigate<EditPlayersViewModel>()));
            }

        }

        public IMvxAsyncCommand StartSecretHitlerCommand
        {
            get
            {
                return _startSecretHitlerCommand ?? (_startSecretHitlerCommand = new MvxAsyncCommand(() => _navigationService.Navigate<SecretHitlerViewModel>()));
            }

        }

        public IMvxAsyncCommand StartResistanceCommand
        {
            get
            {
                return _startResistanceCommand ?? (_startResistanceCommand = new MvxAsyncCommand(() => _navigationService.Navigate<ResistanceViewModel>()));
            }

        }

        #endregion
    }
}
