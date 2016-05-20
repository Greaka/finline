using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using prototyp.Code.Constants;
using prototyp.Code.Game;
using prototyp.Code.Game.Controls;
using prototyp.Code.Game.Entities;
using prototyp.Code.Game.Helper;

namespace prototyp
{
    public class Ingame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Player _player;
        private Ground _ground;

        private readonly List<EnvironmentObject> _environmentObjects;
        private readonly Controller controls = new Controller();
  
        public Ingame()
        {
            _graphics = new GraphicsDeviceManager(this) {IsFullScreen = false};
            Content.RootDirectory = "Content";

            _environmentObjects = new List<EnvironmentObject>();
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            var samplerState = new SamplerState
            {
                Filter = TextureFilter.Anisotropic
            };
            GraphicsDevice.SamplerStates[0] = samplerState;

            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            _ground = new Ground();
            _ground.Initialize();

            _player = new Player();
            _player.Initialize(Content);

            Task.Factory.StartNew(() =>
            {
                var projectileHandler = new Shooting(Content);
                controls.Shoot += projectileHandler.Shoot;
                projectileHandler.Update();
            });

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

            _environmentObjects.Add(new EnvironmentObject(Content, new Vector3(20, -3, 5), GameConstants.EnvObjects.bottle_cap2));
            _environmentObjects.Add(new EnvironmentObject(Content, new Vector3(-2, 6, 6), GameConstants.EnvObjects.bottle_cap2));
            _environmentObjects.Add(new EnvironmentObject(Content, new Vector3(8, 7, 6), GameConstants.EnvObjects.bottle_cap2));

        }

        protected override void Update(GameTime gameTime)
        {
            controls.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _player.Update(gameTime, _environmentObjects);

            foreach (var obj in _environmentObjects)
            {
                obj.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var aspectRatio = _graphics.PreferredBackBufferWidth / (float)_graphics.PreferredBackBufferHeight;

            _ground.Draw(aspectRatio, GraphicsDevice);
            _player.Draw(aspectRatio);

            foreach (var obj in _environmentObjects)
            {
                obj.Draw(aspectRatio);
            }

            foreach (var outch in ControlsHelper.Projectiles)
            {
                outch.Draw(aspectRatio);
            }

            base.Draw(gameTime);
        }
    }
}