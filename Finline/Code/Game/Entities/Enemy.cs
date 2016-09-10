using System.Collections.Generic;

namespace Finline.Code.Game.Entities
{
    using System.Linq;

    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Enemy : LivingEntity
    {
        public bool Shoot = false;

        public Enemy(ContentManager contentManager, Vector3 position)
        {
            this.ModelAnimation = new Animation(3);
            this.DeathAnimation = new Animation(2, false);
            this.position = position;
            this.Angle = 0;
            var enemy = contentManager.Load<Model>("enemy");
            var enemyUnten = contentManager.Load<Model>("enemy_unten");

            this.Model = enemy;

            this.ModelAnimation.Add(enemy);
            this.ModelAnimation.Add(enemyUnten);
            this.ModelAnimation.Add(enemy);

            this.DeathAnimation.Add(enemy);
            this.DeathAnimation.Add(enemyUnten);
        }

        public void Update(Vector3 playerPosition, List<EnvironmentObject> environmentObjects, GameTime gameTime)
        {
            var distance = this.position - playerPosition;
            var view = new Ray(this.position, distance);

            var any =
                environmentObjects.Any(
                    obj =>
                    view.Intersects(new BoundingSphere(obj.Position, obj.GetBound[0].Position.Length())) != null
                    && (obj.Position - this.position).Length() < 0.4f * distance.Length());

            // var any = environmentObjects.Any(obj => new BoundingSphere(obj.Position, obj.GetBound[2].Position.Length()).Intersects(view)
            // == null && (obj.Type == Constants.GameConstants.EnvObjects.wallV));
            if (any)
            {
                this.Shoot = false;
            }
            else
            {
                this.SetViewDirection(distance.Get2D());
                this.Shoot = true;
            }
        }
    }
}
