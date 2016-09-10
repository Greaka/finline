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
        protected float Angle { get; set; }

        protected virtual Model Model
        {
            get
            {
                return this.model;
            }

            set
            {
                this.model = value;
                var sphere = this.Model.GetVerticies().GetHull();
                this.Bound = sphere;
            }
        }

        protected static IList<Vector3> VerschiebeBound(IList<Vector3> list, Vector2 schiebung)
        {
            list = list.Select(vec => vec + new Vector3(schiebung, 0)).ToList();

            return list;
        }

        protected IList<Vector3> Bound { get; set; }

        public Vector3 Position => this.position;

        public virtual VertexPositionColor[] GetBound
        {
            get
            {
                var list = new VertexPositionColor[this.Bound.Count];
                for (var i = 0; i < list.Length; ++i)
                {
                    list[i].Position = this.Bound[i] + this.Position;
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

        public Vector2 GetViewDirection()
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

                    effect.FogEnabled = true; // Nebel
                    effect.FogColor = Color.Gray.ToVector3();
                    effect.FogStart = 15;
                    effect.FogEnd = 30;

                    // effect.LightingEnabled = true;  //Turn on the lighting subsystem.
                     effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0, 0);  // a red light
                     effect.DirectionalLight0.Direction = new Vector3(1, 1, 1);     // coming along the x-axis
                     effect.DirectionalLight0.SpecularColor = new Vector3(0, 0.3f, 0);   // with green highlights
                    // effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);    // Add some overall ambient light.
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
