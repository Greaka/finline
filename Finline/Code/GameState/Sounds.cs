using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finline.Code.Game
{
    using Game;
    using GameState;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Media;
    /*
    class Sounds : Game
    {
        Song musicMainMenu;

        private bool off = false;
        KeyboardState oldKeyState;

        public Sounds(ContentManager Content)
        {
            this.musicMainMenu = Content.Load<Song>("musicMainMenu");
            MediaPlayer.Play(musicMainMenu);
        }


        protected override void Update(GameTime gameTime)
        {
            //if (nextGameState == GameState.EGameState.InGame)

            KeyboardState newKeyState = Keyboard.GetState();
            if (newKeyState.IsKeyDown(Keys.O) && oldKeyState.IsKeyUp(Keys.O))
            {
                if (off == true)
                {
                    MediaPlayer.Pause();
                }
                else
                {
                    MediaPlayer.Resume();
                }
                off = !off;
            }

            oldKeyState = newKeyState;
        }
    }*/
}
