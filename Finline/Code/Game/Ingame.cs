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
            for (var i = 2; i <= 38; i += 4)
            {
                var j = i == 38 ? 1.75f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 0, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 2; i <= 150; i += 4)
            {
                var j = i == 150 ? 149.25f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(0, j, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 1.75f; i <= 117.75f; i += 4)
            {
                var j = i == 117.75f ? 115.25f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(36, j, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 9; i <= 37; i += 4)
            {
                var j = i == 37 ? 34 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 81, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 52, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 23, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 2.25f; i <= 114.25f; i += 4)
            {
                var j = i == 86.25 || i == 74.25 || i == 46.25 || i == 18.25 ? 115.25f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(7, j, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 32; i > 15; i -= 8)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                    this.Game.Content,
                    new Vector3(i, 74, -1),
                    GameConstants.EnvObjects.chair));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                    this.Game.Content,
                    new Vector3(i, 60, -1),
                    GameConstants.EnvObjects.chair));
            }
            for (var i = 32; i > 15; i -= 8)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                    this.Game.Content,
                    new Vector3(i, 74, -1),
                    GameConstants.EnvObjects.desk));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                    this.Game.Content,
                    new Vector3(i, 60, -1),
                    GameConstants.EnvObjects.desk));
            }

            for (var i = 9; i <= 49; i += 4)
            {
                var j = i == 49 ? 46.75f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 117, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 119; i <= 151; i += 4)
            {
                var j = i == 151 ? 149 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(48.5f, j, 2),
                        GameConstants.EnvObjects.cube2));
            }
            for (var i = 26.25f; i <= 50.25f; i += 4)
            {
                var j = i == 50.25f ? 46.75f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 129, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 140, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 151, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 2; i <= 18; i += 4)
            {
                var j = i == 18 ? 16.5f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 151, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 154; i <= 186; i += 4)
            {
                var j = i == 186 ? 152.75f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(18.5f, j, 2),
                        GameConstants.EnvObjects.cube2));
            }
            for (var i = 131; i <= 179; i += 4)
            {
                var j = i == 135 || i == 143 ? 182 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(24.5f, j, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 190; i <= 202; i += 4)
            {
                var j = i == 202 ? 199.75f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(18.5f, j, 2),
                        GameConstants.EnvObjects.cube2));
            }
            this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(24.5f, 190, 2),
                        GameConstants.EnvObjects.cube2));

            this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(20.25f, 201.75f, 2),
                        GameConstants.EnvObjects.cube));

            for (var i = 26.5f; i <= 38.5f; i += 4)
            {
                var j = i == 38.5f ? 36.75f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 191.75f, 2),
                        GameConstants.EnvObjects.cube));
            }


            for (var i = 203.5f; i <= 259.5f; i += 4)
            {
                var j = i == 219.5f || i == 235.5f ? 262 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(22.25f, j, 2),
                        GameConstants.EnvObjects.cube2));
            }
            for (var i = 194; i <= 230; i += 4)
            {
                var j = i == 202 ? 233 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(28.25f, j, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 2; i <= 42; i += 4)
            {
                var j = i == 10 ? 45.5f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 240.5f, 2),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = 30; i <= 46; i += 4)
            {
                var j = i == 46 ? 45.5f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 235f, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 242.5f; i <= 246.5f; i += 4)
            {
                var j = i == 246.5f ? 244 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(47.25f, j, 2),
                        GameConstants.EnvObjects.cube2));
            }
            for (var i = 231.5f; i <= 235.5f; i += 4)
            {
                var j = i == 235.5f ? 233 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(47.25f, j, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 49; i <= 57; i += 4)
            {
                var j = i == 57 ? 56 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 245.75f, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 229.25f, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 231.5f; i >= 211.5f; i -= 4)
            {
                var j = i == 211.5f ? (i + 1.25f) : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(58, j, 2),
                        GameConstants.EnvObjects.cube2));
            }
            for (var i = 243.5f; i <= 263.5f; i += 4)
            {
                var j = i == 263.5f ? (i - 1.25f) : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(58, j, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 60; i <= 132; i += 4)
            {
                var j = i == 132 ? 130.5f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 264, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 211, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 213; i <= 265; i += 4)
            {
                var j = i == 265 ? 262.25f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(132.25f, j, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 2; i <= 22; i += 4)
            {
                var j = i == 22 ? 20.5f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 264, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 223.5f, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 212, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 213.75f; i <= 265.75f; i += 4)
            {
                var j = i == 265.75f ? 262.25f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(0, j, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 2; i <= 18; i += 4)
            {
                var j = i == 18 ? 16.5f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 191, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 177.5f, 2),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = 179.25f; i <= 191.25f; i += 4)
            {
                var j = i == 191.25f ? 189.25f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(0, j, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 30; i <= 38; i += 4)
            {
                var j = i == 38 ? 36.75f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 208, 2),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = 26.5f; i <= 38.5f; i += 4)
            {
                var j = i == 38.5f ? 36.75f : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 182, 2),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = 184; i <= 208; i += 4)
            {
                var j = i == 208 ? 206 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(38.5f, j, 2),
                        GameConstants.EnvObjects.cube2));
            }
        }
    }
}