using System.Collections.Generic;
using System.Linq;
using BoardGameAid.Core.Models;
using BoardGameAid.Core.ViewModels.Services;
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
            set
            {
                SetProperty(ref _playerName, value);
                
                // this needs to run on the UI thread
                Dispatcher.RequestMainThreadAction(() => RemovePlayerCommand.RaiseCanExecuteChanged());

            }
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

        #region constructor and services

        private IPopupService _popupService;

        public EditPlayersViewModel(IPopupService popupService)
        {
            _popupService = popupService;
            Players = new MvxObservableCollection<Player>();
        }

        #endregion

        #region commands

        private MvxCommand _addPlayerCommand;
        private MvxCommand _removePlayerCommand;
        private MvxCommand<Player> _selectPlayerCommand;



        /// <summary>
        /// Command that adds a new player to the list.
        /// </summary>
        public MvxCommand AddPlayerCommand
        {
            get
            {
                return _addPlayerCommand ?? (_addPlayerCommand = new MvxCommand(() =>
                {
                    // if player can't be added, return
                    if (!ValidateNewPlayer())
                    {
                        return;
                    }

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

        /// <summary>
        /// Command that removes a player from the list.
        /// </summary>
        public MvxCommand RemovePlayerCommand
        {
            get
            {
                return _removePlayerCommand ?? (_removePlayerCommand = new MvxCommand(() =>
                {

                    var player = Players.FirstOrDefault(p => p.Name == PlayerName);
                    if (player != null)
                    {
                        Players.Remove(player);
                    }
                    // clean up the text field after adding a player
                    PlayerName = string.Empty;
                }, () =>
                {
                    // remove can only execute if player name exists in the list
                    return Players.Any(p => p.Name == PlayerName);
                }));
            }
        }

        /// <summary>
        /// Select a player from the list
        /// </summary>
        public MvxCommand<Player> SelectPlayerCommand
        {
            get
            {
                return _selectPlayerCommand ?? (_selectPlayerCommand = new MvxCommand<Player>(p =>
                {
                    // we currently select players by just setting their names to the  text box
                    PlayerName = p.Name;
                }));
            }
        }

        /// <summary>
        /// Returns true if all is good and player can be added.
        /// False otherwise.
        /// </summary>
        /// <returns></returns>
        private bool ValidateNewPlayer()
        {
            // for now let's limit player count to 10 players
            if (Players.Count >= 10)
            {
                _popupService.ShowPopup("Max Players", "You can add a maximum number of 10 players. Please remove a player before adding a new one.");
                return false;
            }
            // do not allow duplicate names
            if (Players.Any(p => p.Name == PlayerName))
            {
                _popupService.ShowPopup("Name Exists", $"{PlayerName} already exists, please enter a different name.");
                return false;
            }

            // all is good
            return true;
        }

        #endregion

    }
}