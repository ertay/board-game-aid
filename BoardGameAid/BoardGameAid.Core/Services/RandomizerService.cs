using System;
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
    }
}
