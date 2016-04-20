using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using prototyp.Code.Utility;

namespace prototyp.Code.Game
{
    public class Player
    {
        Model _model;
        private Vector3 _position;
        private float updraft = 1;
        private float groundzero = 1;
        const float jumpheight = 4f;
        private List<EnvironmentObject> _environmentObjects;

        public Model GetModel => _model;

        public BoundingSphere GetBound
        {
            get
            {
                var sphere = _model.Meshes[0].BoundingSphere;
                sphere.Center = _position;
                return sphere;
            }
        }

        public Vector3 Position
        {
            get { return _position; }
            private set
            {
                var pos = _position;
                _position = value;
                if (GraphicsHelper.isColliding(this, _environmentObjects))
                {
                    _position = pos;
                }
            }
        }

        public float ViewDirection { get; private set; }

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
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (_position.Z <= groundzero)
                {
                    updraft = jumpheight;
                }
            }
            if (_position.Z >= jumpheight - 0.05f)
            {
                updraft = groundzero;
            }

            if (updraft <= groundzero)
            {
                Position = new Vector3(_position.X, _position.Y, jumpheight / 4f * Position.Z - 0.2f * updraft);
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
            foreach (var mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    

                    effect.World = GetWorldMatrix();

                    var cameraLookAtVector = _position;
                    var cameraUpVector = Vector3.UnitZ;

                    effect.View = Matrix.CreateLookAt(
                        cameraPosition, cameraLookAtVector, cameraUpVector);

                    float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
                    float nearClipPlane = 1;
                    float farClipPlane = 200;

                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
                }

                mesh.Draw();
            }
        }
        Matrix GetWorldMatrix()
        {

            // this matrix moves the model "out" from the origin
            Matrix translationMatrix = Matrix.CreateTranslation(Position);

            // this matrix rotates everything around the origin
            Matrix rotationMatrix = Matrix.CreateRotationZ(ViewDirection);

            // We combine the two to have the model move in a circle:
            Matrix combined = rotationMatrix * translationMatrix;

            return combined;
        }
    }
}