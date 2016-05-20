using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using prototyp.Code.Constants;

namespace prototyp.Code.Game.Entities
{
    public class EnvironmentObject : Entity
    {
        private readonly GameConstants.EnvObjects _type;
        private readonly bool orbit;
        public bool Visible { get; set; }

        public GameConstants.EnvObjects Type => _type;

        public EnvironmentObject(ContentManager contentManager, Vector3 position, GameConstants.EnvObjects model)
        {
            Visible = true;
            _type = model;
            switch (model)
            {
                case GameConstants.EnvObjects.bottle_cap2:
                    orbit = true;
                    break;
                default:
                    orbit = false;
                    break;
            }
            _model = contentManager.Load<Model>(model.ToString());
            _position = position;
            _angle = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (orbit)
            {
                _angle += 0.1f;
            }
        }

        public void Draw()
        {
            if (Visible)
                base.Draw();
        }
    }





}