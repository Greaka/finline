﻿using System.Collections.Generic;

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
        private int currentSong = 1;
        private static bool soundOn = true;
        private Song musicMainMenu;
        private List<Song> musicIngame = new List<Song>();
        private List<SoundEffect> soundeffectList = new List<SoundEffect>();
        private static SoundEffectInstance soundInstance;

        private KeyboardState oldKeyState;

        public void LoadContent(ContentManager content)
        {
            this.musicMainMenu = content.Load<Song>("Sounds/musicMainMenu");
            this.musicIngame.Add(content.Load<Song>("Sounds/musicIngame1"));
            this.musicIngame.Add(content.Load<Song>("Sounds/musicIngame2"));
            this.soundeffectList.Add(content.Load<SoundEffect>("Sounds/gunshot"));          //position [0]
            this.soundeffectList.Add(content.Load<SoundEffect>("Sounds/enemyshot"));        //position [1]
            this.soundeffectList.Add(content.Load<SoundEffect>("Sounds/playerdeath"));      //position [2]
            this.soundeffectList.Add(content.Load<SoundEffect>("Sounds/enemydeath"));       //position [3]
        }

        public bool GetSoundOn()
        {
            return soundOn;
        }

        public void SoundEffectPlay(int index)
        {
            soundInstance = this.soundeffectList[index].CreateInstance();

            if (this.GetSoundOn() == true) soundInstance.Play();

            // else soundInstance.Stop();
        }

        public void PlayMainMenuMusic()
        {
            MediaPlayer.Play(this.musicMainMenu);
            MediaPlayer.Volume = 1.0f;
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
            KeyboardState newKeyState = Keyboard.GetState();
            if (newKeyState.IsKeyDown(Keys.O) && this.oldKeyState.IsKeyUp(Keys.O))
            {
                soundOn = !soundOn;
            }

            if (this.GetSoundOn() == true)
            {
                MediaPlayer.Resume();
                if (soundInstance != null)
                {
                    soundInstance.Volume = 1.0f;
                }
            }
            else
            {
                MediaPlayer.Pause();
                if (soundInstance != null)
                {
                    soundInstance.Stop();
                    soundInstance.Volume = 0.0f;
                }
            }

            this.oldKeyState = newKeyState;
        }
    }
}
