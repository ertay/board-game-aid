using Android.App;
using Android.OS;

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
    }
}
