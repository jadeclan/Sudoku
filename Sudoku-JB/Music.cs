using Android.Content;
using Android.Media;

namespace Sudoku_JB
{
    class Music
    {
        private static MediaPlayer mp = null;
        /// <summary>
        /// Method to initiate playing of a resource
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resource"></param>
        public static void play(Context context, int resource)
        {
            stop(context);
            // Start music only if its preference is enabled
            if (Preferences.getMusic(context))
            {
                mp = MediaPlayer.Create(context, resource);
                mp.Looping = true;
                mp.Start();
            }
        }

        public static void stop(Context context)
        {
            if(mp != null)
            {
                mp.Stop();
                mp.Release();
                mp = null;
            }
        }
    }

}