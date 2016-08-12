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
            FragmentManager.BeginTransaction().Replace(Android.Resource.Id.Content, new MyPreferenceFragment()).CommitAllowingStateLoss();
        }
        public class MyPreferenceFragment : PreferenceFragment
        {
            public override void OnCreate(Bundle bundle)
            {
                base.OnCreate(bundle);
                AddPreferencesFromResource(Resource.Xml.Settings);
            }
        }
    }
}