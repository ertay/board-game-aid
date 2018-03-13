using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGameAid.Core.Models;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace BoardGameAid.Core.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        private const string PlayersKey = "Players";
        private static readonly string PlayersDefault = string.Empty;

        #endregion


        /// <summary>
        /// Gets or sets list of players in local settings.
        /// Used to save/load players.
        /// We use JSON serialization, since we can't save objects to settings.
        /// </summary>
        public static List<Player> PlayersSetting
        {
            get
            {
                string serialized = AppSettings.GetValueOrDefault(PlayersKey, PlayersDefault);
                if (string.IsNullOrEmpty(serialized))
                {
                    return new List<Player>();
                }
                List<Player> players = JsonConvert.DeserializeObject<List<Player>>(serialized);
                return players;
            }
            set
            {
                string serialized = JsonConvert.SerializeObject(value);
                AppSettings.AddOrUpdateValue(PlayersKey, serialized);
            }
        }

    }
}
