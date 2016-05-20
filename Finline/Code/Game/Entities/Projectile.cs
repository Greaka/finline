using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using prototyp.Code.Game.Helper;
using prototyp.Code.Utility;

namespace prototyp.Code.Game.Entities
{
    public class Projectile : Entity
    {
        private TimeSpan timeStamp;
        private float unitsPerSecond;
        public Projectile(TimeSpan actualTime, ContentManager content)
        {
            _model = content.Load<Model>("Arrow");
            _position = ControlsHelper.PlayerPosition;
            _angle = ControlsHelper.ShootDirection.getAngle();
            timeStamp = actualTime;
            unitsPerSecond = 1;
        }

        public void Update(TimeSpan actualTime)
        {
            var elapsedTime = (actualTime - timeStamp).TotalSeconds;
            _position += new Vector3(this.GetViewDirection(), 0) * unitsPerSecond * (float) elapsedTime;
            timeStamp = actualTime;
        }
    }
}
