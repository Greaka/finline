using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using prototyp.Code.Constants;
using prototyp.Code.Game;
using prototyp.Code.Utility;

namespace prototyp
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Vector3 _cameraPosition = new Vector3(0, 30, 10);

        private Player _player;
        private Ground _ground;

        private List<EnvironmentObject> _environmentObjects;

  
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;

            Content.RootDirectory = "Content";

            _environmentObjects = new List<EnvironmentObject>();
        }

        protected override void Initialize()
        {
            var samplerState = new SamplerState
            {
                Filter = TextureFilter.Anisotropic
            };
            GraphicsDevice.SamplerStates[0] = samplerState;

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            _ground = new Ground();
            _ground.Initialize();

            _player = new Player();
            _player.Initialize(Content);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _ground.LoadContent(GraphicsDevice);

            _environmentObjects.Add(new EnvironmentObject(Content, new Vector3(10, 1, 1), GameConstants.EnvObjects.cube));
            _environmentObjects.Add(new EnvironmentObject(Content, new Vector3(5, -10, 1), GameConstants.EnvObjects.cube));
            _environmentObjects.Add(new EnvironmentObject(Content, new Vector3(10, 3, 3), GameConstants.EnvObjects.cube));
            _environmentObjects.Add(new EnvironmentObject(Content, new Vector3(-15, 1, 1), GameConstants.EnvObjects.cube));
            _environmentObjects.Add(new EnvironmentObject(Content, new Vector3(15, -15, 1), GameConstants.EnvObjects.cube));
            _environmentObjects.Add(new EnvironmentObject(Content, new Vector3(5, -10, 3), GameConstants.EnvObjects.bottle_cap2));
            _environmentObjects.Add(new EnvironmentObject(Content, new Vector3(10, 1, 3), GameConstants.EnvObjects.bottle_cap2));


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _player.Update(gameTime, _environmentObjects);

            EnvironmentObject remove = null;
            foreach (EnvironmentObject obj in _environmentObjects)
            {
                obj.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _cameraPosition = _player.Position + new Vector3(0, -10, 10).rotate2d(_player.ViewDirection);

            float aspectRatio = _graphics.PreferredBackBufferWidth / (float)_graphics.PreferredBackBufferHeight;

            _ground.Draw(_cameraPosition, aspectRatio, _player.Position, GraphicsDevice);
            _player.Draw(_cameraPosition, aspectRatio);

            foreach (var obj in _environmentObjects)
            {
                obj.Draw(_cameraPosition, aspectRatio, _player.Position);
            }

            base.Draw(gameTime);
        }
    }
}