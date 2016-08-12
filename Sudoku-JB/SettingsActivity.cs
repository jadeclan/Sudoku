using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Preferences;

namespace Sudoku_JB
{
    [Activity(Label = "Preferences")]
    public class SettingsActivity : PreferenceActivity
    {
        /// <summary>
        /// Method used to handle display of the Preferences(Settings)
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // todo Need solution for deprecated AddPreferencesFromResource
#pragma warning disable CS0618 // Type or member is obsolete
            this.AddPreferencesFromResource(Resource.Xml.Settings);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}