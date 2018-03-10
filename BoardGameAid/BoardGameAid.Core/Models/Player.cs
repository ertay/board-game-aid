using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameAid.Core.Models
{
    /// <summary>
    /// Base player class. Other types of players will inherit this class.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Player name 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if player is VI, false otherwise.
        /// </summary>
        public bool IsVisuallyImpaired { get; set; }
    }
}
