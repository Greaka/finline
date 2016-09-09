namespace Finline.Code.Game.Entities
{
    using System.Linq;

    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;


    public class Weapon : Entity
    {
        private readonly float unitsPerSecond = 15;
        Player player = new Player();
        protected override Model Model
        {
            get
            {
                return base.Model;
            }

            set
            {
                this.model = value;
            }
        }

        public void Initialize(ContentManager contentManager)
        {
            this.Model = contentManager.Load<Model>("weapon");
            this.position = new Vector3(4, 4, -0.5f);
        }

        public void Update(GameTime gameTime, Vector2 moveDirection, Vector2 shootDirection)
        {
            this.SetViewDirection(shootDirection);
            var pos = moveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * this.unitsPerSecond;
            this.position += player.getPlayerPosition(); //new Vector3(pos, 0);

                    //bekomme nur (0,0,0) von getPlayerPosition zurück


        }

        public override void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
          base.Draw(viewMatrix, projectionMatrix, this.Model);
        }

    }
}
