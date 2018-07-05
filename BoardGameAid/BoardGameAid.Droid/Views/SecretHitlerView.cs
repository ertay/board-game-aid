using Android.App;
using Android.OS;
using BoardGameAid.Core.ViewModels;

namespace BoardGameAid.Droid.Views
{
    [Activity(Label = "Secret Hitler")]
    public class SecretHitlerView : BaseView
    {
        protected override int LayoutResource => Resource.Layout.SecretHitlerView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
        }

        public override void OnBackPressed()
        {
            ((SecretHitlerViewModel)ViewModel).QuitGame();
        }
    }
}
