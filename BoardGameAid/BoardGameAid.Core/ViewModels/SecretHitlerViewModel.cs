using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGameAid.Core.Helpers;
using BoardGameAid.Core.Models;
using BoardGameAid.Core.Services;
using MvvmCross.Core.ViewModels;

namespace BoardGameAid.Core.ViewModels
{
    public class SecretHitlerViewModel : MvxViewModel
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
        private int _currentPlayerIndex = 0;
        private bool _isRoleVisible;

        /// <summary>
        /// The current player that is looking at their role.
        /// </summary>
        public SecretHitlerPlayer CurrentPlayer
        {
            get { return _currentPlayer; }
            set => SetProperty(ref _currentPlayer, value);
        }

        /// <summary>
        /// Returns a string of who t he other fascists and hitler is.
        /// </summary>
        public string OtherFascists
        {
            get
            {
                if (CurrentPlayer.Role != SecretHitlerRole.Fascist)
                {
                    return "";
                }
                IEnumerable<string> names = _players
                    .Where(p => p.Name != CurrentPlayer.Name && p.Role == SecretHitlerRole.Fascist).Select(p => p.Name);
                string hitlerName = _players.First(p => p.Role == SecretHitlerRole.Hitler).Name;
                return $"Fascists: {string.Join(", ", names)}\n\nHitler: {hitlerName}";
            }
            
        }

        /// <summary>
        /// Boolean to show or hide the role.
        /// </summary>
        public bool IsRoleVisible
        {
            get { return _isRoleVisible; }
            set => SetProperty(ref _isRoleVisible, value);
        }



        #endregion

        #region commands and methods

        private MvxCommand _showRoleCommand;
        private MvxCommand _nextPlayerCommand;

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
                    IsRoleVisible = false;
                    if (++_currentPlayerIndex >= _players.Count)
                    {
                        // we've shown all players, show the party checking UI
                        return;
                    }
                    // switch to the next player
                    CurrentPlayer = _players[_currentPlayerIndex];
                    RaisePropertyChanged(() => OtherFascists);
                }));
            }
        }

        #endregion

    }
}
