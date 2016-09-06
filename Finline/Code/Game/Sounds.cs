using System.Collections.Generic;

namespace Finline.Code.Game
{
    using GameState;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;

    public class Sounds
    {
        public int currentSong = 1;

        public Song musicMainMenu;

        public List<Song> musicIngame = new List<Song>(2);

        public SoundEffect gunshot;

        private KeyboardState oldKeyState;

        public void LoadContent(ContentManager content)
        {
            this.musicMainMenu = content.Load<Song>("Sounds/musicMainMenu");
            this.musicIngame.Insert(0, content.Load<Song>("Sounds/musicIngame1"));
            this.musicIngame.Insert(1, content.Load<Song>("Sounds/musicIngame2"));
            this.gunshot = content.Load<SoundEffect>("Sounds/gunshot");
        }

        public void GunshotPlay()
        {
            if (MediaPlayer.State != MediaState.Paused) this.gunshot.Play();
        }

        public void PlayMainMenuMusic()
        {
            MediaPlayer.Play(this.musicMainMenu);
            MediaPlayer.IsRepeating = true;
            this.currentSong = (this.currentSong + 1) % 2;
        }

        public void PlayIngameMusic()
        {
            MediaPlayer.Play(this.musicIngame[this.currentSong]);
            MediaPlayer.IsRepeating = false;
        }

        public void PlayIngameSongChange()
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                this.currentSong = (this.currentSong + 1) % 2;
                MediaPlayer.Play(this.musicIngame[this.currentSong]);
            }
        }

        /*
        public void PauseMusic()
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Pause();
            else MediaPlayer.Resume();
        }*/
        public void Update(GameTime gameTime)
        {
            KeyboardState newKeyState = Keyboard.GetState();
            if (newKeyState.IsKeyDown(Keys.O) && this.oldKeyState.IsKeyUp(Keys.O))
            {
                if (MediaPlayer.State == MediaState.Paused)
                {
                    MediaPlayer.Resume();
                }
                else MediaPlayer.Pause();
            }

            this.oldKeyState = newKeyState;
        }
    }
}
