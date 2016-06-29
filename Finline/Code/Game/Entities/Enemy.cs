using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finline.Code.Game.Entities
{
    using Finline.Code.Game.Controls;
    using Finline.Code.Game.Helper;
    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Enemy : Entity
    {
        public bool shoot = false;

        public Enemy(ContentManager contentManager, Vector3 position)
        {
            this._model = contentManager.Load<Model>("enemy");
            this._position = position;
            this._angle = 0;
        }

        public void Update()
        {
            var view = new Ray(this._position, ControlsHelper.PlayerPosition - this._position);
            if (ControlsHelper.EnvironmentObjects.Values.Any(obj => view.Intersects(obj.GetBound) == null))
            {
                this.SetViewDirection(ControlsHelper.PlayerPosition.get2d());
                this.shoot = true;
            }
            else
            {
                this.shoot = false;
            }
        }
    }
}
