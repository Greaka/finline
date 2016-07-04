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
            var distance = ControlsHelper.PlayerPosition - this._position;
            var view = new Ray(this._position, distance);
            bool any = false;
            foreach (var obj in ControlsHelper.EnvironmentObjects.Values)
            {
                if (view.Intersects(obj.GetBound) != null && (obj.Position - this._position).Length() < distance.Length())
                {
                    any = true;
                    break;
                }
            }
            if (any)
            {
                this.shoot = false;
            }
            else
            {
                this.SetViewDirection(ControlsHelper.PlayerPosition.get2d());
                this.shoot = true;
            }
        }
    }
}
