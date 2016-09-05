using System.Collections.Generic;

namespace Finline.Code.Game.Entities
{
    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Boss : Entity
    {
        public bool shoot = false;
        List<Model> AnimationBoss = new List<Model>(4);
        Model boss, bossVorn, bossHinten;
        int i = 0;
        float time;

        public Boss(ContentManager contentManager, Vector3 position)
        {
            this._model = contentManager.Load<Model>("boss");
            this.position = position;
            this._angle = 0;
            boss = contentManager.Load<Model>("boss");
            bossVorn = contentManager.Load<Model>("boss_vorn");
            bossHinten = contentManager.Load<Model>("boss_hinten");

            AnimationBoss.Insert(0, bossVorn);
            AnimationBoss.Insert(0, boss);
            AnimationBoss.Insert(0, bossHinten);
            AnimationBoss.Insert(0, boss);
        }

        public void Update(Vector3 playerPosition, List<EnvironmentObject> environmentObjects, GameTime gameTime)
        {
            if (time > 0.1f)
            {
                i = i + 1;
                time = 0;
            }

            if (i > 3) i = 0;
            this._model = AnimationBoss[i];

            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            var distance = playerPosition - this.position;
            var view = new Ray(this.position, distance);
            bool any = false;
            foreach (var obj in environmentObjects)
            {
                if (view.Intersects(obj.GetBound) != null && (obj.Position - this.position).Length() < distance.Length())
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
