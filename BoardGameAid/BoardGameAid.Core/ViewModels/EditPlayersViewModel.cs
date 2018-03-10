using System.Collections.Generic;
using BoardGameAid.Core.Models;
using MvvmCross.Core.ViewModels;

namespace BoardGameAid.Core.ViewModels
{
    /// <summary>
    /// View model for the page where you insert/remove/edit players.
    /// </summary>
    public class EditPlayersViewModel : MvxViewModel
    {

        #region properties & fields

        private string _playerName;
        private bool _isVisuallyImpaired;

        /// <summary>
        /// Property for the player's name that will be inserted.
        /// </summary>
        public string PlayerName
        {
            get { return _playerName; }
            set => SetProperty(ref _playerName, value);
        }

        /// <summary>
        /// Checkbox to mark a visually impaired player
        /// Can be used to enabale text-to-speech capabilities for this player
        /// </summary>
        public bool IsVisuallyImpaired
        {
            get { return _isVisuallyImpaired; }
            set => SetProperty(ref _isVisuallyImpaired, value);

        }

        /// <summary>
        /// Collection to hold all the players.
        /// </summary>
        public MvxObservableCollection<Player> Players { get; set; }

        #endregion

        #region constructor

        public EditPlayersViewModel()
        {
            Players = new MvxObservableCollection<Player>();
        }

        #endregion

        #region commands

        private MvxCommand _addPlayerCommand;

        /// <summary>
        /// Command that adds a new player to the list.
        /// </summary>
        public MvxCommand AddPlayerCommand
        {
            get
            {
                return _addPlayerCommand ?? (_addPlayerCommand = new MvxCommand(() =>
                {
                    // if no name is entered, just set a name of "Player N", where N is a number
                    Player player = new Player()
                    {
                        Name = string.IsNullOrEmpty(PlayerName) ? $"Player {Players.Count + 1}" : PlayerName,
                        IsVisuallyImpaired = this.IsVisuallyImpaired
                    };

                    Players.Add(player);
                    // clean up the text field after adding a player
                    PlayerName = string.Empty;
                }));
            }
        }

        #endregion

    }
}