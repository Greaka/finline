using System;

using Finline.Code.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.Game.Entities
{
    using System.Collections.Generic;

    public sealed class Projectile : Entity
    {
        private TimeSpan timeStamp;
        private readonly float unitsPerSecond;

        private Entity firingEntity;

        Sounds sounds = new Sounds();

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
            this.unitsPerSecond = 60;
            this.Bound = new List<Vector3>() { Vector3.Zero };
            this.sounds.LoadContent(content);
        }

        public void Update(TimeSpan actualTime, Player player, List<Boss> bosses, List<Enemy> enemies, List<EnvironmentObject> environmentObjects, List<Projectile> remove)
        {
            var elapsedTime = (actualTime - this.timeStamp).TotalSeconds;
            var direction = this.GetViewDirection() * this.unitsPerSecond * (float)elapsedTime;
            this.timeStamp = actualTime;

            if (this.IsColliding(player, direction))
            {
                if (this.firingEntity == player)
                {
                    this.position += new Vector3(direction, 0);
                    return;
                }

                remove.Add(this);
                player.Dead = true;
                this.sounds.SoundEffectPlay(2);
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
                hitEntity.Dead = true;
                this.sounds.SoundEffectPlay(3);
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
                hitEntity.Dead = true;
                this.sounds.SoundEffectPlay(3); this.sounds.SoundEffectPlay(3);
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
