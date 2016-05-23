using System.Collections.Generic;
using Finline.Code.Game.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Finline.Code.Utility;

namespace Finline.Code.Game.Entities
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
                float intersection;
                if (this.isColliding(ControlsHelper.EnvironmentObjects, out intersection))
                {
                    _position += new Vector3(ControlsHelper.MoveDirection * intersection, 0);
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