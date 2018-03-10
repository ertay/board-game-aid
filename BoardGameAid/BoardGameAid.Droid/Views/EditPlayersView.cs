using Android.App;
using Android.OS;

namespace BoardGameAid.Droid.Views
{
    [Activity(Label = "Players")]
    public class EditPlayersView : BaseView
    {
        protected override int LayoutResource => Resource.Layout.EditPlayersView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
        }
    }
}
