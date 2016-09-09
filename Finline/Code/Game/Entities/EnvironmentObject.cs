using Finline.Code.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.Game.Entities
{
    using Finline.Code.Utility;

    public class EnvironmentObject : Entity
    {
        private readonly bool orbit = false;
        public bool Visible { get; set; }

        public GameConstants.EnvObjects Type { get; }

        protected override Model Model
        {
            get
            {
                return base.Model;
            }
            set
            {
                this.model = value;
                var sphere = this.Model.GetVerticies().GetHull();

                switch (this.Type)
                {
                        case GameConstants.EnvObjects.wallV:
                        sphere = EnvironmentObject.VerschiebeBound(sphere, new Vector2(-1.6f, 0.2f));
                        break;
                        case GameConstants.EnvObjects.wallH:
                        sphere = EnvironmentObject.VerschiebeBound(sphere, new Vector2(0, -2f));
                        break;
                }

                this.Bound = sphere;
            }
        }

        public EnvironmentObject(ContentManager contentManager, Vector3 position, GameConstants.EnvObjects model)
        {
            this.Visible = true;
            this.Type = model;

            this.Model = contentManager.Load<Model>(model.ToString());
            this.position = position;
            this.Angle = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (this.orbit)
            {
                this.Angle += 0.1f;
            }
        }

        public override void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            if (this.Visible)
                base.Draw(viewMatrix, projectionMatrix);
        }
    }

}