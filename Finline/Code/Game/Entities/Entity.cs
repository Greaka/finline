using System;

using Finline.Code.Constants;
using Finline.Code.Game.Helper;
using Finline.Code.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.Game.Entities
{
    public abstract class Entity
    {
        protected Model _model;
        protected Vector3 _position;
        protected float _angle;

        public Model GetModel => this._model;

        public BoundingSphere GetBound
        {
            get
            {
                var sphere = this._model.Meshes[0].BoundingSphere;
                for (var i = 1; i < this._model.Meshes.Count; i++)
                    sphere = BoundingSphere.CreateMerged(sphere, this._model.Meshes[i].BoundingSphere);
                sphere.Center += this._position;
                sphere.Radius *= 0.8f;
                return sphere;
            }
        }

        public void SetViewDirection(float angle)
        {
            this._angle = angle;
        }

        public void SetViewDirection(Vector2 direction)
        {
            if (direction.Length() > 0) this._angle = direction.getAngle();
        }

        public Vector2 GetViewDirection()
        {
            return Vector2.UnitY.rotate(this._angle);
        }

        public void Draw()
        {
            var worldMatrix = this.GetWorldMatrix();

            foreach (var mesh in this._model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = worldMatrix;
                    effect.View = ControlsHelper.ViewMatrix;
                    effect.Projection = ControlsHelper.ProjectionMatrix;
                }

                mesh.Draw();
            }
        }

        private Matrix GetWorldMatrix()
        {

            // this matrix moves the model "out" from the origin
            var translationMatrix = Matrix.CreateTranslation(this._position);

            // this matrix rotates everything around the origin
            var rotationMatrix = Matrix.CreateRotationZ(this._angle);

            // We combine the two to have the model move in a circle:
            var combined = rotationMatrix * translationMatrix;

            return combined;
        }
    }
}
