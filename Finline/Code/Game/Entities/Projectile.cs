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

        public Projectile(TimeSpan actualTime, ContentManager content, Vector3 position, Vector2 direction)
        {
            this._model = content.Load<Model>("Arrow");
            this.position = position;
            this._angle = direction.getAngle();
            this.timeStamp = actualTime;
            this.unitsPerSecond = 60;
        }

        public void Update(TimeSpan actualTime, List<EnvironmentObject> environmentObjects, List<Projectile> remove)
        {
            var elapsedTime = (actualTime - this.timeStamp).TotalSeconds;
            if (this.IsColliding(environmentObjects, this.GetViewDirection() * this.unitsPerSecond * (float)elapsedTime).HasValue)
            {
                remove.Add(this);
            }

            this.timeStamp = actualTime;
        }
    }
}
