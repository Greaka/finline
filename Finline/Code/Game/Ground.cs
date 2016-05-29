using Finline.Code.Constants;
using Finline.Code.Game.Helper;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.Game
{
    class Ground
    {
        private Texture2D _checkerboardTexture;

        private VertexPositionNormalTexture[] _floorVerts;

        private BasicEffect _effect;

        public void Initialize()
        {
            const float x = 20;
            const float y = 20;
            const float z = -1;

            this._floorVerts = new VertexPositionNormalTexture[6];

            this._floorVerts[0].Position = new Vector3(-x, -y, z);
            this._floorVerts[1].Position = new Vector3(-x, y, z);
            this._floorVerts[2].Position = new Vector3(x, -y, z);

            this._floorVerts[3].Position = this._floorVerts[1].Position;
            this._floorVerts[4].Position = new Vector3(x, y, z);
            this._floorVerts[5].Position = this._floorVerts[2].Position;

            const int repetitions = 5;

            this._floorVerts[0].TextureCoordinate = new Vector2(0, 0);
            this._floorVerts[1].TextureCoordinate = new Vector2(0, repetitions);
            this._floorVerts[2].TextureCoordinate = new Vector2(repetitions, 0);

            this._floorVerts[3].TextureCoordinate = this._floorVerts[1].TextureCoordinate;
            this._floorVerts[4].TextureCoordinate = new Vector2(repetitions, repetitions);
            this._floorVerts[5].TextureCoordinate = this._floorVerts[2].TextureCoordinate;
        }

        public void LoadContent(GraphicsDevice gdevice)
        {
            this._effect = new BasicEffect(gdevice);
            using (var stream = TitleContainer.OpenStream("Content/ground.jpg"))
            {
                this._checkerboardTexture = Texture2D.FromStream(gdevice, stream);
            }
        }

        public void Draw(GraphicsDevice gdevice)
        {
            this._effect.View = ControlsHelper.ViewMatrix;

            this._effect.Projection = ControlsHelper.ProjectionMatrix;

            this._effect.TextureEnabled = true;
            this._effect.Texture = this._checkerboardTexture;

            foreach (var pass in this._effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                gdevice.DrawUserPrimitives(
                            PrimitiveType.TriangleList, this._floorVerts, 
                    0, 
                    2);
            }
        }
    }
}
