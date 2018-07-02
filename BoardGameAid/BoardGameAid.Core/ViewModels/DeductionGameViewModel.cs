using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace BoardGameAid.Core.ViewModels
{
    public abstract class DeductionGameViewModel : MvxViewModel
    {
        /// <summary>
        /// Uses text to speech to rerad role information for VI players
        /// </summary>
        /// <returns></returns>
        public abstract Task SpeakRoleInfo();
    }
}