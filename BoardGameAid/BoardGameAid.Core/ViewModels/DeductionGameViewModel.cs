using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace BoardGameAid.Core.ViewModels
{
    /// <summary>
    /// This class should be a parent to the other games.
    /// TODO: Refactor to hold shared logic in this class for different games.
    /// </summary>
    public abstract class DeductionGameViewModel : MvxViewModel
    {
        /// <summary>
        /// Uses text to speech to read role information for VI players
        /// </summary>
        /// <returns></returns>
        public abstract Task SpeakRoleInfo();

        /// <summary>
        /// Uses text to speech to read party information.
        /// </summary>
        /// <returns></returns>
        public abstract Task SpeakPartyInfo();
    }
}