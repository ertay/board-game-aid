using System.Threading.Tasks;
using BoardGameAid.Core.Helpers;
using BoardGameAid.Core.Services;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins;

namespace BoardGameAid.Core.ViewModels
{
    [Preserve(AllMembers = true)]
    public class MainViewModel
        : MvxViewModel
    {
        #region constructor and services

        private IMvxNavigationService _navigationService;
        private IPopupService _popupService;
        private IEmailService _emailService;

        public MainViewModel(IMvxNavigationService navigationService, IPopupService popupService, IEmailService emailService)
        {
            _navigationService = navigationService;
            _popupService = popupService;
            _emailService = emailService;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the role timer in seconds.
        /// </summary>
        public int RoleTimerSetting
        {
            get { return Settings.ShowRoleTimerSetting; }
            set
            {
                Settings.ShowRoleTimerSetting = value;
                RaisePropertyChanged(() => RoleTimerSetting);
                
            }
        }

        /// <summary>
        /// Enable or disable Percival
        /// </summary>
        public bool EnablePercival
        {
            get { return Settings.IsPercivalEnabledSetting; }
            set
            {
                Settings.IsPercivalEnabledSetting= value;
                RaisePropertyChanged(() => EnablePercival);

            }
        }

        /// <summary>
        /// Enable or disable Morgana
        /// </summary>
        public bool EnableMorgana
        {
            get { return Settings.IsMorganaEnabledSetting; }
            set
            {
                Settings.IsMorganaEnabledSetting = value;
                RaisePropertyChanged(() => EnableMorgana);

            }
        }

        /// <summary>
        /// Enable or Disable Mordred
        /// </summary>
        public bool EnableMordred
        {
            get { return Settings.IsMordredEnabledSetting; }
            set
            {
                Settings.IsMordredEnabledSetting= value;
                RaisePropertyChanged(() => EnableMordred);

            }
        }

        /// <summary>
        /// Enable or Disable Oberon
        /// </summary>
        public bool EnableOberon
        {
            get { return Settings.IsOberonEnabledSetting; }
            set
            {
                Settings.IsOberonEnabledSetting= value;
                RaisePropertyChanged(() => EnableOberon);

            }
        }

        #endregion


            #region commands

        private IMvxAsyncCommand _showEditPlayersCommand;
        private IMvxAsyncCommand _startSecretHitlerCommand;
        private IMvxAsyncCommand _startResistanceCommand;
        private IMvxCommand _sendFeedbackCommand;

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
                return _startSecretHitlerCommand ?? (_startSecretHitlerCommand = new MvxAsyncCommand(() =>
                {
                    if (Settings.PlayersSetting.Count < 5)
                    {
                        _popupService.ShowPopup("Players", "You need at least 5 players to play. Tap on 'Edit Players' to add more players.");
                        return Task.CompletedTask;
                    }
                    else
                    {
                        return _navigationService.Navigate<SecretHitlerViewModel>();
                    }
                    
                
                }));
            }

        }

        public IMvxAsyncCommand StartResistanceCommand
        {
            get
            {
                return _startResistanceCommand ?? (_startResistanceCommand = new MvxAsyncCommand(() =>
                {
                    if (Settings.PlayersSetting.Count < 5)
                    {
                        _popupService.ShowPopup("Players", "You need at least 5 players to play. Tap on 'Edit Players' to add more players.");
                        return Task.CompletedTask;
                    }
                    else
                    {
                        return _navigationService.Navigate<ResistanceViewModel>();
                    }
                }));
            }

        }

        /// <summary>
        /// Commands used to send feedback email.
        /// </summary>
        public IMvxCommand SendFeedbackCommand
        {
            get
            {
                return _sendFeedbackCommand ?? (_sendFeedbackCommand = new MvxCommand(() =>
                _emailService.SendEmail("sightlessfun@outlook.com", "Board Game Aid Feedback")));
            }

        }

        #endregion
    }
}
