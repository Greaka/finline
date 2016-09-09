﻿namespace Finline.Code.Game
{
    using System.Collections.Generic;
    using System.Linq;

    using Constants;
    using Controls;
    using Entities;
#if DEBUG
    using Finline.Code.DebugUtils;
#endif
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
        public readonly PlayerController PlayerControls = new PlayerController();

        /// <summary>
        /// The graphics.
        /// </summary>
        private readonly GraphicsDeviceManager graphics;

        private EnemyController enemyControls;

        private BossController bossControls;

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

        private readonly List<Enemy> enemies = new List<Enemy>();
        private readonly List<Boss> bosses = new List<Boss>();

        private readonly List<EnvironmentObject> environmentObjects = new List<EnvironmentObject>();
        private readonly List<NonEnvironmentObject> nonenvironmentObjects = new List<NonEnvironmentObject>();

        private readonly List<Projectile> projectiles = new List<Projectile>();

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
            this.bossControls = new BossController();
            this.projectileHandler = new Shooting(this.Game.Content, this.projectiles);
            this.PlayerControls.Shoot += this.projectileHandler.Shoot;
            this.enemyControls.Shoot += this.projectileHandler.Shoot;
            this.bossControls.Shoot += this.projectileHandler.Shoot;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.ground.LoadContent(this.Game.GraphicsDevice, this.Game.Content);

            // this.enemies.Add(new Enemy(this.Game.Content, new Vector3(8, -15, 0)));
            // this.bosses.Add(new Boss(this.Game.Content, new Vector3(12, -10, 0)));
            this.enemies.Add(new Enemy(this.Game.Content, new Vector3(25, 5, 0)));
            this.enemies.Add(new Enemy(this.Game.Content, new Vector3(11, 27, 0)));
            this.enemies.Add(new Enemy(this.Game.Content, new Vector3(32, 27, 0)));
            this.enemies.Add(new Enemy(this.Game.Content, new Vector3(30, 65, 0)));
            this.enemies.Add(new Enemy(this.Game.Content, new Vector3(27, 105, 0)));
            this.enemies.Add(new Enemy(this.Game.Content, new Vector3(18, 90, 0)));
            this.enemies.Add(new Enemy(this.Game.Content, new Vector3(43, 121, 0)));
            this.enemies.Add(new Enemy(this.Game.Content, new Vector3(14, 147, 0)));
            this.enemies.Add(new Enemy(this.Game.Content, new Vector3(38, 148, 0)));
            this.enemies.Add(new Enemy(this.Game.Content, new Vector3(5, 182, 0)));
            this.enemies.Add(new Enemy(this.Game.Content, new Vector3(33, 187, 0)));
            this.enemies.Add(new Enemy(this.Game.Content, new Vector3(34, 197, 0)));
            this.enemies.Add(new Enemy(this.Game.Content, new Vector3(7, 228, 0)));
            this.enemies.Add(new Enemy(this.Game.Content, new Vector3(4, 233, 0)));
            this.enemies.Add(new Enemy(this.Game.Content, new Vector3(18, 246, 0)));
            this.enemies.Add(new Enemy(this.Game.Content, new Vector3(3, 259, 0)));
            this.bosses.Add(new Boss(this.Game.Content, new Vector3(100, 228, 0)));
            this.bosses.Add(new Boss(this.Game.Content, new Vector3(100, 252, 0)));

#if DEBUG
            this.hullDrawing.LoadEntities(this.environmentObjects, this.enemies, this.projectiles, this.player);
#endif
            this.LoadEnvironment();
        }

        public override void Update(GameTime gameTime)
        {
            this.PlayerControls.Update(
                this.GraphicsDevice, 
                out this.moveDirection, 
                ref this.shootDirection, 
                this.player, 
                this.projectionMatrix, 
                this.viewMatrix);
            this.projectileHandler.Update(this.player, this.bosses, this.enemies, this.environmentObjects);

            this.enemyControls.Update(this.enemies, this.player.Position);
            this.bossControls.Update(this.bosses, this.player.Position);

            // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            // || Keyboard.GetState().IsKeyDown(Keys.Escape))
            // {
            // this.Game.Exit();
            // }
            this.player.Update(gameTime, this.moveDirection, this.shootDirection, this.environmentObjects);

            foreach (var obj in this.environmentObjects)
            {
                obj.Update(gameTime);
            }

            foreach(var obj in this.nonenvironmentObjects)
            {
                obj.Update(gameTime);
            }

            foreach (var enemy in this.enemies)
            {
                enemy.Update(this.player.Position, this.environmentObjects, gameTime);
            }

            foreach (var boss in this.bosses)
            {
                boss.Update(this.player.Position, this.environmentObjects, gameTime);
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

            foreach (var obj in this.environmentObjects)
            {
                obj.Draw(this.viewMatrix, this.projectionMatrix);
            }

            foreach (var obj in this.nonenvironmentObjects)
            {
                obj.Draw(this.viewMatrix, this.projectionMatrix);
            }

            foreach (var outch in this.projectiles)
            {
                outch.Draw(this.viewMatrix, this.projectionMatrix);
            }

            foreach (var enemy in this.enemies)
            {
                enemy.Draw(this.viewMatrix, this.projectionMatrix);
            }

            foreach (var boss in this.bosses)
            {
                boss.Draw(this.viewMatrix, this.projectionMatrix);
            }

#if DEBUG
            this.hullDrawing.Draw(gameTime, this.projectionMatrix, this.viewMatrix);
#endif

            base.Draw(gameTime);
        }

        private void LoadEnvironment()
        {
            
            for (var i = 2; i <= 38; i += 4)
            {
                var j = i == 38 ? 1.75f : i;
                this.LevelObjects(j, 0, 2, GameConstants.EnvObjects.wallV);
            }

            for (var i = 2; i <= 150; i += 4)
            {
                var j = i == 150 ? 149.25f : i;
                this.LevelObjects(0, j, 2, GameConstants.EnvObjects.wallH);
            }

            for (var i = 1.75f; i <= 117.75f; i += 4)
            {
                var j = i == 117.75f ? 115.25f : i;
                this.LevelObjects(36, j, 2, GameConstants.EnvObjects.wallH);
            }

            for (var i = 9; i <= 37; i += 4)
            {
                var j = i == 37 ? 34 : i;
                this.LevelObjects(j, 81, 2, GameConstants.EnvObjects.wallV);
                this.LevelObjects(j, 52, 2, GameConstants.EnvObjects.wallV);
                this.LevelObjects(j, 23, 2, GameConstants.EnvObjects.wallV);
            }

            for (var i = 2.25f; i <= 114.25f; i += 4)
            {
                var j = i == 86.25 || i == 74.25 || i == 46.25 || i == 18.25 ? 115.25f : i;
                this.LevelObjects(7, j, 2, GameConstants.EnvObjects.wallH);
            }

            for (var i = 9; i <= 49; i += 4)
            {
                var j = i == 49 ? 46.75f : i;
                this.LevelObjects(j, 117, 2, GameConstants.EnvObjects.wallV);
            }

            for (var i = 119; i <= 151; i += 4)
            {
                var j = i == 151 ? 149 : i;
                this.LevelObjects(48.5f, j, 2, GameConstants.EnvObjects.wallH);
            }

            for (var i = 26.25f; i <= 50.25f; i += 4)
            {
                var j = i == 50.25f ? 46.75f : i;
                this.LevelObjects(j, 129, 2, GameConstants.EnvObjects.wallV);
                this.LevelObjects(j, 140, 2, GameConstants.EnvObjects.wallV);
                this.LevelObjects(j, 151, 2, GameConstants.EnvObjects.wallV);
            }

            for (var i = 2; i <= 18; i += 4)
            {
                var j = i == 18 ? 16.5f : i;
                this.LevelObjects(j, 151, 2, GameConstants.EnvObjects.wallV);
            }

            for (var i = 154; i <= 186; i += 4)
            {
                var j = i == 186 ? 152.75f : i;
                this.LevelObjects(18.5f, j, 2, GameConstants.EnvObjects.wallH);
            }

            for (var i = 131; i <= 179; i += 4)
            {
                var j = i == 135 || i == 143 ? 182 : i;
                this.LevelObjects(24.5f, j, 2, GameConstants.EnvObjects.wallH);
            }

            for (var i = 190; i <= 202; i += 4)
            {
                var j = i == 202 ? 199.75f : i;
                this.LevelObjects(18.5f, j, 2, GameConstants.EnvObjects.wallH);
            }

            this.LevelObjects(24.5f, 190, 2, GameConstants.EnvObjects.wallH);
            this.LevelObjects(20.5f, 201.75f, 2, GameConstants.EnvObjects.wallV);

            for (var i = 26.5f; i <= 38.5f; i += 4)
            {
                var j = i == 38.5f ? 36.75f : i;
                this.LevelObjects(j, 191.75f, 2, GameConstants.EnvObjects.wallV);
            }


            for (var i = 203.5f; i <= 259.5f; i += 4)
            {
                var j = i == 219.5f || i == 235.5f ? 262 : i;
                this.LevelObjects(22.25f, j, 2, GameConstants.EnvObjects.wallH);
            }

            for (var i = 194; i <= 230; i += 4)
            {
                var j = i == 202 ? 233 : i;
                this.LevelObjects(28.25f, j, 2, GameConstants.EnvObjects.wallH);
            }

            for (var i = 2; i <= 42; i += 4)
            {
                var j = i == 10 ? 45.5f : i;
                this.LevelObjects(j, 240.5f, 2, GameConstants.EnvObjects.wallV);
            }

            for (var i = 30; i <= 46; i += 4)
            {
                var j = i == 46 ? 45.5f : i;
                this.LevelObjects(j, 235f, 2, GameConstants.EnvObjects.wallV);
            }

            for (var i = 242.5f; i <= 246.5f; i += 4)
            {
                var j = i == 246.5f ? 244 : i;
                this.LevelObjects(47.25f, j, 2, GameConstants.EnvObjects.wallH);
            }

            for (var i = 231.5f; i <= 235.5f; i += 4)
            {
                var j = i == 235.5f ? 233 : i;
                this.LevelObjects(47.25f, j, 2, GameConstants.EnvObjects.wallH);
            }

            for (var i = 49; i <= 57; i += 4)
            {
                var j = i == 57 ? 56 : i;
                this.LevelObjects(j, 245.75f, 2, GameConstants.EnvObjects.wallV);
                this.LevelObjects(j, 229.25f, 2, GameConstants.EnvObjects.wallV);
            }

            for (var i = 231.5f; i >= 211.5f; i -= 4)
            {
                var j = i == 211.5f ? (i + 1.25f) : i;
                this.LevelObjects(58, j, 2, GameConstants.EnvObjects.wallH);
            }

            for (var i = 243.5f; i <= 263.5f; i += 4)
            {
                var j = i == 263.5f ? (i - 1.25f) : i;
                this.LevelObjects(58, j, 2, GameConstants.EnvObjects.wallH);
            }

            for (var i = 60; i <= 132; i += 4)
            {
                var j = i == 132 ? 130.5f : i;
                this.LevelObjects(j, 264, 2, GameConstants.EnvObjects.wallV);
                this.LevelObjects(j, 211, 2, GameConstants.EnvObjects.wallV);
            }

            for (var i = 213; i <= 265; i += 4)
            {
                var j = i == 265 ? 262.25f : i;
                this.LevelObjects(132.25f, j, 2, GameConstants.EnvObjects.wallH);
            }

            for (var i = 2; i <= 22; i += 4)
            {
                var j = i == 22 ? 20.5f : i;
                this.LevelObjects(j, 264, 2, GameConstants.EnvObjects.wallV);
                this.LevelObjects(j, 223.5f, 2, GameConstants.EnvObjects.wallV);
                this.LevelObjects(j, 212, 2, GameConstants.EnvObjects.wallV);
            }

            for (var i = 213.75f; i <= 265.75f; i += 4)
            {
                var j = i == 265.75f ? 262.25f : i;
                this.LevelObjects(0, j, 2, GameConstants.EnvObjects.wallH);
            }

            for (var i = 2; i <= 18; i += 4)
            {
                var j = i == 18 ? 16.5f : i;
                this.LevelObjects(j, 191, 2, GameConstants.EnvObjects.wallV);
                this.LevelObjects(j, 177.5f, 2, GameConstants.EnvObjects.wallV);
            }

            for (var i = 179.25f; i <= 191.25f; i += 4)
            {
                var j = i == 191.25f ? 189.25f : i;
                this.LevelObjects(0, j, 2, GameConstants.EnvObjects.wallH);
            }

            for (var i = 30; i <= 38; i += 4)
            {
                var j = i == 38 ? 36.75f : i;
                this.LevelObjects(j, 208, 2, GameConstants.EnvObjects.wallV);
            }

            for (var i = 26.5f; i <= 38.5f; i += 4)
            {
                var j = i == 38.5f ? 36.75f : i;
                this.LevelObjects(j, 182, 2, GameConstants.EnvObjects.wallV);
            }

            for (var i = 184; i <= 208; i += 4)
            {
                var j = i == 208 ? 206 : i;
                this.LevelObjects(38.5f, j, 2, GameConstants.EnvObjects.wallH);
            }
            



            for (var i = 32; i > 15; i -= 4)
            {
                this.LevelObjects(i, 33, -1, GameConstants.EnvObjects.deskRight);
                this.LevelObjects(i, 40, -1, GameConstants.EnvObjects.deskRight);
                this.LevelObjects(i, 47, -1, GameConstants.EnvObjects.deskRight);
            }

            for (var i = 33; i > 14; i -= 3)
            {
                this.LevelObjects(i, 33, -1, GameConstants.EnvObjects.chairRight);
                this.LevelObjects(i, 40, -1, GameConstants.EnvObjects.chairRight);
                this.LevelObjects(i, 47, -1, GameConstants.EnvObjects.chairRight);
            }




            for (var i = 32; i > 15; i -= 8)
            {
                this.LevelObjects(i, 60, -1, GameConstants.EnvObjects.chairDown);
                this.LevelObjects(i, 74, -1, GameConstants.EnvObjects.chairDown);
                this.LevelObjects(i, 60, -1, GameConstants.EnvObjects.deskDown);
                this.LevelObjects(i, 74, -1, GameConstants.EnvObjects.deskDown);
            }



#region 333
            for (var i = 25.1f; i < 34; i += 4)
            {
                this.LevelObjects(i, 91, -1, GameConstants.EnvObjects.deskRight);
                this.LevelObjects(i, 101, -1, GameConstants.EnvObjects.deskRight);
                this.LevelObjects(i, 111, -1, GameConstants.EnvObjects.deskRight);
            }

            for (var i = 25; i < 35; i += 3)
            {
                this.LevelObjects(i, 91, -1, GameConstants.EnvObjects.chairRight);
                this.LevelObjects(i, 101, -1, GameConstants.EnvObjects.chairRight);
                this.LevelObjects(i, 111, -1, GameConstants.EnvObjects.chairRight);
            }

            for (var i = 26.6f; i < 35; i += 4)
            {
                this.LevelObjects(i, 88, -1, GameConstants.EnvObjects.deskLeft);
                this.LevelObjects(i, 98, -1, GameConstants.EnvObjects.deskLeft);
                this.LevelObjects(i, 108, -1, GameConstants.EnvObjects.deskLeft);
            }

            for (var i = 25.8f; i < 36.5f; i += 3)
            {
                this.LevelObjects(i, 88, -1, GameConstants.EnvObjects.chairLeft);
                this.LevelObjects(i, 98, -1, GameConstants.EnvObjects.chairLeft);
                this.LevelObjects(i, 108, -1, GameConstants.EnvObjects.chairLeft);
            }

            for (var i = 8.1f; i < 17; i += 4)
            {
                this.LevelObjects(i, 101, -1, GameConstants.EnvObjects.deskRight);
                this.LevelObjects(i, 111, -1, GameConstants.EnvObjects.deskRight);
            }

            for (var i = 8; i < 18; i += 3)
            {
                this.LevelObjects(i, 101, -1, GameConstants.EnvObjects.chairRight);
                this.LevelObjects(i, 111, -1, GameConstants.EnvObjects.chairRight);
            }

            for (var i = 9.6f; i < 18; i += 4)
            {
                this.LevelObjects(i, 98, -1, GameConstants.EnvObjects.deskLeft);
                this.LevelObjects(i, 108, -1, GameConstants.EnvObjects.deskLeft);
            }

            for (var i = 8.8f; i < 19.5f; i += 3)
            {
                this.LevelObjects(i, 98, -1, GameConstants.EnvObjects.chairLeft);
                this.LevelObjects(i, 108, -1, GameConstants.EnvObjects.chairLeft);
            }

#endregion

#region Treppenhaus
            for (var i = 30.1f; i < 39; i += 4)
            {
                this.LevelObjects(i, 124.5f, -1, GameConstants.EnvObjects.deskRight);
            }

            for (var i = 30; i < 40; i += 3)
            {
                this.LevelObjects(i, 124.5f, -1, GameConstants.EnvObjects.chairRight);
            }

            for (var i = 31.6f; i < 40; i += 4)
            {
                this.LevelObjects(i, 121.5f, -1, GameConstants.EnvObjects.deskLeft);
            }

            for (var i = 30.8f; i < 41.5f; i += 3)
            {
                this.LevelObjects(i, 121.5f, -1, GameConstants.EnvObjects.chairLeft);
            }

#endregion

#region Erster Raum oben

            this.LevelObjects(36.5f, 183.5f, -1, GameConstants.EnvObjects.deskUp);
#endregion

#region Erster Raum unten

            this.LevelObjects(10, 188, -1, GameConstants.EnvObjects.chairUp);
            this.LevelObjects(10, 188, -1, GameConstants.EnvObjects.deskUp);
            for (var i = 4.86f; i < 9; i += 4)
            {
                this.LevelObjects(i, 186.2f, -1, GameConstants.EnvObjects.deskRight);
            }

            this.LevelObjects(4.86f, 186.2f, -1, GameConstants.EnvObjects.chairRight);
#endregion

#region Zweiter Raum oben
            for (var i = 31; i <= 39; i += 4)
            {
                var j = i == 39 ? 37 : i;
                this.LevelObjects(j, 206, -1, GameConstants.EnvObjects.deskLeft);
                this.LevelObjects(j, 206, -1, GameConstants.EnvObjects.chairLeft);
            }

#endregion

#region Zweiter Raum unten
            for (var i = 7; i <= 15; i += 8)
            {
                this.LevelObjects(i-3, 213.5f, -1, GameConstants.EnvObjects.deskUp);
                this.LevelObjects(i-3, 213.5f, -1, GameConstants.EnvObjects.chairUp);
                this.LevelObjects(i, 215, -1, GameConstants.EnvObjects.deskDown);
                this.LevelObjects(i, 215, -1, GameConstants.EnvObjects.chairDown);
                this.LevelObjects(i-2, 217, -1, GameConstants.EnvObjects.chairRight);
            }

#endregion

#region Dritter Raum unten
            for (var i = 17; i<=21; i += 4)
            {
                this.LevelObjects(i, 228, -1, GameConstants.EnvObjects.deskLeft);
                this.LevelObjects(i, 228, -1, GameConstants.EnvObjects.chairLeft);
            }

            for (var i = 3.5f; i < 6; i += 2)
            {
                this.environmentObjects.Add(
                    new EnvironmentObject(
                    this.Game.Content, 
                    new Vector3(i, 239, -1), 
                    GameConstants.EnvObjects.chairRight));
            }

            for (var i = 231; i <= 237; i += 2)
            {
                this.environmentObjects.Add(
                    new EnvironmentObject(
                    this.Game.Content, 
                    new Vector3(1.8f, i, -1), 
                    GameConstants.EnvObjects.chairUp));
            }

            this.environmentObjects.Add(
                    new EnvironmentObject(
                    this.Game.Content, 
                    new Vector3(5, 235, -2), 
                    GameConstants.EnvObjects.deskDown));
#endregion

#region Vierter Raum unten
            for (var i = 11; i < 18; i += 2)
            {
                this.LevelObjects(i, 260.5f, -1, GameConstants.EnvObjects.chairRight);
            }

            for (var i = 12.0f; i < 17; i += 4)
            {
                this.LevelObjects(i, 260, -1, GameConstants.EnvObjects.deskRight);
            }

            for (var i = 12.45f; i < 19; i += 2)
            {
                this.LevelObjects(i, 252.22f, -1, GameConstants.EnvObjects.chairLeft);
            }

            for (var i = 13.45f; i < 18; i += 4)
            {
                this.LevelObjects(i, 252.72f, -1, GameConstants.EnvObjects.deskLeft);
            }

            for (var i = 254.1f; i < 261; i += 2)
            {
                this.LevelObjects(18.66f, i, -1, GameConstants.EnvObjects.chairDown);
            }

            this.LevelObjects(18.16f, 257.1f, -1, GameConstants.EnvObjects.deskDown);

            for (var i = 253f; i < 260; i += 2)
            {
                this.LevelObjects(10.5f, i, -1, GameConstants.EnvObjects.chairUp);
            }

            this.LevelObjects(11.3f, 255.6f, -1, GameConstants.EnvObjects.deskUp);

            for (var i = 242.5f; i < 249; i += 3)
            {
                this.LevelObjects(1.8f, i, -1, GameConstants.EnvObjects.chairDown);
            }

            for (var i = 243.6f; i < 248; i += 4)
            {
                this.LevelObjects(1.8f, i, -1, GameConstants.EnvObjects.deskDown);
            }

#endregion



            this.LevelObjects(90, 245, 0, GameConstants.EnvObjects.podest);  // podest

#region Loading Schränke
            this.LevelObjects(34.8f, 5, -1, GameConstants.EnvObjects.schrank);
            this.LevelObjects(34.8f, 9, -1, GameConstants.EnvObjects.schrank);
            this.LevelObjects(34.8f, 13, -1, GameConstants.EnvObjects.schrank);
#endregion

#region Loading Monitore

            this.LevelObjects(14.8f, 60.5f, 2, GameConstants.EnvObjects.monitor);
            this.LevelObjects(14.8f, 74, 2, GameConstants.EnvObjects.monitor);
            this.LevelObjects(22.8f, 60.5f, 2, GameConstants.EnvObjects.monitor);
            this.LevelObjects(22.8f, 74, 2, GameConstants.EnvObjects.monitor);
            this.LevelObjects(30.8f, 60.5f, 2, GameConstants.EnvObjects.monitor);
            this.LevelObjects(30.8f, 74, 2, GameConstants.EnvObjects.monitor);
            this.LevelObjects(10, 188, 2, GameConstants.EnvObjects.monitor_gedreht);    // Erster Raum unten links
            this.LevelObjects(36.3f, 184.5f, 2, GameConstants.EnvObjects.monitor_gedreht);    // Raum links neben Klos
            this.LevelObjects(37.4f, 184.5f, 2, GameConstants.EnvObjects.monitor_gedreht);    // Raum links neben Klos
#endregion

#region Loading Rechner

            this.LevelObjects(15.5f, 58.5f, 2, GameConstants.EnvObjects.rechner);
            this.LevelObjects(15.5f, 72.2f, 2, GameConstants.EnvObjects.rechner);
            this.LevelObjects(23.5f, 58.5f, 2, GameConstants.EnvObjects.rechner);
            this.LevelObjects(23.5f, 72.2f, 2, GameConstants.EnvObjects.rechner);
            this.LevelObjects(31.5f, 58.5f, 2, GameConstants.EnvObjects.rechner);
            this.LevelObjects(31.5f, 72.2f, 2, GameConstants.EnvObjects.rechner);
            this.LevelObjects(10.5f, 190, 2, GameConstants.EnvObjects.rechner_gedreht);  // Erster Raum unten links
#endregion

#region Loading Poster

            this.LevelObjects(6.99f, 5, 2, GameConstants.EnvObjects.mirkopir);
            this.LevelObjects2(6.7f, 10, 2, GameConstants.NonEnvObjects.poster_vader);
            this.LevelObjects2(6.7f, 25, 2, GameConstants.NonEnvObjects.poster_deadpool);
            this.LevelObjects2(6.7f, 32, 2, GameConstants.NonEnvObjects.poster_zombie);
            this.LevelObjects2(6.7f, 40, 2, GameConstants.NonEnvObjects.poster_cat);
            this.LevelObjects2(6.7f, 52, 2, GameConstants.NonEnvObjects.poster_dragonball);
            this.LevelObjects2(6.7f, 58, 2, GameConstants.NonEnvObjects.poster_obama);
            this.LevelObjects2(6.7f, 64, 2, GameConstants.NonEnvObjects.poster_totoro);
            this.LevelObjects2(6.7f, 69.5f, 2, GameConstants.NonEnvObjects.poster_godzilla);
            #endregion

            #region Loading Whiteboards

            this.LevelObjects(29, 23.5f, 2, GameConstants.EnvObjects.whiteboard);
            this.LevelObjects(16, 23.5f, 2, GameConstants.EnvObjects.whiteboard);
            this.LevelObjects(21, 116.65f, 2, GameConstants.EnvObjects.whiteboard_gedreht);

#endregion

#region Loading Pissour
            for (var i = 30; i<=45; i+=5)
            {
                this.LevelObjects(i, 139.9f, 0, GameConstants.EnvObjects.pissoir);
                this.LevelObjects(i, 140, 0, GameConstants.EnvObjects.pissoir_gedreht);
            }

#endregion

#region Loading Plants

            this.LevelObjects(9, 3, 1, GameConstants.EnvObjects.plant);
            this.LevelObjects(14.5f, 256.5f, 0, GameConstants.EnvObjects.plant);
#endregion
        }

        private void LevelObjects(float x, float y, float z, GameConstants.EnvObjects model)
        {
            this.environmentObjects.Add(
                    new EnvironmentObject(
                    this.Game.Content, 
                    new Vector3(x, y, z ), 
                    model));
        }

        private void LevelObjects2(float x, float y, float z, GameConstants.NonEnvObjects model)
        {
            this.nonenvironmentObjects.Add(
                    new NonEnvironmentObject(
                    this.Game.Content, 
                    new Vector3(x, y, z), 
                    model));
        }

    }
}