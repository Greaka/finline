using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using prototyp.Code.Constants;

namespace prototyp.Code.Game
{
    public class EnvironmentObject : Entity
    {
        private GameConstants.EnvObjects _type;
        private bool orbit;

        public GameConstants.EnvObjects Type => _type;

        public EnvironmentObject(ContentManager contentManager, Vector3 position, GameConstants.EnvObjects model)
        {
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

        public void Draw(Vector3 cameraPosition, float aspectRatio, Vector3 playerPosition)
        {
            base.Draw(cameraPosition, aspectRatio, playerPosition);
        }
    }


}