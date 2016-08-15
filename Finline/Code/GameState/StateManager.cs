﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StateManager.cs" company="Acagamics e.V.">
//   https://creativecommons.org/licenses/by-nc-sa/4.0/legalcode
// </copyright>
// <summary>
//   Defines the StateManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Finline.Code.GameState
{
    using System;

    using Game;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using Microsoft.Xna.Framework.Media;

    /// <summary>
    /// The game state manager.
    /// </summary>
    public class StateManager : Game
    {
        /// <summary>
        /// The actual game state object.
        /// </summary>
        private DrawableGameComponent gameState;

        /// <summary>
        /// The current game state.
        /// </summary>
        private EGameState currentGameState;

        /// <summary>
        /// The main menu.
        /// </summary>
        private MainMenu main;

        /// <summary>
        /// The next game state.
        /// </summary>
        private EGameState nextGameState;

        /// <summary>
        /// The <see cref="SpriteBatch"/>.
        /// </summary>
        private SpriteBatch spriteBatch;
        private bool isPressed = false;
        private SpriteFont font;
        

        private bool paused = false;
        private Texture2D pausedTexture2D;
        private Rectangle pausedRectangle;

        private Texture2D playTexture2D;
        private Rectangle playRectangle;

        private Texture2D optionTexture2D;
        private Rectangle optionRectangle;

        private Texture2D quitGameTexture2D;
        private Rectangle quitGameRectangle;

        private Song musicMainMenu;
        

        /// <summary>
        /// Initializes a new instance of the <see cref="StateManager"/> class. 
        /// </summary>
        public StateManager()
        {
            this.Graphics = new GraphicsDeviceManager(this);
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Gets the <see cref="GraphicsDeviceManager"/>.
        /// </summary>
        public GraphicsDeviceManager Graphics { get; private set; }

        /// <summary>
        /// Initializes the <see cref="StateManager"/>.
        /// </summary>
        protected override void Initialize()
        {
            this.nextGameState = EGameState.InGame;
            base.Initialize();
        }

        /// <summary>
        /// Loading content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Content.RootDirectory = "Content";
            this.main = new MainMenu(this, this.spriteBatch);
            this.main.GoIngame += this.StartNewGame;
            this.main.Initialize();

            this.pausedTexture2D = this.Content.Load<Texture2D>("PauseTrans");
            this.pausedRectangle = new Rectangle(345, 50, this.pausedTexture2D.Width, this.pausedTexture2D.Height);

            this.playTexture2D = this.Content.Load<Texture2D>("PlayTrans");
            this.playRectangle = new Rectangle(280, 150, this.playTexture2D.Width - 20, this.playTexture2D.Height - 20);

            this.optionTexture2D = this.Content.Load<Texture2D>("Option2Trans");
            this.optionRectangle = new Rectangle(280, 250, this.optionTexture2D.Width - 20, this.optionTexture2D.Height - 20);
            

            this.quitGameTexture2D = this.Content.Load<Texture2D>("End2Trans");
            this.quitGameRectangle = new Rectangle(280, 350, this.quitGameTexture2D.Width - 20, this.quitGameTexture2D.Height - 20);

            this.musicMainMenu = this.Content.Load<Song>("musicMainMenu");
            if (this.nextGameState == EGameState.MainMenu)
                MediaPlayer.Play(this.musicMainMenu);
        }

        /// <summary>
        /// The unload content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        protected override void Update(GameTime gameTime)
        {

            if (this.nextGameState == EGameState.InGame)
                MediaPlayer.Stop();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            if (this.nextGameState != this.currentGameState)
            {
                this.HandleGameState();
            }

            MouseState mouse = Mouse.GetState();
            KeyboardState k = Keyboard.GetState();
            if(this.currentGameState == EGameState.InGame)
            if (k.IsKeyDown(Keys.P) && !this.isPressed )
            {
                this.paused = !this.paused;
                this.isPressed = true;
            }

            if (this.isPressed && !k.IsKeyDown(Keys.P))
            {
                this.isPressed = false;
            }

            if (!this.paused)
            {
                this.gameState.Update(gameTime);
                
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Invokes Draws of the actual <see cref="DrawableGameComponent"/>.
        /// </summary>
        /// <param name="gameTime">
        /// The <see cref="GameTime"/>.
        /// </param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.White);
            RasterizerState r = new RasterizerState();
            r.CullMode = CullMode.None;
            this.GraphicsDevice.RasterizerState = r;
            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.GraphicsDevice.BlendState = BlendState.Opaque;
            this.gameState.Draw(gameTime);

            this.spriteBatch.Begin();
            if (this.paused)
            {
                this.spriteBatch.Draw(this.pausedTexture2D, this.pausedRectangle, Color.White);
                this.spriteBatch.Draw(this.playTexture2D, this.playRectangle, Color.White);
                this.spriteBatch.Draw(this.quitGameTexture2D, this.quitGameRectangle, Color.White);
                this.spriteBatch.Draw(this.optionTexture2D, this.optionRectangle, Color.White);
            }

            this.spriteBatch.End();
            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.GraphicsDevice.BlendState = BlendState.Opaque;

            base.Draw(gameTime);
        }

        /// <summary>
        /// Observes game states and calls for game state changes.
        /// </summary>
        private void HandleGameState()
        {
            switch (this.nextGameState)
            {
                case EGameState.MainMenu:
                    this.gameState = this.main;
                    break;

                case EGameState.InGame:
                    this.gameState = new Ingame(this);
                    break;
                case EGameState.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.gameState.Initialize();
            
            this.currentGameState = this.nextGameState;
        }

        /// <summary>
        /// Calls for a new game.
        /// </summary>
        private void StartNewGame()
        {
            this.nextGameState = EGameState.InGame;
        }
    }
}