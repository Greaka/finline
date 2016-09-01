using System.Collections.Generic;

namespace Finline.Code.Game
{
    using GameState;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Media;
    using Microsoft.Xna.Framework.Audio;

    
    public class Sounds
    {
        public int currentSong = 0;
        public Song musicMainMenu;
        public List<Song> musicIngame = new List<Song>(2);
        private SoundEffect gunshot;
        private KeyboardState oldKeyState;


        public void LoadContent(ContentManager content)
        {
            musicMainMenu = content.Load<Song>("Sounds/musicMainMenu");
            musicIngame.Insert(0, content.Load<Song>("Sounds/musicIngame1"));
            musicIngame.Insert(1, content.Load<Song>("Sounds/musicIngame2"));
            //gunshot = content.Load<SoundEffect>("Sounds/gunshot");
        }

        
        public void Update(GameTime gameTime)
        {
            #region Hintergrundmusik pausieren
            KeyboardState newKeyState = Keyboard.GetState();
            if (newKeyState.IsKeyDown(Keys.O) && oldKeyState.IsKeyUp(Keys.O))
            {
                if (MediaPlayer.State == MediaState.Paused)
                {
                    MediaPlayer.Resume();
                }
                else MediaPlayer.Pause();
            }
            oldKeyState = newKeyState;
            #endregion
        }

    }
}
