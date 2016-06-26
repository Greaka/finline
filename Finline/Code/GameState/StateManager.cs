// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StateManager.cs" company="Acagamics e.V.">
//   https://creativecommons.org/licenses/by-nc-sa/4.0/legalcode
// </copyright>
// <summary>
//   Defines the StateManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Finline.Code.GameState
{
    using System;

    using Game;
    using Game.Controls;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The game state manager.
    /// </summary>
    public class StateManager : Game
    {
        /// <summary>
        /// The Input Parser.
        /// </summary>
        public readonly Controller Controls = new Controller();

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

        private Texture2D quitGameTexture2D;
        private Rectangle quitGameRectangle;


        
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
            this.nextGameState = EGameState.MainMenu;
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

            pausedTexture2D = Content.Load<Texture2D>("PauseTrans");
            pausedRectangle = new Rectangle(345, 100, pausedTexture2D.Width, pausedTexture2D.Height);

            quitGameTexture2D = Content.Load<Texture2D>("End2Trans");
            quitGameRectangle = new Rectangle(280, 300, quitGameTexture2D.Width - 20, quitGameTexture2D.Height - 20);

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
            if (k.IsKeyDown(Keys.P) && !isPressed )
            {
                paused = !paused;
                isPressed = true;

            }

            if (isPressed && !k.IsKeyDown(Keys.P))
            {
                isPressed = false;
            }


            if (!paused)
            {
                this.Controls.Update(this.GraphicsDevice);
                this.gameState.Update(gameTime);
                
            }
            else
            {
                

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
            
            
            spriteBatch.Begin();
            if (paused)
            {
                spriteBatch.Draw(pausedTexture2D, pausedRectangle, Color.White);
                spriteBatch.Draw(quitGameTexture2D, quitGameRectangle, Color.White);
            }
            spriteBatch.End();
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