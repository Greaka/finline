using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using prototyp.Code.Game;
using prototyp.Code.Utility;

namespace prototyp
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        private VertexPositionNormalTexture[] _floorVerts;

        private BasicEffect _effect;

        Texture2D _checkerboardTexture;

        private Vector3 _cameraPosition = new Vector3(0, 30, 10);

        private Player _player;
  
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _floorVerts = new VertexPositionNormalTexture[6];

            _floorVerts[0].Position = new Vector3(-20, -20, 0);
            _floorVerts[1].Position = new Vector3(-20, 20, 0);
            _floorVerts[2].Position = new Vector3(20, -20, 0);

            _floorVerts[3].Position = _floorVerts[1].Position;
            _floorVerts[4].Position = new Vector3(20, 20, 0);
            _floorVerts[5].Position = _floorVerts[2].Position;

            int repetitions = 20;

            _floorVerts[0].TextureCoordinate = new Vector2(0, 0);
            _floorVerts[1].TextureCoordinate = new Vector2(0, repetitions);
            _floorVerts[2].TextureCoordinate = new Vector2(repetitions, 0);

            _floorVerts[3].TextureCoordinate = _floorVerts[1].TextureCoordinate;
            _floorVerts[4].TextureCoordinate = new Vector2(repetitions, repetitions);
            _floorVerts[5].TextureCoordinate = _floorVerts[2].TextureCoordinate;

            _effect = new BasicEffect(_graphics.GraphicsDevice);

            _player = new Player();
            _player.Initialize(Content);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            using (var stream = TitleContainer.OpenStream("Content/checkerboard.png"))
            {
                _checkerboardTexture = Texture2D.FromStream(this.GraphicsDevice, stream);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                 ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                 Keys.Escape))
                Exit();

          

            _player.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawGround();

            float aspectRatio =
                _graphics.PreferredBackBufferWidth / (float)_graphics.PreferredBackBufferHeight;
            _player.Draw(_cameraPosition, aspectRatio);

            base.Draw(gameTime);
        }

      

       
        void DrawGround()
        {
            _cameraPosition = _player.Position + new Vector3(0, -10, 10).rotate2d(_player.ViewDirection);
            var cameraLookAtVector = _player.Position;
            var cameraUpVector = Vector3.UnitZ;

            _effect.View = Matrix.CreateLookAt(
                _cameraPosition, cameraLookAtVector, cameraUpVector);

            float aspectRatio =
                _graphics.PreferredBackBufferWidth / (float)_graphics.PreferredBackBufferHeight;
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

                _graphics.GraphicsDevice.DrawUserPrimitives(
                            PrimitiveType.TriangleList,
                    _floorVerts,
                    0,
                    2);
            }
        }
    }
}