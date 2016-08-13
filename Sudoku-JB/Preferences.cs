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
    class Preferences : PreferenceActivity
    {
        // Option names and default values
        private const String OPT_MUSIC = "music";
        private const Boolean OPT_MUSIC_DEFAULT = true;

        private const String OPT_HINTS = "hints";
        private const Boolean OPT_HINTS_DEFAULT = true;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
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

        // Get the current value of the hints option
        public static Boolean getHints(Context context)
        {
            return PreferenceManager.GetDefaultSharedPreferences(context).GetBoolean(OPT_HINTS, OPT_HINTS_DEFAULT);
        }

        // Get the current value of the music option
        public static Boolean getMusic(Context context)
        {
            return PreferenceManager.GetDefaultSharedPreferences(context).GetBoolean(OPT_MUSIC, OPT_MUSIC_DEFAULT);
        }
    }
}