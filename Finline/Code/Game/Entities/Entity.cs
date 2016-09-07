using Finline.Code.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.Game.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Entity
    {
        protected Model model;
        protected Vector3 position;
        protected float Angle;

        protected Model Model
        {
            get
            {
                return this.model;
            }

            set
            {
                this.model = value;
                var sphere = this.Model.GetVerticies().GetHull();
                this.bound = sphere;
            }
        }


        private IList<Vector3> bound;

        public Vector3 Position => this.position;

        public Model GetModel => this.Model;

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
            this.Angle = angle;
        }

        protected void SetViewDirection(Vector2 direction)
        {
            if (direction.Length() > 0) this.Angle = direction.GetAngle();
        }

        protected Vector2 GetViewDirection()
        {
            return Vector2.UnitY.Rotate(this.Angle);
        }

        protected virtual void Draw(Matrix viewMatrix, Matrix projectionMatrix, Model model)
        {
            var worldMatrix = this.GetWorldMatrix();

            foreach (var mesh in model.Meshes)
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

        public virtual void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            this.Draw(viewMatrix, projectionMatrix, this.Model);
        }

        private Matrix GetWorldMatrix()
        {

            // this matrix moves the _model "out" from the origin
            var translationMatrix = Matrix.CreateTranslation(this.position);

            // this matrix rotates everything around the origin
            var rotationMatrix = Matrix.CreateRotationZ(this.Angle);

            // We combine the two to have the _model move in a circle:
            var combined = rotationMatrix * translationMatrix;

            return combined;
        }
    }
}
