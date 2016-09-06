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
        public int CurrentSong = 1;

        public Song MusicMainMenu;

        public List<Song> MusicIngame = new List<Song>(2);

        public SoundEffect Gunshot;

        private KeyboardState oldKeyState;

        public void LoadContent(ContentManager content)
        {
            this.MusicMainMenu = content.Load<Song>("Sounds/musicMainMenu");
            this.MusicIngame.Insert(0, content.Load<Song>("Sounds/musicIngame1"));
            this.MusicIngame.Insert(1, content.Load<Song>("Sounds/musicIngame2"));
            this.Gunshot = content.Load<SoundEffect>("Sounds/gunshot");
        }

        public void GunshotPlay()
        {
            if (MediaPlayer.State != MediaState.Paused) this.Gunshot.Play();
        }

        public void PlayMainMenuMusic()
        {
            MediaPlayer.Play(this.MusicMainMenu);
            MediaPlayer.IsRepeating = true;
            this.CurrentSong = (this.CurrentSong + 1) % 2;
        }

        public void PlayIngameMusic()
        {
            MediaPlayer.Play(this.MusicIngame[this.CurrentSong]);
            MediaPlayer.IsRepeating = false;
        }

        public void PlayIngameSongChange()
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                this.CurrentSong = (this.CurrentSong + 1) % 2;
                MediaPlayer.Play(this.MusicIngame[this.CurrentSong]);
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
