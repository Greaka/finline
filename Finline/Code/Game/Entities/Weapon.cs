namespace Finline.Code.Game.Entities
{
    using System;

    using Finline.Code.Game.Entities.LivingEntity;
    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Weapon : Entity
    {
        private readonly Player player;

        protected override Model Model
        {
            get
            {
                return base.Model;
            }

            set
            {
                this.model = value;
            }
        }

        public Weapon(Player player)
        {
            this.player = player;
        }

        public void Initialize(ContentManager contentManager)
        {
            this.Model = contentManager.Load<Model>("weapon");
        }

        /// <summary>
        /// The death time.
        /// </summary>
        private double deathTime = 0;

        public void Update(GameTime gameTime)
        {
            this.SetViewDirection(this.player.GetViewDirection());
            var offset = new Vector3(0.5f, 1, 2.3f);
            
            if (!this.player.Dead)
            {
                this.position = this.player.Position + offset.Rotate2D(this.Angle);
                return;
            }

            if (this.deathTime == 0)
            {
                this.deathTime = gameTime.TotalGameTime.TotalSeconds;
            }

            this.position.Z -= 5 * (float)Math.Pow(gameTime.TotalGameTime.TotalSeconds - this.deathTime, 2) * offset.Z;
            if (this.position.Z < this.player.Position.Z)
            {
                this.position.Z = this.player.Position.Z;
            }
        }

        public override void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
          base.Draw(viewMatrix, projectionMatrix, this.Model);
        }
    }
}
