using System;
using Android.Content;
using Android.Views;
using Android.Graphics;
using Android.Util;
using static Android.Graphics.Paint;
using Android.Runtime;
using Android.Views.Animations;

namespace Sudoku_JB
{
    class PuzzleView : View
    {
        private const String TAG = "***Sudoku***";
        private GameActivity game;
        private float width;
        private float height;
        private int selX;
        private int selY;
        private Rect selRect = new Rect();
        //TODO: Discuss with Jordan the syntax of PuzzleView constructor
        public PuzzleView(Context context) : base(context)
        {
            this.game = (GameActivity)context;
            this.RequestFocus();
            this.RequestFocusFromTouch();
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            width = w / 9f;
            height = h / 9f;
            getRect(selX, selY, selRect);
            base.OnSizeChanged(w, h, oldw, oldh);
        }

        private void getRect(int x, int y, Rect rect)
        {
            rect.Set((int)(x * width),
                     (int)(y * height),
                     (int)(x * width + width),
                     (int)(y * height + height));
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            Paint background = new Paint
            {
                Color = Resources.GetColor(Resource.Color.puzzle_background,null)
            };

            // Parameters are Left, Top, Right, Bottom, paint to use
            canvas.DrawRect(0, 0, canvas.Width, canvas.Height, background);

            //Draw the Board
            //Define Colors for the grid lines
            Paint dark = new Paint
            {
                Color = Resources.GetColor(Resource.Color.puzzle_dark, null)
            };

            Paint hilite = new Paint
            {
                Color = Resources.GetColor(Resource.Color.puzzle_hilite, null)
            };

            Paint light = new Paint
            {
                Color = Resources.GetColor(Resource.Color.puzzle_light, null)
            };
            if(Preferences.getHints(Context)){
                //Draw the hints
                // Pick a hint based on number of moves left
                int[] c =
                {
                Resource.Color.puzzle_hint_0,
                Resource.Color.puzzle_hint_1,
                Resource.Color.puzzle_hint_2
            };
                Paint hint = new Paint();
                Rect r = new Rect();
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        int movesLeft = 9 - game.getUsedTiles(i, j).Length;
                        // Log.Debug(TAG, i +","+j+"="+movesLeft);
                        if (movesLeft < c.Length)
                        {
                            if (game.getTile(i, j) == 0)
                            {
                                getRect(i, j, r);
                                hint.Color = Resources.GetColor(c[movesLeft], null);
                                canvas.DrawRect(r, hint);
                            }
                        }
                    }
                }

                // Determine if the square must be a particular value
                // Might make this another level of hints.
                int startx;
                int starty;
                int count;
                int hintx = 0;
                int hinty = 0;
                hint.Color = Resources.GetColor(c[1], null);

