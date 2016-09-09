// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="">
// </copyright>
// <summary>
//   Defines the Player type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Finline.Code.Game.Entities
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;

    using Finline.Code.Utility;

    using GameState;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Player : LivingEntity
    {
        private readonly float unitsPerSecond = 15;
        Animation DeathAnimation = new Animation(4);
        private bool isMoving;
        private bool dead = false;
        Vector2 pos;

        
       
        protected override Model Model
        {
            get
            {
                return base.Model;
            }

            set
            {
                this.model = value;
                var sphere = this.Model.GetVerticies().Select(vec => vec * 0.14f).ToList().GetHull();
                this.Bound = sphere;
            }
        }

        public override VertexPositionColor[] GetBound
        {
            get
            {
                var list = new VertexPositionColor[this.Bound.Count];
                for (var i = 0; i < list.Length; ++i)
                {
                    list[i].Position = (this.Bound[i] + this.Position).RotateOrigin(this.Position, this.Angle);
                    list[i].Color = Color.GreenYellow;
                }

                return list;
            }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum PlayerSelection
        {
            student, 
            prof
        }

        public void Initialize(ContentManager contentManager)
        {
            this.ModelAnimation = new Animation(4);
            this.position = new Vector3(4, 4, -0.5f); // Standard

            // Animation DeathAnimation = new Animation(4);

            // this.position = new Vector3(90, 240, 0);   // Hörsaal
            // this.position = new Vector3(26, 90, 0);      // 333
            var modelStand = contentManager.Load<Model>(GuiElement.Ausgewaehlt + "_stand");
            var modelLinks = contentManager.Load<Model>(GuiElement.Ausgewaehlt + "_linkesBein");
            var modelRechts = contentManager.Load<Model>(GuiElement.Ausgewaehlt.ToString());

            var Death1 = contentManager.Load<Model>(GuiElement.Ausgewaehlt + "_Death1");
            var Death2 = contentManager.Load<Model>(GuiElement.Ausgewaehlt + "_Death2");
            var Death3 = contentManager.Load<Model>(GuiElement.Ausgewaehlt + "_Death3");
            var Death4 = contentManager.Load<Model>(GuiElement.Ausgewaehlt + "_Death4");

            DeathAnimation.Add(Death1);
            DeathAnimation.Add(Death2);
            DeathAnimation.Add(Death3);
            DeathAnimation.Add(Death4);

            this.ModelAnimation.Add(modelRechts);
            this.ModelAnimation.Add(modelStand);
            this.ModelAnimation.Add(modelLinks);
            this.ModelAnimation.Add(modelStand);

            this.Model = modelStand;
            
        }

        public void Update(GameTime gameTime, Vector2 moveDirection, Vector2 shootDirection, List<EnvironmentObject> environmentObjects)
        {
            
            this.SetViewDirection(shootDirection);
         

            this.isMoving = !moveDirection.Equals(Vector2.Zero);

            pos = moveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * this.unitsPerSecond;
            var collisionResult = this.IsColliding(environmentObjects, pos);
            this.position += new Vector3(pos, 0);
        //  getPlayerPosition();
            if (collisionResult.HasValue)
            {
                this.position += new Vector3(collisionResult.Value, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.N)) dead = true;
            if (Keyboard.GetState().IsKeyDown(Keys.M)) dead = false;
        }

        public override void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            if (dead == false) base.Draw(viewMatrix, projectionMatrix, this.isMoving ? this.ModelAnimation.CurrentModel : this.Model); else base.Draw(viewMatrix, projectionMatrix, this.DeathAnimation.CurrentModel);
        }

        public Vector3 getPlayerPosition()
        {
            return position;
        }
    }
}