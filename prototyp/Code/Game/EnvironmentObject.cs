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
    public class EnvironmentObject
    {
        private Model _model;
        private Vector3 _position;
        private float _angle;
        private bool orbit;
        
        public EnvironmentObject(ContentManager contentManager, Vector3 position, GameConstants.EnvObjects model)
        {
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
            foreach (var mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;


                    effect.World = GetWorldMatrix();

                    var cameraLookAtVector = playerPosition;
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

        private Matrix GetWorldMatrix()
        {

            // this matrix moves the model "out" from the origin
            Matrix translationMatrix = Matrix.CreateTranslation(_position);

            // this matrix rotates everything around the origin
            Matrix rotationMatrix = Matrix.CreateRotationZ(_angle);

            // We combine the two to have the model move in a circle:
            Matrix combined = rotationMatrix * translationMatrix;

            return combined;
        }
    }


}