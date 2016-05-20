using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using prototyp.Code.Constants;
using prototyp.Code.Game.Helper;

namespace prototyp.Code.Game
{
    class Ground
    {
        private Texture2D _checkerboardTexture;

        private VertexPositionNormalTexture[] _floorVerts;

        private BasicEffect _effect;

        public void Initialize()
        {
            _floorVerts = new VertexPositionNormalTexture[6];

            _floorVerts[0].Position = new Vector3(-20, -20, 0);
            _floorVerts[1].Position = new Vector3(-20, 20, 0);
            _floorVerts[2].Position = new Vector3(20, -20, 0);

            _floorVerts[3].Position = _floorVerts[1].Position;
            _floorVerts[4].Position = new Vector3(20, 20, 0);
            _floorVerts[5].Position = _floorVerts[2].Position;

            const int repetitions = 5;

            _floorVerts[0].TextureCoordinate = new Vector2(0, 0);
            _floorVerts[1].TextureCoordinate = new Vector2(0, repetitions);
            _floorVerts[2].TextureCoordinate = new Vector2(repetitions, 0);

            _floorVerts[3].TextureCoordinate = _floorVerts[1].TextureCoordinate;
            _floorVerts[4].TextureCoordinate = new Vector2(repetitions, repetitions);
            _floorVerts[5].TextureCoordinate = _floorVerts[2].TextureCoordinate;
        }

        public void LoadContent(GraphicsDevice gdevice)
        {
            _effect = new BasicEffect(gdevice);
            using (var stream = TitleContainer.OpenStream("Content/ground.jpg"))
            {
                _checkerboardTexture = Texture2D.FromStream(gdevice, stream);
            }
        }

        public void Draw(GraphicsDevice gdevice)
        {
            _effect.View = ControlsHelper.ViewMatrix;

            _effect.Projection = ControlsHelper.ProjectionMatrix;

            _effect.TextureEnabled = true;
            _effect.Texture = _checkerboardTexture;

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                gdevice.DrawUserPrimitives(
                            PrimitiveType.TriangleList,
                    _floorVerts,
                    0,
                    2);
            }
        }
    }
}
