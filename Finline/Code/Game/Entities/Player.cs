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
        private float updraft = 1;
        private float groundzero = 1;

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
            Position += new Vector3(ControlsHelper.MoveDirection, 0)/10;
            ControlsHelper.PlayerPosition = _position;
        }
    }
}