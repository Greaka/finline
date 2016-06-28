namespace Finline.Code.Game
{
    using System;
    using System.Threading.Tasks;

    using Constants;
    using Controls;
    using Entities;
    using GameState;
    using Helper;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Ingame : DrawableGameComponent
    {
        private readonly Controller controls;
        private readonly GraphicsDeviceManager graphics;
        private Player player;
        private Ground ground;
        
        public Ingame(StateManager game)
            : base(game)
        {
            this.controls = game.Controls;
            this.graphics = game.Graphics;
            this.Game.Content.RootDirectory = "Content";
        }

        public override void Initialize()
        {
            this.Game.IsMouseVisible = true;
            var samplerState = new SamplerState
            {
                Filter = TextureFilter.Anisotropic
            };
            this.Game.GraphicsDevice.SamplerStates[0] = samplerState;

            var rasterizerState = new RasterizerState { CullMode = CullMode.None };
            this.Game.GraphicsDevice.RasterizerState = rasterizerState;

            this.ground = new Ground();
            this.ground.Initialize();

            this.player = new Player();
            this.player.Initialize(this.Game.Content);

            Task.Factory.StartNew(() =>
            {
                var projectileHandler = new Shooting(this.Game.Content);
                this.controls.Shoot += projectileHandler.Shoot;
                projectileHandler.Update();
            });

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.ground.LoadContent(this.Game.GraphicsDevice);
            ControlsHelper.EnvironmentObjects.TryAdd(ControlsHelper.EnvironmentObjects.Count, new EnvironmentObject(this.Game.Content, new Vector3(-5, 55, 0), GameConstants.EnvObjects.enemy));
            ControlsHelper.EnvironmentObjects.TryAdd(ControlsHelper.EnvironmentObjects.Count, new EnvironmentObject(this.Game.Content, new Vector3(15, 40, 0), GameConstants.EnvObjects.enemy));
            ControlsHelper.EnvironmentObjects.TryAdd(ControlsHelper.EnvironmentObjects.Count, new EnvironmentObject(this.Game.Content, new Vector3(5, -4, 0), GameConstants.EnvObjects.enemy));
            ControlsHelper.EnvironmentObjects.TryAdd(ControlsHelper.EnvironmentObjects.Count, new EnvironmentObject(this.Game.Content, new Vector3(-8, -28, 0), GameConstants.EnvObjects.enemy));
            ControlsHelper.EnvironmentObjects.TryAdd(ControlsHelper.EnvironmentObjects.Count, new EnvironmentObject(this.Game.Content, new Vector3(10, -40, 0), GameConstants.EnvObjects.enemy));

            for (var i = -20; i < 21; i += 2)
            {
                ControlsHelper.EnvironmentObjects.TryAdd(
                    ControlsHelper.EnvironmentObjects.Count, 
                    new EnvironmentObject(
                        this.Game.Content, 
                        new Vector3(i, -60, 0), 
                        GameConstants.EnvObjects.cube));
                ControlsHelper.EnvironmentObjects.TryAdd(
                    ControlsHelper.EnvironmentObjects.Count, 
                    new EnvironmentObject(
                        this.Game.Content, 
                        new Vector3(i, 60, 0), 
                        GameConstants.EnvObjects.cube));
            }
            for (var i = -59; i < 60; i += 2)
            {
                ControlsHelper.EnvironmentObjects.TryAdd(
                    ControlsHelper.EnvironmentObjects.Count,
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(20, i, 0),
                        GameConstants.EnvObjects.cube));
                ControlsHelper.EnvironmentObjects.TryAdd(
                    ControlsHelper.EnvironmentObjects.Count,
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(-20, -i, 0),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = -12; i < 21; i += 2)
            {
                //var bla = i == 0 ? 20 : Math.Abs(i) / i;
                ControlsHelper.EnvironmentObjects.TryAdd(
                    ControlsHelper.EnvironmentObjects.Count,
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 18, 0),
                        GameConstants.EnvObjects.cube));
                ControlsHelper.EnvironmentObjects.TryAdd(
                    ControlsHelper.EnvironmentObjects.Count,
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, -8, 0),
                        GameConstants.EnvObjects.cube));
                ControlsHelper.EnvironmentObjects.TryAdd(
                    ControlsHelper.EnvironmentObjects.Count,
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, -34, 0),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = -58; i < 60; i += 2)
            {
                var j = i == 26 || i == 24 || i == 12 || i == 10 || i == -14 || i == -16 || i == -40 || i == -42 ? 60 : i;
                ControlsHelper.EnvironmentObjects.TryAdd(
                    ControlsHelper.EnvironmentObjects.Count,
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(-12, j, 0),
                        GameConstants.EnvObjects.cube));
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Game.Exit();
            }

            this.player.Update(gameTime);

            foreach (var obj in ControlsHelper.EnvironmentObjects.Values)
            {
                obj.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.Black);

            var aspectRatio = this.graphics.PreferredBackBufferWidth / (float)this.graphics.PreferredBackBufferHeight;
            ControlsHelper.ViewMatrix = Matrix.CreateLookAt(
                GraphicConstants.CameraPosition, ControlsHelper.PlayerPosition, Vector3.UnitZ);
            ControlsHelper.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                        GraphicConstants.FieldOfView, aspectRatio, GraphicConstants.NearClipPlane, GraphicConstants.FarClipPlane);

            this.ground.Draw(this.Game.GraphicsDevice);
            this.player.Draw();

            foreach (var obj in ControlsHelper.EnvironmentObjects.Values)
            {
                obj.Draw();
            }

            foreach (var outch in ControlsHelper.Projectiles.Values)
            {
                outch.Draw();
            }

            base.Draw(gameTime);
        }
    }
}