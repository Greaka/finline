using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.GameState
{
   enum EGameState 
    {
        None,
       MainMenu,
        InGame,


    }

    interface IGameState
    {
        void initialize(ContentManager content);
        EGameState Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);


    }


}
