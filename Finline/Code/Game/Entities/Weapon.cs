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
        private readonly Player player;

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

        public Weapon(Player player)
        {
            this.player = player;
        }

        public void Initialize(ContentManager contentManager)
        {
            this.Model = contentManager.Load<Model>("weapon");
        }

        public void Update()
        {
            this.SetViewDirection(this.player.GetViewDirection());
            var offset = new Vector3(0.5f, 1, 2);
            this.position = this.player.Position + offset.Rotate2D(this.Angle);
        }

        public override void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
          base.Draw(viewMatrix, projectionMatrix, this.Model);
        }
    }
}
