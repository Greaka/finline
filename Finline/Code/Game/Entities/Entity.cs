using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using prototyp.Code.Constants;
using prototyp.Code.Game.Helper;
using prototyp.Code.Utility;

namespace prototyp.Code.Game.Entities
{
    public abstract class Entity
    {
        protected Model _model;
        protected Vector3 _position;
        protected float _angle;

        public Model GetModel => _model;

        public BoundingSphere GetBound
        {
            get
            {
                var sphere = _model.Meshes[0].BoundingSphere;
                sphere.Center += _position;
                sphere.Radius *= 0.8f;
                return sphere;
            }
        }

        public void SetViewDirection(float angle)
        {
            _angle = angle;
        }

        public void SetViewDirection(Vector2 direction)
        {
            if (direction.Length() > 0)
                _angle = direction.getAngle();
        }

        public void Draw(float aspectRatio)
        {
            var worldMatrix = GetWorldMatrix();
            var cameraLookAtVector = ControlsHelper.PlayerPosition;
            var cameraUpVector = Vector3.UnitZ;

            ControlsHelper.ViewMatrix = Matrix.CreateLookAt(
                GraphicConstants.CameraPosition, cameraLookAtVector, cameraUpVector);
            var projection = Matrix.CreatePerspectiveFieldOfView(
                        GraphicConstants.FieldOfView, aspectRatio, GraphicConstants.NearClipPlane, GraphicConstants.FarClipPlane);

            foreach (var mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = worldMatrix;
                    effect.View = ControlsHelper.ViewMatrix;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }

        private Matrix GetWorldMatrix()
        {

            // this matrix moves the model "out" from the origin
            var translationMatrix = Matrix.CreateTranslation(_position);

            // this matrix rotates everything around the origin
            var rotationMatrix = Matrix.CreateRotationZ(_angle);

            // We combine the two to have the model move in a circle:
            var combined = rotationMatrix * translationMatrix;

            return combined;
        }
    }
}
