namespace Finline.Code.Game
{
    using System.Collections.Generic;

    using Constants;
    using Controls;
    using Entities;
    using GameState;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The ingame.
    /// </summary>
    public class Ingame : DrawableGameComponent
    {
        /// <summary>
        /// The Input Parser.
        /// </summary>
        public readonly PlayerController playerControls = new PlayerController();

        /// <summary>
        /// The graphics.
        /// </summary>
        private readonly GraphicsDeviceManager graphics;

        private EnemyController enemyControls;

        private Shooting projectileHandler;

        private Matrix projectionMatrix;

        private Matrix viewMatrix;

        private Vector2 moveDirection;

        private Vector2 shootDirection;

        /// <summary>
        /// The player.
        /// </summary>
        private Player player;

        /// <summary>
        /// The ground.
        /// </summary>
        private Ground ground;

        private readonly List<Enemy> Enemies = new List<Enemy>();

        private readonly List<EnvironmentObject> EnvironmentObjects = new List<EnvironmentObject>();

        private readonly List<Projectile> Projectiles = new List<Projectile>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Ingame"/> class. 
        /// </summary>
        /// <param name="game">
        /// </param>
        public Ingame(StateManager game)
            : base(game)
        {
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

            this.enemyControls = new EnemyController();
            this.projectileHandler = new Shooting(this.Game.Content, this.Projectiles);
            this.playerControls.Shoot += this.projectileHandler.Shoot;
            this.enemyControls.Shoot += this.projectileHandler.Shoot;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.ground.LoadContent(this.Game.GraphicsDevice, this.Game.Content);
            this.Enemies.Add(new Enemy(this.Game.Content, new Vector3(-5, 55, 0)));
            this.Enemies.Add(new Enemy(this.Game.Content, new Vector3(15, 40, 0)));
            this.Enemies.Add(new Enemy(this.Game.Content, new Vector3(5, -4, 0)));
            this.Enemies.Add(new Enemy(this.Game.Content, new Vector3(-8, -28, 0)));
            this.Enemies.Add(new Enemy(this.Game.Content, new Vector3(10, -40, 0)));

            this.LoadEnvironment();
        }

        public override void Update(GameTime gameTime)
        {
            this.playerControls.Update(this.GraphicsDevice, 
                out this.moveDirection, 
                ref this.shootDirection, 
                this.player.Position, 
                this.projectionMatrix, 
                this.viewMatrix);
            this.projectileHandler.Update(this.EnvironmentObjects);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Game.Exit();
            }

            this.player.Update(gameTime, this.moveDirection, this.shootDirection, this.EnvironmentObjects);

            foreach (var obj in this.EnvironmentObjects)
            {
                obj.Update(gameTime);
            }

            foreach (var enemy in this.Enemies)
            {
                enemy.Update(this.player.Position, this.EnvironmentObjects);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.Black);

            var aspectRatio = this.graphics.PreferredBackBufferWidth / (float)this.graphics.PreferredBackBufferHeight;
            this.viewMatrix = Matrix.CreateLookAt(
                this.player.Position + GraphicConstants.CameraOffset, this.player.Position, Vector3.UnitZ);
            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                        GraphicConstants.FieldOfView, aspectRatio, GraphicConstants.NearClipPlane, GraphicConstants.FarClipPlane);

            this.ground.Draw(this.Game.GraphicsDevice, this.viewMatrix, this.projectionMatrix);
            this.player.Draw(this.viewMatrix, this.projectionMatrix);

            foreach (var obj in this.EnvironmentObjects)
            {
                obj.Draw(this.viewMatrix, this.projectionMatrix);
            }

            foreach (var outch in this.Projectiles)
            {
                outch.Draw(this.viewMatrix, this.projectionMatrix);
            }

            foreach (var enemy in this.Enemies)
            {
                enemy.Draw(this.viewMatrix, this.projectionMatrix);
            }

            base.Draw(gameTime);
        }

        private void LoadEnvironment()
        {
            for (var i = -20; i < 21; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content, 
                        new Vector3(i, -60, 0), 
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content, 
                        new Vector3(i, 60, 0), 
                        GameConstants.EnvObjects.cube));
            }

            for (var i = -59; i < 60; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content, 
                        new Vector3(20, i, 0), 
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content, 
                        new Vector3(-20, -i, 0), 
                        GameConstants.EnvObjects.cube));
            }

            for (var i = -12; i < 21; i += 2)
            {
                // var bla = i == 0 ? 20 : Math.Abs(i) / i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content, 
                        new Vector3(i, 18, 0), 
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content, 
                        new Vector3(i, -8, 0), 
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content, 
                        new Vector3(i, -34, 0), 
                        GameConstants.EnvObjects.cube));
            }

            for (var i = -58; i < 60; i += 2)
            {
                var j = i == 26 || i == 24 || i == 12 || i == 10 || i == -14 || i == -16 || i == -40 || i == -42 ? 60 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content, 
                        new Vector3(-12, j, 0), 
                        GameConstants.EnvObjects.cube));

            }

            for (var i = 12; i > -5; i -= 8)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                    this.Game.Content, 
                    new Vector3(i, 14, -1), 
                    GameConstants.EnvObjects.desk));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                    this.Game.Content, 
                    new Vector3(i, 0, -1), 
                    GameConstants.EnvObjects.desk));
            }

            for (var i = 12; i > -5; i -= 8)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                    this.Game.Content, 
                    new Vector3(i, 14, -1), 
                    GameConstants.EnvObjects.chair));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                    this.Game.Content, 
                    new Vector3(i, 0, -1), 
                    GameConstants.EnvObjects.chair));
            }
        }
    }
}