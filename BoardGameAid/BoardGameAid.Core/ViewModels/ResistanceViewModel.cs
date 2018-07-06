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
    public class ResistanceViewModel : DeductionGameViewModel
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
        public override string CurrentPlayerRoleOrParty
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

        // helper list to store spy names
        private IEnumerable<string> _otherSpyNames;

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


                    _otherSpyNames = _players
                        .Where(p => p.Name != CurrentPlayer.Name && p.Role == ResistanceRole.Spy)
                        .Select(p => p.Name);
                    
                    return $"Spies: {string.Join(", ", _otherSpyNames)}";
                
                
            }
            
        }

        /// <summary>
        /// Returns true if current player is visually impaired.
        /// </summary>
        protected override bool IsCurrentPlayerVisuallyImpaired { get { return CurrentPlayer.IsVisuallyImpaired; } }

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
            RaisePropertyChanged(() => OtherSpies);
        }


        public override async Task SpeakRoleInfo()
        {
            string message = "";
            if (CurrentPlayer.Role == ResistanceRole.Loyal)
            {
                message = "You are a Loyalist. You are a Loyalist.";
            }
            else
            {
                if (_players.Count < 7)
                {
                    message = $"You are a Spy. {_otherSpyNames.First()} is the other spy.";
                }
                else if (_players.Count < 10)
                {
                    message = $"You are a Spy. {string.Join(" and ", _otherSpyNames)} are the other spies.";

                }
                else
                {
                    message = $"You are a Spy. {string.Join(", ", _otherSpyNames)} are the other spies.";

                }

            }

            await CrossTextToSpeech.Current.Speak(message);
        }

        public override async Task SpeakPartyInfo()
        {
            string message = $"{CurrentPlayer.Name} is a ";
            message += CurrentPlayer.IsLoyal ? "Loyalist." : "Spy.";
            await CrossTextToSpeech.Current.Speak(message);
        }

        #endregion

    }
}