                for (int cellBlock = 0; cellBlock < 9; cellBlock++)
                {
                    starty = (cellBlock / 3) * 3;
                    startx = (cellBlock % 3) * 3;
                    for (int number = 1; number <= 9; number++)
                    {
                        count = 0;
                        for (int j = startx; j < startx + 3; j++)
                        {
                            for (int k = starty; k < starty + 3; k++)
                            {
                                int usedLength = game.getUsedTiles(j, k).Length;
                                // Log.Debug(TAG, "cell block " + cellBlock + "(" + j + "," + k + ") used = " + usedLength);
                                for (int l = 0; l < usedLength; l++)
                                {
                                    if (game.getUsedTiles(j, k)[l] == number || game.getTile(j, k) != 0)
                                    {
                                        count++;
                                        break;
                                    }
                                    else
                                    {
                                        if (game.getUsedTiles(j, k)[l] > number)
                                        {
                                            hintx = j;
                                            hinty = k;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (count == 8)
                        {
                            getRect(hintx, hinty, r);
                            canvas.DrawRect(r, hint);
                            // Log.Debug(TAG, "DRAWING (" + hintx + "," + hinty + ") number "+number+", cell block " +cellBlock + ", startx "+startx+", starty "+ starty+ ", count "+count);
                            count = 0;
                            hintx = 0;
                            hinty = 0;
                        }
                    }
                }
            }

            //Draw the minor grid lines
            for (int i = 0; i < 9; i++)
            {
                //Parameters(start x, start y, stop x, stop y, paint)
                canvas.DrawLine(0, i * height, canvas.Width, i * height, light);
                canvas.DrawLine(0, i * height + 1, canvas.Width, i * height + 1, hilite);
                canvas.DrawLine(i * width, 0, i * width, canvas.Height, light);
                canvas.DrawLine(i * width + 1, 0, i * width + 1, canvas.Height, hilite);
            }

            // Draw the major grid lines
            for (int i = 0; i < 9; i++)
            {
                if (i % 3 != 0) continue;
                canvas.DrawLine(0, i * height - 1, canvas.Width, i * height -1, dark);
                canvas.DrawLine(0, i * height, canvas.Width, i * height, dark);
                canvas.DrawLine(0, i * height + 1, canvas.Width, i * height + 1, hilite);
                canvas.DrawLine(i * width - 1, 0, i * width - 1, canvas.Height, dark);
                canvas.DrawLine(i * width, 0, i * width, canvas.Height, dark);
                canvas.DrawLine(i * width + 1, 0, i * width + 1, canvas.Height, hilite);
            }

            //Draw the numbers
            //Define color and style for numbers
            Paint foreground = new Paint
            {
                Color = Resources.GetColor(Resource.Color.puzzle_foreground, null),
                AntiAlias = true
            };
            foreground.SetStyle(Paint.Style.Fill);
            foreground.TextSize = height * .75f;
            foreground.TextScaleX = (width / height);
            foreground.TextAlign = (Paint.Align.Center);

            //Draw the number in the center of the tile
            FontMetrics fm = foreground.GetFontMetrics();
            //Centering in X: use alignment( and X at midpoint)
            float x = width / 2;
            //Centering in Y: measure ascent and descent first
            float y = height / 2 - (fm.Ascent + fm.Descent) / 2;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    canvas.DrawText(this.game.getTileString(i, j),
                        i * width + x, j * height + y, foreground);
                }
            }

            //Draw the selection
            Paint selected = new Paint
            {
                Color = Resources.GetColor(Resource.Color.puzzle_selected, null)
            };
            canvas.DrawRect(selRect, selected);
        }
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            switch (keyCode)
            {
                case Keycode.DpadUp:
                    select(selX, selY - 1);
                    break;
                case Keycode.DpadDown:
                    select(selX, selY + 1);
                    break;
                case Keycode.DpadLeft:
                    select(selX - 1, selY);
                    break;
                case Keycode.DpadRight:
                    select(selX + 1, selY);
                    break;
                case Keycode.Num0:
                case Keycode.Space:   setSelectedTile(0); break;
                case Keycode.Num1:    setSelectedTile(1); break;
                case Keycode.Num2:    setSelectedTile(2); break;
                case Keycode.Num3:    setSelectedTile(3); break;
                case Keycode.Num4:    setSelectedTile(4); break;
                case Keycode.Num5:    setSelectedTile(5); break;
                case Keycode.Num6:    setSelectedTile(6); break;
                case Keycode.Num7:    setSelectedTile(7); break;
                case Keycode.Num8:    setSelectedTile(8); break;
                case Keycode.Num9:    setSelectedTile(9); break;
                case Keycode.Enter:
                case Keycode.DpadCenter:
                    game.showKeypadOrError(selX, selY);
                    break;
            }
            return base.OnKeyDown(keyCode, e);
        }
        private void select(int x, int y)
        {
            Invalidate(selRect);
            selX = Math.Min(Math.Max(x, 0), 8);
            selY = Math.Min(Math.Max(y, 0), 8);
            getRect(selX, selY, selRect);
            Invalidate(selRect);
        }
        public void setSelectedTile(int tile)
        {
            if(game.setTileIfValid(selX, selY, tile))
            {
                Invalidate();
            } else
            {
                Log.Debug(TAG, "setSelectedTile: invalid: " + tile);
                // TODO: Test shake screen once KeyPad is functional
                StartAnimation(AnimationUtils.LoadAnimation(game, Resource.Animation.shake));
            }
        }
        public override bool OnTouchEvent(MotionEvent e)
        {
            if(e.Action != MotionEventActions.Down)
            {
                return base.OnTouchEvent(e);
            }
            select((int)(e.GetX() / width),
                   (int)(e.GetY() / height));
            game.showKeypadOrError(selX, selY);
            Log.Debug(TAG, "onTouchEvent: x " + selX + ", y " + selY);
            return true;
        }
    }
}