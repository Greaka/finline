using System.Collections.Generic;

namespace Finline.Code.Game.Entities
{
    using System.Linq;

    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Boss : Entity
    {
        public bool shoot = false;
        List<Model> AnimationBoss = new List<Model>(4);
        Model boss;

        Model bossVorn;

        Model bossHinten;

        int i = 0;
        float time;

        public Boss(ContentManager contentManager, Vector3 position)
        {
            this._model = contentManager.Load<Model>("boss");
            this.position = position;
            this._angle = 0;
            this.boss = contentManager.Load<Model>("boss");
            this.bossVorn = contentManager.Load<Model>("boss_vorn");
            this.bossHinten = contentManager.Load<Model>("boss_hinten");

            this.AnimationBoss.Insert(0, this.bossVorn);
            this.AnimationBoss.Insert(0, this.boss);
            this.AnimationBoss.Insert(0, this.bossHinten);
            this.AnimationBoss.Insert(0, this.boss);
        }

        public void Update(Vector3 playerPosition, List<EnvironmentObject> environmentObjects, GameTime gameTime)
        {
            if (this.time > 0.1f)
            {
                this.i = this.i + 1;
                this.time = 0;
            }

            if (this.i > 3) this.i = 0;
            this._model = this.AnimationBoss[this.i];

            this.time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            var distance = playerPosition - this.position;
            var view = new Ray(this.position, distance);
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
