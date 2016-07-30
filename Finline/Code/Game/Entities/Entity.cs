using Finline.Code.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.Game.Entities
{
    public abstract class Entity
    {
        private Model model;
        protected Vector3 position;
        protected float _angle;

        private Vector3 centerOffset;

        protected Model _model
        {
            get
            {
                return this.model;
            }

            set
            {
                this.model = value;
                var sphere = this._model.Meshes[0].BoundingSphere;
                for (var i = 1; i < this._model.Meshes.Count; i++) sphere = BoundingSphere.CreateMerged(sphere, this._model.Meshes[i].BoundingSphere);
                this.centerOffset = sphere.Center;
                sphere.Center += this.position;
                sphere.Radius *= this._sphereScaling;
                this.bound = sphere;
            }
        }

        private BoundingSphere bound;

        protected float _sphereScaling = 0.8f;

        public Vector3 Position => this.position;

        public Model GetModel => this._model;

        public BoundingSphere GetBound
        {
            get
            {
                this.bound.Center = this.centerOffset + this.position;
                return this.bound;
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

        public virtual void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            var worldMatrix = this.GetWorldMatrix();

            foreach (var mesh in this._model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = worldMatrix;
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                }

                mesh.Draw();
            }
        }

        private Matrix GetWorldMatrix()
        {

            // this matrix moves the model "out" from the origin
            var translationMatrix = Matrix.CreateTranslation(this.position);

            // this matrix rotates everything around the origin
            var rotationMatrix = Matrix.CreateRotationZ(this._angle);

            // We combine the two to have the model move in a circle:
            var combined = rotationMatrix * translationMatrix;

            return combined;
        }
    }
}
