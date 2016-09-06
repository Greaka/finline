using System.Collections.Generic;

namespace Finline.Code.Game.Entities
{
    using System.Linq;

    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Enemy : Entity
    {
        public bool shoot = false;
        List<Model> AnimationEnemy = new List<Model>(3);
        Model enemy;

        Model enemyUnten;

        int i = 0;
        float time;

        public Enemy(ContentManager contentManager, Vector3 position)
        {
            this._model = contentManager.Load<Model>("enemy");
            this.position = position;
            this._angle = 0;
            this.enemy = contentManager.Load<Model>("enemy");
            this.enemyUnten = contentManager.Load<Model>("enemy_unten");

            this.AnimationEnemy.Insert(0, this.enemy);
            this.AnimationEnemy.Insert(0, this.enemyUnten);
            this.AnimationEnemy.Insert(0, this.enemy);
        }

        public void Update(Vector3 playerPosition, List<EnvironmentObject> environmentObjects, GameTime gameTime)
        {
            var distance = playerPosition - this.position;
            var view = new Ray(this.position, distance);
            this.time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (this.time > 0.1f)
            {
                this.i = this.i + 1;
                this.time = 0;
            }

            if (this.i > 2) this.i = 0;
            this._model = this.AnimationEnemy[this.i];
            var any = environmentObjects.Any(obj => view.Intersects(new BoundingSphere(obj.Position, obj.GetBound[0].Position.Length())) != null && (obj.Position - this.position).Length() < distance.Length());

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
