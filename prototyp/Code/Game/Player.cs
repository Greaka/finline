using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

        }

        
        public void Draw(Vector3 cameraPosition, float aspectRatio)
        {
            base.Draw(cameraPosition, aspectRatio, _position);
        }
    }
}