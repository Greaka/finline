using Finline.Code.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.Game.Entities
{
    using System.Collections.Generic;

    public abstract class Entity
    {
        private Model model;
        protected Vector3 position;
        protected float _angle;

        protected Model _model
        {
            get
            {
                return this.model;
            }

            set
            {
                this.model = value;
                var sphere = this.RelativeToPosition(this._model.GetVerticies().GetHull());
                this.bound = sphere;
            }
        }

        private IList<Vector3> RelativeToPosition(IEnumerable<Vector3> points)
        {
            var list = new List<Vector3>();
            foreach (var vec in points)
            {
                 list.Add(vec - this.Position);
            }

            return list;
        }

        private IList<Vector3> bound;

        public Vector3 Position => this.position;

        public Model GetModel => this._model;

        public VertexPositionColor[] GetBound
        {
            get
            {
                var list = new VertexPositionColor[this.bound.Count];
                for (var i = 0; i < list.Length; ++i)
                {
                    list[i].Position = this.bound[i] + this.Position;
                    list[i].Color = Color.GreenYellow;
                }
                
                return list;
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
