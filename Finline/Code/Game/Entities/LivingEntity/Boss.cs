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

        public Boss(ContentManager contentManager, Vector3 position, List<EnvironmentObject> environmentObjects)
        {
            this.EnvironmentObjects = environmentObjects;
            this.ModelAnimation = new Animation(4);
            this.DeathAnimation = new Animation(4, false);
            this.position = position;
            this.Angle = 0;
            var boss = contentManager.Load<Model>("boss");
            var bossVorn = contentManager.Load<Model>("boss_vorn");
            var bossHinten = contentManager.Load<Model>("boss_hinten");
            var bossHinten2 = contentManager.Load<Model>("boss_hinten2");
            var bossHinten3 = contentManager.Load<Model>("boss_hinten3");
            var bossHinten4 = contentManager.Load<Model>("boss_hinten4");

            this.Model = boss;

            this.ModelAnimation.Add(bossVorn);
            this.ModelAnimation.Add(boss);
            this.ModelAnimation.Add(bossHinten);
            this.ModelAnimation.Add(boss);

            this.DeathAnimation.Add(boss);
            this.DeathAnimation.Add(bossHinten);
            this.DeathAnimation.Add(bossHinten2);
            this.DeathAnimation.Add(bossHinten3);
            this.DeathAnimation.Add(bossHinten4);
        }

        public void Update(Vector3 playerPosition)
        {
            base.Update();

            var distance = this.position - playerPosition;
            var view = new Ray(this.position, distance);

            var any = this.EnvironmentObjects.Any(obj => view.Intersects(new BoundingSphere(obj.Position, obj.GetBound[0].Position.Length()))
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
