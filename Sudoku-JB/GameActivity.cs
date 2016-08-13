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

namespace Sudoku_JB
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : Activity
    {
        private const String TAG = "Sudoku";
        public const int EASY = 0;
        public const int MEDIUM = 1;
        private const int HARD = 2;

        private int[] puzzle;
        private PuzzleView puzzleView;

        //TODO: Develop method to create random games based on selected difficulty
        private String easyPuzzle =
            "360000000004230800000004200" +
            "070460003820000014500013020" +
            "001900000007048300000000045";
        private String mediumPuzzle =
            "650000070000506000014000005" +
            "007009000002314700000700800" +
            "500000630000201000030000097";
        private String hardPuzzle =
            "009000000080605020501078000" +
            "000000700706040102004000000" +
            "000720903090301080000000600";

        private int[,][] used = new int[9, 9][];

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            int difficultyLevel = Intent.GetIntExtra("difficultyLevel",0);
            puzzle = getPuzzle(difficultyLevel);
            calculateUsedTiles();

            puzzleView = new PuzzleView(this);
            SetContentView(puzzleView);
            puzzleView.RequestFocus();
        }

        protected override void OnResume()
        {
            base.OnResume();
            Music.play(this, Resource.Raw.game);
        }

        protected override void OnPause()
        {
            base.OnPause();
            Music.stop(this);
        }

        static private String toPuzzleString(int[] puzzle)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (int element in puzzle)
            {
                buffer.Append(element);
            }
            return buffer.ToString();
        }

        static protected int[] fromPuzzleString(String puzzleString)
        {
            int[] refreshedPuzzle = new int[puzzleString.Length];
            char[] tempPuzzle = puzzleString.ToCharArray();
            for (int i =0; i < puzzleString.Length; i++)
            {
                refreshedPuzzle[i] = (int) (tempPuzzle[i] - 48);
            }
            return refreshedPuzzle;
        }

        public int getTile(int x, int y)
        {
            return puzzle[y * 9 + x];
        }

        public bool setTileIfValid(int x, int y, int value)
        {
            int[] tiles = getUsedTiles(x, y);
            if (value != 0)
            {
                foreach (int tile in tiles)
                {
                    if (tile == value) return false;
                }
            }
            setTile(x, y, value);
            calculateUsedTiles();
            return true;
        }

        private void setTile(int x, int y, int value)
        {
            puzzle[y * 9 + x] = value;
        }
        public String getTileString(int x, int y)
        {
            int v = getTile(x, y);
            if (v == 0) return "";
            // Convert an integer to a character value (ie,zero = 48)
            v = v + 48;
            char charValue = (char)v;
            string stringValue = "" + charValue;
            return stringValue;
        }

        private int[] getPuzzle(int difficultyLevel)
        {
            String puzzle = easyPuzzle;
            //TODO: Continue last game
            switch (difficultyLevel)
            {
                case HARD:
                    puzzle = hardPuzzle;
                    break;
                case MEDIUM:
                    puzzle = mediumPuzzle;
                    break;
                default:
                    break;
            }
            return fromPuzzleString(puzzle);
       }
        
        public int[] getUsedTiles(int x, int y)
        {
            return used[x,y];
        }
        private void calculateUsedTiles()
        {
            for(int x = 0; x < 9; x++)
            {
                for(int y = 0; y < 9; y++)
                {
                    used[x,y] = calculateUsedTiles(x, y);
                }
            }
        }

        private int[] calculateUsedTiles(int x, int y)
        {
            int[] c = new int[9];

            // horizontal
            for(int i = 0; i < 9; i++)
            {
                //if (i == x) continue;
                int t = getTile(i, y);
                if (t != 0) c[t - 1] = t;
            }

            // vertical
            for (int i = 0; i < 9; i++)
            {
                //if (i == y) continue;
                int t = getTile(x, i);
                if (t != 0) c[t - 1] = t;
            }

            // Same cell block
            int startx = (x / 3) * 3;
            int starty = (y / 3) * 3;
            for (int i = startx; i < startx + 3; i++)
            {
                for(int j=starty; j < starty + 3; j++)
                {
                    //if (i == x && j == y) continue;
                    int t = getTile(i, j);
                    if (t != 0) c[t - 1] = t;
                }
            }

            // Compress
            int nused = 0;
            foreach(int t in c)
            {
                if (t != 0) nused++;
            }
            int[] c1 = new int[nused];
            nused = 0;
            foreach (int t in c)
            {
                if (t != 0) c1[nused++] = t;
            } 
            return c1;
        }

        public void showKeypadOrError(int x, int y)
        {
            int[] tiles = getUsedTiles(x, y);
            if (tiles.Length == 9)
            {
                Toast toast = Toast.MakeText(this,
                    Resource.String.no_moves_label, ToastLength.Short);
                toast.SetGravity(GravityFlags.Center, 0, 0);
                toast.Show();
            } else
            {
                Android.Util.Log.Debug(TAG, "Number used " + tiles.Length);
                Android.Util.Log.Debug(TAG, "showKeypad: used=" + toPuzzleString(tiles));
                Dialog v = new Keypad(this, tiles, puzzleView);
                v.Show();
            }
        }
    }
}