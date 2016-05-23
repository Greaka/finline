using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finline.Code.Game.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Finline.Code.Game.Controls;
using Finline.Code.Utility;

namespace Finline.Code.Game.Entities
{
    public class Projectile : Entity
    {
        private TimeSpan timeStamp;
        private float unitsPerSecond;
        private int _index;

        public delegate void Destroy(int index);
        public event Destroy Destruct;

        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                float unused;
                if (!this.isColliding(ControlsHelper.EnvironmentObjects, out unused)) return;
                Destruct?.Invoke(_index);
            }
        }

        public Projectile(TimeSpan actualTime, ContentManager content, int index)
        {
            _index = index;
            _model = content.Load<Model>("Arrow");
            _position = ControlsHelper.PlayerPosition;
            _angle = ControlsHelper.ShootDirection.getAngle();
            timeStamp = actualTime;
            unitsPerSecond = 10;
        }

        public void Update(TimeSpan actualTime)
        {
            var elapsedTime = (actualTime - timeStamp).TotalSeconds;
            Position += new Vector3(this.GetViewDirection(), 0) * unitsPerSecond * (float) elapsedTime;
            timeStamp = actualTime;
        }
    }
}
