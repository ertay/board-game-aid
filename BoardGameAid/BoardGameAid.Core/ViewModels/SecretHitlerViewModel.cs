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
using Plugin.TextToSpeech;

namespace BoardGameAid.Core.ViewModels
{
    public class SecretHitlerViewModel : DeductionGameViewModel
    {

        #region constructor and services

        public SecretHitlerViewModel(IRandomizerService randomizerService)
        {
            _players = randomizerService.GetShuffledPlayers(Settings.PlayersSetting.Count, "SecretHitler");
            CurrentPlayer = _players[_currentPlayerIndex];
        }

        #endregion

        #region properties and fields

        private List<SecretHitlerPlayer> _players { get; }
        private SecretHitlerPlayer _currentPlayer;
        private string _currentPlayerRoleOrParty;
        private int _currentPlayerIndex = 0;
        

        /// <summary>
        /// The current player that is looking at their role.
        /// </summary>
        public SecretHitlerPlayer CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                SetProperty(ref _currentPlayer, value);
                RaisePropertyChanged(() => CurrentPlayerRoleOrParty);
            }
        }

        // helpers to store fascist names
        private IEnumerable<string> _fascistNames;
        private string _hitlerPlayerName;

        /// <summary>
        /// Returns a string of who the other fascists and hitler is.
        /// </summary>
        public string OtherFascists
        {
            get
            {
                if (_isPartyRevealGameState)
                {
                    return "";
                }
                bool showOtherFascistToHitler = CurrentPlayer != null && (_players.Count < 7 && CurrentPlayer.Role == SecretHitlerRole.Hitler);
                if (CurrentPlayer.Role == SecretHitlerRole.Fascist || showOtherFascistToHitler)
                {


                    _fascistNames = _players
                        .Where(p => p.Name != CurrentPlayer.Name && p.Role == SecretHitlerRole.Fascist)
                        .Select(p => p.Name);
                    _hitlerPlayerName = string.Empty;
                    if (!showOtherFascistToHitler)
                    {
                        _hitlerPlayerName = _players.First(p => p.Role == SecretHitlerRole.Hitler).Name;
                    }
                    return $"Fascists: {string.Join(", ", _fascistNames)}\n\nHitler: {_hitlerPlayerName}";
                }
                return "";
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
        /// Returns true if current player is visually impaired.
        /// </summary>
        protected override bool IsCurrentPlayerVisuallyImpaired { get { return CurrentPlayer.IsVisuallyImpaired; } }

        public override string CurrentPlayerRoleOrParty {
            get
            {
                // if party reveal state, return the party information
                if (_isPartyRevealGameState)
                {
                    return CurrentPlayer.IsLiberal ? "Liberal" : "Fascist";
                }
                // we're  still in the role deaaling phase, show role
                return CurrentPlayer.Role.ToString();
            }
        }

        #endregion

        #region commands and methods

        
        /// <summary>
        /// Switches to the next player. After first pass, enables the party reveal state instead
        /// of the role reveal state.
        /// </summary>
        protected override void NextPlayer()
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
            RaisePropertyChanged(() => OtherFascists);
        }

        /// <summary>
        /// Speaks the player role and other information if needed.
        /// </summary>
        /// <returns></returns>
        public override async Task SpeakRoleInfo()
        {
            string message = "";
            if (CurrentPlayer.IsLiberal)
            {
                message = "You are a Liberal player. You are a Liberal player. You are a Liberal player.";
            }
            else if (CurrentPlayer.Role == SecretHitlerRole.Fascist)
            {
                message = $"You are a Fascist player. {_hitlerPlayerName} is Hitler.";
                if (_players.Count < 7)
                {
                    message += $" You are a Fascist player. {_hitlerPlayerName} is Hitler.";
                }
                else if (_players.Count < 9)
                {
                    message += $" {_fascistNames.First()} is the other Fascist player.";
                }
                else
                {
                    message += $" {string.Join("and ", _fascistNames)} are the other Fascists.";
                }
            }
            else
            {
                message = "You are Hitler. You are Hitler. You are Hitler.";
                if (_players.Count < 7)
                {
                    message += $" {_fascistNames.First()} is the other Fascist player.";
                }
            }

            await CrossTextToSpeech.Current.Speak(message);
        }

        /// <summary>
        /// Speaks the party information for the selected player.
        /// </summary>
        /// <returns></returns>
        public override async Task SpeakPartyInfo()
        {
            string message = CurrentPlayer.IsLiberal ? $"{CurrentPlayer.Name} is Liberal." : $"{CurrentPlayer.Name} is Fascist.";
            
            await CrossTextToSpeech.Current.Speak(message);
        }

        #endregion

    }
}
