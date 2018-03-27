using System.Collections.Generic;
using BoardGameAid.Core.Models;

namespace BoardGameAid.Core.Services
{
    public interface IRandomizerService
    {
        List<SecretHitlerPlayer> GetShuffledPlayers(int playerCount, string gameName);
        List<ResistancePlayer> GetShuffledResistancePlayers(int playerCount, string gameName);

    }
}