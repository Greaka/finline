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
        List<Model> AnimationEnemy = new List<Model>(3);
        Model enemy, enemyUnten;
        int i = 0;
        float time;

        public Enemy(ContentManager contentManager, Vector3 position)
        {
            this._model = contentManager.Load<Model>("enemy");
            this.position = position;
            this._angle = 0;
            enemy = contentManager.Load<Model>("enemy");
            enemyUnten = contentManager.Load<Model>("enemy_unten");

            AnimationEnemy.Insert(0, enemy);
            AnimationEnemy.Insert(0, enemyUnten);
            AnimationEnemy.Insert(0, enemy);
        }

        public void Update(Vector3 playerPosition, List<EnvironmentObject> environmentObjects, GameTime gameTime)
        {
            var distance = playerPosition - this.position;
            var view = new Ray(this.position, distance);
            bool any = false;
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time > 0.1f)
            {
                i = i + 1;
                time = 0;
            }

            if (i > 2) i = 0;
            this._model = AnimationEnemy[i];
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
