using Finline.Code.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.Game.Entities
{
    public class NonEnvironmentObject : Entity
    {
        private readonly GameConstants.NonEnvObjects type;
        private readonly bool orbit = false;
        public bool Visible { get; set; }

        public GameConstants.NonEnvObjects Type => this.type;

        public NonEnvironmentObject(ContentManager contentManager, Vector3 position, GameConstants.NonEnvObjects model)
        {
            this.Visible = true;
            this.type = model;

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