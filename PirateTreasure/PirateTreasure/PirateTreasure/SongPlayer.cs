using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace PirateTreasure
{
    public class SongPlayer
    {
        private bool isPlaying = false;

        public void Play(Song sound)
        {
            if (!isPlaying)
            {
                MediaPlayer.Play(sound);
                MediaPlayer.IsRepeating = true;
                isPlaying = true;
            }
        }

        public void Play(SoundEffect soundEffect)
        {
            soundEffect.Play();
        }

        public void Stop()
        {
            if (isPlaying)
                MediaPlayer.Stop();

            isPlaying = false;

        }
    }
}
