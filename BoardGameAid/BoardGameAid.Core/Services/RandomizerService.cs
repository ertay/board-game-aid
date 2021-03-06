﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGameAid.Core.Helpers;
using BoardGameAid.Core.Models;

namespace BoardGameAid.Core.Services
{
    public class RandomizerService : IRandomizerService
    {
        public List<SecretHitlerPlayer> GetShuffledPlayers(int playerCount, string gameName)
        {
            // this needs to be a generic method
            // we should read the roles and number of theme from a config file
            // for now we're just gonna test using hardcoded secret hitler role counts

            Dictionary<SecretHitlerRole, int> roleCounts = new Dictionary<SecretHitlerRole, int>();
            int fascistCount = 0;
            switch (playerCount)
            {
                case 5:
                case 6:
                    fascistCount = 1;
                    break;
                case 7:
                case 8:
                    fascistCount = 2;
                    break;
                case 9:
                case 10:
                    fascistCount = 3;
                    break;
            }

            roleCounts.Add(SecretHitlerRole.Hitler, 1);
            roleCounts.Add(SecretHitlerRole.Fascist, fascistCount);
            roleCounts.Add(SecretHitlerRole.Liberal, playerCount - fascistCount - 1);

            // now fill a list with Roles
            List<SecretHitlerRole> roles = new List<SecretHitlerRole>();
            foreach (var roleCount in roleCounts)
            {
                for (int i = 0; i < roleCount.Value; i++)
                {
                    roles.Add(roleCount.Key);
                }
            }

            // now shuffle the roles and return the list
            roles.Shuffle();
            List<SecretHitlerPlayer> shPlayers = new List<SecretHitlerPlayer>();
            int index = 0;
            foreach (var player in Settings.PlayersSetting)
            { 
                shPlayers.Add(new SecretHitlerPlayer(player, roles[index++]));
            }

            return shPlayers;
        }

        public List<ResistancePlayer> GetShuffledResistancePlayers(int playerCount, string gameName)
        {
            // this also needs to be merged with the above and turned into a generic algorithm

            Dictionary<ResistanceRole, int> roleCounts = new Dictionary<ResistanceRole, int>();
            int spiesCount = 0;
            switch (playerCount)
            {
                case 5:
                case 6:
                    spiesCount = 2;
                    break;
                case 7:
                case 8:
                case 9:
                    spiesCount = 3;
                    break;
                case 10:
                    spiesCount = 4;
                    break;
            }

            roleCounts.Add(ResistanceRole.Spy, spiesCount);
            roleCounts.Add(ResistanceRole.Loyal, playerCount - spiesCount);

            // now fill a list with Roles
            List<ResistanceRole> roles = new List<ResistanceRole>();
            foreach (var roleCount in roleCounts)
            {
                for (int i = 0; i < roleCount.Value; i++)
                {
                    roles.Add(roleCount.Key);
                }
            }

            // set the special character roles
            // should grab this from settings for 
            // optional characters
            roles[roles.IndexOf(ResistanceRole.Loyal)] = ResistanceRole.Merlin;
            roles[roles.IndexOf(ResistanceRole.Spy)] = ResistanceRole.Assassin;

            if (Settings.IsPercivalEnabledSetting)
            {
                roles[roles.IndexOf(ResistanceRole.Loyal)] = ResistanceRole.Percival;
            }

            if (Settings.IsMorganaEnabledSetting)
            {
                roles[roles.IndexOf(ResistanceRole.Spy)] = ResistanceRole.Morgana;
            }

            if (Settings.IsMordredEnabledSetting)
            {
                roles[roles.IndexOf(ResistanceRole.Spy)] = ResistanceRole.Mordred;
            }

            if (Settings.IsOberonEnabledSetting)
            {
                roles[roles.IndexOf(ResistanceRole.Spy)] = ResistanceRole.Oberon;
            }

            
            // now shuffle the roles and return the list
            roles.Shuffle();
            List<ResistancePlayer> resistancePlayers = new List<ResistancePlayer>();
            int index = 0;
            foreach (var player in Settings.PlayersSetting)
            {
                resistancePlayers.Add(new ResistancePlayer(player, roles[index++]));
            }

            return resistancePlayers;
        }


    }
}
