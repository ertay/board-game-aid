namespace BoardGameAid.Core.Models
{
    public enum ResistanceRole
    {
        Loyal,
        Spy

    }

    /// <summary>
    /// Model for The Resistance players.
    /// </summary>
    public class ResistancePlayer : Player
    {
        public ResistancePlayer(Player player, ResistanceRole resistanceRole)
        {
            Name = player.Name;
            IsVisuallyImpaired = player.IsVisuallyImpaired;
            Role = resistanceRole;
        }

        public ResistanceRole Role { get; set; }
        public bool IsLoyal => Role == ResistanceRole.Loyal;
    }
}