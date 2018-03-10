using Android.App;
using Android.OS;

namespace BoardGameAid.Droid.Views
{
    [Activity(Label = "Board Game Aid")]
    public class MainView : BaseView
    {
        protected override int LayoutResource => Resource.Layout.MainView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
        }
    }
}
