namespace Finline.Code.GameState

{
    using Finline.Code.Game;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class StateManager : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;
        MainMenu main;
        private EGameState currentGameState;
        private EGameState nextGameState;
        private DrawableGameComponent GameState;

        public StateManager()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.IsMouseVisible = true;
        }
  
        protected override void Initialize()
        {
            this.nextGameState = EGameState.MainMenu;
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Content.RootDirectory = "Content";
            this.main = new MainMenu(this, this.spriteBatch);
            this.main.GoIngame += this.StartNewGame;
            this.main.Initialize();
        }

        /// <summary>
        /// The unload content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (this.nextGameState != this.currentGameState)
                this.HandleGameState();
            this.GameState.Update(gameTime);
            
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.White);

            this.spriteBatch.Begin();
            this.GameState.Draw(gameTime);
            this.spriteBatch.End();
            base.Draw(gameTime);
        }

        private void HandleGameState()
        {
            switch (this.nextGameState)
            {
                case EGameState.MainMenu:
                    this.GameState = this.main;
                    break;

                    case EGameState.InGame:
                    this.GameState = new Ingame(this);
                    break;
            }

            this.GameState.Initialize();

            this.currentGameState = this.nextGameState;
        }

        private void StartNewGame()
        {
            this.nextGameState = EGameState.InGame;
        }
    }
}
