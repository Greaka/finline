namespace Finline.Code.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    internal class Ground
    {
        private Texture2D checkerboardTexture;

        private VertexPositionNormalTexture[] floorVerts;

        private BasicEffect effect;

        public void Initialize()
        {
            const float X = 66;
            const float Y = 132;
            const float Z = -1;

            this.floorVerts = new VertexPositionNormalTexture[6];

            this.floorVerts[0].Position = new Vector3(0, 0, Z);
            this.floorVerts[1].Position = new Vector3(0, 2 * Y, Z);
            this.floorVerts[2].Position = new Vector3(2 * X, 0, Z);

            this.floorVerts[3].Position = this.floorVerts[1].Position;
            this.floorVerts[4].Position = new Vector3(2 * X, 2 * Y, Z);
            this.floorVerts[5].Position = this.floorVerts[2].Position;

            const int Repetitions = 1;

            this.floorVerts[0].TextureCoordinate = new Vector2(0, 0);
            this.floorVerts[1].TextureCoordinate = new Vector2(0, Repetitions);
            this.floorVerts[2].TextureCoordinate = new Vector2(Repetitions, 0);

            this.floorVerts[3].TextureCoordinate = this.floorVerts[1].TextureCoordinate;
            this.floorVerts[4].TextureCoordinate = new Vector2(Repetitions, Repetitions);
            this.floorVerts[5].TextureCoordinate = this.floorVerts[2].TextureCoordinate;
        }

        public void LoadContent(GraphicsDevice gdevice, ContentManager content)
        {
            this.effect = new BasicEffect(gdevice);

            this.checkerboardTexture = (Texture2D)content.Load<Texture>("MapV6a");
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
