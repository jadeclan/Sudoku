using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Preferences;
using Android.Util;

namespace Sudoku_JB
{
    [Activity(MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Button to continue an existing game
            View continueButton = 
                FindViewById<Button>(Resource.Id.continue_button);
            continueButton.Click += (sender, e) =>
            {
                // TODO: Implement Continue a Game
            };

            // Button to start a new game
            View newButton = FindViewById<Button>(Resource.Id.new_button);
            newButton.Click += (sender, e) =>
            {
                PopupMenu menu = new PopupMenu(this, newButton);
                menu.Inflate(Resource.Menu.DifficultyMenu);

                menu.MenuItemClick += (difficultyLevelChosen, arg) =>
                {
                /* Work around to get an integer value of the choice
                   arg.Item.NumericShortcut returns ascii value of choice
                   DifficultyMenu.xml file includes 
                   android:numericShortcut=xx" where xx is 0(easy), 1 or 2
                */
                    startGame(arg.Item.NumericShortcut - 48);
                };

                /*menu.DismissEvent += (s2, arg1) =>
                {
                    Console.WriteLine("menu dismissed");
                };*/

                menu.Show();
            };

            // Button to view the about screen
            View aboutButton = FindViewById<Button>(Resource.Id.about_button);
            aboutButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(AboutActivity));
                StartActivity(intent);
            };

            // Button to exit the game
            View exitButton = FindViewById<Button>(Resource.Id.exit_button);
            exitButton.Click += (sender, e) =>
            {
               Finish();
            };

        }

        protected override void OnResume()
        {
            base.OnResume();
            Music.play(this, Resource.Raw.main);
        }

        protected override void OnPause()
        {
            base.OnPause();
            Music.stop(this);
        }
        /// <summary>
        /// Initiation of management of shared preferences (game settings)
        /// Note: XML for choices contained in Resources/menu/SettingsMenu.xml
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.SettingsMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        /// <summary>
        /// Method to handle a choice made from a Menu
        /// Set Resources/menu/SettingsMenu for cases
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.settings:
                    var intent = new Intent(this, typeof(SettingsActivity));
                    StartActivity(intent);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
        private void startGame(int i)
        {
            Console.WriteLine("Option " + i+ " selected");
            var intent = new Intent(this, typeof(GameActivity));
            intent.PutExtra("difficultyLevel", i);
            StartActivity(intent);
        }
    }
}

