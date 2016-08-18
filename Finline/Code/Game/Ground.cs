using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.Game
{
    using Microsoft.Xna.Framework.Content;

    internal class Ground
    {
        private Texture2D checkerboardTexture;

        private VertexPositionNormalTexture[] floorVerts;

        private BasicEffect effect;

        public void Initialize()
        {
            const float X = 20 * 3.68f;
            const float Y = 60 * 2.24f;
            const float Z = -1;

            this.floorVerts = new VertexPositionNormalTexture[6];

            this.floorVerts[0].Position = new Vector3(-X + 20 * 3.68f, -Y + 60 * 2.24f, Z);
            this.floorVerts[1].Position = new Vector3(-X + 20 * 3.68f, Y + 60 * 2.24f, Z);
            this.floorVerts[2].Position = new Vector3(X + 20 * 3.68f, -Y + 60 * 2.24f, Z);

            this.floorVerts[3].Position = this.floorVerts[1].Position;
            this.floorVerts[4].Position = new Vector3(X + 20 * 3.68f, Y + 60 * 2.24f, Z);
            this.floorVerts[5].Position = this.floorVerts[2].Position;

            const int repetitions = 1;

            this.floorVerts[0].TextureCoordinate = new Vector2(0, 0);
            this.floorVerts[1].TextureCoordinate = new Vector2(0, repetitions);
            this.floorVerts[2].TextureCoordinate = new Vector2(repetitions, 0);

            this.floorVerts[3].TextureCoordinate = this.floorVerts[1].TextureCoordinate;
            this.floorVerts[4].TextureCoordinate = new Vector2(repetitions, repetitions);
            this.floorVerts[5].TextureCoordinate = this.floorVerts[2].TextureCoordinate;
        }

        public void LoadContent(GraphicsDevice gdevice, ContentManager content)
        {
            this.effect = new BasicEffect(gdevice);

            this.checkerboardTexture = (Texture2D)content.Load<Texture>("MapV2t");
        }

        public void Draw(GraphicsDevice gdevice, Matrix viewMatrix, Matrix projectionMatrix)
        {
            this.effect.View = viewMatrix;

            this.effect.Projection = projectionMatrix;

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
