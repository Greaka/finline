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
        private int currentSong = 1;
        public bool SoundOn = true;
        private Song musicMainMenu;
        private List<Song> musicIngame = new List<Song>(2);
        private SoundEffect playerShot;
        private static SoundEffectInstance playerShotInstance;
        private SoundEffect enemyShot;
        private static SoundEffectInstance enemyShotInstance;


        private KeyboardState oldKeyState;

        public void LoadContent(ContentManager content)
        {
            this.musicMainMenu = content.Load<Song>("Sounds/musicMainMenu");
            this.musicIngame.Insert(0, content.Load<Song>("Sounds/musicIngame1"));
            this.musicIngame.Insert(1, content.Load<Song>("Sounds/musicIngame2"));
            this.playerShot = content.Load<SoundEffect>("Sounds/gunshot");
        }
 
        public void GunshotPlay()
        {
            playerShotInstance = this.playerShot.CreateInstance();
            if (this.SoundOn == true)
                playerShotInstance.Play();
            else playerShotInstance.Stop();
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
        
        public void TurnSoundOnOff()
        {
            this.SoundOn = !this.SoundOn;
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
                this.SoundOn = !this.SoundOn;
            }

            if (this.SoundOn == true)
            {
                MediaPlayer.Resume();
                if (playerShotInstance != null || enemyShotInstance != null)
                {
                    playerShotInstance.Volume = 1.0f;

                    // enemyShotInstance.Volume = 1.0f;
                }
            }
            else
            {
                MediaPlayer.Pause();
                if (playerShotInstance != null || enemyShotInstance != null)
                {
                    playerShotInstance.Volume = 0.0f;

                    // enemyShotInstance.Volume = 0.0f;
                }
            }

            this.oldKeyState = newKeyState;
        }
    }
}
