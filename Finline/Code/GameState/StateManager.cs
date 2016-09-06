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
        /// newRes puts an EGameState to null
        /// </summary>
        private EGameState? newRes = null;

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

        private SpriteFont font;
#region PauseStuff
        private bool isPressed = false;
        private bool paused = false;
        private Texture2D pausedTexture2D;
        private Rectangle pausedRectangle;
        #endregion

#region SoundIcons
        private Texture2D soundOnTexture2D;
        private Rectangle soundOnRectangle;
        private Texture2D soundOffTexture2D;
        private Rectangle soundOffRectangle;
#endregion

        private readonly Dictionary<EGameState, List<GuiElement>> guiElements = new Dictionary<EGameState, List<GuiElement>>();

#region MusicStuff
        Sounds sounds = new Sounds();
#endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="StateManager"/> class. 
        /// </summary>
        public StateManager()
        {
            this.Graphics = new GraphicsDeviceManager(this);
            this.IsMouseVisible = true;
            this.guiElements.Add(EGameState.InGame, new List<GuiElement>());

            this.guiElements[EGameState.InGame].Add(new GuiElement("Play"));
            this.guiElements[EGameState.InGame].Add(new GuiElement("Back2MainMenu"));

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
            this.font = this.Content.Load<SpriteFont>("font");

            this.pausedTexture2D = this.Content.Load<Texture2D>("Icons/PauseIcon");
            this.pausedRectangle = new Rectangle(320, 30, this.pausedTexture2D.Width, this.pausedTexture2D.Height);

            this.soundOnTexture2D = this.Content.Load<Texture2D>("Icons/SoundOn");
            this.soundOnRectangle = new Rectangle(700, 30, this.soundOnTexture2D.Width, this.soundOnTexture2D.Height);
            this.soundOffTexture2D = this.Content.Load<Texture2D>("Icons/SoundOff");
            this.soundOffRectangle = new Rectangle(700, 30, this.soundOffTexture2D.Width, this.soundOffTexture2D.Height);
            

#region Moved Buttons in PauseScreen
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
            this.guiElements[EGameState.InGame].Find(x => x.AssetName == "Play").MoveElement(0, -40);
            this.guiElements[EGameState.InGame].Find(x => x.AssetName == "Back2MainMenu").MoveElement(0, 40);
            #endregion

#region LoadingMusic

            this.sounds.LoadContent(this.Content);
#endregion
        }

        /// <summary>
        /// The unload content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        private float timer = 0;
        private float deltaTime;
        private bool timePaused = false;
        private bool MouseIsPressed = false;

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        protected override void Update(GameTime gameTime)
        {
            this.sounds.Update(gameTime);

            if (this.currentGameState == EGameState.None || this.newRes.HasValue) this.sounds.PlayMainMenuMusic();

            if (this.currentGameState == EGameState.MainMenu && this.nextGameState == EGameState.InGame) this.sounds.PlayIngameMusic();

            if (this.currentGameState == EGameState.InGame) this.sounds.PlayIngameSongChange();

            if (this.newRes.HasValue)
            {
                this.nextGameState = this.newRes.Value;
                this.newRes = null;
            }

            if (this.nextGameState != this.currentGameState)
            {
                this.HandleGameState();
            }

            #region Pause

            // MouseState mouse = Mouse.GetState();
            KeyboardState k = Keyboard.GetState();
            if (this.currentGameState == EGameState.InGame && this.paused)

                foreach (var element in this.guiElements[this.currentGameState])
                {
                    element.Update(ref this.MouseIsPressed);
                }

            if (this.currentGameState == EGameState.InGame && k.IsKeyDown(Keys.Escape) && !this.isPressed)
            {
                this.paused = !this.paused;
                this.isPressed = true;
                this.timePaused = true;

                if (this.paused)
                {
                    MediaPlayer.Pause();
                }
                else MediaPlayer.Resume();
            }

            if (this.isPressed && !k.IsKeyDown(Keys.Escape))
            {
                this.isPressed = false;
                this.timePaused = false;
            }

            if (!this.paused)
            {
                this.timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (this.currentGameState == EGameState.MainMenu)
                {
                    this.timer = 0;
                }

                this.gameState.Update(gameTime);
            }

            #endregion

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
            var r = new RasterizerState();
            r.CullMode = CullMode.None;
            this.GraphicsDevice.RasterizerState = r;
            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.GraphicsDevice.BlendState = BlendState.Opaque;
            this.gameState.Draw(gameTime);

            this.spriteBatch.Begin();



            if (this.currentGameState == EGameState.InGame)
            {
                if (MediaPlayer.State == MediaState.Paused)
                {
                    this.deltaTime = 0;
                    this.spriteBatch.Draw(this.soundOffTexture2D, this.soundOffRectangle, Color.White);
                }

                if (MediaPlayer.State == MediaState.Playing)
                {
                    this.deltaTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (this.deltaTime < 1.5f)
                        this.spriteBatch.Draw(this.soundOnTexture2D, this.soundOnRectangle, Color.White);
                    else this.spriteBatch.Draw(this.soundOnTexture2D, this.soundOnRectangle, Color.Transparent);

                    
                }
            }

            if (this.paused)
            {
                this.spriteBatch.Draw(this.pausedTexture2D, this.pausedRectangle, Color.White);
                if (this.currentGameState != EGameState.None && this.currentGameState != EGameState.MainMenu)
                    foreach (var element in this.guiElements[this.currentGameState])
                    {
                        element.Draw(this.spriteBatch);
                    }
            }

            if (this.currentGameState == EGameState.InGame) this.spriteBatch.DrawString(this.font, "Your current time is: " + this.timer.ToString("00.0")+ "s", new Vector2(500, 440), Color.WhiteSmoke);
            this.spriteBatch.End();
            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.GraphicsDevice.BlendState = BlendState.Opaque;
            
            base.Draw(gameTime);
        }

        /// <summary>
        /// when click on the button, do something
        /// </summary>
        /// <param name="element"></param>

        private void OnClick(string element)
        {
            if (this.isPressed) return;
            this.isPressed = true;

            if (element == "Play")
            {
                this.paused = !this.paused;
                MediaPlayer.Resume();
            }

            if (element != "Back2MainMenu") return;
            this.newRes = EGameState.MainMenu;
            this.paused = false;
            this.main.MakeHeile();
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
                    this.gameState = new Ingame(this, this.spriteBatch);
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