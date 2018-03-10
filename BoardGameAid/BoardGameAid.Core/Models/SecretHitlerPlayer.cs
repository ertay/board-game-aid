namespace BoardGameAid.Core.Models
{
    /// <summary>
    /// The different roles in the Secret Hitler game
    /// </summary>
    public enum SecretHitlerRole
    {
        Liberal,
        Fascist,
        Hitler
    }

    /// <summary>
    /// Secret Hitler Player with game specific properties.
    /// </summary>
    public class SecretHitlerPlayer : Player
    {
        public SecretHitlerRole Role { get; set; }
        public bool IsLiberal => Role == SecretHitlerRole.Liberal;
    }
}