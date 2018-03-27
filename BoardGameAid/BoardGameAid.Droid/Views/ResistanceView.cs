using Android.App;
using Android.OS;

namespace BoardGameAid.Droid.Views
{
    [Activity(Label = "The Resistance")]
    public class ResistanceView : BaseView
    {
        protected override int LayoutResource => Resource.Layout.ResistanceView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
        }
    }
}
