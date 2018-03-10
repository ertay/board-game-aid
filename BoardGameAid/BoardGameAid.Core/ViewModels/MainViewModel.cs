using MvvmCross.Core.ViewModels;

namespace BoardGameAid.Core.ViewModels
{
    public class MainViewModel
        : MvxViewModel
    {
        string hello = "Hello MvvmCross";
        public string Hello
        {
            get { return hello; }
            set => SetProperty(ref hello, value);
        }

        #region commands

        private MvxCommand _showEditPlayersCommand;

        public MvxCommand ShowEditPlayersCommand
        {
            get
            {
                return _showEditPlayersCommand ?? (_showEditPlayersCommand = new MvxCommand(() =>
                {
                    ShowViewModel<EditPlayersViewModel>();
                }));
            }

        }

        #endregion
    }
}
