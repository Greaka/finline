using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using prototyp.Code.Game.Helper;
using prototyp.Code.Utility;

namespace prototyp.Code.Game.Entities
{
    public class Player : Entity
    {
        private float unitsPerSecond = 15;

        public Vector3 Position
        {
            get { return _position; }
            private set
            {
                var pos = _position;
                _position = value;
                if (this.isColliding(ControlsHelper.EnvironmentObjects))
                {
                    _position = pos;
                }
            }
        }

        public void Initialize(ContentManager contentManager)
        {
            _model = contentManager.Load<Model>("Undead");
            _position = Vector3.UnitZ;
        }

        public void Update(GameTime gameTime)
        {
            SetViewDirection(ControlsHelper.MoveDirection);
            Position += new Vector3(ControlsHelper.MoveDirection * 
               (float)gameTime.ElapsedGameTime.TotalSeconds *
               unitsPerSecond,
               0);
            ControlsHelper.PlayerPosition = _position;
        }
    }
}