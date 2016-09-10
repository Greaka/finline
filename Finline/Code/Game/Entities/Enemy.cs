﻿using System.Collections.Generic;

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

        public Enemy(ContentManager contentManager, Vector3 position, List<EnvironmentObject> environmentObjects)
        {
            this.EnvironmentObjects = environmentObjects;
            this.ModelAnimation = new Animation(3);
            this.DeathAnimation = new Animation(2, false);
            this.position = position;
            this.Angle = 0;
            var enemy = contentManager.Load<Model>("enemy");
            var enemyUnten = contentManager.Load<Model>("enemy_unten");
            var enemyUnten2 = contentManager.Load<Model>("enemy_unten2");
            var enemyUnten3 = contentManager.Load<Model>("enemy_unten3");
            var enemyUnten4 = contentManager.Load<Model>("enemy_unten4");
            var enemyUnten5 = contentManager.Load<Model>("enemy_unten5");

            this.Model = enemy;

            this.ModelAnimation.Add(enemy);
            this.ModelAnimation.Add(enemyUnten);
            this.ModelAnimation.Add(enemy);

            this.DeathAnimation.Add(enemy);
            this.DeathAnimation.Add(enemyUnten);
            this.DeathAnimation.Add(enemyUnten2);
            this.DeathAnimation.Add(enemyUnten3);
            this.DeathAnimation.Add(enemyUnten4);
            this.DeathAnimation.Add(enemyUnten5);
        }

        public void Update(Vector3 playerPosition)
        {
            base.Update();

            var distance = this.position - playerPosition;
            var view = new Ray(this.position, distance);

            var any = this.EnvironmentObjects.Any(
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
