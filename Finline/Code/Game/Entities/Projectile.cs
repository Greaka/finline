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

        public Projectile(TimeSpan actualTime, ContentManager content, Vector3 position, Vector2 direction)
        {
            this.Model = content.Load<Model>("ball");
            this.position = new Vector3(position.X, position.Y, 3f);
            this.Angle = direction.GetAngle();
            this.timeStamp = actualTime;
            this.unitsPerSecond = 60;
            this.Bound = new List<Vector3>() { Vector3.Zero };
        }

        public void Update(TimeSpan actualTime, Player player, List<Enemy> enemies, List<EnvironmentObject> environmentObjects, List<Projectile> remove)
        {
            var elapsedTime = (actualTime - this.timeStamp).TotalSeconds;
            var direction = this.GetViewDirection() * this.unitsPerSecond * (float)elapsedTime;

            if (this.IsColliding(player, direction))
            {
                // TODO: Player töten
            }

            if (this.IsColliding(enemies, direction).HasValue)
            {
                // TODO: Enemy töten
            }

            if (this.IsColliding(environmentObjects, direction).HasValue)
            {
                remove.Add(this);
            }
            else
            {
                this.position += new Vector3(direction, 0);
            }

            this.timeStamp = actualTime;
        }
    }
}
