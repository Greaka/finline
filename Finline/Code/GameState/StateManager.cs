// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StateManager.cs" company="Acagamics e.V.">
//   https://creativecommons.org/licenses/by-nc-sa/4.0/legalcode
// </copyright>
// <summary>
//   Defines the StateManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

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

        private readonly Dictionary<EGameState, List<GuiElement>> guiElements = new Dictionary<EGameState, List<GuiElement>>();

        private Song musicMainMenu;
        

        /// <summary>
        /// Initializes a new instance of the <see cref="StateManager"/> class. 
        /// </summary>
        public StateManager()
        {
            this.Graphics = new GraphicsDeviceManager(this);
            this.IsMouseVisible = true;
            this.guiElements.Add(EGameState.InGame, new List<GuiElement>());
            

            this.guiElements[EGameState.InGame].Add(new GuiElement("Play"));
            this.guiElements[EGameState.InGame].Add(new GuiElement("Options"));
            this.guiElements[EGameState.InGame].Add(new GuiElement("End"));

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

            this.pausedTexture2D = this.Content.Load<Texture2D>("PauseTrans");
            this.pausedRectangle = new Rectangle(360, 30, this.pausedTexture2D.Width, this.pausedTexture2D.Height);


            this.font = this.Content.Load<SpriteFont>("font");
            foreach (var elementList in this.guiElements.Values)
            {
                foreach (var element in elementList)
                {
                    element.LoadContent(this.Content);
                    element.CenterElement(600, 800);
                    element.ClickEvent += this.OnClick;
                }
            }

            // buttons in the pausescreen
            this.guiElements[EGameState.InGame].Find(x => x.AssetName == "Play").MoveElement(0, -100);
            this.guiElements[EGameState.InGame].Find(x => x.AssetName == "Options").MoveElement(0, 0);
            this.guiElements[EGameState.InGame].Find(x => x.AssetName == "End").MoveElement(0, 100);

            //music in titlescreen and menusystem
            this.musicMainMenu = this.Content.Load<Song>("musicMainMenu");
            if (this.nextGameState == EGameState.MainMenu)
                MediaPlayer.Play(this.musicMainMenu);
        }

        private void OnClick(string element)
        {
            if (this.isPressed) return;
                this.isPressed = true;

            if (element == "Play")                 //muss noch verbessert werden, da die buttons noch nicht funktionieren

                this.paused = !this.paused;
            
            //if(element == "Option2Trans")
                   
            

            if (element == "End")
                this.Exit();

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
        private bool off = false;
        KeyboardState oldKeyState;
        protected override void Update(GameTime gameTime)
        {
             
            if (this.nextGameState == EGameState.InGame)
                MediaPlayer.Stop();

            KeyboardState newKeyState = Keyboard.GetState();
            if (newKeyState.IsKeyDown(Keys.O) && oldKeyState.IsKeyUp(Keys.O))
            {
                if (off == true)
                {
                    MediaPlayer.Pause();
                }
                else
                {
                    MediaPlayer.Resume();
                }
                off = !off;
            }
            oldKeyState = newKeyState;


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



            if (k.IsKeyDown(Keys.P) && !this.isPressed)
                {
                    foreach (var element in this.guiElements[this.currentGameState])  //muss noch verbessert werden, da die buttons noch nicht verwendet werden können
                    {
                        element.Update(ref this.isPressed);
                    }

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
            this.GraphicsDevice.Clear(Color.Black);
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
                if (this.currentGameState != EGameState.None)
                    foreach (var element in this.guiElements[this.currentGameState])
                    {
                        element.Draw(this.spriteBatch);
                    }
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