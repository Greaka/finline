// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="">
// </copyright>
// <summary>
//   Defines the Player type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Finline.Code.Game.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;

    using Finline.Code.Utility;

    using GameState;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Player : LivingEntity
    {
        /// <summary>
        /// The units per second.
        /// </summary>
        private const float UnitsPerSecond = 15;

        /// <summary>
        /// The is moving.
        /// </summary>
        private bool isMoving;

        /// <summary>
        /// Gets the move direction.
        /// </summary>
        public Vector2 MoveDirection { get; private set; }

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

        public void Initialize(ContentManager contentManager, List<EnvironmentObject> environmentObjects)
        {
            this.EnvironmentObjects = environmentObjects;
            this.ModelAnimation = new Animation(4);
            this.DeathAnimation = new Animation(4, false);

            this.position = new Vector3(4, 4, -0.5f); // Standard

            // Animation DeathAnimation = new Animation(4);

            // this.position = new Vector3(90, 240, 0);   // Hörsaal
            // this.position = new Vector3(26, 90, 0);      // 333
            var modelStand = contentManager.Load<Model>(GuiElement.Ausgewaehlt + "_stand");
            var modelLinks = contentManager.Load<Model>(GuiElement.Ausgewaehlt + "_linkesBein");
            var modelRechts = contentManager.Load<Model>(GuiElement.Ausgewaehlt.ToString());

            var death1 = contentManager.Load<Model>(GuiElement.Ausgewaehlt + "_Death1");
            var death2 = contentManager.Load<Model>(GuiElement.Ausgewaehlt + "_Death2");
            var death3 = contentManager.Load<Model>(GuiElement.Ausgewaehlt + "_Death3");
            var death4 = contentManager.Load<Model>(GuiElement.Ausgewaehlt + "_Death4");

            this.DeathAnimation.Add(death1);
            this.DeathAnimation.Add(death2);
            this.DeathAnimation.Add(death3);
            this.DeathAnimation.Add(death4);

            this.ModelAnimation.Add(modelRechts);
            this.ModelAnimation.Add(modelStand);
            this.ModelAnimation.Add(modelLinks);
            this.ModelAnimation.Add(modelStand);

            this.Model = modelStand;
        }

        public void Update(GameTime gameTime, Vector2 moveDirection, Vector2 shootDirection)
        {
            this.Update();

            this.SetViewDirection(shootDirection);
            this.MoveDirection = moveDirection;

            this.isMoving = !moveDirection.Equals(Vector2.Zero);

            var pos = moveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * UnitsPerSecond;
            var collisionResult = this.IsColliding(this.EnvironmentObjects, pos);
            this.position += new Vector3(pos, 0);
            if (collisionResult.Translation.HasValue)
            {
                this.position += new Vector3(collisionResult.Translation.Value, 0);
            }
        }

        public override void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            if (this.Dead == false)
            {
                base.Draw(viewMatrix, projectionMatrix, this.isMoving ? this.ModelAnimation.CurrentModel : this.Model);
            }
            else
            {
                base.Draw(viewMatrix, projectionMatrix, this.DeathAnimation.CurrentModel);
            }
        }
    }
}