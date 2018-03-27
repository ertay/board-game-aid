using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BoardGameAid.Core.Helpers;
using BoardGameAid.Core.Models;
using BoardGameAid.Core.Services;
using MvvmCross.Core.ViewModels;

namespace BoardGameAid.Core.ViewModels
{
    public class ResistanceViewModel : MvxViewModel
    {

        #region constructor and services

        public ResistanceViewModel(IRandomizerService randomizerService)
        {
            _players = randomizerService.GetShuffledResistancePlayers(Settings.PlayersSetting.Count, "Resistance");
            CurrentPlayer = _players[_currentPlayerIndex];
        }

        #endregion

        #region properties and fields

        private List<ResistancePlayer> _players { get; }
        private ResistancePlayer _currentPlayer;
        private string _currentPlayerRoleOrParty;
        private int _currentPlayerIndex = 0;
        private bool _isRoleVisible;
        private bool _isPartyVisible;
        private bool _isPartyRevealGameState;

        private string _showRoleTime;
        private bool _isRoleTimerVisible;

        /// <summary>
        /// The current player that is looking at their role.
        /// </summary>
        public ResistancePlayer CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                SetProperty(ref _currentPlayer, value);
                RaisePropertyChanged(() => CurrentPlayerRoleOrParty);
            }
        }

        /// <summary>
        /// Depending on the game state, it returns the current player role or party
        /// </summary>
        public string CurrentPlayerRoleOrParty
        {
            get
            {
                // if party reveal state, return the party information
                if (_isPartyRevealGameState)
                {
                    return CurrentPlayer.IsLoyal ? "Loyal" : "Spy";
                }
                // we're  still in the role deaaling phase, show role
                return CurrentPlayer.Role.ToString();
            }
        }

        /// <summary>
        /// Returns a string of who the other spies are.
        /// </summary>
        public string OtherSpies
        {
            get
            {
                if (_isPartyRevealGameState)
                {
                    return "";
                }

                if (CurrentPlayer.Role == ResistanceRole.Loyal)
                    return "";


                    IEnumerable<string> names = _players
                        .Where(p => p.Name != CurrentPlayer.Name && p.Role == ResistanceRole.Spy)
                        .Select(p => p.Name);
                    
                    return $"Spies: {string.Join(", ", names)}";
                
                
            }
            
        }

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
        public string ShowOrHideText => IsRoleVisible ? "Hide Role" : "Show Role";

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
        /// We hide the show role button in the party reveal phase.
        /// </summary>
        public bool IsShowRoleButtonVisible
        {
            get { return !_isPartyRevealGameState; }
        }

        /// <summary>
        /// Next player and show/hide party buttons are visible in the party reveal phase
        /// </summary>
        public bool ArePartyButtonsVisible
        {
            get { return _isPartyRevealGameState; }
        }



        #endregion

        #region commands and methods

        private MvxCommand _showRoleCommand;
        private MvxCommand _nextPlayerCommand;
        private MvxCommand _showOrHidePartyCommand;


        /// <summary>
        /// Command to show the player role.
        /// </summary>
        public MvxCommand ShowRoleCommand
        {
            get
            {
                return _showRoleCommand ?? (_showRoleCommand = new MvxCommand(() =>
                {
                    IsRoleVisible = true;
                    Dispatcher.RequestMainThreadAction(() => ShowRoleCommand.RaiseCanExecuteChanged());
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
        /// Switches to the next player. After first pass, enables the party reveal state instead
        /// of the role reveal state.
        /// </summary>
        private void NextPlayer()
        {
            IsRoleVisible = false;


            if (++_currentPlayerIndex >= _players.Count)
            {
                // check if we should switch to the party reveal phase
                if (!_isPartyRevealGameState)
                {

                    _isPartyRevealGameState = true;
                    RaisePropertyChanged(() => ArePartyButtonsVisible);
                    RaisePropertyChanged(() => IsShowRoleButtonVisible);
                }
                _currentPlayerIndex = 0;
            }
            // switch to the next player
            CurrentPlayer = _players[_currentPlayerIndex];
            RaisePropertyChanged(() => OtherSpies);
        }
        
        #endregion

    }
}
