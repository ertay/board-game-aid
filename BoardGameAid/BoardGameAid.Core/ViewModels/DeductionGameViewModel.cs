using System;
using System.Threading.Tasks;
using BoardGameAid.Core.Helpers;
using BoardGameAid.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace BoardGameAid.Core.ViewModels
{
    /// <summary>
    /// This class should be a parent to the other games.
    /// TODO: Refactor to hold shared logic in this class for different games.
    /// </summary>
    public abstract class DeductionGameViewModel : MvxViewModel
    {
        #region properties

        private bool _isRoleVisible;
        private bool _isPartyVisible;
        protected bool _isPartyRevealGameState;

        private string _showRoleTime;
        private bool _isRoleTimerVisible;

        /// <summary>
        /// Depending on the game state, it returns the current player role or party
        /// </summary>
        public abstract string CurrentPlayerRoleOrParty { get; }

        /// <summary>
        /// Shows the remaining time until we switch to next player
        /// </summary>
        public string ShowRoleTime
        {
            get { return _showRoleTime; }
            set
            {
                SetProperty(ref _showRoleTime, value);
            }
        }

        /// <summary>
        /// Boolean to show or hide the role.
        /// </summary>
        public bool IsRoleVisible
        {
            get { return _isRoleVisible; }
            set
            {
                SetProperty(ref _isRoleVisible, value);
                RaisePropertyChanged(() => IsRoleTimerVisible);
                RaisePropertyChanged(() => ShowOrHideText);

            }

        }



        /// <summary>
        /// Holds the button text for different states (hide, show).
        /// </summary>
        public string ShowOrHideText => IsRoleVisible ? "Hide Party" : "Show Party";

        /// <summary>
        /// Shows the role timer in the initial roles phase
        /// </summary>
        public bool IsRoleTimerVisible
        {
            get
            {
                // show timer if not in the party reveal phase and 
                // when the role is visible
                return !_isPartyRevealGameState && _isRoleVisible;
            }
        }

        /// <summary>
        /// Next player and show/hide party buttons are visible in the party reveal phase
        /// </summary>
        public bool ArePartyButtonsVisible
        {
            get { return _isPartyRevealGameState; }
        }

        /// <summary>
        /// We hide the show role button in the party reveal phase.
        /// </summary>
        public bool IsShowRoleButtonVisible
        {
            get { return !_isPartyRevealGameState; }
        }

        /// <summary>
        /// Returns true if player is marked as visually imapaired.
        /// </summary>
        protected abstract bool IsCurrentPlayerVisuallyImpaired { get; }
        #endregion

        #region commands and methods

        private MvxAsyncCommand _showRoleCommand;

        /// <summary>
        /// Command to show the player role.
        /// </summary>
        public MvxAsyncCommand ShowRoleCommand
        {
            get
            {
                return _showRoleCommand ?? (_showRoleCommand = new MvxAsyncCommand(async () =>
                {
                    Dispatcher.RequestMainThreadAction(() => ShowRoleCommand.RaiseCanExecuteChanged());

                    // if  player has marked visually impaired, we use tts
                    // else we show role on screen
                    if (IsCurrentPlayerVisuallyImpaired)
                    {
                        await SpeakRoleInfo();
                        NextPlayer();
                        Dispatcher.RequestMainThreadAction(() => ShowRoleCommand.RaiseCanExecuteChanged());

                        return;
                    }


                    IsRoleVisible = true;


                    TimeSpan time = TimeSpan.FromSeconds(Settings.ShowRoleTimerSetting);
                    ShowRoleTime = time.ToString(@"mm\:ss");

                    PclTimer roleTimer = new PclTimer(time);
                    roleTimer.TimeElapsed += (sender, span) =>
                    {
                        ShowRoleTime = span.ToString(@"mm\:ss");

                        if (span.TotalSeconds <= 0)
                        {
                            NextPlayer();
                            Dispatcher.RequestMainThreadAction(() => ShowRoleCommand.RaiseCanExecuteChanged());

                        }
                    };
                    roleTimer.StartAsync();

                }, () => !IsRoleVisible));
            }
        }

        private MvxCommand _nextPlayerCommand;
        private MvxCommand _showOrHidePartyCommand;
        private MvxAsyncCommand _speakPartyCommand;




        /// <summary>
        /// Command to show or hide the party.
        /// </summary>
        public MvxCommand ShowOrHidePartyCommand
        {
            get
            {
                return _showOrHidePartyCommand ?? (_showOrHidePartyCommand = new MvxCommand(() =>
                {
                    // in the party phase, we just reuse the role variable to show party
                    IsRoleVisible = !IsRoleVisible;
                }));
            }

        }

        /// <summary>
        /// Fires the speak party method.
        /// </summary>
        public MvxAsyncCommand SpeakPartyCommand
        {
            get
            {
                return _speakPartyCommand ?? (_speakPartyCommand = new MvxAsyncCommand(async () =>
                {
                    await SpeakPartyInfo();
                }));
            }

        }

        /// <summary>
        /// Command that switches to the next player
        /// </summary>
        public MvxCommand NextPlayerCommand
        {
            get
            {
                return _nextPlayerCommand ?? (_nextPlayerCommand = new MvxCommand(() =>
                {

                    NextPlayer();


                }));
            }
        }

        /// <summary>
        /// Switch to next plaayer.
        /// TODO: Consider moving the players to this class.
        /// </summary>
        protected abstract void NextPlayer();

        #endregion

        /// <summary>
        /// Uses text to speech to read role information for VI players
        /// </summary>
        /// <returns></returns>
        public abstract Task SpeakRoleInfo();

        /// <summary>
        /// Uses text to speech to read party information.
        /// </summary>
        /// <returns></returns>
        public abstract Task SpeakPartyInfo();

        /// <summary>
        /// Creates a popup to ask whether we should quit the game and go back to the main menu.
        /// </summary>
        public void QuitGame()
        {
            var popupService = Mvx.Resolve<IPopupService>();

            popupService.ShowPopup("Quit Game?", "Would you like to quit the game and go back to the main menu?", "MAIN MENU", "Cancel", () => Close(this), null, true);

        }
    }
}