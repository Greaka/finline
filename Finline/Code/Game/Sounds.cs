// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Sounds.cs" company="Acagamics e.V.">
//   APGL
// </copyright>
// <summary>
//   The sounds.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Finline.Code.Game
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;

    /// <summary>
    /// The sounds.
    /// </summary>
    public class Sounds
    {
        /// <summary>
        /// The music in game.
        /// </summary>
        private readonly List<Song> musicIngame = new List<Song>();

        /// <summary>
        /// The sound effect list.
        /// </summary>
        private readonly List<SoundEffect> soundEffectList = new List<SoundEffect>();

        /// <summary>
        /// The current song.
        /// </summary>
        private int currentSong = 1;

        /// <summary>
        /// The sound on.
        /// </summary>
        private bool soundOn = true;

        /// <summary>
        /// The music main menu.
        /// </summary>
        private Song musicMainMenu;

        /// <summary>
        /// The sound instance.
        /// </summary>
        private SoundEffectInstance soundInstance;

        /// <summary>
        /// The old key state.
        /// </summary>
        private KeyboardState oldKeyState;

        /// <summary>
        /// The pause music.
        /// </summary>
        public static void PauseMusic()
        {
            MediaPlayer.Volume = Math.Abs(MediaPlayer.Volume - 1.0f) < 1e-10 ? 0.4f : 1.0f;
        }

        /// <summary>
        /// The load content.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        public void LoadContent(ContentManager content)
        {
            this.musicMainMenu = content.Load<Song>("Sounds/musicMainMenu");
            this.musicIngame.Add(content.Load<Song>("Sounds/musicIngame1"));
            this.musicIngame.Add(content.Load<Song>("Sounds/musicIngame2"));
            this.soundEffectList.Add(content.Load<SoundEffect>("Sounds/gunshot"));          // position [0]
            this.soundEffectList.Add(content.Load<SoundEffect>("Sounds/enemyshot"));        // position [1]
            this.soundEffectList.Add(content.Load<SoundEffect>("Sounds/playerdeath"));      // position [2]
            this.soundEffectList.Add(content.Load<SoundEffect>("Sounds/enemydeath"));       // position [3]
            this.soundEffectList.Add(content.Load<SoundEffect>("Sounds/enemydeath"));       // position [4]
        }

        /// <summary>
        /// The get sound on.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool GetSoundOn()
        {
            return this.soundOn;
        }

        /// <summary>
        /// The sound effect play.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        public void SoundEffectPlay(int index)
        {
            this.soundInstance = this.soundEffectList[index].CreateInstance();

            if (this.GetSoundOn())
            {
                this.soundInstance.Play();
            }

            // else soundInstance.Stop();
        }

        /// <summary>
        /// The play main menu music.
        /// </summary>
        public void PlayMainMenuMusic()
        {
            MediaPlayer.Play(this.musicMainMenu);
            if (Math.Abs(MediaPlayer.Volume - 1.0f) > 1e-10)
            {
                MediaPlayer.Volume = 1.0f;
            }

            MediaPlayer.IsRepeating = true;
            this.currentSong = (this.currentSong + 1) % 2;
        }

        /// <summary>
        /// The play in game music.
        /// </summary>
        public void PlayIngameMusic()
        {
            MediaPlayer.Play(this.musicIngame[this.currentSong]);
            MediaPlayer.IsRepeating = false;
        }

        /// <summary>
        /// The play in game song change.
        /// </summary>
        public void PlayIngameSongChange()
        {
            if (MediaPlayer.State != MediaState.Stopped)
            {
                return;
            }

            this.currentSong = (this.currentSong + 1) % 2;
            MediaPlayer.Play(this.musicIngame[this.currentSong]);
        }

        /// <summary>
        /// The update.
        /// </summary>
        public void Update()
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
