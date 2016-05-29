using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finline.Code.Game.Controls;
using Finline.Code.Game.Helper;
using Finline.Code.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.Game.Entities
{
    public class Projectile : Entity
    {
        private TimeSpan timeStamp;
        private readonly float unitsPerSecond;
        private readonly int index;

        public delegate void Destroy(int index);
        public event Destroy Destruct;

        private Vector3 Position
        {
            get { return this._position; }
            set
            {
                this._position = value;
                float unused;
                if (!this.IsColliding(ControlsHelper.EnvironmentObjects, out unused)) return;
                this.Destruct?.Invoke(this.index);
            }
        }

        public Projectile(TimeSpan actualTime, ContentManager content, int index)
        {
            this.index = index;
            this._model = content.Load<Model>("Arrow");
            this._position = ControlsHelper.PlayerPosition;
            this._angle = ControlsHelper.ShootDirection.getAngle();
            this.timeStamp = actualTime;
            this.unitsPerSecond = 10;
        }

        public void Update(TimeSpan actualTime)
        {
            var elapsedTime = (actualTime - this.timeStamp).TotalSeconds;
            this.Position += new Vector3(this.GetViewDirection(), 0) * this.unitsPerSecond * (float) elapsedTime;
            this.timeStamp = actualTime;
        }
    }
}
