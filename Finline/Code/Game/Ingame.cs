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

            ControlsHelper.EnvironmentObjects.TryAdd(ControlsHelper.EnvironmentObjects.Count, new EnvironmentObject(this.Game.Content, new Vector3(10, 1, 1), GameConstants.EnvObjects.cube));
            ControlsHelper.EnvironmentObjects.TryAdd(ControlsHelper.EnvironmentObjects.Count, new EnvironmentObject(this.Game.Content, new Vector3(5, -10, 1), GameConstants.EnvObjects.cube));
            ControlsHelper.EnvironmentObjects.TryAdd(ControlsHelper.EnvironmentObjects.Count, new EnvironmentObject(this.Game.Content, new Vector3(10, 3, 3), GameConstants.EnvObjects.cube));
            ControlsHelper.EnvironmentObjects.TryAdd(ControlsHelper.EnvironmentObjects.Count, new EnvironmentObject(this.Game.Content, new Vector3(-15, 1, 1), GameConstants.EnvObjects.cube));
            ControlsHelper.EnvironmentObjects.TryAdd(ControlsHelper.EnvironmentObjects.Count, new EnvironmentObject(this.Game.Content, new Vector3(15, -15, 1), GameConstants.EnvObjects.cube));
            ControlsHelper.EnvironmentObjects.TryAdd(ControlsHelper.EnvironmentObjects.Count, new EnvironmentObject(this.Game.Content, new Vector3(5, -10, 3), GameConstants.EnvObjects.enemy));
            ControlsHelper.EnvironmentObjects.TryAdd(ControlsHelper.EnvironmentObjects.Count, new EnvironmentObject(this.Game.Content, new Vector3(10, 1, 3), GameConstants.EnvObjects.enemy));

            ControlsHelper.EnvironmentObjects.TryAdd(ControlsHelper.EnvironmentObjects.Count, new EnvironmentObject(this.Game.Content, new Vector3(20, -3, 5), GameConstants.EnvObjects.enemy));
            ControlsHelper.EnvironmentObjects.TryAdd(ControlsHelper.EnvironmentObjects.Count, new EnvironmentObject(this.Game.Content, new Vector3(-2, 6, 6), GameConstants.EnvObjects.enemy));
            ControlsHelper.EnvironmentObjects.TryAdd(ControlsHelper.EnvironmentObjects.Count, new EnvironmentObject(this.Game.Content, new Vector3(8, 7, 6), GameConstants.EnvObjects.enemy));

            for (var i = -20; i < 21; i += 2)
            {
                var bla = i == 0 ? 20 : Math.Abs(i) / i;
                ControlsHelper.EnvironmentObjects.TryAdd(
                    ControlsHelper.EnvironmentObjects.Count, 
                    new EnvironmentObject(
                        this.Game.Content, 
                        new Vector3(i, bla * 20, 0), 
                        GameConstants.EnvObjects.cube));
                ControlsHelper.EnvironmentObjects.TryAdd(
                    ControlsHelper.EnvironmentObjects.Count, 
                    new EnvironmentObject(
                        this.Game.Content, 
                        new Vector3(bla * 20, i, 0), 
                        GameConstants.EnvObjects.cube));
                ControlsHelper.EnvironmentObjects.TryAdd(
                    ControlsHelper.EnvironmentObjects.Count, 
                    new EnvironmentObject(
                        this.Game.Content, 
                        new Vector3(-i, bla * 20, 0), 
                        GameConstants.EnvObjects.cube));
                ControlsHelper.EnvironmentObjects.TryAdd(
                    ControlsHelper.EnvironmentObjects.Count, 
                    new EnvironmentObject(
                        this.Game.Content, 
                        new Vector3(bla * 20, -i, 0), 
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
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

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