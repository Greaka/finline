using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using prototyp.Code.Utility;

namespace prototyp.Code.Game
{
    public class Player : Entity
    {
        private float updraft = 1;
        private float groundzero = 1;
        const float jumpheight = 3f;
        private List<EnvironmentObject> _environmentObjects;

        public Vector3 Position
        {
            get { return _position; }
            private set
            {
                var pos = _position;
                _position = value;
                if (this.isColliding(_environmentObjects))
                {
                    _position = pos;
                }
            }
        }

        public float ViewDirection
        {
            get { return _angle; }
            private set { _angle = value; }
        }

        public void Initialize(ContentManager contentManager)
        {
            _model = contentManager.Load<Model>("Undead");
            _position = Vector3.UnitZ;
        }
        public void Update(GameTime gameTime, List<EnvironmentObject> environmentObjects)
        {
            _environmentObjects = environmentObjects;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                ViewDirection += 0.05f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                ViewDirection -= 0.05f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                var richtung = new Vector3(0, 1, 0).rotate2d(ViewDirection);
                richtung.Normalize();
                Position += richtung * 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                var richtung = new Vector3(0, 1, 0).rotate2d(ViewDirection);
                richtung.Normalize();
                Position -= richtung * 0.1f;
            }
            var pos = new Vector3(_position.X, _position.Y, updraft).lerp(_position, 0.85f);
            if ((pos - _position).Length() < 0.05f && _position.Z > groundzero)
            {
                pos = new Vector3(Position.X, Position.Y, Position.Z - 0.05f);
            }
            //var pos = new Vector3(_position.X, _position.Y, jumpheight/4f*Position.Z - 0.2f*updraft);
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                var oldPosition = Position;
                Position = pos;
                if (_position.Z <= groundzero || Position == oldPosition)
                {
                    updraft = Position.Z + jumpheight;
                }
            }
            if (Math.Abs(updraft - groundzero) > 0.05f && _position.Z >= updraft - 0.05f)
            {
                updraft = groundzero;
            }

            if (updraft <= groundzero)
            {
                Position = pos;
                if (_position.Z < groundzero)
                {
                    _position.Z = groundzero;
                }
            }
            else
            {
                Position = _position.lerp(new Vector3(_position.X, _position.Y, updraft), 0.2f);
            }
        }

        
        public void Draw(Vector3 cameraPosition, float aspectRatio)
        {
            base.Draw(cameraPosition, aspectRatio, _position);
        }
    }
}