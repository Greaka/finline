using System;
using System.Collections.Generic;

namespace Finline.Code.Game
{
    using Game;
    using GameState;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Media;
    using Microsoft.Xna.Framework.Audio;

    
    public class Sounds : Game
    {
        /*
        Song musicMainMenu;
        SoundEffect shot;
        KeyboardState oldKeyState;
        private bool off = false;


        public Sounds()
        {
            Content.RootDirectory = "Content";
        }


        protected override void LoadContent()
        {
            this.musicMainMenu = Content.Load<Song>("Sounds/musicMainMenu");
            MediaPlayer.Stop();
        }

        
        protected override void Update(GameTime gameTime)
        {
            MediaPlayer.Play(musicMainMenu);
            base.Update(gameTime);
            KeyboardState newKeyState = Keyboard.GetState();

            if (newKeyState.IsKeyDown(Keys.P) && oldKeyState.IsKeyUp(Keys.P))
            {
                Exit();
                
                off = !off;
                if (off == true)
                {
                    MediaPlayer.Pause();
                }
                else
                {
                    MediaPlayer.Resume();
                }
                
            }
            oldKeyState = newKeyState;
        }
    */
    }
}
