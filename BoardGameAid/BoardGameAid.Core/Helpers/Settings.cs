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

        private const string ShowRoleTimerSecondsKey = "ShowRoleTimerSeconds";
        private static readonly int ShowRoleTimerSecondsDefault = 15;

        private const string IsPercivalEnabledKey= "IsPercivalEnabled";
        private const string IsMorganaEnabledKey = "IsMorganaEnabled";
        private const string IsMordredEnabledKey= "IsMordredEnabled";
        private const string IsOberonEnabledKey = "IsOberonEnabled";

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

        public static int ShowRoleTimerSetting
        {
            get => AppSettings.GetValueOrDefault(ShowRoleTimerSecondsKey, ShowRoleTimerSecondsDefault);
            set => AppSettings.AddOrUpdateValue(ShowRoleTimerSecondsKey, value);
        }


        /// <summary>
        /// Setting to enable/disable Percival
        /// </summary>
        public static bool IsPercivalEnabledSetting
        {
            get => AppSettings.GetValueOrDefault(IsPercivalEnabledKey, false);
            set => AppSettings.AddOrUpdateValue(IsPercivalEnabledKey, value);
        }

        /// <summary>
        /// Setting  to enable or disable Morgana
        /// </summary>
        public static bool IsMorganaEnabledSetting
        {
            get => AppSettings.GetValueOrDefault(IsMorganaEnabledKey, false);
            set => AppSettings.AddOrUpdateValue(IsMorganaEnabledKey, value);
        }

        /// <summary>
        /// Setting to enable or disabale Mordred
        /// </summary>
        public static bool IsMordredEnabledSetting
        {
            get => AppSettings.GetValueOrDefault(IsMordredEnabledKey, false);
            set => AppSettings.AddOrUpdateValue(IsMordredEnabledKey, value);
        }

        /// <summary>
        /// Setting to enable or disable Oberon
        /// </summary>
        public static bool IsOberonEnabledSetting
        {
            get => AppSettings.GetValueOrDefault(IsOberonEnabledKey, false);
            set => AppSettings.AddOrUpdateValue(IsOberonEnabledKey, value);
        }
    }
}



