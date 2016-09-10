using System;

using Finline.Code.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.Game.Entities
{
    using System.Collections.Generic;

    using Finline.Code.Game.Entities.LivingEntity;

    public sealed class Projectile : Entity
    {
        private TimeSpan timeStamp;

        /// <summary>
        /// The units per second.
        /// </summary>
        public const float UnitsPerSecond = 60;

        private readonly Entity firingEntity;

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        protected override Model Model
        {
            get
            {
                return this.model;
            }

            set
            {
                this.model = value;
            }
        }

        public Projectile(TimeSpan actualTime, ContentManager content, Entity firedFrom, Vector2 direction, int index)
        {
            this.firingEntity = firedFrom;
            if (index == 0) this.Model = content.Load<Model>("ball");
            else this.Model = content.Load<Model>("blobball");
            this.position = new Vector3(firedFrom.Position.X, firedFrom.Position.Y, 3f);
            this.Angle = direction.GetAngle();
            this.timeStamp = actualTime;
            this.Bound = new List<Vector3>() { Vector3.Zero };
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="actualTime">
        /// The actual time.
        /// </param>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="bosses">
        /// The bosses.
        /// </param>
        /// <param name="enemies">
        /// The enemies.
        /// </param>
        /// <param name="environmentObjects">
        /// The environment objects.
        /// </param>
        /// <param name="remove">
        /// The remove.
        /// </param>
        /// <param name="healthSystem">
        /// The health system.
        /// </param>
        /// <param name="sounds">
        /// The sounds.
        /// </param>
        public void Update(
            TimeSpan actualTime, 
            Player player, 
            List<Boss> bosses, 
            List<Enemy> enemies, 
            IEnumerable<EnvironmentObject> environmentObjects, 
            List<Projectile> remove, 
            HealthSystem healthSystem, 
            Sounds sounds)
        {
            var elapsedTime = (actualTime - this.timeStamp).TotalSeconds;
            var direction = this.GetViewDirection() * UnitsPerSecond * (float)elapsedTime;
            this.timeStamp = actualTime;

            if (this.IsColliding(player, direction))
            {
                if (this.firingEntity == player)
                {
                    this.position += new Vector3(direction, 0);
                    return;
                }

                remove.Add(this);
                if (player.Dead == false)
                {
                    healthSystem.Update(2, sounds);
                }

                player.Dead = true;
                return;
            }

            var colliding = this.IsColliding(enemies, direction);
            if (colliding.Translation.HasValue)
            {
                var hitEntity = enemies[enemies.IndexOf((Enemy)colliding.HitEntities[0])];
                if (this.firingEntity == hitEntity)
                {
                    this.position += new Vector3(direction, 0);
                    return;
                }

                remove.Add(this);
                if (hitEntity.Dead == false)
                {
                    healthSystem.Update(3, sounds);
                }

                hitEntity.Dead = true;
                return;
            }

            colliding = this.IsColliding(bosses, direction);
            if (colliding.Translation.HasValue)
            {
                var hitEntity = bosses[bosses.IndexOf((Boss)colliding.HitEntities[0])];
                if (this.firingEntity == hitEntity)
                {
                    this.position += new Vector3(direction, 0);
                    return;
                }

                remove.Add(this);
                if (healthSystem.GetBossHealth() != 0)
                {
                    healthSystem.Update(4, sounds);
                }

                hitEntity.Dead = true;
                return;
            }

            if (this.IsColliding(environmentObjects, direction).Translation.HasValue)
            {
                remove.Add(this);
            }
            else
            {
                this.position += new Vector3(direction, 0);
            }
        }
    }
}
