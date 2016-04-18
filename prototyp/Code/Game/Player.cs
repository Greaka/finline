using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using prototyp.Code.Utility;

namespace prototyp.Code.Game
{
    public class Player
    {
        Model model;
        float angle;
        private Vector3 _position;
        private float updraft = 1;

        public Vector3 position
        {
            get { return _position; }
        }

        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("Undead");
            _position = Vector3.UnitZ;
        }
        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _position.X += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _position.X -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _position.Y -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                _position.Y += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (_position.Z <= 1)
                {
                    updraft = 2.5f;
                }
            }
            if (_position.Z >= 2.45f)
            {
                updraft = 1f;
            }

            if (updraft <= 1)
            {
                _position.Z = 1.05f * position.Z - 0.2f * updraft;
                if (_position.Z < 1)
                {
                    _position.Z = 1;
                }
            }
            else
            {
                _position = _position.lerp(new Vector3(_position.X, _position.Y, updraft), 0.2f);
            }
        }

        // For now we'll take these values in, eventually we'll
        // take a Camera object
        public void Draw(Vector3 cameraPosition, float aspectRatio)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    // We’ll be doing our calculations here...
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
            Matrix translationMatrix = Matrix.CreateTranslation(position);

            // this matrix rotates everything around the origin
            Matrix rotationMatrix = Matrix.CreateRotationZ(angle);

            // We combine the two to have the model move in a circle:
            Matrix combined = rotationMatrix * translationMatrix;

            return combined;
        }
    }
}