namespace Finline.Code.Game.Entities.LivingEntity
{
    using System.Collections.Generic;

    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Boss : LivingEntity
    {
        public bool Shoot;

        /// <summary>
        /// Gets the life.
        /// </summary>
        public ushort Life { get; private set; }

        public override bool Dead
        {
            get
            {
                return this.Life == 0;
            }

            set
            {
                if (value)
                {
                    --this.Life;
                }
            }
        }

        public Boss(ContentManager contentManager, Vector3 position, List<EnvironmentObject> environmentObjects, ushort life)
        {
            this.Life = life;
            this.EnvironmentObjects = environmentObjects;
            this.ModelAnimation = new Animation(4);
            this.DeathAnimation = new Animation(4, false);
            this.position = position;
            this.Angle = 0;
            var boss = contentManager.Load<Model>("boss");
            var bossVorn = contentManager.Load<Model>("boss_vorn");
            var bossHinten = contentManager.Load<Model>("boss_hinten");
            var bossHinten2 = contentManager.Load<Model>("boss_hinten2");
            var bossHinten3 = contentManager.Load<Model>("boss_hinten3");
            var bossHinten4 = contentManager.Load<Model>("boss_hinten4");

            this.Model = boss;

            this.ModelAnimation.Add(bossVorn);
            this.ModelAnimation.Add(boss);
            this.ModelAnimation.Add(bossHinten);
            this.ModelAnimation.Add(boss);

            this.DeathAnimation.Add(boss);
            this.DeathAnimation.Add(bossHinten);
            this.DeathAnimation.Add(bossHinten2);
            this.DeathAnimation.Add(bossHinten3);
            this.DeathAnimation.Add(bossHinten4);
        }

        public void Update(GameTime gameTime, Vector3 playerPosition)
        {
            this.Update(gameTime);

            var canSee = this.Position.CanSee(playerPosition, this.EnvironmentObjects, 1600);
            if (!canSee)
            {
                this.Shoot = false;
            }
            else
            {
                this.SetViewDirection((this.position - playerPosition).Get2D());
                this.Shoot = true;
            }
        }
    }
}
