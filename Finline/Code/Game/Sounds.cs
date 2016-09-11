namespace Finline.Code.Game
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;

    public class Sounds
    {
        private int currentSong = 1;
        private bool soundOn = true;
        private Song musicMainMenu;
        private readonly List<Song> musicIngame = new List<Song>();
        private readonly List<SoundEffect> soundeffectList = new List<SoundEffect>();
        private SoundEffectInstance soundInstance;

        private KeyboardState oldKeyState;

        public void LoadContent(ContentManager content)
        {
            this.musicMainMenu = content.Load<Song>("Sounds/musicMainMenu");
            this.musicIngame.Add(content.Load<Song>("Sounds/musicIngame1"));
            this.musicIngame.Add(content.Load<Song>("Sounds/musicIngame2"));
            this.soundeffectList.Add(content.Load<SoundEffect>("Sounds/gunshot"));          // position [0]
            this.soundeffectList.Add(content.Load<SoundEffect>("Sounds/enemyshot"));        // position [1]
            this.soundeffectList.Add(content.Load<SoundEffect>("Sounds/playerdeath"));      // position [2]
            this.soundeffectList.Add(content.Load<SoundEffect>("Sounds/enemydeath"));       // position [3]
            this.soundeffectList.Add(content.Load<SoundEffect>("Sounds/enemydeath"));       // position [4]
        }

        public bool GetSoundOn()
        {
            return this.soundOn;
        }

        public void SoundEffectPlay(int index)
        {
            this.soundInstance = this.soundeffectList[index].CreateInstance();

            if (this.GetSoundOn()) this.soundInstance.Play();

            // else soundInstance.Stop();
        }

        public void PlayMainMenuMusic()
        {
            MediaPlayer.Play(this.musicMainMenu);
            if (MediaPlayer.Volume != 1.0f) MediaPlayer.Volume = 1.0f;
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

        public void PauseMusic()
        {
            if (MediaPlayer.Volume == 1.0f)
                MediaPlayer.Volume = 0.4f;
            else MediaPlayer.Volume = 1.0f;
        }

        public void Update(GameTime gameTime)
        {
            var newKeyState = Keyboard.GetState();
            if (newKeyState.IsKeyDown(Keys.O) && this.oldKeyState.IsKeyUp(Keys.O))
            {
                this.soundOn = !this.soundOn;
            }

            if (this.GetSoundOn())
            {
                MediaPlayer.Resume();
                if (this.soundInstance != null)
                {
                    this.soundInstance.Volume = 1.0f;
                }
            }
            else
            {
                MediaPlayer.Pause();
                if (this.soundInstance != null)
                {
                    this.soundInstance.Stop();
                    this.soundInstance.Volume = 0.0f;
                }
            }

            this.oldKeyState = newKeyState;
        }
    }
}
