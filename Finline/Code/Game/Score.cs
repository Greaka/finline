using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.Game
{
    public class Score
    {
        private SpriteBatch _batch;
        private int _score = 0;
        private SpriteFont font;

        public Score(ContentManager content, GraphicsDevice gg)
        {
            font = content.Load<SpriteFont>("font");
            _batch = new SpriteBatch(gg);
        }

        public void AddToScore(int addtoscore)
        {
            _score += addtoscore;
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(Rectangle bound)
        {
            _batch.Begin();
            _batch.DrawString(font, "Score: " + _score.ToString(), new Vector2(bound.Right - 5, bound.Top - 2), Color.Pink);
            _batch.End();
        }
    }
}
