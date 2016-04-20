using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using prototyp.Code.Utility;

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

            int repetitions = 5;

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

        public void Draw(Vector3 cameraPosition, float aspectRatio, Vector3 cameraLookAt, GraphicsDevice gdevice)
        {
            var cameraUpVector = Vector3.UnitZ;

            _effect.View = Matrix.CreateLookAt(
                cameraPosition, cameraLookAt, cameraUpVector);

            float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
            float nearClipPlane = 1;
            float farClipPlane = 200;

            _effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

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
