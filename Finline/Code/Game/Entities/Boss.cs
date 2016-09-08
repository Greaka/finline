using System.Collections.Generic;

namespace Finline.Code.Game.Entities
{
    using System.Linq;

    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Boss : LivingEntity
    {
        public bool Shoot = false;

        public Boss(ContentManager contentManager, Vector3 position)
        {
            this.ModelAnimation = new Animation(4);
            this.position = position;
            this.Angle = 0;
            var boss = contentManager.Load<Model>("boss");
            var bossVorn = contentManager.Load<Model>("boss_vorn");
            var bossHinten = contentManager.Load<Model>("boss_hinten");

            this.Model = boss;

            this.ModelAnimation.Add(bossVorn);
            this.ModelAnimation.Add(boss);
            this.ModelAnimation.Add(bossHinten);
            this.ModelAnimation.Add(boss);
        }

        public void Update(Vector3 playerPosition, List<EnvironmentObject> environmentObjects, GameTime gameTime)
        {
            var distance = this.position - playerPosition;
            var view = new Ray(this.position, distance);

            var any = environmentObjects.Any(obj => view.Intersects(new BoundingSphere(obj.Position, obj.GetBound[0].Position.Length()))
                        != null && (obj.Position - this.position).Length() < 0.4f*distance.Length());

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
