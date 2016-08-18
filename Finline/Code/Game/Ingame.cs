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
            for (var i = 0; i < 41; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 0, 0),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 1; i < 120; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(40, i, 0),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 1; i < 154; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(0, i, 0),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 8; i < 41; i += 2)
            {
                // var bla = i == 0 ? 20 : Math.Abs(i) / i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 78, 0),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 52, 0),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 26, 0),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 2; i < 120; i += 2)
            {
                var j = i == 86 || i == 84 || i == 72 || i == 70 || i == 46 || i == 44 || i == 20 || i == 18 ? 120 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(8, j, 0),
                        GameConstants.EnvObjects.cube));
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

            for (var i = 8; i < 59; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 120, 0),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 120; i <= 154; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(59, i, 0),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 1; i < 59; i += 2)
            {
                var j = i == 21 || i == 23 || i == 25 ? 0 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 154, 0),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 154; i < 205; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(20, i, 0),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = 154; i < 195; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(27, i, 0),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 20; i < 26; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 205, 0),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = 27; i < 33; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 195, 0),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 205; i < 245; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(26, i, 0),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = 195; i < 239; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(32, i, 0),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 26; i < 54; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 245, 0),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = 32; i < 54; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 239, 0),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 245; i < 251; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(53, i, 0),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = 239; i > 233; i -= 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(53, i, 0),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 53; i < 65; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 251, 0),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 233, 0),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 247; i < 269; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(65, i, 0),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = 237; i > 215; i -= 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(65, i, 0),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 65; i < 148; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 269, 0),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 215, 0),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 215; i < 271; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(148, i, 0),
                        GameConstants.EnvObjects.cube));
            }
        }
    }
}