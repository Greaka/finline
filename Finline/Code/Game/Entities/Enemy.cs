using System.Collections.Generic;

namespace Finline.Code.Game.Entities
{
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
            this.position = position;
            this._angle = 0;
        }

        public void Update(Vector3 playerPosition, List<EnvironmentObject> environmentObjects)
        {
            var distance = playerPosition - this.position;
            var view = new Ray(this.position, distance);
            var any = false;
            foreach (var obj in environmentObjects)
            {
                if (view.Intersects(new BoundingSphere(obj.Position, obj.GetBound[0].Position.Length())) != null && (obj.Position - this.position).Length() < distance.Length())
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
                this.SetViewDirection(playerPosition.get2d());
                this.shoot = true;
            }
        }
    }
}
