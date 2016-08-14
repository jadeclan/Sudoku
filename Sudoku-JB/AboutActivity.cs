using Android.App;
using Android.OS;

namespace Sudoku_JB
{
    [Activity(Label = "@string/about_title")]
    class AboutActivity : Activity
    {   
        /// <summary>
        /// Method to manage creating the About Page
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // include the about layout in the view
            SetTheme(Android.Resource.Style.ThemeDialog);
            SetContentView(Resource.Layout.About);
        }
    }
}