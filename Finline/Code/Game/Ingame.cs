namespace Finline.Code.Game
{
    using System.Collections.Generic;
    using System.Linq;

    using Constants;
    using Controls;
    using Entities;

    using Finline.Code.DebugUtils;

    using GameState;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The ingame.
    /// </summary>
    public class Ingame : DrawableGameComponent
    {
#if DEBUG
        private HullDrawing hullDrawing;
#endif

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
        public Ingame(StateManager game, SpriteBatch sb)
            : base(game)
        {
            this.graphics = game.Graphics;
            this.Game.Content.RootDirectory = "Content";
#if DEBUG
            this.hullDrawing = new HullDrawing(game, sb);
#endif
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

#if DEBUG
            this.hullDrawing.LoadEntities(this.EnvironmentObjects, this.Enemies, this.Projectiles, this.player);
#endif
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

#if DEBUG
            this.hullDrawing.Draw(gameTime, this.projectionMatrix, this.viewMatrix);
#endif

            base.Draw(gameTime);
        }

        private void LoadEnvironment()
        {
            for (var i = 2; i < 39; i += 4)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 0, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 1; i < 120; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(40, i, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 1; i < 154; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(0, i, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 8; i < 41; i += 2)
            {
                // var bla = i == 0 ? 20 : Math.Abs(i) / i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 78, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 52, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 26, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 2; i < 120; i += 2)
            {
                var j = i == 86 || i == 84 || i == 72 || i == 70 || i == 46 || i == 44 || i == 20 || i == 18 ? 120 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(8, j, 2),
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

            for (var i = 8; i < 55; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 120, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 120; i <= 154; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(55, i, 2),
                        GameConstants.EnvObjects.cube2));
            }
            for (var i = 28; i < 55; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 132, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 143, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 1; i < 55; i += 2)
            {
                var j = i == 21 || i == 23 || i == 25 ? 0 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 154, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 155; i < 205; i += 2)
            {
                var j = i == 189 || i == 191 ? 154 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(20, j, 2),
                        GameConstants.EnvObjects.cube2));
            }
            for (var i = 133; i < 195; i += 2)
            {
                var j = i == 135 || i == 137 || i == 145 || i == 147 || i == 189 || i == 191 ? 132 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(27, j, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 20; i < 26; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 205, 2),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = 27; i < 43; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 195, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 205; i < 271; i += 2)
            {
                var j = i == 223 || i == 225 || i == 239 || i == 241 ? 205 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(26, j, 2),
                        GameConstants.EnvObjects.cube2));
            }
            for (var i = 197; i < 239; i += 2)
            {
                var j = i == 205 || i == 207 ? 195 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(32, j, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 0; i < 54; i += 2)
            {
                var j = i == 8 || i == 10 ? 0 : i;
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(j, 245, 2),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = 32; i < 54; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 239, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 245; i < 251; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(53, i, 2),
                        GameConstants.EnvObjects.cube2));
            }
            for (var i = 239; i > 233; i -= 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(53, i, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 53; i < 65; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 251, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 233, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 247; i < 269; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(65, i, 2),
                        GameConstants.EnvObjects.cube2));
            }
            for (var i = 237; i > 215; i -= 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(65, i, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 65; i < 148; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 269, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 215, 2),
                        GameConstants.EnvObjects.cube));
            }

            for (var i = 215; i < 271; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(148, i, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 0; i < 26; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 269, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 228, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 215, 2),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = 215; i < 269; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(0, i, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 0; i < 20; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 194, 2),
                        GameConstants.EnvObjects.cube));
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 180, 2),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = 180; i < 194; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(0, i, 2),
                        GameConstants.EnvObjects.cube2));
            }

            for (var i = 33; i < 45; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 212, 2),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = 28; i < 43; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(i, 185, 2),
                        GameConstants.EnvObjects.cube));
            }
            for (var i = 185; i < 213; i += 2)
            {
                this.EnvironmentObjects.Add(
                    new EnvironmentObject(
                        this.Game.Content,
                        new Vector3(43, i, 2),
                        GameConstants.EnvObjects.cube2));
            }
        }
    }
}