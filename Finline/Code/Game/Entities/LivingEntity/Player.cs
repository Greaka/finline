// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="Acagamics e.V.">
//   APGL
// </copyright>
// <summary>
//   Defines the Player type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Finline.Code.Game.Entities.LivingEntity
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Finline.Code.GameState;
    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The player.
    /// </summary>
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

#if DEBUG
        /// <summary>
        /// The god mode.
        /// </summary>
        private bool godMode;

        /// <summary>
        /// The already pressed.
        /// </summary>
        private bool alreadyPressed;

        /// <summary>
        /// The dead.
        /// </summary>
        private bool dead;
#endif

        /// <summary>
        /// The player selection.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "is later used as string for content loading")]
        public enum PlayerSelection
        {
            /// <summary>
            /// The student.
            /// </summary>
            student,

            /// <summary>
            /// The prof.
            /// </summary>
            prof
        }

        /// <summary>
        /// Gets the move direction.
        /// </summary>
        public Vector2 MoveDirection { get; private set; }

#if DEBUG
        /// <summary>
        /// Gets or sets a value indicating whether dead.
        /// </summary>
        public override bool Dead
        {
            get
            {
                return this.dead;
            }

            set
            {
                if (!this.godMode)
                {
                    this.dead = value;
                }
            }
        }
#endif

        /// <summary>
        /// Gets the get bound.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        protected override Model Model
        {
            get
            {
                return base.Model;
            }

            set
            {
                this.model = value;
                var sphere = this.Model.GetVertices().Select(vec => vec * 0.14f).ToList().GetHull();
                this.Bound = sphere;
            }
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <param name="contentManager">
        /// The content manager.
        /// </param>
        /// <param name="environmentObjects">
        /// The environment objects.
        /// </param>
        public void Initialize(ContentManager contentManager, List<EnvironmentObject> environmentObjects)
        {
            this.EnvironmentObjects = environmentObjects;
            this.ModelAnimation = new Animation(4);
            this.DeathAnimation = new Animation(4, false);

            this.position = new Vector3(4, 4, -0.5f); // Standard

            // this.position = new Vector3(100, 240, -0.5f); // BossRaum

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

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        /// <param name="moveDirection">
        /// The move direction.
        /// </param>
        /// <param name="shootDirection">
        /// The shoot direction.
        /// </param>
        public void Update(GameTime gameTime, Vector2 moveDirection, Vector2 shootDirection)
        {
#if DEBUG
            if (!this.alreadyPressed && Keyboard.GetState().IsKeyDown(Keys.G))
            {
                this.godMode = !this.godMode;
                this.alreadyPressed = true;
            }
            else if (this.alreadyPressed && Keyboard.GetState().IsKeyUp(Keys.G))
            {
                this.alreadyPressed = false;
            }
#endif
            this.Update(gameTime);
            if (!this.Dead)
            {
                this.SetViewDirection(shootDirection);
            }

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

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="viewMatrix">
        /// The view matrix.
        /// </param>
        /// <param name="projectionMatrix">
        /// The projection matrix.
        /// </param>
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