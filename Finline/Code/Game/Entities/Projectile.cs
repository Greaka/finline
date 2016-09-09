using System;

using Finline.Code.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.Game.Entities
{
    using System.Collections.Generic;

    public class Projectile : Entity
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

        private Vector2? IsColliding(List<Entity> environmentObjects, Vector2 direction)
        {
            Vector2? colliding = null;
            var bound = new VertexPositionColor[1]
                            {
                                new VertexPositionColor(this.Position, Color.Blue)
                            };
            foreach (var obj in environmentObjects)
            {
                if (!((this.Position - obj.Position).LengthSquared() < 256))
                {
                    continue;
                }

                var collision = bound.PolygonCollision(obj.GetBound, direction);
                if (!collision.WillIntersect)
                {
                    continue;
                }

                colliding = collision.MinimumTranslationVector;
            }

            return colliding;
        }
    }
}
