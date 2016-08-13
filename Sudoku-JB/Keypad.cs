using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;

namespace Sudoku_JB
{
    class Keypad : Dialog
    {
        protected const String TAG = "Sudoku";
        private View[] keys = new View[9];
        private View keypad;
        private int[] useds;
        private PuzzleView puzzleView;

        public Keypad(Context context, int[] useds, PuzzleView puzzleView) : base(context)
        {
            this.useds = useds;
            this.puzzleView = puzzleView;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetTitle(Resource.String.keypad_title);
            SetContentView(Resource.Layout.keypad);
            findViews();
            foreach (int element in useds)
            {
                if (element != 0) keys[element - 1].Visibility = ViewStates.Invisible;
            }
            setListeners();
        }
        private void findViews()
        {
            keypad = FindViewById(Resource.Id.keypad);
            keys[0] = FindViewById(Resource.Id.keypad_1);
            keys[1] = FindViewById(Resource.Id.keypad_2);
            keys[2] = FindViewById(Resource.Id.keypad_3);
            keys[3] = FindViewById(Resource.Id.keypad_4);
            keys[4] = FindViewById(Resource.Id.keypad_5);
            keys[5] = FindViewById(Resource.Id.keypad_6);
            keys[6] = FindViewById(Resource.Id.keypad_7);
            keys[7] = FindViewById(Resource.Id.keypad_8);
            keys[8] = FindViewById(Resource.Id.keypad_9);
        }

        private void setListeners()
        {
           for(int i = 0; i<keys.Length; i++)
           {
               int t = i + 1;
               keys[i].Click += (sender,e) => returnResult(t);
           }
            keypad.Click += (sender, e) => returnResult(0);
        }
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            int tile = 0;
            switch (keyCode)
            {
                case Keycode.Num0:
                case Keycode.Space: tile = 0; break;
                case Keycode.Num1: tile = 1; break;
                case Keycode.Num2: tile = 2; break;
                case Keycode.Num3: tile = 3; break;
                case Keycode.Num4: tile = 4; break;
                case Keycode.Num5: tile = 5; break;
                case Keycode.Num6: tile = 6; break;
                case Keycode.Num7: tile = 7; break;
                case Keycode.Num8: tile = 8; break;
                case Keycode.Num9: tile = 9; break;
                default:
                    return base.OnKeyDown(keyCode, e);
            }
            if (isValid(tile))
            {
                returnResult(tile);
            }
            return true;
        }
        private bool isValid(int tile)
        {
            foreach (int t in useds)
            {
                if (tile == t) return false;
            }
            return true;
        }

        private void returnResult(int tile)
        {
            puzzleView.setSelectedTile(tile);
            Dismiss();
        }
    }
}