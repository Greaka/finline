using Finline.Code.Game.Helper;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.Game
{
    class Ground
    {
        private Texture2D checkerboardTexture;

        private VertexPositionNormalTexture[] floorVerts;

        private BasicEffect effect;

        public void Initialize()
        {
            const float X = 20;
            const float Y = 20;
            const float Z = -1;

            this.floorVerts = new VertexPositionNormalTexture[6];

            this.floorVerts[0].Position = new Vector3(-X, -Y, Z);
            this.floorVerts[1].Position = new Vector3(-X, Y, Z);
            this.floorVerts[2].Position = new Vector3(X, -Y, Z);

            this.floorVerts[3].Position = this.floorVerts[1].Position;
            this.floorVerts[4].Position = new Vector3(X, Y, Z);
            this.floorVerts[5].Position = this.floorVerts[2].Position;

            const int repetitions = 1;

            this.floorVerts[0].TextureCoordinate = new Vector2(0, 0);
            this.floorVerts[1].TextureCoordinate = new Vector2(0, repetitions);
            this.floorVerts[2].TextureCoordinate = new Vector2(repetitions, 0);

            this.floorVerts[3].TextureCoordinate = this.floorVerts[1].TextureCoordinate;
            this.floorVerts[4].TextureCoordinate = new Vector2(repetitions, repetitions);
            this.floorVerts[5].TextureCoordinate = this.floorVerts[2].TextureCoordinate;
        }

        public void LoadContent(GraphicsDevice gdevice)
        {
            this.effect = new BasicEffect(gdevice);
            using (var stream = TitleContainer.OpenStream("Content/ground.jpg"))
            {
                this.checkerboardTexture = Texture2D.FromStream(gdevice, stream);
            }
        }

        public void Draw(GraphicsDevice gdevice)
        {
            this.effect.View = ControlsHelper.ViewMatrix;

            this.effect.Projection = ControlsHelper.ProjectionMatrix;

            this.effect.TextureEnabled = true;
            this.effect.Texture = this.checkerboardTexture;

            foreach (var pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                gdevice.DrawUserPrimitives(
                    PrimitiveType.TriangleList, 
                    this.floorVerts, 
                    0, 
                    2);
            }
        }
    }
}
