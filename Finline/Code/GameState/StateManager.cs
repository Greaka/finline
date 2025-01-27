﻿// --------------------------------------------------------------------------------------------------------------------
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
    using System.Collections.Generic;

    using Finline.Code.Game;
    using Finline.Code.Game.Entities.LivingEntity;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The game state manager.
    /// </summary>
    public class StateManager : Game
    {
        #region MusicStuff
        /// <summary>
        /// The sounds.
        /// </summary>
        private readonly Sounds sounds = new Sounds();
        #endregion

        /// <summary>
        /// The GUI elements.
        /// </summary>
        private readonly Dictionary<EGameState, List<GuiElement>> guiElements = new Dictionary<EGameState, List<GuiElement>>();

        /// <summary>
        /// The actual game state object.
        /// </summary>
        private DrawableGameComponent gameState;
        
        /// <summary>
        /// The current game state.
        /// </summary>
        private EGameState currentGameState;

        /// <summary>
        /// The next game state.
        /// </summary>
        private EGameState nextGameState;

        /// <summary>
        /// The <see cref="SpriteBatch"/>.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// The delta time.
        /// </summary>
        private float deltaTime;

        /// <summary>
        /// The mouse is pressed.
        /// </summary>
        private bool mouseIsPressed;
        #region PauseStuff

        /// <summary>
        /// The is pressed.
        /// </summary>
        private bool isPressed;

        /// <summary>
        /// The paused.
        /// </summary>
        private bool paused;

        /// <summary>
        /// The paused texture 2 d.
        /// </summary>
        private Texture2D pausedTexture2D;

        /// <summary>
        /// The paused rectangle.
        /// </summary>
        private Rectangle pausedRectangle;
        #endregion

#region SoundIcons

        /// <summary>
        /// The sound on texture 2 d.
        /// </summary>
        private Texture2D soundOnTexture2D;

        /// <summary>
        /// The sound on rectangle.
        /// </summary>
        private Rectangle soundOnRectangle;

        /// <summary>
        /// The sound off texture 2 d.
        /// </summary>
        private Texture2D soundOffTexture2D;

        /// <summary>
        /// The sound off rectangle.
        /// </summary>
        private Rectangle soundOffRectangle;
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
        /// Gets the main.
        /// </summary>
        public MainMenu Main { get; private set; }

        /// <summary>
        /// Gets the <see cref="GraphicsDeviceManager"/>.
        /// </summary>
        public GraphicsDeviceManager Graphics { get; private set; }

        /// <summary>
        /// The go menu.
        /// </summary>
        /// <param name="me">
        /// The me.
        /// </param>
        public void GoMenu(LivingEntity me)
        {
            this.nextGameState = EGameState.MainMenu;
        }

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
            this.Main = new MainMenu(this, this.spriteBatch);
            this.Main.GoIngame += this.StartNewGame;

            this.Main.Initialize();

            this.pausedTexture2D = this.Content.Load<Texture2D>("Icons/PauseIcon");
            this.pausedRectangle = new Rectangle(320, 30, this.pausedTexture2D.Width, this.pausedTexture2D.Height);

            this.soundOnTexture2D = this.Content.Load<Texture2D>("Icons/SoundOn");
            this.soundOnRectangle = new Rectangle(700, 30, this.soundOnTexture2D.Width, this.soundOnTexture2D.Height);
            this.soundOffTexture2D = this.Content.Load<Texture2D>("Icons/SoundOff");
            this.soundOffRectangle = new Rectangle(700, 30, this.soundOffTexture2D.Width, this.soundOffTexture2D.Height);
            
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
            
            this.sounds.LoadContent(this.Content);
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
            this.sounds.Update();

            if (this.currentGameState == EGameState.None
                || (this.currentGameState == EGameState.InGame && this.nextGameState == EGameState.MainMenu))
            {
                this.sounds.PlayMainMenuMusic();
            }

            if (this.currentGameState == EGameState.MainMenu && this.nextGameState == EGameState.InGame)
            {
                this.sounds.PlayIngameMusic();
            }

            if (this.currentGameState == EGameState.InGame)
            {
                this.sounds.PlayIngameSongChange();
            }

            // if (this.newRes.HasValue)
            // {
            // this.nextGameState = this.newRes.Value;
            // this.newRes = null;
            // }
            if (this.nextGameState != this.currentGameState)
            {
                this.HandleGameState();
            }
            
            // MouseState mouse = Mouse.GetState();
            var k = Keyboard.GetState();
            if (this.currentGameState == EGameState.InGame && this.paused)
            {
                foreach (var element in this.guiElements[this.currentGameState])
                {
                    element.Update(ref this.mouseIsPressed);
                }
            }

            if (this.currentGameState == EGameState.InGame && k.IsKeyDown(Keys.Escape) && !this.isPressed)
            {
                this.paused = !this.paused;
                this.isPressed = true;

                Sounds.PauseMusicVolume();
            }

            if (this.isPressed && !k.IsKeyDown(Keys.Escape))
            {
                this.isPressed = false;
            }

            if (!this.paused)
            {
                this.gameState.Update(gameTime);
                if (this.currentGameState == EGameState.InGame)
                {
                    var game = (Ingame)this.gameState;
                    if (game.Won)
                    {
                        this.nextGameState = EGameState.MainMenu;
                        this.Main.MenuState = MainMenu.EMenuState.WinningScreen;
                    }
                }
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
            var r = new RasterizerState { CullMode = CullMode.None };
            this.GraphicsDevice.RasterizerState = r;
            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.GraphicsDevice.BlendState = BlendState.Opaque;
            this.gameState.Draw(gameTime);

            this.spriteBatch.Begin();

            if (this.currentGameState == EGameState.InGame)
            {
                if (this.sounds.GetSoundOn() == false)
                {
                    this.deltaTime = 0;
                    this.spriteBatch.Draw(this.soundOffTexture2D, this.soundOffRectangle, Color.White);
                }
                else
                {
                    this.deltaTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    this.spriteBatch.Draw(
                        this.soundOnTexture2D, 
                        this.soundOnRectangle, 
                        this.deltaTime < 1.5f ? Color.White : Color.Transparent);
                }
            }

            if (this.paused)
            {
                this.spriteBatch.Draw(this.pausedTexture2D, this.pausedRectangle, Color.White);
                if (this.currentGameState != EGameState.None && this.currentGameState != EGameState.MainMenu)
                {
                    foreach (var element in this.guiElements[this.currentGameState])
                    {
                        element.Draw(this.spriteBatch);
                    }
                }
            }
            
            this.spriteBatch.End();
            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.GraphicsDevice.BlendState = BlendState.Opaque;
            
            base.Draw(gameTime);
        }

        /// <summary>
        /// when click on the button, do something
        /// </summary>
        /// <param name="element">
        /// Which element.
        /// </param>
        private void OnClick(string element)
        {
            if (this.isPressed)
            {
                return;
            }

            this.isPressed = true;

            if (element == "Play")
            {
                this.paused = !this.paused;
                Sounds.PauseMusicVolume();
            }

            if (element != "Back2MainMenu")
            {
                return;
            }
            this.nextGameState = EGameState.MainMenu;
            this.paused = false;
            this.Main.MakeHeile();
        }

        /// <summary>
        /// Observes game states and calls for game state changes.
        /// </summary>
        private void HandleGameState()
        {
            switch (this.nextGameState)
            {
                case EGameState.MainMenu:
                    this.gameState = this.Main;
                    break;

                case EGameState.InGame:
                    this.gameState = new Ingame(this, this.spriteBatch, this.sounds);
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