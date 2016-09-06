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

        private bool isMoving;

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

            // this.position = new Vector3(90, 240, 0);   // Hörsaal
            // this.position = new Vector3(26, 90, 0);      // 333
            var modelStand = contentManager.Load<Model>(GuiElement.Ausgewaehlt + "_stand");
            var modelLinks = contentManager.Load<Model>(GuiElement.Ausgewaehlt + "_linkesBein");
            var modelRechts = contentManager.Load<Model>(GuiElement.Ausgewaehlt.ToString());

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

            var pos = moveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * this.unitsPerSecond;
            var collisionResult = this.IsColliding(environmentObjects, pos);
            this.position += new Vector3(pos, 0);
            if (collisionResult.HasValue)
            {
                this.position += new Vector3(collisionResult.Value, 0);
            }
        }

        public override void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            base.Draw(viewMatrix, projectionMatrix, this.isMoving ? this.ModelAnimation.CurrentModel : this.Model);
        }
    }
}